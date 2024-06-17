

using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Helpers.Web;
using OWP.Enums;

namespace OWP.Application.QuickSearch
{
    using Constants;
    using Fanex.Data.Repository;
    using Resources;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface Repository
    {
        Task<IEnumerable<MatchInfoModel>> Query(QueryModel model);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository repository;
        private readonly INapSession session;
        private readonly PermissionService permissionService;
        private readonly ValueTuple<List<MatchInfoModel>, List<Assignee>> matchInfoSuggestion;
        private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;


        public RepositoryImp(IDynamicRepository repository, INapSession session, PermissionService permissionService, GetAdminSiteMatchIdService getAdminSiteMatchIdService)
        {
            this.repository = repository;
            this.session = session;
            this.permissionService = permissionService;
            this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
            matchInfoSuggestion.Item1 = new List<MatchInfoModel>();
            matchInfoSuggestion.Item2 = new List<Assignee>();

        }

        public async Task<IEnumerable<MatchInfoModel>> Query(QueryModel model)
        {
            var hasFullTimeView = permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
            var hasTimeRange = permissionService.HasRight(FunctionKeys.TimeRangeView, Constant.Policy.Read);
            model.QueryFullTime = hasFullTimeView;
            model.QueryTimeRange = hasTimeRange;
            var tuple = await GetSearchResult(model);
            var matchGroup = tuple.Item1.GroupBy(x => x.OWPMatchId);
            foreach (var group in matchGroup)
            {
                matchInfoSuggestion.Item1.Add(group.FirstOrDefault());
            }
            matchInfoSuggestion.Item2.AddRange(tuple.Item2);
            var assignees = matchInfoSuggestion.Item2;
            OrderDisplayResultFilter();
            foreach (var matchInfo in matchInfoSuggestion.Item1)
            {
                matchInfo.MainTrader = GetAssignmentInfo(ViewLang.Trader, matchInfo.OWPMatchId, true, (int)AssignmentType.Main, assignees);
                matchInfo.OtherTrader = GetAssignmentInfo($"${ViewLang.Other} ${ViewLang.Trader}", matchInfo.OWPMatchId, true, (int)AssignmentType.Others, assignees);
                matchInfo.MainLA = GetAssignmentInfo(ViewLang.LA, matchInfo.OWPMatchId, false, (int)AssignmentType.Main, assignees);
                matchInfo.OtherLA = GetAssignmentInfo($"${ViewLang.Other} ${ViewLang.LA}", matchInfo.OWPMatchId, false, (int)AssignmentType.Others, assignees);
                matchInfo.IPTVCopy = IPTVBuilder.Build(new IPTVModel
                {
                    TeleVision = string.Join("/", tuple.Item3.Where(x => x.OWPMatchId == matchInfo.OWPMatchId).Select(x => x.TVCode)),
                    IpTv = matchInfo.IPTV,
                    Channel = matchInfo.Channel,
                    Stream = matchInfo.WatchBet
                });
                matchInfo.AdminSiteId = await getAdminSiteMatchIdService.GetAdminSiteMatchId(matchInfo.OWPMatchId,
                    matchInfo.ParentMatchId ?? 0, matchInfo.Bettype);
            }
            var indexBefore = matchInfoSuggestion.Item1.FindLastIndex(m => m.OWPMatchId == model.LastOWPMatchId);
            return matchInfoSuggestion.Item1.Skip(indexBefore + 1).Take(model.SelectTop);
        }

        private void OrderDisplayResultFilter()
        {
            var result = new List<MatchInfoModel>();
            var matchInfo = matchInfoSuggestion.Item1;
            var hasFullTimeView = permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
            if (hasFullTimeView)
            {
                result.AddRange(OrderResultFilter.Order(matchInfo));
            }
            else
            {
                result.AddRange(matchInfo.OrderBy(m => m.EventDate));
            }
            matchInfoSuggestion.Item1.Clear();
            matchInfoSuggestion.Item1.AddRange(result);
        }

        private async Task<ValueTuple<IEnumerable<MatchInfoModel>, IEnumerable<Assignee>, IEnumerable<Television>>> GetSearchResult(QueryModel model)
        {
            var criteria = new GetSearchResultCriteria
            {
                UserId = session.UserId,
                SearchKeywords = model.SearchKeywords,
                IsNumericKeywords = model.IsNumericKeywords && int.TryParse(model.SearchKeywords, out _),
                IsFullTimeView = model.QueryFullTime,
                IsTimeRange = model.QueryTimeRange
            };

            return await repository.FetchMultipleAsync<MatchInfoModel, Assignee, Television>(criteria);
        }

        private static AssignmentInfo GetAssignmentInfo(string positionName, int matchId, bool isTrader, int assignmentType, IEnumerable<Assignee> assignees)
        {
            AssignmentInfo result = null;
            var filteredAssignees = assignees.Where(t => t.OWPMatchId == matchId && t.IsTrader == isTrader && t.AssignmentType == assignmentType);

            if (filteredAssignees.Any())
            {
                var stringBuilder = new StringBuilder();

                foreach (var assignee in filteredAssignees)
                {
                    stringBuilder.Append((stringBuilder.Length > 0 ? " - " : " ") + assignee.AssigneeName);
                }

                result = new AssignmentInfo
                {
                    PositionName = positionName,
                    AssigneesNameConjunction = stringBuilder.ToString()
                };
            }

            return result;
        }
    }
}
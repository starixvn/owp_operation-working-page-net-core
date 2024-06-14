using Fanex.Data.Repository;
using OWP.Models;
using OWP.OddsCompiler.Criteria;
using OWP.OddsCompiler.Enumerations;
using OWP.OddsCompiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Fanex.NAP.Client.Helpers.Web;
using OWP.Extensions;
using OWP.Services;
using InfoSiteTimeModel = OWP.OddsCompiler.Models.InfoSiteTimeModel;

namespace OWP.OddsCompiler;
public interface Repository
{
    Task<List<MatchInfoModel>> GetOddsCompiler(GetOddsCompilerCriteria criteria);

    Task<ResultDataObject> SaveMarket(MarketUpdateModel model);

    Task<ResultDataObject> InsertOdds(InsertOddsModel model);

    Task<ResultDataObject> UpdateOdds(UpdateOddsModel model);

    Task<ResultDataObject> DeleteOdds(DeleteOddsModel model);

    Task<IEnumerable<ReferenceResource>> GetResourceByName(string name);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSession currentSession;

    private readonly NotificationOddCompilerService notificationService;

    public RepositoryImp(IDynamicRepository dynamicRepository,
        INapSession currentSession,
        NotificationOddCompilerService notificationService)
    {
        this.dynamicRepository = dynamicRepository;
        this.currentSession = currentSession;
        this.notificationService = notificationService;
    }

    public async Task<ResultDataObject> SaveMarket(MarketUpdateModel model)
    {
        var criteria = new UpdateMarketOddCompilerCriteria
        {
            ActorId = currentSession.UserId,
            Market = model.Market,
            OWPMatchId = model.OWPMatchId
        };

        await dynamicRepository.ExecuteAsync(criteria);
        var updateResult = new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg
        };

        if (updateResult.IsSuccess && !string.IsNullOrEmpty(updateResult.Message))
        {
            updateResult = await SendNotificationWithEditMarket(model.OWPMatchId, updateResult.Message);
        }

        return updateResult;
    }

    public async Task<List<MatchInfoModel>> GetOddsCompiler(GetOddsCompilerCriteria criteria)
    {
        var fetchedData = await dynamicRepository.FetchMultipleAsync<MatchInfoModel, MatchSetting, OddsDetailModel>(criteria);
        var matches = fetchedData.FirstResultSet.ToList();
        var settings = fetchedData.SecondResultSet.ToList();
        var odds = fetchedData.ThirdResultSet.ToList();
        var oddsDetails = TransformOdds(odds);
        foreach (var match in matches)
        {
            match.MatchSettings = settings.Where(i => i.OWPMatchId == match.OWPMatchId).ToList();
            var matchOdds = oddsDetails.Where(i => i.OWPMatchId == match.OWPMatchId).ToList();
            match.MatchOddsRow = GetMatchOddRow(match.OWPMatchId, matchOdds);
        }

        await BuildInfoSiteTime(matches);
        return matches;
    }

    public async Task BuildInfoSiteTime(List<MatchInfoModel> matches)
    {
        var parentMatchIds = string.Join(",", matches.Select(x => x.ParentMatchId).Distinct());
        var criteria = new GetInfoSiteTimeCriteria
        {
            ParentMatchIds = parentMatchIds
        };

        var result = (await dynamicRepository.FetchAsync<InfoSiteTimeModel>(criteria)).ToList();

        foreach (var match in matches)
        {
            var infoSite = result.Find(x => x.ParentMatchId == match.ParentMatchId);
            match.InfoSiteTime = infoSite?.KickOffTime;
            match.InfoSiteTimeDisplay = infoSite?.KickOffTimeDisplay;
        }
    }

    public async Task<ResultDataObject> InsertOdds(InsertOddsModel model)
    {
        var criteria = new InsertOddsCompilerCriteria
        {
            ActorId = currentSession.UserId,
            OWPMatchId = model.OWPMatchId,
            Bettype = model.Bettype,
            HDP = model.HDP,
            TeamChoice = model.TeamChoice,
            Odds1 = model.Odds1,
            Odds2 = model.Odds2,
            Odds3 = model.Odds3
        };


        var result = await dynamicRepository.FetchAsync<MatchOdds>(criteria);

        var resultObject = new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            Data = criteria.ErrCode == (int)ErrorCode.Ok ? result.First().Id : 0
        };
        resultObject.ThrowExceptionIfError();

        if (resultObject.IsSuccess)
        {
            await SendOddsCompilerNotificationWithBettype(model.OWPMatchId, new[] { model.Bettype });
        }

        return resultObject;
    }

    public async Task<ResultDataObject> UpdateOdds(UpdateOddsModel model)
    {
        var criteria = new UpdateOddsCompilerCriteria
        {
            ActorId = currentSession.UserId,
            MatchRecommendOddsId = model.Id,
            HDP = model.HDP,
            TeamChoice = model.TeamChoice,
            Odds1 = model.Odds1,
            Odds2 = model.Odds2,
            Odds3 = model.Odds3
        };

        await dynamicRepository.ExecuteAsync(criteria);
        var resultObject = new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg
        };
        resultObject.ThrowExceptionIfError();
        if (resultObject.IsSuccess)
        {
            await SendOddsCompilerNotificationWithBettype(model.OWPMatchId, new[] { model.Bettype });
        }

        return resultObject;
    }

    public async Task<ResultDataObject> DeleteOdds(DeleteOddsModel model)
    {
        if (model.List == null || model.List.Count == 0)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Invalid criteria")
            };
        //    throw new HttpResponseException(resp);
        }

        var criteria = new DeleteOddsCompilerCriteria
        {
            ActorId = currentSession.UserId,
            MatchOddsIds = string.Join(",", model.List.Select(x => x.Id))
        };

        await dynamicRepository.ExecuteAsync(criteria);
        var resultObject = new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg
        };
        resultObject.ThrowExceptionIfError();
        if (resultObject.IsSuccess)
        {
            await SendOddsCompilerNotificationWithBettype(model.List[0].OWPMatchId, model.List.Select(x => x.Bettype).ToList());
        }

        return resultObject;
    }

    public async Task<IEnumerable<ReferenceResource>> GetResourceByName(string name)
    {
        var criteria = new GetRefResourceByNameCriteria { RefName = name };
        var result = await dynamicRepository.FetchAsync<ReferenceResource>(criteria);
        return result;
    }

    private static List<MatchOdds> TransformOdds(List<OddsDetailModel> rows)
    {
        return rows.Select(x => x.Id).Distinct().Select(id =>
        {
            var filterRows = rows.Where(x => x.Id == id);
            var firstRow = filterRows.First();
            var odds = new decimal?[3];
            foreach (var row in filterRows)
            {
                var choice = Math.Max((int)row.Choice, 1);
                odds[choice - 1] = row.Odds;
            }
            return new MatchOdds
            {
                Id = id,
                OWPMatchId = firstRow.OWPMatchId,
                Bettype = firstRow.Bettype,
                TeamChoice = firstRow.TeamChoice,
                HDP = firstRow.HDP,
                Odds1 = odds[0],
                Odds2 = odds[1],
#pragma warning disable S109 // Magic numbers should not be used
                Odds3 = odds[2]
#pragma warning restore S109 // Magic numbers should not be used
            };
        }).ToList();
    }

    private static MatchOddsRow GetMatchOddRow(int owpMatchId, List<MatchOdds> matchOdds)
    {
        var handicap = matchOdds.Find(x => x.Bettype == (int)MajorBettype.Handicap);
        handicap = handicap ?? new MatchOdds
        {
            OWPMatchId = owpMatchId,
            Bettype = (int)MajorBettype.Handicap
        };

        var overUnder = matchOdds.Find(x => x.Bettype == (int)MajorBettype.OverUnder);
        overUnder = overUnder ?? new MatchOdds
        {
            OWPMatchId = owpMatchId,
            Bettype = (int)MajorBettype.OverUnder
        };

        var oneXTwo = matchOdds.Find(x => x.Bettype == (int)MajorBettype.OneXTwo);
        oneXTwo = oneXTwo ?? new MatchOdds
        {
            OWPMatchId = owpMatchId,
            Bettype = (int)MajorBettype.OneXTwo
        };

        return new MatchOddsRow
        {
            Handicap = handicap,
            OverUnder = overUnder,
            OneXTwo = oneXTwo
        };
    }

    private async Task<ResultDataObject> SendNotificationWithEditMarket(int owpMatchId, string message)
    {
        var result = new ResultDataObject
        {
            Code = 0,
            Message = ""
        };
        var martketSendNotificationModel = message.ParseXml<MartketSendNotificationModel>();
        if (!string.IsNullOrEmpty(martketSendNotificationModel.OldValue) || !string.IsNullOrEmpty(martketSendNotificationModel.NewValue))
        {
            var getUserIdResult = await GetUsersToSendNotification(owpMatchId,
                (int)NotificationSettings.WatcherMatchInfoSetting,
                (int)NotificationSettings.AssigneeMatchInfoSetting);
            var listAssigneeIds = getUserIdResult.Item1;
            var listWatcherIds = getUserIdResult.Item2;
            if (listAssigneeIds != null && listAssigneeIds.Any() || listWatcherIds != null && listWatcherIds.Any())
            {
                var assigneeIds = string.Join(",", listAssigneeIds.Select(x => x.UserId).ToArray());
                var watcherIds = string.Join(",", listWatcherIds.Select(x => x.UserId).ToArray());

                result = await SendNotificationWithEditMarket(new MarketNotificationModel
                {
                    OWPMatchId = owpMatchId,
                    AssigneeIds = assigneeIds,
                    WatcherIds = watcherIds,
                    OldValue = martketSendNotificationModel.OldValue,
                    NewValue = martketSendNotificationModel.NewValue
                });
            }
        }

        return result;
    }

    private async Task<ResultDataObject> SendNotificationWithEditMarket(MarketNotificationModel model)
    {
        var criteria = new SendMarketNotificationCriteria
        {
            ActorId = currentSession.UserId,
            OWPMatchId = model.OWPMatchId,
            OldValue = model.OldValue,
            NewValue = model.NewValue,
            AssigneeIds = model.AssigneeIds,
            WatcherIds = model.WatcherIds
        };

        await dynamicRepository.ExecuteAsync(criteria);

        await notificationService.Notify(criteria.SessionId);
        return new ResultDataObject
        {
            Code = 0,
            Message = "Ok"
        };
    }

    private async Task<ResultDataObject> SendOddsCompilerNotificationWithBettype(int owpMatchId, IList<int> bettypes)
    {
        var result = new ResultDataObject
        {
            Code = 0,
            Message = ""
        };

        foreach (var bettype in bettypes)
        {
            var getUserIdResult = await GetUsersToSendNotification(owpMatchId,
                (int)NotificationSettings.WatcherOddsCompilerSetting,
                (int)NotificationSettings.AssigneeOddsCompilerSetting);
            var listAssigneeIds = getUserIdResult.Item1;
            var listWatcherIds = getUserIdResult.Item2;
            if (listAssigneeIds != null && listAssigneeIds.Any() || listWatcherIds != null && listWatcherIds.Any())
            {
                var assigneeIds = string.Join(",", listAssigneeIds.Select(x => x.UserId).ToArray());
                var watcherIds = string.Join(",", listWatcherIds.Select(x => x.UserId).ToArray());

                result = await SendOddsCompilerNotificationWithBettype(new OddsCompilerNotificationModel
                {
                    OWPMatchId = owpMatchId,
                    AssigneeIds = assigneeIds,
                    WatcherIds = watcherIds,
                    Bettype = bettype
                });
            }
        }

        return result;
    }

    private async Task<ResultDataObject> SendOddsCompilerNotificationWithBettype(OddsCompilerNotificationModel model)
    {
        var criteria = new SendOddsCompilerNotificationCriteria
        {
            ActorId = currentSession.UserId,
            OWPMatchId = model.OWPMatchId,
            AssigneeIds = model.AssigneeIds,
            WatcherIds = model.WatcherIds,
            Bettype = model.Bettype
        };

        await dynamicRepository.ExecuteAsync(criteria);
        await notificationService.Notify(criteria.SessionId);
        return new ResultDataObject
        {
            Code = 0,
            Message = "Ok"
        };
    }

    private async Task<Tuple<IEnumerable<UserIdModel>, IEnumerable<UserIdModel>>> GetUsersToSendNotification(int owpMatchId, int AssigneeSettingId, int WatcherSettingId)
    {
        var mappingTypes = new[] { typeof(UserIdModel), typeof(UserIdModel) };
        var criteria = new GetUsersSendNotificationCriteria
        {
            OWPMatchId = owpMatchId,
            AssigneeSettingId = AssigneeSettingId,
            WatcherSettingId = WatcherSettingId
        };
        var fetchedData = await dynamicRepository.FetchMultipleAsync(criteria, mappingTypes);
        var listAssigneeIds = fetchedData[0].Cast<UserIdModel>();
        var listWatcherIds = fetchedData[1].Cast<UserIdModel>();
        return new Tuple<IEnumerable<UserIdModel>, IEnumerable<UserIdModel>>(listAssigneeIds, listWatcherIds);
    }
}

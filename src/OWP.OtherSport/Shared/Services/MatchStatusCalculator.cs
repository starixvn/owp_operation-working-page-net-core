using Fanex.Data.Repository;
using OWP.Extensions;
using OWP.OtherSport.Shared.Constants;
using OWP.OtherSport.Submit.Models;
using OWP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.OtherSport.Shared.Services
{
    public interface MatchStatusCalculator
    {
        Task Build(Dictionary<int, MatchInfo> matchInfoDict);
    }

    public class MatchStatusCalculatorImp : MatchStatusCalculator
    {
        private readonly IDynamicRepository dynamicRepository;

        public MatchStatusCalculatorImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task Build(Dictionary<int, MatchInfo> matchInfoDict)
        {
            await MapMatchStatusFromMainDb(matchInfoDict);

            foreach (var key in matchInfoDict.Keys)
            {
                var match = matchInfoDict[key];
                if (!match.OfferLive.IsContains(OfferLive.Live))
                {
                    match.MatchStatus = String.Empty;
                }

                if (match.MatchStatus.Equals(MatchStatus.Completed, StringComparison.OrdinalIgnoreCase))
                {
                    match.MatchStatus = match.MatchStatus;
                }

                var nowDateTime = DateTimeExtensions.ToGmt8(DateTime.Now);
                if (match.DateTime.Date > nowDateTime.Date)
                {
                    match.MatchStatus = MatchStatus.Early;
                }
                else if (match.DateTime >= nowDateTime)
                {
                    match.MatchStatus = MatchStatus.InPlay;
                }
                else
                {
                    match.MatchStatus = MatchStatus.PreMatch;
                }
            }
        }

        private async Task MapMatchStatusFromMainDb(Dictionary<int, MatchInfo> matchInfoDict)
        {
            var adminSiteMatchIds = matchInfoDict.Values.Select(x => x.AdminsiteMatchId).Distinct();
            var matchStatusFromMainDBs = await QueryMatchStatus(adminSiteMatchIds);
            foreach (var key in matchInfoDict.Keys)
            {
                var match = matchInfoDict[key];
                var matchFromMainDb = matchStatusFromMainDBs.GetValue(key, new EventStatus());
                match.MatchStatus = matchFromMainDb.MatchStatus;
            }
        }

        public async Task<Dictionary<int, EventStatus>> QueryMatchStatus(IEnumerable<int> adminsiteMatchIds)
        {
            var criteria = new GetEventStatusFromMainDBCriteria
            {
                AdminsiteMatchIds = string.Join(",", adminsiteMatchIds)
            };

            var result = await dynamicRepository.FetchAsync<EventStatus>(criteria);

            return result.ToDictionary(x => x.AdminsiteMatchId);
        }

    }

    public class GetEventStatusFromMainDBCriteria : CriteriaBase
    {
        public string AdminsiteMatchIds { get; set; }

        public override string GetSettingKey() => "OWP_OSLiveGameAssignment_GetEventStatusFromMainDB";

        public override bool IsValid() => true;
    }

    public class EventStatus
    {
        public int AdminsiteMatchId { get; set; }

        public string MatchStatus { get; set; }
    }
}

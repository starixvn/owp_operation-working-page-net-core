namespace OWP.LeagueManagement.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using AutoMapper;
    using Criteria;
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Helpers.Web;
    using Olympus.Xml;
    using Models;
    using Newtonsoft.Json;
    using OWP.Models;
    using OWP.LeagueManagement.Misc;

    public interface LeagueManagementService
    {
        Task<IEnumerable<ReferenceResource>> GetResourceByName(string name);

        Task<ResultDataObject> UpdateTvList(IEnumerable<TVList> models);

        Task<ResultDataObject> UpdateCondition(LeagueSettingCondition model);

        Task<LeagueSettingCondition> GetCondition(int leagueSettingId);

        Task<IList<LeagueInfoResponse>> GetAllLeague(LeagueInfoQuery query);

        // string GetConditionRegex(Condition condition);

        // LeagueSettingCondition ConvertToLeagueSettingCondition(int leagueSettingId, string condition);
    }

    public class LeagueManagementServiceImp : LeagueManagementService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IMapper mapper;
        private readonly INapSession session;

        public LeagueManagementServiceImp(
            INapSession session,
            IDynamicRepository dynamicRepository,
            IMapper mapper)
        {
            this.session = session;
            this.dynamicRepository = dynamicRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ReferenceResource>> GetResourceByName(string name)
        {
            var criteria = new GetRefResourceByNameCriteria { RefName = name };

            return await dynamicRepository.FetchAsync<ReferenceResource>(criteria);
        }

        public async Task<ResultDataObject> UpdateTvList(IEnumerable<TVList> models)
        {
            var criteria = new UpdateTvListCriteria
            {
                UpdateTvListXml = XmlConverter.Serialize(models),
                ActorId = session.UserId
            };
            await dynamicRepository.FetchAsync<ResultDataObject>(criteria);
            return new ResultDataObject
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }

        public async Task<ResultDataObject> UpdateCondition(LeagueSettingCondition model)
        {
            var rounds = model.Rounds.Select(GetConditionRegex);
            var divisions = model.Divisions.Select(GetConditionRegex);

            var criteria = new UpdateConditionCriteria
            {
                ActorId = session.UserId,
                LeagueSettingId = model.LeagueSettingId,
                HasTV = model.HasTV,
                TV = model.TV,
                Division = string.Join("/", divisions),
                Round = string.Join("/", rounds),
                FullConditionDescription = JsonConvert.SerializeObject(model)
            };

            await dynamicRepository.FetchAsync<ResultDataObject>(criteria);
            return new ResultDataObject
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }

        public string GetConditionRegex(Condition condition)
        {
            return condition.Type == Constant.ContainsCondition
                ? $"*{condition.Value}*"
                : condition.Value;
        }

        public async Task<LeagueSettingCondition> GetCondition(int leagueSettingId)
        {
            var criteria = new GetConditionCriteria { LeagueSettingId = leagueSettingId };
            var condition = await dynamicRepository.FetchAsync<string>(criteria);
            var result = ConvertToLeagueSettingCondition(leagueSettingId, condition?.FirstOrDefault());

            result.LeagueSettingId = leagueSettingId;

            return result;
        }
        public LeagueSettingCondition ConvertToLeagueSettingCondition(int leagueSettingId, string condition)
        {
            return string.IsNullOrEmpty(condition)
                ? new LeagueSettingCondition { LeagueSettingId = leagueSettingId }
                : JsonConvert.DeserializeObject<LeagueSettingCondition>(condition);
        }


        public async Task<IList<LeagueInfoResponse>> GetAllLeague(LeagueInfoQuery query)
        {
            var criteria = mapper.Map<GetAllLeagueInfoCriteria>(query);
            var leagueInfoResponses = (await dynamicRepository.FetchAsync<LeagueInfoResponse>(criteria)).ToList();
            var leagueIds = leagueInfoResponses.Select(l => l.OWPLeagueId);
            var leagueIdString = string.Join(",", leagueIds);

            var leagueLinkToTeam = (await dynamicRepository.FetchAsync<int>(new GetLinkLeagueAndTeamCriteria
            {
                LeagueIdString = leagueIdString
            })).ToList();

            foreach (var league in leagueInfoResponses)
            {
                league.HasLinkLeagueAndTeam = leagueLinkToTeam.Any(id => league.OWPLeagueId == id);
            }

            return leagueInfoResponses?.ToList();
        }
    }
}
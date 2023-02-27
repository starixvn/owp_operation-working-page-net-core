using Fanex.NAP.Client.Helpers.Web;
using Newtonsoft.Json;
using Olympus.Xml;
using OWP.LeagueManagement.Criteria;
using OWP.LeagueManagement.Misc;
using OWP.LeagueManagement.Models;
using OWP.Models;

namespace OWP.LeagueManagement.Services;

public interface LeagueManagementService
{

    Task<ResultDataObject> UpdateTvList(IEnumerable<TVList> models);

    Task<ResultDataObject> UpdateCondition(LeagueSettingCondition model);

    Task<LeagueSettingCondition> GetCondition(int leagueSettingId);

    Task<IList<LeagueInfoResponse>> GetAllLeague(LeagueInfoQuery query);

    string GetConditionRegex(Condition condition);

    LeagueSettingCondition ConvertToLeagueSettingCondition(int leagueSettingId, string condition);
}

public class LeagueManagementServiceImp : LeagueManagementService
{
    private readonly Repository repository;
    private readonly INapSession session;

    public LeagueManagementServiceImp(INapSession session, Repository repository)
    {
        this.session = session;
        this.repository = repository;
    }

    public async Task<ResultDataObject> UpdateTvList(IEnumerable<TVList> models)
    {
        var criteria = new UpdateTvListCriteria
        {
            UpdateTvListXml = XmlConverter.Serialize(models),
            ActorId = session.UserId
        };
        return await repository.UpdateTVList(criteria);
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
        return await repository.UpdateCondition(criteria);
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
        var condition = await repository.GetCondition(criteria);
        var result = ConvertToLeagueSettingCondition(leagueSettingId, condition);
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
        var leagueInfoResponses = await repository.GetAllLeague(query);
        var leagueIds = leagueInfoResponses.Select(l => l.OWPLeagueId);
        var leagueIdString = string.Join(",", leagueIds);

        var leagueLinkToTeam = await repository.GetLeagueLinkToTeam(leagueIdString);

        foreach (var league in leagueInfoResponses)
        {
            league.HasLinkLeagueAndTeam = leagueLinkToTeam.Any(id => league.OWPLeagueId == id);
        }

        return leagueInfoResponses.ToList();
    }
}
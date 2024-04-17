

namespace OWP.ProviderVerification;

public interface ProviderVerificationRepository
{
    Task<IEnumerable<ProviderVerificationModel>> GetProviderVerification(CriteriaBase criteria);

    Task<IEnumerable<AgentOption>> GetAgentOptions();

    Task SaveAgentConfirm(int owpMatchId, string agentSelected, int actorId);

    Task SaveEstNote(int owpMatchId, string estNote);

    Task SaveEstAgent(int owpMatchId, string estAgent);
}

public class ProviderVerificationRepositoryImp : ProviderVerificationRepository
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSession session;

    public ProviderVerificationRepositoryImp(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task<IEnumerable<ProviderVerificationModel>> GetProviderVerification(CriteriaBase criteria)
    {
        var result = await dynamicRepository.FetchMultipleAsync(criteria,
            new[]
            {
                typeof(EventModel),
                typeof(AgentConfirmModel),
                typeof(MatchSetting)
            });
        var matchInfo = result[0].Cast<EventModel>().ToList();
        var agentConfirms = result[1].Cast<AgentConfirmModel>().ToList();
        var matchSettings = result[2].Cast<MatchSetting>().ToList();

        return await MappingInfo(matchInfo, agentConfirms, matchSettings);
    }

    private async Task<IList<ProviderVerificationModel>> MappingInfo(IList<EventModel> matches,
        IList<AgentConfirmModel> agentConfirms, IList<MatchSetting> matchSettings)
    {

        var owpIds = string.Join(",", matches.Select(x => x.OWPMatchId));
        var hasTicketModel = (await dynamicRepository.FetchAsync<HasTicketModel>(new DetectEventHasTicketCriteria
        {
            OWPMatchIds = owpIds,
            ActorId = session.UserId
        })).ToList();

        var result = new List<ProviderVerificationModel>();

        foreach (var match in matches)
        {
            var providerInfo = new ProviderVerificationModel
            {
                OWPMatchId = match.OWPMatchId,
                MatchNo = match.MatchNo,
                MatchStatus = match.MatchStatus,
                EventDateTime = match.EventDateTime,
                KoTime = match.KoTime,
                League = match.League,
                Home = match.Home,
                Away = match.Away,
                OtherProvider = match.OtherProvider,
                Provider = match.Provider,
                AgentEst = match.AgentEst,
                RemarkEst = match.RemarkEst,
                RemarkBy = match.RemarkBy,
                EstNote = match.EstNote,
                HandicapSetting = GetMatchSettings(matchSettings, match.OWPMatchId, (int)MajorBetType.Handicap),
                OverUnderSetting = GetMatchSettings(matchSettings, match.OWPMatchId, (int)MajorBetType.OverUnder),
                AgentConfirm = agentConfirms.Where(x => x.OwpMatchId == match.OWPMatchId).Select(y =>
                    new AgentConfirmOption
                    {
                        Label = y.AgentName,
                        Value = y.AgentId
                    }).ToList()
            };
            var ticket = hasTicketModel.Find(x => x.ParentMatchId == match.OWPMatchId);
            if (ticket is not null)
            {
                providerInfo.HasTicket = ticket.HasTicket;
                providerInfo.IsRead = ticket.IsRead;
            }

            result.Add(providerInfo);
        }

        return result;
    }

    private static MatchSetting GetMatchSettings(IList<MatchSetting> matchSettings, int owpMatchId, int bettype)
    {
        return matchSettings.FirstOrDefault(x => x.OWPMatchId == owpMatchId && x.Bettype == bettype);
    }

    public async Task<IEnumerable<AgentOption>> GetAgentOptions()
    => await dynamicRepository.FetchAsync<AgentOption>(new GetAgentOptionsCriteria());

    public async Task SaveAgentConfirm(int owpMatchId, string agentSelected, int actorId)
        => await dynamicRepository.ExecuteAsync(new SaveAgentConfirmCriteria
        {
            AgentSelected = agentSelected,
            OwpMatchId = owpMatchId,
            ActorId = actorId
        });

    public async Task SaveEstNote(int owpMatchId, string estNote)
        => await dynamicRepository.ExecuteAsync(new SaveEstNoteCriteria
        {
            EstNote = estNote,
            OwpMatchId = owpMatchId
        });

    public async Task SaveEstAgent(int owpMatchId, string estAgent)
        => await dynamicRepository.ExecuteAsync(new SaveEstAgentCriteria
        {
            EstAgent = estAgent,
            OwpMatchId = owpMatchId,
            ActorId = session.UserId
        });
}

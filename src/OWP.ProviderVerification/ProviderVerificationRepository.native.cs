namespace OWP.ProviderVerification;

public interface ProviderVerificationRepository
{
    Task<IEnumerable<ProviderVerificationModel>> GetProviderVerification(CriteriaBase criteria);

    Task<IEnumerable<ProviderVerificationDifferenceModel>> GetProviderVerificationDiff(CriteriaBase criteria);

    Task<IEnumerable<AgentOption>> GetAgentOptions();

    Task<IEnumerable<LSTBookProviderOption>> GetLSTBookOptions(DateTime eventDate);

    Task SaveAgentConfirm(int owpMatchId, string agentSelected, int actorId);

    Task SaveRemark(int owpMatchId, string remark);

    Task SaveRemarkBy(int owpMatchId, string remarkBy);

    Task UpdateIsMismatch( string owpMatchIds, DateTime date);
}

public class ProviderVerificationRepositoryImp(IDynamicRepository dynamicRepository, INapSession session) : ProviderVerificationRepository
{
    public async Task<IEnumerable<ProviderVerificationModel>> GetProviderVerification(CriteriaBase criteria)
    {
        return await FetchAndMapAsync(criteria, info => info);
    }

    public async Task<IEnumerable<ProviderVerificationDifferenceModel>> GetProviderVerificationDiff(CriteriaBase criteria)
    {
        return await FetchAndMapAsync(criteria, info => new ProviderVerificationDifferenceModel
        {
            OWPMatchId = info.OWPMatchId,
            MatchNo = info.MatchNo,
            MatchStatus = info.MatchStatus,
            EventDateTime = info.EventDateTime,
            KoTime = info.KoTime,
            League = info.League,
            Home = info.Home,
            Away = info.Away,
            LstBookProvider = info.LstBookProvider,
            InHouseProviderId = info.InHouseProviderId,
            Remark = info.Remark,
            RemarkBy = info.RemarkBy,
            AgentConfirm = info.AgentConfirm,
            ProviderMatch = info.ProviderMatch,
            HasTicket = info.HasTicket,
            IsRead = info.IsRead
        });
    }

    private async Task<IList<T>> FetchAndMapAsync<T>(CriteriaBase criteria, Func<ProviderVerificationModel, T> transform)
    {
        var result = await dynamicRepository.FetchMultipleAsync<EventModel, AgentConfirmModel, ProviderMatchModel>(criteria);
        var matches = result.Item1.ToList();
        var agentConfirms = result.Item2.ToList();
        var providerMatches = result.Item3.ToList();

        var owpIds = string.Join(",", matches.Select(x => x.OWPMatchId));
        var hasTicketModel = (await dynamicRepository.FetchAsync<HasTicketModel>(new DetectEventHasTicketCriteria
        {
            OWPMatchIds = owpIds,
            ActorId = session.UserId
        })).ToList();

        return matches.Select(match =>
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
                LstBookProvider = match.LstBookProvider,
                InHouseProviderId = string.Equals(match.MatchStatus, "cancel", StringComparison.OrdinalIgnoreCase)
                    ? null
                    : string.Join("/", providerMatches
                        .Where(p => p.OWPMatchId == match.OWPMatchId)
                        .Select(p => p.ProviderMatchId)),
                Remark = match.Remark,
                RemarkBy = match.RemarkBy,
                AgentConfirm = agentConfirms
                    .Where(x => x.OwpMatchId == match.OWPMatchId)
                    .Select(y => new AgentConfirmOption
                    {
                        Label = y.AgentName,
                        Value = y.AgentId
                    }).DistinctBy(a => a.Value).ToList(),
                ProviderMatch = providerMatches.Where(x => x.OWPMatchId == match.OWPMatchId).ToList(),
                OtherProvider = match.OtherProvider,
                MaxBet = match.MaxBet,
                MaxBetToDisplay = match.MaxBetToDisplay
            };

            var ticket = hasTicketModel.Find(x => x.ParentMatchId == match.OWPMatchId);
            if (ticket is not null)
            {
                providerInfo.HasTicket = ticket.HasTicket;
                providerInfo.IsRead = ticket.IsRead;
            }

            return transform(providerInfo);
        }).ToList();
    }

    public async Task<IEnumerable<AgentOption>> GetAgentOptions()
    => await dynamicRepository.FetchAsync<AgentOption>(new GetAgentOptionsCriteria());

    public async Task<IEnumerable<LSTBookProviderOption>> GetLSTBookOptions(DateTime eventDate)
        => await dynamicRepository.FetchAsync<LSTBookProviderOption>(new GetLSTBookOptionsCriteria
            {
                EventDate = eventDate
            });

    public async Task SaveAgentConfirm(int owpMatchId, string agentSelected, int actorId)
        => await dynamicRepository.ExecuteAsync(new SaveAgentConfirmCriteria
        {
            AgentSelected = agentSelected,
            OwpMatchId = owpMatchId,
            ActorId = actorId
        });

    public async Task SaveRemark(int owpMatchId, string remark)
        => await dynamicRepository.ExecuteAsync(new SaveRemarkCriteria
        {
            Remark = remark,
            OwpMatchId = owpMatchId,
            ActorId = session.UserId
        });

    public async Task SaveRemarkBy(int owpMatchId, string remarkBy)
        => await dynamicRepository.ExecuteAsync(new SaveRemarkByCriteria
        {
            RemarkBy = remarkBy,
            OwpMatchId = owpMatchId,
            ActorId = session.UserId
        });

    public async Task UpdateIsMismatch( string owpMatchIds, DateTime date)
        => await dynamicRepository.ExecuteAsync(new UpdateIsMismatchCriteria()
        {
            EventDate = date,
            OwpMatchIds = owpMatchIds,
            ActorId = session.UserId
        });
}

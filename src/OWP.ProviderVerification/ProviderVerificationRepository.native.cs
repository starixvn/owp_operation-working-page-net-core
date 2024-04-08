using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using OWP.ProviderVerification.Criteria;
using OWP.ProviderVerification.Models;

namespace OWP.ProviderVerification;

public interface ProviderVerificationRepository
{
    Task<IEnumerable<ProviderVerificationModel>> GetProviderVerification(CriteriaBase criteria);

    Task<IEnumerable<AgentOption>> GetAgentOptions();
}

public class ProviderVerificationRepositoryImp: ProviderVerificationRepository
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
                typeof(ProviderVerificationModel),
                typeof(AgentConfirmModel)
            });
        var matchInfo = result[0].Cast<ProviderVerificationModel>().ToList();
        var agentConfirm = result[1].Cast<AgentConfirmModel>().ToList();

        var owpIds = string.Join(",", matchInfo.Select(x => x.OwpMatchId));
        var hasTicketModel = (await dynamicRepository.FetchAsync<HasTicketModel>(new DetectEventHasTicketCriteria
        {
            OWPMatchIds = owpIds,
            ActorId = session.UserId
        })).ToList();

        foreach (var match in matchInfo)
        {
            var ticket = hasTicketModel.FirstOrDefault(x => x.ParentMatchId == match.ParentMatchId);

            if (ticket is not null)
            {
                match.HasTicket = ticket.HasTicket;
                match.IsRead = ticket.IsRead;
            }
            match.AgentConfirm = agentConfirm.Where(x => x.OwpMatchId == match.OwpMatchId).Select(y =>
                new AgentConfirmOption
                {
                    Label = y.AgentName,
                    Value = y.AgentId
                }).ToList();
        }

        return matchInfo;
    }

    public async Task<IEnumerable<AgentOption>> GetAgentOptions()
    => await dynamicRepository.FetchAsync<AgentOption>(new GetAgentOptionsCriteria());
}

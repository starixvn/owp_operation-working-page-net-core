namespace OWP.ProviderVerification.Agents.QueryAgents
{
    public record CreateAgentEstRequest(int OwpMatchId, string EstAgent) : Command<Unit>;

    internal class CreateAgentEstHandler(ProviderVerificationRepository repository) : CommandHandler<CreateAgentEstRequest, Unit>
    {
        public async Task<Unit> Handle(CreateAgentEstRequest request, CancellationToken cancellationToken)
        {
            await repository.SaveEstAgent(request.OwpMatchId, request.EstAgent);
            return Unit.Value;
        }
    }
}
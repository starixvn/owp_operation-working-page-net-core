namespace OWP.NewLeagueSetting.Command;

public record RemoveLeagueSettingRequest(int LeagueSettingId) : Command<Unit>;

public class RemoveLeagueSettingHandler(Repository repository, INapSession session) : CommandHandler<RemoveLeagueSettingRequest, Unit>
{
    
    public async Task<Unit> Handle(RemoveLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        await repository.ExecuteNonQueryCommand(new RemoveSettingCriteria()
        {
            LeagueSettingId = request.LeagueSettingId,
            ActorId= session.UserId
        });
        return Unit.Value;
    }
}

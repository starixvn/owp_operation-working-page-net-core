using Fanex.NAP.Client;
using OWP.EventPicking.Api.Helper;

namespace OWP.EventPicking.Api.Handlers;

public record RemoveSpecialProductRequest(RemoveSpecialProduct RemoveInfo) : Command<Unit>;

public class RemoveSpecialProductHandler(INapSession session, Repository repository) : CommandHandler<RemoveSpecialProductRequest, Unit>
{
    public async Task<Unit> Handle(RemoveSpecialProductRequest request, CancellationToken cancellationToken)
    {
        await repository.ExecuteNonQuery(new RemoveSpecialProductCriteria
        {
            ActorId = session.UserId,
            ParentId = request.RemoveInfo.ParentId,
            MinorBettypeId = request.RemoveInfo.MinorBettypeId,
        });

        var specialProductIds = request.RemoveInfo.MinorBettypeId
            .Split(',')
            .Select(int.Parse)
            .ToList();
        await WriteLogWhenRemoveSpecialProduct(specialProductIds, int.Parse(request.RemoveInfo.ParentId), cancellationToken);

        return Unit.Value;
    }

    private async Task WriteLogWhenRemoveSpecialProduct(List<int> specialProductIds, int parentId, CancellationToken cancellationToken = default)
    {
        var logData = new List<LoggedChangeModel>();
        foreach (var sp in specialProductIds)
        {
            var teamPostfix = SpecialProductHelper.GetCleanedTeamPostfix(sp);
            var action = $"Delete the product <b>{teamPostfix}</b>";
            logData.Add(SpecialProductHelper.CreateLoggedChangeModel(
                parentId,
                session.UserName,
                action,
                null,
                null));
        }

        await SpecialProductHelper.SaveEventLogAsync(repository, logData, cancellationToken);
    }
}
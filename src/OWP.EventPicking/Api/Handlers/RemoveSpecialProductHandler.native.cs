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
        
        var parentIds = request.RemoveInfo.ParentId
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x.Trim()))
            .ToList();
        
        await WriteLogWhenRemoveSpecialProduct(specialProductIds, parentIds, cancellationToken);

        return Unit.Value;
    }

    private async Task WriteLogWhenRemoveSpecialProduct(List<int> specialProductIds, List<int> parentIds, CancellationToken cancellationToken = default)
    {
        var logData = new List<LoggedChangeModel>();
        foreach (var parentId in parentIds)
        {
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
        }

        await SpecialProductHelper.SaveEventLogAsync(repository, logData, cancellationToken);
    }
}
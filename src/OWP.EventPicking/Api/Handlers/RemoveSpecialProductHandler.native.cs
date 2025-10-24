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
        await WriteLogWhenRemoveSpecialProduct(specialProductIds, int.Parse(request.RemoveInfo.ParentId));

        return Unit.Value;
    }

    private async Task WriteLogWhenRemoveSpecialProduct(List<int> specialProductIds, int parentId)
    {
        var logData = new List<LoggedChangeModel>();
        foreach (var sp in specialProductIds)
        {
            var (_, teamPostfix) = SpecialProductHelper.BuildPostFixSpecialProduct(sp);
            if (!string.IsNullOrEmpty(teamPostfix))
            {
                teamPostfix = teamPostfix.TrimStart('-', ' ').Trim();
            }
            var action = $"Delete the product <b>{teamPostfix}</b>";
            logData.Add(new LoggedChangeModel
            {
                InfoSiteMatchId = parentId,
                UpdateTypeId = null,
                ModifiedBy = session.UserName,
                Action = action,
                OldValue = null,
                NewValue = null,
                UpDateTime = DateTime.Now.ToGmt8(),
            });
        }

        await SaveEventLogAsync(logData);
    }

    private async Task SaveEventLogAsync(List<LoggedChangeModel> logData)
    {
        if (logData == null)
        {
            return;
        }

        var xml = XmlConverter.Serialize(logData);
        var criteria = new SaveEditEventLogsCriteria { Xml = xml };
        await repository.SaveEventLog(criteria).ConfigureAwait(false);
    }
}
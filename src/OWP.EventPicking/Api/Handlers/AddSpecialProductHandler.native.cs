using Fanex.NAP.Client;
using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Services;

namespace OWP.EventPicking.Api.Handlers;

public record AddSpecialProductRequest(AddSpecialProduct AddInfo) : Command<Unit>;

public class AddSpecialProductHandler(
    INapSession session,
    Repository repository,
    BuildChildMatchMaxBet childMatchMaxBet) : CommandHandler<AddSpecialProductRequest, Unit>
{
    public async Task<Unit> Handle(AddSpecialProductRequest request, CancellationToken cancellationToken)
    {
        var specialProducts = GenerateSpecialProducts(request.AddInfo.ParentId, request.AddInfo.MinorBettypeId);

        if(specialProducts.Count == 0)
        {
            return Unit.Value;
        }

        var mainEvent = await repository.QueryMainMatch(request.AddInfo.ParentId);

        await childMatchMaxBet.BuildMBForAddSpecialProduct(specialProducts, mainEvent);
        await repository.ExecuteNonQuery(new AddSpecialProductCriteria
        {
            ActorId = session.UserId,
            ChildMatches =  XmlConverter.Serialize(specialProducts),
        });
        await WriteLogWhenAddSpecialProduct(specialProducts);
        return Unit.Value;
    }

    private async Task WriteLogWhenAddSpecialProduct(List<SpecialProductMaxbetSpread> specialProducts)
    {
        var logData = new List<LoggedChangeModel>();
        foreach (var sp in specialProducts)
        {
            var (_, teamPostfix) = SpecialProductHelper.BuildPostFixSpecialProduct(sp.MinorBettypeId);
            if (!string.IsNullOrEmpty(teamPostfix))
            {
                teamPostfix = teamPostfix.TrimStart('-', ' ').Trim();
            }
            var action = $"Add a new product <b>{teamPostfix}</b>";
            logData.Add(new LoggedChangeModel
            {
                InfoSiteMatchId = sp.ParentId,
                UpdateTypeId = null,
                ModifiedBy = session.UserName,
                Action = action,
                OldValue = "Initialize",
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

    private static List<SpecialProductMaxbetSpread> GenerateSpecialProducts(string parentMatchIds, string minorBettypeIds)
    {
        var parentIds = parentMatchIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x.Trim()))
            .Distinct()
            .ToList();

        var minorIds = minorBettypeIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x.Trim()))
            .Distinct()
            .ToList();

        var specialProducts = new List<SpecialProductMaxbetSpread>();

        foreach (var parentId in parentIds)
        {
            foreach (var minorId in minorIds)
            {
                specialProducts.Add(new SpecialProductMaxbetSpread
                {
                    ParentId = parentId,
                    MinorBettypeId = minorId,
                    MaxbetHdp = null,
                    MaxbetOu = null,
                    SpreadHdp = null,
                    SpreadOu = null,
                    NoOfHDP = null
                });
            }
        }

        return specialProducts;
    }
}
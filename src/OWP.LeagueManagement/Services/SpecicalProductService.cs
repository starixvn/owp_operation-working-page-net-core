namespace OWP.LeagueManagement.Service;

using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Fanex.NAP.Client.Helpers.Web;
public interface SpecialProductService
{
    Task<IList<LogDisplayModel>> BuildLog(QueryLogModel model);

    Task<ProductInfo> UpdateSpecialProduct(SpecialProduct model);
}
public class SpecialProductServiceImp : SpecialProductService
{
    private readonly Repository repository;
    private readonly INapSession session;

    public SpecialProductServiceImp(Repository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    public async Task<IList<LogDisplayModel>> BuildLog(QueryLogModel model)
    {
        var userLog = await repository.SearchUserLog(model);
        return LogBuilder.Build(userLog);
    }

    public async Task<ProductInfo> UpdateSpecialProduct(SpecialProduct model)
    {
        model.ActorId = session.UserId;
        return await repository.UpdateSpecialProduct(model);
    }
}
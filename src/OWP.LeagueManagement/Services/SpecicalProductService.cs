namespace OWP.LeagueManagement.Service;

using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Fanex.NAP.Client.Helpers.Web;
using AutoMapper;
using Fanex.Data.Repository;
using Criteria;
public interface SpecialProductService
{
    Task<IList<LogDisplayModel>> BuildLog(QueryLogModel model);

    Task<ProductInfo> UpdateSpecialProduct(SpecialProduct model);
}
public class SpecialProductServiceImp : SpecialProductService
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession session;
    private readonly IMapper mapper;

    public SpecialProductServiceImp(IDynamicRepository dynamicRepository, INapSession session, IMapper mapper)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
        this.mapper = mapper;
    }

    public async Task<IList<LogDisplayModel>> BuildLog(QueryLogModel model)
    {
        var criteria = new SpecialProductGetLogCriteria
        {
            ObjectId = model.ObjectId,
            PageSize = model.PageSize
        };
        var userLog = (await dynamicRepository.FetchAsync<UserLogModel>(criteria)).ToList();
        return LogBuilder.Build(userLog);
    }

    public async Task<ProductInfo> UpdateSpecialProduct(SpecialProduct model)
    {
        model.ActorId = session.UserId;
        var criteria = mapper.Map<UpdateSpecialProductCriteria>(model);
        return (await dynamicRepository.FetchAsync<ProductInfo>(criteria)).FirstOrDefault();
    }
}
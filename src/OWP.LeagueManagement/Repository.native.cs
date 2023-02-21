using AutoMapper;
using Fanex.Data.Repository;
using OWP.LeagueManagement.Criteria;
using OWP.LeagueManagement.Models;
using OWP.Models;

namespace OWP.LeagueManagement;

public interface Repository
{
    Task<ResultDataObject> UpdateTVList(UpdateTvListCriteria criteria);

    Task<ResultDataObject> UpdateCondition(UpdateConditionCriteria criteria);

    Task<string> GetCondition(GetConditionCriteria criteria);

    Task<List<UserLogModel>> SearchUserLog(QueryLogModel model);

    Task<ProductInfo> UpdateSpecialProduct(SpecialProduct model);

    Task<IList<LeagueInfoResponse>> GetAllLeague(LeagueInfoQuery query);

    Task<IList<int>> GetLeagueLinkToTeam(string leagueIdString);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly IMapper mapper;

    public RepositoryImp(
        IMapper mapper,
        IDynamicRepository dynamicRepository)
    {
        this.mapper = mapper;
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<ResultDataObject> UpdateTVList(UpdateTvListCriteria criteria)
    {
        await dynamicRepository.FetchAsync<ResultDataObject>(criteria);
        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg
        };
    }

    public async Task<ResultDataObject> UpdateCondition(UpdateConditionCriteria criteria)
    {
        await dynamicRepository.FetchAsync<ResultDataObject>(criteria);
        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg
        };
    }

    public async Task<string> GetCondition(GetConditionCriteria criteria)
    {
        var result = await dynamicRepository.FetchAsync<string>(criteria);
        return result?.FirstOrDefault();
    }

    public async Task<List<UserLogModel>> SearchUserLog(QueryLogModel model)
    {
        var criteria = new SpecialProductGetLogCriteria
        {
            ObjectId = model.ObjectId,
            PageSize = model.PageSize
        };
        return (await dynamicRepository.FetchAsync<UserLogModel>(criteria)).ToList();
    }

    public async Task<ProductInfo> UpdateSpecialProduct(SpecialProduct model)
    {
        var criteria = mapper.Map<UpdateSpecialProductCriteria>(model);
        return (await dynamicRepository.FetchAsync<ProductInfo>(criteria)).FirstOrDefault();
    }

    public async Task<IList<int>> GetLeagueLinkToTeam(string leagueIdString)
    {
        return (await dynamicRepository.FetchAsync<int>(new GetLinkLeagueAndTeamCriteria
        {
            LeagueIdString = leagueIdString
        })).ToList();
    }


    public async Task<IList<LeagueInfoResponse>> GetAllLeague(LeagueInfoQuery query)
    {
        var criteria = mapper.Map<GetAllLeagueInfoCriteria>(query);
        return (await dynamicRepository.FetchAsync<LeagueInfoResponse>(criteria)).ToList();
    }
}
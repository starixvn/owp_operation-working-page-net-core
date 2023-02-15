namespace OWP.LiveDetail.MatchOdd;

using System.Xml.Linq;
using AutoMapper;
using Extensions;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;

public interface MatchOddRepository
{
    Task<IEnumerable<MatchOdd>> Query(QueryModel model);

    Task<CreateResult> Create(CreateModel model);

    Task<ResponseResult> Update(UpdateModel model);

    Task<ResponseResult> Delete(DeleteModel model);
}

public class MatchOddRepositoryImp : MatchOddRepository
{
    private readonly IMapper mapper;
    private readonly IDynamicRepository repository;
    private readonly INapSession session;

    public MatchOddRepositoryImp(IMapper mapper, IDynamicRepository repository, INapSession session)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.session = session;
    }

    public async Task<IEnumerable<MatchOdd>> Query(QueryModel model)
    {
        var criteria = mapper.Map<QueryCriteria>(model);

        criteria.ThrowExceptionIfInvalid();
        return await repository.FetchAsync<MatchOdd>(criteria);
    }

    public async Task<ResponseResult> Update(UpdateModel model)
    {
        var criteria = mapper.Map<UpdateCriteria>(model);
        criteria.RecommendOdds = model.RecommendOdds.ToXML().ToString(SaveOptions.DisableFormatting);
        criteria.UserId = session.UserId;

        criteria.ThrowExceptionIfInvalid();
        var result = await repository.ExecuteCommandAsync(criteria);

        var output = new ResponseResult
        {
            Code = result.Code,
            Message = result.Message,
            SessionId = criteria.SessionId
        };

        return await Task.FromResult(output);
    }

    public async Task<CreateResult> Create(CreateModel model)
    {
        var criteria = mapper.Map<CreateCriteria>(model);
        criteria.RecommendOdds = model.RecommendOdds.ToXML().ToString(SaveOptions.DisableFormatting);
        criteria.UserId = session.UserId;

        criteria.ThrowExceptionIfInvalid();
        var result = await repository.ExecuteCommandAsync(criteria);

        var output = new CreateResult
        {
            Id = criteria.Id,
            Code = result.Code,
            Message = result.Message,
            SessionId = criteria.SessionId
        };
        return await Task.FromResult(output);
    }

    public async Task<ResponseResult> Delete(DeleteModel model)
    {
        var criteria = new DeleteCriteria
        {
            Id = model.Id,
            UserId = session.UserId
        };
        criteria.ThrowExceptionIfInvalid();
        await repository.ExecuteAsync(criteria);
        var output = new ResponseResult
        {
            Code = 0,
            Message = "Ok",
            SessionId = criteria.SessionId
        };

        return await Task.FromResult(output);
    }
}
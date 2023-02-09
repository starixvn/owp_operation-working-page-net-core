namespace OWP.LiveDetail.Assignment;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using OWP.Constants;
using OWP.Extensions;
using Utils;

public interface AssignmentRepository
{
    Task<IEnumerable<Assignment>> Query(QueryModel model);

    Task<ValidateResult> Validate(ValidateModel model);

    Task<CommandResult> Create(CreateModel model);

    Task<CommandResult> Update(UpdateModel model);

    Task<CommandResult> Delete(DeleteModel model);
}

public class AssignmentRepositoryImp : AssignmentRepository
{
    private readonly IMapper mapper;
    private readonly IDynamicRepository repository;
    private readonly INapSession session;

    public AssignmentRepositoryImp(IMapper mapper, IDynamicRepository repository, INapSession session)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.session = session;
    }

    public async Task<IEnumerable<Assignment>> Query(QueryModel model)
    {
        var criteria = mapper.Map<QueryCriteria>(model);
        criteria.ThrowExceptionIfInvalid();
        return await repository.FetchAsync<Assignment>(criteria);
    }

    public async Task<ValidateResult> Validate(ValidateModel model)
    {
        var criteria = mapper.Map<ValidateCriteria>(model);
        criteria.IsValidateAssignment = true;
        criteria.ThrowExceptionIfInvalid();
        var results = await repository.FetchAsync<ValidateEntity>(criteria);

        return new ValidateResult
        {
            Results = results.Where(x => x.ResultType != (int)RuleReturnType.Passed)
        };
    }

    public async Task<CommandResult> Create(CreateModel model)
    {
        var criteria = mapper.Map<CreateCriteria>(model);
        criteria.ActorId = session.UserId;
        criteria.ThrowExceptionIfInvalid();
        await repository.ExecuteAsync(criteria);
        return new CommandResult
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            AssignmentId = criteria.ErrCode == 0 ? Converter.ToInt(criteria.ErrMsg) : 0,
            SessionId = criteria.SessionId
        };
    }

    public async Task<CommandResult> Update(UpdateModel model)
    {
        var criteria = mapper.Map<UpdateCriteria>(model);
        criteria.ActorId = session.UserId;

        criteria.ThrowExceptionIfInvalid();
        await repository.ExecuteAsync(criteria);

        return new CommandResult
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            SessionId = criteria.SessionId
        };
    }

    public async Task<CommandResult> Delete(DeleteModel model)
    {
        var criteria = mapper.Map<DeleteCriteria>(model);
        criteria.ActorId = session.UserId;
        criteria.ThrowExceptionIfInvalid();
        await repository.ExecuteAsync(criteria);

        return new CommandResult
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            SessionId = criteria.SessionId
        };
    }
}
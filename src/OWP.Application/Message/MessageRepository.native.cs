
using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;

namespace OWP.Application.Message;
public interface MessageRepository
{
    Task<QueryResult> Query(QueryModel model);

    Task UpdateIsViewed(UpdateIsViewedModel model);
}

public class MessageRepositoryImp : MessageRepository
{
    private readonly IMapper mapper;
    private readonly IDynamicRepository repository;
    private readonly INapSession session;

    public MessageRepositoryImp(IMapper mapper, IDynamicRepository repository, INapSession session)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.session = session;
    }

    public async Task<QueryResult> Query(QueryModel model)
    {
        model.UserId = session.UserId;
        var criteria = mapper.Map<QueryCriteria>(model);

        var messages = await repository.FetchAsync<MessageModel>(criteria);
        return new QueryResult
        {
            Messages = messages,
            TotalNotViewMessage = criteria.TotalNotViewMessage
        };
    }

    public async Task UpdateIsViewed(UpdateIsViewedModel model)
    {
        var criteria = new UpdateCriteria
        {
            UserId = session.UserId,
            IsWatcher = model.IsWatcher
        };

        await repository.ExecuteAsync(criteria);
    }
}
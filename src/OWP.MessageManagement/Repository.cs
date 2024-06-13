using AutoMapper;
using Fanex.Data.Repository;
using OWP.MessageManagement.Criteria;
using OWP.MessageManagement.Models;
using System.Linq;
using System.Threading.Tasks;
using Fanex.NAP.Client.Helpers.Web;

namespace OWP.MessageManagement;
public interface Repository
{
    Task<MessageBoxViewModel> GetByUserIdWithPaging(MessageBoxQueryParam param);

    Task UpdateReadStatus(MessageBoxUpdateParam model);
}

public class RepositoryImp : Repository
{
    private readonly IMapper mapper;
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;


    public RepositoryImp(IMapper mapper, IDynamicRepository dynamicRepository, INapSession currentSession)
    {
        this.mapper = mapper;
        this.dynamicRepository = dynamicRepository;
        this.currentSession = currentSession;
    }

    public async Task<MessageBoxViewModel> GetByUserIdWithPaging(MessageBoxQueryParam param)
    {
        var criteria = mapper.Map<MessageBoxQueryCriteria>(param);
        criteria.UserId = currentSession.UserId;
        var messages = await dynamicRepository.FetchAsync<MessageModel>(criteria);
        return new MessageBoxViewModel
        {
            Messages = messages.ToList()
        };
    }

    public async Task UpdateReadStatus(MessageBoxUpdateParam model)
    {
        var criteria = new MessageBoxUpdateCriteria
        {
            UserId = currentSession.UserId,
            MessageId = model.MessageId
        };

        await dynamicRepository.ExecuteAsync(criteria);
    }
}

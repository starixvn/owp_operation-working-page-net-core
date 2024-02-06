using Fanex.Data.Repository;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Notification;

public interface NotificationRepository
{
    Task<int> GetTicketInfoOfPreMatch(int ticketId, bool isTicketLeague);

    Task<List<T>> QueryPrematchTicket<T>(CriteriaBase criteria) where T : class;

}

public class NotificationRepositoryImp : NotificationRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public NotificationRepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<List<T>> QueryPrematchTicket<T>(CriteriaBase criteria) where T : class
    => (await dynamicRepository.FetchAsync<T>(criteria)).ToList();


    public async Task<int> GetTicketInfoOfPreMatch(int ticketId, bool isTicketLeague)
    {
        var criteria = new GetInfoTicketPrematchCriteria { TicketId = ticketId, IsTicketLeague = isTicketLeague };

        return (await dynamicRepository.FetchAsync<int>(criteria)).FirstOrDefault();
    }
}

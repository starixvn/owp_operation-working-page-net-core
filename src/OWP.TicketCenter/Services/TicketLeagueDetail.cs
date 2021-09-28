using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Services
{
    public interface TicketLeagueDetail
    {
        Task<TicketLeagueDetailViewModel> GetTicketLeagueDetail(int ticketId);
    }

    public class TicketLeagueDetailImp: TicketLeagueDetail
    {
        private readonly Repository repository;

        public TicketLeagueDetailImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<TicketLeagueDetailViewModel> GetTicketLeagueDetail(int ticketId)
        {
            var await repository.GetTicketLeagueDetail(ticketId);
        }
    }
}

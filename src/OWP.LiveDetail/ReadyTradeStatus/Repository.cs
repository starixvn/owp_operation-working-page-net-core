namespace OWP.LiveDetail.ReadyTradeStatus
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Helpers.Web;
    using OWP.Extensions;

    public interface TraderStatusRepository
    {
        Task<IEnumerable<DateTime>> InsertReadyTradeStatus(int owpMatchId);

    }
    public class TraderStatusRepositoryImp : TraderStatusRepository
    {
        private readonly IDynamicRepository repository;

        private readonly INapSession currentSession;

        public TraderStatusRepositoryImp(IDynamicRepository repository, INapSession currentSession)
        {
            this.repository = repository;
            this.currentSession = currentSession;
        }

        public async Task<IEnumerable<DateTime>> InsertReadyTradeStatus(int owpMatchId)
        {
            var criteria = new InsertReadyTradeStatusCommand
            {
                OWPMatchId = owpMatchId,
                UserId = currentSession.UserId
            };

            return await repository.FetchAsync<DateTime>(criteria);
        }
    }
}

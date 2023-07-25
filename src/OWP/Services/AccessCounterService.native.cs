using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using OWP.Criteria;

namespace OWP.Services
{
    public interface AccessCounter
    {
        Task UpdateCounter(string function);
    }

    public class AccessCounterImp : AccessCounter
    {
        private readonly INapSession currentSession;

        private readonly IDynamicRepository dynamicRepository;

        public AccessCounterImp(INapSession currentSession, IDynamicRepository dynamicRepository)
        {
            this.currentSession = currentSession;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task UpdateCounter(string function) =>
            await dynamicRepository.ExecuteAsync(new UpdateAccessCounterCriteria
            {
                ActorId = currentSession.UserId,
                FunctionName = function
            });
    }
}

namespace OWP.OtherSport.Validation.Services
{
    using Fanex.Data.Repository;
    using Newtonsoft.Json;
    using OWP.OtherSport.Validation.Criteria;
    using OWP.OtherSport.Validation.Models;

    public interface Repository
    {
        Task<List<Trader>> GetTraders(List<string> traderNames);

        Task<IEnumerable<OSMatchInfo>> ExistsOSMatch(List<OSMatchInfo> matchInfos);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository repository;

        public RepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<OSMatchInfo>> ExistsOSMatch(List<OSMatchInfo> matchInfos)
        {
            var criteria = new ExistsOSMatchCriteria
            {
                OSMatches = JsonConvert.SerializeObject(matchInfos)
            };

            var result = await repository.FetchAsync<OSMatchInfo>(criteria);

            return result;
        }

        public async Task<List<Trader>> GetTraders(List<string> traderNames)
        {
            var traderNamesInString = string.Join(",", traderNames);
            if (string.IsNullOrEmpty(traderNamesInString))
            {
                return new List<Trader>();
            }

            var criteria = new TraderByUsernameQueryCriteria
            {
                Username = traderNamesInString
            };

            return (await repository.FetchAsync<Trader>(criteria)).ToList();
        }
    }
}
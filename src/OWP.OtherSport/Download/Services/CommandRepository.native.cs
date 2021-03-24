namespace OWP.OtherSport.Download.Services
{
    using Fanex.Data.Repository;
    using OWP.OtherSport.EventAssignment.Criteria;
    using OWP.OtherSport.Submit.Models;
    using System.Linq;
    using System.Threading.Tasks;

    public interface CommandRepository
    {
        Task<AssignmentSnapshot> Query(DownloadAssignmentRequest request);
    }

    public class CommandRepositoryImp: CommandRepository
    {
        private readonly IDynamicRepository dynamicRepository;

        public CommandRepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<AssignmentSnapshot> Query(DownloadAssignmentRequest request)
        {
            var criteria = new GetEventAssignmentCriteria
            {
                FromDate = request.From,
                ToDate = request.To,
                Sports = request.Sports,
                MatchStatuses = request.MatchStatuses
            };

            var (matches, assignments, maxbets) = await dynamicRepository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchMaxBet>(criteria);

            return new AssignmentSnapshot
            {
                Matches = matches.ToDictionary(m => m.OSMatchId),
                MatchAssignments = assignments.GroupBy(a => a.OSMatchId).ToDictionary(a => a.Key, a => a.ToList()),
                MaxBet = maxbets.GroupBy(a => a.OSMatchId).ToDictionary(a => a.Key, a => a.ToList())
            };
        }
    }
}

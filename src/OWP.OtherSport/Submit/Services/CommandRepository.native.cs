namespace OWP.OtherSport.Submit.Services
{
    using Fanex.Data.Repository;
    using OWP.OtherSport.Submit.Criteria;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface CommandRepository
    {
        Task<AssignmentSnapshot> Query(string eventDates);

        Task<string> GetOldMatchIds(string eventDateInString);

        Task Update(string oldMatchIds, List<AssignmentModel> newAssignments);
    }

    public class CommandRepositoryImp: CommandRepository
    {
        private readonly IDynamicRepository repository;

        public CommandRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<AssignmentSnapshot> Query(string eventDates)
        {
            return new AssignmentSnapshot();
        }

        public async Task<string> GetOldMatchIds(string eventDateInString)
        {
            var criteria = new GetMatchIdsByDateList
            {
                EventDates = eventDateInString
            };
            var matchIds = await repository.FetchAsync<List<int>>(criteria);

            return string.Join(',', matchIds);
        }

        public async Task Update(string oldMatchIds, List<AssignmentModel> newAssignments)
        {

        }
    }
}

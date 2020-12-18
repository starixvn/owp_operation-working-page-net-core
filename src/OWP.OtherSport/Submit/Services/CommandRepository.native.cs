namespace OWP.OtherSport.Submit.Services
{
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
        public async Task<AssignmentSnapshot> Query(string eventDates)
        {
            return new AssignmentSnapshot();
        }

        public async Task<string> GetOldMatchIds(string eventDateInString)
        {
            return string.Empty;
        }

        public async Task Update(string oldMatchIds, List<AssignmentModel> newAssignments)
        {
        }
    }
}

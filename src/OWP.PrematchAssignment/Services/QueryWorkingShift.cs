using Fanex.NAP.Client.Helpers.Web;

namespace OWP.PreMatchAssignment.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Web.Session;
    using Models;

    public interface QueryWorkingShift
    {
        Task<List<WorkingShift>> QueryWorkingShift(DateTime eventDate);
    }
    public class QueryWorkingShiftImp : QueryWorkingShift
    {
        private readonly IDynamicRepository dynamicRepository;

        private readonly INapSession currentSession;

        public QueryWorkingShiftImp(IDynamicRepository dynamicRepository, INapSession currentSession)
        {
            this.dynamicRepository = dynamicRepository;
            this.currentSession = currentSession;
        }

        private async Task<List<WorkingShiftModel>> GetWorkingShiftByDate(DateTime eventDate)
        {
            var criteria = new GetWorkingShiftByDateCriteria
            {
                EventDate = eventDate,
                UserId = currentSession.UserId
            };

            var result = (await dynamicRepository.FetchAsync<WorkingShiftModel>(criteria)).ToList();

            return result;
        }

        public async Task<List<WorkingShift>> QueryWorkingShift(DateTime eventDate)
        {
            var workingShiftByDate = await GetWorkingShiftByDate(eventDate);
            foreach (var shiftCode in workingShiftByDate)
            {
                shiftCode.RecalculateShiftCode();
            }
            var workingShifts = workingShiftByDate.GroupBy(a => a.ShiftCode).ToDictionary(a => a.Key, a => a.Select(b => b.NickName).ToList());

            var workingModel = new List<WorkingShift>();
            foreach (var workingShift in workingShifts)
            {
                var shift = new WorkingShift
                {
                    Shifts = workingShift.Key,
                    Usernames = string.Join(", ", workingShift.Value.Distinct())
                };
                workingModel.Add(shift);
            }

            return workingModel.OrderBy(x => x.Shifts).ToList();
        }
    }
}

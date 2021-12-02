using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface TraderUserService
    {
        Task<IList<SchedulingTraderInfo>> GetTradersInfo(DateTime eventDate);

        Task<bool> ExistsTraderValid(DateTime eventDate);
    }

    public class TraderUserServiceImp : TraderUserService
    {
        private readonly Repository repository;

        public TraderUserServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<SchedulingTraderInfo>> GetTradersInfo(DateTime eventDate)
        {
            var traders = await repository.GetTraders(eventDate);
            var napUserIds = await repository.GetUserInfoFromNAPUserControl();
            var db02CustIds = await repository.GetCustInfoFromDB02();
            var filteredUsers = traders.Item1.Where(x => napUserIds.Any(y => y == x.TraderId) && db02CustIds.Any(y => y == x.TraderId));
            MapMultipleGames(filteredUsers.ToList());

            var workingShifts = traders.Item2;

            return BuildTradersInfo(filteredUsers, workingShifts);
        }

        public IList<SchedulingTraderInfo> BuildTradersInfo(IEnumerable<SchedulingTraderInfo> users, IEnumerable<UserWorkingShift> workingShifts)
        {
            var userList = new List<SchedulingTraderInfo>();
            foreach (var user in users)
            {
                var userWorkingShifts = workingShifts.Where(x => x.UserId == user.TraderId);
                if (userWorkingShifts.Any())
                {
                    user.WorkingShifts = userWorkingShifts.Select(ws => new SchedulingWorkingShift
                    {
                        InTime = ws.ShiftStartTime.ToString(AutoConstant.FormatDateTime),
                        OutTime = ws.ShiftEndTime.ToString(AutoConstant.FormatDateTime)
                    }).ToArray();

                    userList.Add(user);
                }
            }

            return userList;
        }

        private static void MapMultipleGames(List<SchedulingTraderInfo> traders)
        {
            var maxGameDict = new Dictionary<string, int?>
            {
                {string.Empty, 2},
                {"1G", 1}
            };

            traders.ForEach(x => x.MaxGames = maxGameDict.GetValue(x.MultipleGames ?? string.Empty, null));
        }

        public async Task<bool> ExistsTraderValid(DateTime eventDate)
        => (await GetTradersInfo(eventDate)).Any();
    }
}
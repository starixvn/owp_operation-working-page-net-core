using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface SchedulingUserBuilderService
    {
        IEnumerable<SchedulingLAUser> BuildListSchedulingLAUser(ValueTuple<IEnumerable<LAUserInfo>, IEnumerable<UserWorkingShift>> userInfoTuple);

        IEnumerable<TraderInfo> BuildTradersInfo(IEnumerable<TraderInfo> users, IEnumerable<UserWorkingShift> workingShifts);

        Task<IEnumerable<TraderInfo>> GetTradersInfo(DateTime eventDate);

        Task<IEnumerable<SchedulingLAUser>> GetLAUsersInfo(DateTime eventDate);
    }

    public class SchedulingUserBuilderServiceImp : SchedulingUserBuilderService
    {
        private readonly Repository repository;

        public SchedulingUserBuilderServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<SchedulingLAUser> BuildListSchedulingLAUser(ValueTuple<IEnumerable<LAUserInfo>, IEnumerable<UserWorkingShift>> userInfoTuple)
        {
            var users = userInfoTuple.Item1;
            var workingShifts = userInfoTuple.Item2;
            var listSchedulingUser = new List<SchedulingLAUser>();
            foreach (var user in users)
            {
                var userWorkingShifts = workingShifts.Where(x => x.UserId == user.UserId);
                if (userWorkingShifts.Any())
                {
                    listSchedulingUser.Add(BuildSchedulingLAUser(user, userWorkingShifts));
                }
            }

            return listSchedulingUser;
        }

        public static SchedulingLAUser BuildSchedulingLAUser(LAUserInfo user, IEnumerable<UserWorkingShift> userWorkingShifts)
         => new SchedulingLAUser
         {
             UserId = user.UserId,
             Level = user.Level,
             Location = user.Location ?? "",
             SpeakEnglish = user.SpeakEnglish,
             SpeakChinese = user.SpeakChinese,
             WorkingShifts = userWorkingShifts.Any(u => u.IsShiftDeleted)
             ? new List<SchedulingWorkingShift>().ToArray()
             : userWorkingShifts.Select(ws => new SchedulingWorkingShift
             {
                 InTime = ws.ShiftStartTime.ToString(AutoConstant.FormatDateTime),
                 OutTime = ws.ShiftEndTime.ToString(AutoConstant.FormatDateTime)
             }).ToArray()
         };

        public async Task<IEnumerable<TraderInfo>> GetTradersInfo(DateTime eventDate)
        {
            var traders = await repository.GetTraders(eventDate);
            var napUserIds = await repository.GetUserInfoFromNAPUserControl();
            var db02CustIds = await repository.GetCustInfoFromDB02();
            var filteredUsers = traders.Item1.Where(x => napUserIds.Any(y => y == x.TraderId) && db02CustIds.Any(y => y == x.TraderId));
            MapMultipleGames(filteredUsers.ToList());

            var workingShifts = traders.Item2;

            return BuildTradersInfo(filteredUsers, workingShifts);
        }

        public async Task<IEnumerable<SchedulingLAUser>> GetLAUsersInfo(DateTime eventDate)
        {
            var lAUsers = await repository.GetLAInfoByDate(eventDate);
            return BuildListSchedulingLAUser(lAUsers);
        }

        public IEnumerable<TraderInfo> BuildTradersInfo(IEnumerable<TraderInfo> users, IEnumerable<UserWorkingShift> workingShifts)
        {
            var userList = new List<TraderInfo>();
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

        private static void MapMultipleGames(List<TraderInfo> traders)
        {
            var maxGameDict = new Dictionary<string, int?>
            {
                {string.Empty, 2},
                {"1G", 1}
            };

            traders.ForEach(x => x.MaxGames = maxGameDict.GetValue(x.MultipleGames ?? string.Empty, null));
        }
    }
}
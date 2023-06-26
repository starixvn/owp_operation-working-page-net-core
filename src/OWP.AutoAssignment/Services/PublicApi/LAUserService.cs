using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.PublicApi;
// ReSharper disable LoopCanBeConvertedToQuery

namespace OWP.AutoAssignment.Services.PublicApi;

public interface LAUserService
{
    Task<IList<SchedulingLAUser>> GetLAUsersInfo(DateTime eventDate);

    Task<bool> ExistsLAUserValid(DateTime eventDate);
}

public class LAUserServiceImp : LAUserService
{
    private readonly Repository repository;

    public LAUserServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<SchedulingLAUser>> GetLAUsersInfo(DateTime eventDate)
    {
        var lAUsers = await repository.GetLAInfoByDate(eventDate);
        return BuildListSchedulingLAUser(lAUsers);
    }

    private IList<SchedulingLAUser> BuildListSchedulingLAUser(ValueTuple<IEnumerable<LAUserInfo>, IEnumerable<UserWorkingShift>> userInfoTuple)
    {
        var users = userInfoTuple.Item1;
        var workingShifts = userInfoTuple.Item2.ToList();
        var listSchedulingUser = new List<SchedulingLAUser>();
        foreach (var user in users)
        {
            var userWorkingShifts = workingShifts.Where(x => x.UserId == user.UserId).ToList();
            if (userWorkingShifts.Any())
            {
                listSchedulingUser.Add(BuildSchedulingLAUser(user, userWorkingShifts));
            }
        }

        return listSchedulingUser;
    }

    private static SchedulingLAUser BuildSchedulingLAUser(LAUserInfo user, List<UserWorkingShift> userWorkingShifts)
        => new()
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

    public async Task<bool> ExistsLAUserValid(DateTime eventDate)
        => (await GetLAUsersInfo(eventDate)).Any();
}
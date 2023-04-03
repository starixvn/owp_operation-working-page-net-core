using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services;
public interface WorkingShiftService
{
    Task<List<WorkingSchedule>> Calculate(DateTime eventDate);
}

public class WorkingShiftServiceImp : WorkingShiftService
{
    private readonly Repository repository;

    public WorkingShiftServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<List<WorkingSchedule>> Calculate(DateTime eventDate)
    {
        var userInfos = await GetValidUsers(eventDate);

        return userInfos.Select(user => FillWorkingShift(user, eventDate)).ToList();
    }

    private static WorkingSchedule FillWorkingShift(UserInfo user, DateTime eventDate)
    {
        var shift = new WorkingSchedule { UserInfo = user };
        var type = shift.GetType();
        var properties = PropertyScheduleHelper
            .GetValidProperties(eventDate, user.ShiftStartTime, user.ShiftEndTime);
        foreach (var property in properties)
        {
            var propertyInfo = type.GetProperty(property);
            if (propertyInfo == null) continue;
            propertyInfo.SetValue(shift, new AssignmentCell { Color = ColorCodeConst.SHIFT, Text = string.Empty });
        }
        return shift;
    }

    private async Task<List<UserInfo>> GetValidUsers(DateTime eventDate)
    {
        var userInfos = await repository.QueryTraderInfo(eventDate);
        var napUserIds = await repository.GetUserInfoFromNAPUserControl();
        var db02UserIds = await repository.GetCustInfoFromDB02();

        return userInfos.Where(x => napUserIds.Any(y => y == x.UserId)
                                    && db02UserIds.Any(y => y == x.UserId)).ToList();
    }
}

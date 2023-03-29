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
        var workingSchedules = new List<WorkingSchedule>();
        foreach (var user in userInfos)
        {
            workingSchedules.Add(FillWorkingShift(user, eventDate));
        }

        return workingSchedules;
    }

    private WorkingSchedule FillWorkingShift(UserInfo user, DateTime eventDate)
    {
        var shift = new WorkingSchedule { UserInfo = user };
        Type type = shift.GetType();
        var properties = AssigneeSuggestionHelper
            .GetValidProperties(eventDate, user.ShiftStartTime, user.ShiftEndTime);
        foreach (var property in properties)
        {
            var propertyInfo = type.GetProperty(property);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(shift, new AssignmentCell { Color = "BFBFBF", Text = string.Empty });
            }

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

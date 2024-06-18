namespace OWP.WorkingShift.WeeklyWorkingShift.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Models;

public static class Extensions
{
    public static List<WorkingShiftUserModel> Order(this List<WorkingShiftUserModel> users)
    {
        return users.OrderBy(x => x.SectionName)
            .ThenBy(x => x.OrderId.HasValue ? 1 : 0)
            .ThenBy(x => x.OrderId)
            .ThenBy(x => x.DisplayName)
            .ToList();
    }

    public static List<WorkingShiftUserModel> AddWorkingShifts(
        this List<WorkingShiftUserModel> users,
        DateTime fromDate,
        DateTime toDate,
        IList<WorkingShiftModel> workingShifts
        )
    {
        var totalDays = Convert.ToInt32((toDate - fromDate).TotalDays) + 1;
        foreach (var user in users)
        {
            user.Remark = WebUtility.HtmlEncode(user.Remark);
            user.WorkingShifts = new WorkingShiftModel[totalDays];
            workingShifts.Where(ws => ws.UserId == user.UserId).ToList().ForEach(ws =>
            {
                var dateIndex = Convert.ToInt32((ws.ShiftDate - fromDate).TotalDays);
                user.WorkingShifts[dateIndex] = ws;
            });
        }

        return users;
    }

    public static List<WorkingShiftUserModel> AddWorkingShiftDefaults(
        this List<WorkingShiftUserModel> users,
        DateTime fromDate,
        DateTime toDate,
        IList<WorkingShiftDefaultModel> workingShiftDefaults
        )
    {
        var totalDays = Convert.ToInt32((toDate - fromDate).TotalDays) + 1;
        foreach (var user in users)
        {
            user.DefaultShiftIds = new int?[totalDays];
            workingShiftDefaults.Where(ws => ws.UserId == user.UserId).ToList().ForEach(ws =>
            {
                user.DefaultShiftIds[ws.DayOfTheWeek - 1] = ws.ShiftId;
            });
        }

        return users;
    }
}
using OWP.WorkingShift.Common.Constants;

namespace OWP.WorkingShift.Common.Utils;



public static class WorkingShiftExtensionHelper
{
    public static int ToExtensionNumber(this string shiftName)
    {
        return (int)WorkingShiftExtension.Real;
    }

    public static WorkingShiftExtension ToWorkingShiftExtension(this int extension)
    {
        return EnumUtils<WorkingShiftExtension>.ToEnum(extension);
    }

    public static string ToExtension(this int extension)
    {
        return EnumUtils<WorkingShiftExtension>.GetDescription(extension);
    }

    public static string ToShiftCodeWithExtension(this string shiftCode, int extension)
    {
        var ext = EnumUtils<WorkingShiftExtension>.GetDescription(extension);
        return shiftCode + ext;
    }

    public static Tuple<string, int> ToShiftCodeWithExtension(this string shiftCode)
    {
        var realShift = EnumUtils<WorkingShiftExtension>.GetDescription(WorkingShiftExtension.Real);
        var specialShift = EnumUtils<WorkingShiftExtension>.GetDescription(WorkingShiftExtension.Special);

        var extension = 0;
        var newShiftCode = shiftCode;
        if (shiftCode.EndsWith(realShift))
        {
            extension = (int)WorkingShiftExtension.Real;
            newShiftCode = shiftCode.Replace(realShift, string.Empty);
        }
        if (shiftCode.EndsWith(specialShift))
        {
            extension = (int)WorkingShiftExtension.Special;
            newShiftCode = shiftCode.Replace(specialShift, string.Empty);
        }

        return new Tuple<string, int>(newShiftCode, extension);
    }

    public static string GenerateBackgroundColor(this string shiftName)
    {
        var random = new Random();
        const int maxRandomNumber = 256;
        if (!string.IsNullOrEmpty(shiftName))
        {
            var isOffShift = shiftName.Equals(WorkingShiftCodes.Off, StringComparison.CurrentCultureIgnoreCase);
            var isLeaveShift = shiftName.Equals(WorkingShiftCodes.AnnualLeave, StringComparison.CurrentCultureIgnoreCase);
            if (isOffShift || isLeaveShift)
            {
                return "#839bb2";
            }
        }

        return "#" + random.Next(maxRandomNumber).ToString("X2") + random.Next(maxRandomNumber).ToString("X2") + random.Next(maxRandomNumber).ToString("X2");
    }

    public static string RemoveSpecificCharacters(this string text)
    {
        var acceptedChars = new List<char>();
        foreach (var ch in text)
        {
            if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z')
            {
                acceptedChars.Add(ch);
            }
        }

        return new string(acceptedChars.ToArray());
    }

    public static WorkingShiftModel[] ToArray(this IList<WorkingShiftModel> workingShifts, DateTime fromDate, DateTime toDate)
    {
        var totalDays = Convert.ToInt32((toDate - fromDate).TotalDays) + 1;
        var array = new WorkingShiftModel[totalDays];
        foreach (var workingShift in workingShifts)
        {
            var dateIndex = Convert.ToInt32((workingShift.ShiftDate - fromDate).TotalDays);
            array[dateIndex] = workingShift;
        }

        return array;
    }

    public static bool IsValidShiftCode(this string shiftCode)
    {
        return !string.IsNullOrWhiteSpace(shiftCode) && !shiftCode.Contains("<") && !shiftCode.Contains(",");
    }

    public static IEnumerable<GroupWorkingShiftByUserModel> GroupWorkingShiftByUser(this IList<WorkingShiftModel> workingShifts, DateTime fromDate, DateTime toDate)
    {
        var groupWorkingShift = new List<GroupWorkingShiftByUserModel>();
        foreach (var shift in workingShifts)
        {
            if (!groupWorkingShift.Exists(n => n.UserId == shift.UserId))
            {
                var workingShiftModel = new List<WorkingShiftModel>();

                foreach (var item in workingShifts)
                {
                    if (shift.UserId == item.UserId)
                    {
                        workingShiftModel.Add(item);
                    }
                }
                groupWorkingShift.Add(new GroupWorkingShiftByUserModel
                {
                    UserId = shift.UserId,
                    WorkingShift = workingShiftModel.ToArray(fromDate, toDate)
                });
            }
        };

        return groupWorkingShift;
    }
}
namespace OWP.ApiSyncWorkingShift.Utils;

using OWP.Utils;

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
}
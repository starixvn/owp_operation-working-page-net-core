using Newtonsoft.Json;
using OWP.ApiSyncWorkingShift.Const;
using OWP.ApiSyncWorkingShift.Utils;
using System.Xml.Serialization;

namespace OWP.ApiSyncWorkingShift.Models;

[Serializable]
public class WorkingShiftModel
{
    public string EmployeeID { get; set; }

    public DateTime? ShiftDate { get; set; }

    public string ShiftCode { get; set; }

    public DateTime? ShiftStartTime { get; set; }

    public DateTime? ShiftEndTime { get; set; }

    public DateTime? LastUpdated { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public string ShiftOT { get; set; }

    public bool IsOffDay => string.Equals(ShiftOT, HrmConst.OFF);

    public bool IsPublicHoliday => string.Equals(ShiftOT, HrmConst.PUBLIC_HOLIDAY);

    [XmlIgnore]
    public bool IsTimeShift { get; set; }

    [XmlIgnore]
    public int? Extension { get; set; }

    public void RecalculateShiftCode()
    {
        if (!IsTimeShift)
        {
            return;
        }

        var startTime = ShiftStartTime?.TimeOfDay ?? new TimeSpan();
        var endTime = ShiftEndTime?.TimeOfDay ?? new TimeSpan();

        var startTimeFormat = startTime.Minutes == 0 ? "hh" : @"hh\:mm";
        var endTimeFormat = endTime.Minutes == 0 ? "hh" : @"hh\:mm";
        ShiftCode = startTime.ToString(startTimeFormat) + " >" + endTime.ToString(endTimeFormat);
        if (Extension.HasValue)
        {
            var getExtension = Extension.Value.ToExtension();
            ShiftCode = ShiftCode + " " + getExtension;
        }
        ShiftCode = ShiftCode.TrimEnd();
    }
}
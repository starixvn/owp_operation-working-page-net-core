using OWP.Extensions;
using OWP.Utils;
using System.Xml.Serialization;

namespace OWP.PrematchAssignment.QueryAndSave.Models;

public class WorkingShiftModel
{
    [XmlIgnore]
    public int UserId { get; set; }

    [XmlAttribute("ShiftCode")]
    public string ShiftCode { get; set; }

    [XmlAttribute("NickName")]
    public string NickName { get; set; }

    [XmlIgnore]
    public TimeSpan? StartTime { get; set; }

    [XmlIgnore]
    public TimeSpan? EndTime { get; set; }

    [XmlAttribute("IsTimeShift")]
    public bool IsTimeShift { get; set; }

    [XmlAttribute("Extension")]
    public int? Extension { get; set; }

    public void RecalculateShiftCode()
    {
        if (!IsTimeShift)
        {
            return;
        }

        var startTimeFormat = StartTime is { Minutes: 0 } ? "hh" : @"hh\:mm";
        var endTimeFormat = EndTime is { Minutes: 0 } ? "hh" : @"hh\:mm";
        ShiftCode = StartTime?.ToString(startTimeFormat) + " > " + EndTime?.ToString(endTimeFormat);

        if (Extension.HasValue)
        {
            var getExtension = Extension.HasValue ? ToExtension(Extension.Value) : string.Empty;
            ShiftCode = ShiftCode + " " + getExtension;
        }

        ShiftCode = ShiftCode.TrimEnd();
    }

    private static string ToExtension(int extension)
        => EnumUtils<WorkingShiftExtension>.GetDescription(extension);
}
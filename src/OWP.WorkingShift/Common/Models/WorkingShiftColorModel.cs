namespace OWP.WorkingShift.Common.Models;

using Utils;
using System;
using System.Xml.Serialization;


[Serializable]
public class WorkingShiftColorModel
{
    [XmlIgnore]
    public int ShiftId { get; set; }

    [XmlAttribute("ShiftCode")]
    public string ShiftCode { get; set; }

    [XmlAttribute("Background")]
    public string Background { get; set; }

    [XmlAttribute("Color")]
    public string Color { get; set; }

    [XmlIgnore]
    public TimeSpan? StartTime { get; set; }

    [XmlAttribute("StartTime")]
    public string StartTimeString { get; set; }

    [XmlIgnore]
    public TimeSpan? EndTime { get; set; }

    [XmlAttribute("EndTime")]
    public string EndTimeString { get; set; }

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

        var startTimeFormat = StartTime.Value.Minutes == 0 ? "hh" : @"hh\:mm";
        var endTimeFormat = EndTime.Value.Minutes == 0 ? "hh" : @"hh\:mm";
        ShiftCode = StartTime.Value.ToString(startTimeFormat) + " >" + EndTime.Value.ToString(endTimeFormat);
        if (Extension.HasValue)
        {
            var getExtension = Extension.Value.ToExtension();
            ShiftCode = ShiftCode + " " + getExtension;
        }
        ShiftCode = ShiftCode.TrimEnd();
    }
}
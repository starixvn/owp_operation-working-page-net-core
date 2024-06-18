namespace OWP.WorkingShift.ViewLog.Models;

using System;
using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class DeleteWholeShiftsModel
{
    [XmlAttribute("sid")]
    public string ShiftId { get; set; }

    [XmlAttribute("sd")]
    public DateTime ShiftDate { get; set; }

    [XmlAttribute("st")]
    public DateTime ShiftStartTime { get; set; }

    [XmlAttribute("et")]
    public DateTime ShiftEndTime { get; set; }

    [XmlAttribute("rsid")]
    public string RefShiftId { get; set; }
}
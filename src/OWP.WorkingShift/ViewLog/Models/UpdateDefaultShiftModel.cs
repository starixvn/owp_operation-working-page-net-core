namespace OWP.WorkingShift.ViewLog.Models;

using System;
using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class UpdateDefaultShiftModel
{
    [XmlAttribute("iacw")]
    public bool IsApplyCurrentWeek { get; set; }

    [XmlAttribute("sids")]
    public string ShiftIds { get; set; }

    [XmlAttribute("fd")]
    public DateTime FromDate { get; set; }

    [XmlAttribute("td")]
    public DateTime ToDate { get; set; }
}

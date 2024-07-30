namespace OWP.WorkingShift.ViewLog.Models;

using System;
using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class DeleteShiftModel
{

    [XmlAttribute("sd")]
    public DateTime Date { get; set; }

    [XmlAttribute("sid")]
    public string ShiftId { get; set; }

    [XmlAttribute("rsid")]
    public string RefShiftId { get; set; }
}

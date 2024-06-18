namespace OWP.WorkingShift.ViewLog.Models;

using System;
using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class UploadWorkingShiftModel
{
    [XmlAttribute("sd")]
    public DateTime Date { get; set; }

    [XmlAttribute("sid")]
    public int ShiftId { get; set; }
}
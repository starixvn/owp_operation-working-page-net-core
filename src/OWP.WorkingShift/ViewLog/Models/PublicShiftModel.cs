namespace OWP.WorkingShift.ViewLog.Models;

using System;
using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class PublicShiftModel
{
    [XmlAttribute("frd")]
    public DateTime FromDate { get; set; }

    [XmlAttribute("tod")]
    public DateTime ToDate { get; set; }

    [XmlAttribute("isp")]
    public bool Isp { get; set; }
}

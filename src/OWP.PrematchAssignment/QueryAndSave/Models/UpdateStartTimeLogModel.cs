using System.Xml.Serialization;

namespace OWP.PrematchAssignment.Models;

[XmlRoot(ElementName = "z")]
public class UpdateStartTimeLogModel
{
    [XmlAttribute("d")]
    public DateTime Date { get; set; }

    [XmlAttribute("t")]
    public DateTime StartTime { get; set; }
}
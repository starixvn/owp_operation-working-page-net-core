using System.Xml.Serialization;

namespace OWP.PrematchAssignment.Models;

[XmlRoot(ElementName = "z")]
public class UpdateRemarkLogModel
{
    [XmlAttribute("rm")]
    public string Remark { get; set; }
}
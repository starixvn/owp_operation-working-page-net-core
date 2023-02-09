namespace OWP.PreMatchAssignment.Models;

using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class UpdateRemarkLogModel
{
    [XmlAttribute("rm")]
    public string Remark { get; set; }
}
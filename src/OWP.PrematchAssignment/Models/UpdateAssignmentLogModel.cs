namespace OWP.PreMatchAssignment.Models;

using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class UpdateAssignmentLogModel
{
    [XmlAttribute("uid")]
    public int UserId { get; set; }

    [XmlAttribute("un")]
    public string Username { get; set; }

    [XmlAttribute("bt")]
    public int Bettype { get; set; }

    [XmlAttribute("ito")]
    public bool IsTakeOver { get; set; }

    [XmlAttribute("at")]
    public int AssignmentType { get; set; }
}
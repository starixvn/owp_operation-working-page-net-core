namespace OWP.PreMatchAssignment.Models
{
    
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "z")]
    public class UpdateStartTimeLogModel
    {
        [XmlAttribute("d")]
        public DateTime Date { get; set; }

        [XmlAttribute("t")]
        public DateTime StartTime { get; set; }
    }
}
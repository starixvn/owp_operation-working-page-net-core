
namespace OWP.OddsCompiler.Models;
using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class MartketSendNotificationModel
{
    [XmlAttribute("old")]
    public string OldValue { get; set; }

    [XmlAttribute("new")]
    public string NewValue { get; set; }
}
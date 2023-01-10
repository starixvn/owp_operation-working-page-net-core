using System.Xml.Serialization;

namespace OWP.ProviderArrangement.Models;

[XmlRoot(ElementName = "z")]
public class ProviderModelDeserialize
{
    [XmlAttribute("lProvider")] public string Providers { get; set; }
    [XmlAttribute("lActionName")] public string ActionName { get; set; }
}
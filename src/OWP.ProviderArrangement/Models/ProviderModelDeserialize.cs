namespace OWP.ProviderArrangement.Models;

using System.Xml.Serialization;

[XmlRoot(ElementName = "z")]
public class ProviderModelDeserialize
{
    [XmlAttribute("lProvider")] public string Providers { get; set; }
    [XmlAttribute("lActionName")] public string ActionName { get; set; }
}
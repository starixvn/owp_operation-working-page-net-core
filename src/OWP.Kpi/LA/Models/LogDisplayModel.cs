using System.Xml.Serialization;

namespace OWP.Kpi.LA.Models;

public class LogDisplayModel
{
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public string ModifyDate { get; set; }
    public string ModifyBy { get; set; }
}

[XmlRoot(ElementName = "z")]
public class TargetSettingModelDeserialize
{
    [XmlAttribute("target")] public string Target { get; set; }
}
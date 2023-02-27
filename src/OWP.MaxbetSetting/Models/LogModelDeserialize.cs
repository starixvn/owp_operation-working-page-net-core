using System.Xml.Serialization;

namespace OWP.MaxbetSetting.Models;

[XmlRoot(ElementName = "z")]
public class LogModelDeserialize
{
    [XmlAttribute("fT_HDP")]
    public string FT_HDP { get; set; }
    [XmlAttribute("fT_OU")]
    public string FT_OU { get; set; }
    [XmlAttribute("firstHalf_HDP")]
    public string FirstHalf_HDP { get; set; }
    [XmlAttribute("firstHalf_OU")]
    public string FirstHalf_OU { get; set; }
    [XmlAttribute("noOfCorner_FT_HDP")]
    public string NoOfCorner_FT_HDP { get; set; }
    [XmlAttribute("noOfCorner_FT_OU")]
    public string NoOfCorner_FT_OU { get; set; }
    [XmlAttribute("noOfCorner_1H_HDP")]
    public string NoOfCorner_1H_HDP { get; set; }
    [XmlAttribute("noOfCorner_1H_OU")]
    public string NoOfCorner_1H_OU { get; set; }
    [XmlAttribute("nextCorner_FT_HDP")]
    public string NextCorner_FT_HDP { get; set; }
    [XmlAttribute("totalBooking_FT_HDP")]
    public string TotalBooking_FT_HDP { get; set; }
    [XmlAttribute("totalBooking_FT_OU")]
    public string TotalBooking_FT_OU { get; set; }
    [XmlAttribute("totalBooking_1H_HDP")]
    public string TotalBooking_1H_HDP { get; set; }
    [XmlAttribute("totalBooking_1H_OU")]
    public string TotalBooking_1H_OU { get; set; }
    [XmlAttribute("nextBooking_FT_HDP")]
    public string NextBooking_FT_HDP { get; set; }
    [XmlAttribute("secondAccept_Main")]
    public string SecondAccept_Main { get; set; }
    [XmlAttribute("secondAccept_Corner")]
    public string SecondAccept_Corner { get; set; }
    [XmlAttribute("secondAccept_TotalBooking")]
    public string SecondAccept_TotalBooking { get; set; }
    [XmlAttribute("secondAccept_Fantasy")]
    public string SecondAccept_Fantasy { get; set; }
}
namespace OWP.MaxbetSetting.Models
{
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "z")]
    public class LogModelDeserialize
    {
        [XmlAttribute("fT_HDP")]
        public decimal FT_HDP { get; set; }
        [XmlAttribute("fT_OU")]
        public decimal FT_OU { get; set; }
        [XmlAttribute("firstHalf_HDP")]
        public decimal FirstHalf_HDP { get; set; }
        [XmlAttribute("firstHalf_OU")]
        public decimal FirstHalf_OU { get; set; }
        [XmlAttribute("noOfCorner_FT_HDP")]
        public decimal NoOfCorner_FT_HDP { get; set; }
        [XmlAttribute("noOfCorner_FT_OU")]
        public decimal NoOfCorner_FT_OU { get; set; }
        [XmlAttribute("noOfCorner_1H_HDP")]
        public decimal NoOfCorner_1H_HDP { get; set; }
        [XmlAttribute("noOfCorner_1H_OU")]
        public decimal NoOfCorner_1H_OU { get; set; }
        [XmlAttribute("nextCorner_FT_HDP")]
        public decimal NextCorner_FT_HDP { get; set; }
        [XmlAttribute("totalBooking_FT_HDP")]
        public decimal TotalBooking_FT_HDP { get; set; }
        [XmlAttribute("totalBooking_FT_OU")]
        public decimal TotalBooking_FT_OU { get; set; }
        [XmlAttribute("totalBooking_1H_HDP")]
        public decimal TotalBooking_1H_HDP { get; set; }
        [XmlAttribute("totalBooking_1H_OU")]
        public decimal TotalBooking_1H_OU { get; set; }
        [XmlAttribute("nextBooking_FT_HDP")]
        public decimal NextBooking_FT_HDP { get; set; }
        [XmlAttribute("secondAccept_Main")]
        public string SecondAccept_Main { get; set; }
        [XmlAttribute("secondAccept_Corner")]
        public string SecondAccept_Corner { get; set; }
        [XmlAttribute("secondAccept_TotalBooking")]
        public string SecondAccept_TotalBooking { get; set; }
        [XmlAttribute("secondAccept_Fantasy")]
        public string SecondAccept_Fantasy { get; set; }
    }
}
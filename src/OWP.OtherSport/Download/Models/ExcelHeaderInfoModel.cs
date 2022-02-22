using Epplus.Mapper.Annotations;

namespace OWP.OtherSport.Download.Models
{
    public class ExcelHeaderInfoModel
    {
        [Cell("B1")]
        public string FromDate { get; set; }

        [Cell("D1")]
        public string ToDate { get; set; }

        [Cell("F1")]
        public string Employee { get; set; }

        [Cell("H1")]
        public string Sports { get; set; }

        [Cell("J1")]
        public string OfferLive { get; set; }

        [Cell("L1")]
        public string MatchStatus { get; set; }

        [Cell("N1")]
        public string LinkedMatch { get; set; }
    }
}
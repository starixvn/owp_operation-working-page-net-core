using Epplus.Mapper.Annotations;
using System;

namespace OWP.OtherSport.Download.Models
{
    public class ExcelHeaderInfoModel
    {
        [Cell("B1")]
        public DateTime FromDate { get; set; }

        [Cell("D1")]
        public DateTime ToDate { get; set; }

        [Cell("F1")]
        public string Employee { get; set; }

        [Cell("H1")]
        public string Sports { get; set; }

        [Cell("J1")]
        public string MatchStatus { get; set; }
    }
}

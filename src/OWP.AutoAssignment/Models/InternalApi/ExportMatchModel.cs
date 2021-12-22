using System;
using System.Globalization;
using Epplus.Mapper.Annotations;
using OfficeOpenXml.Attributes;

namespace OWP.AutoAssignment.Models.InternalApi
{
    [EpplusTable]
    public record ExportMatchModel
    {
        private string _time;

        [Cell("C3")]
        public string EventDateTime
        {
            get
            {
                return _time;
            }
            set
            {
                DateTime time;

                _time = DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out time)
                    ? time.ToString("hh:mm tt", CultureInfo.InvariantCulture) : value;
            }
        }

        [EpplusIgnore]
        public int Bettype { get; init; }

        [Cell("G3")]
        public string LeagueName { get; set; }

        [Cell("E3")]
        public string HomeName { get; set; }

        [Cell("F3")]
        public string AwayName { get; set; }

        [Cell("H3")]
        public string MatchOffer { get; set; }

        [Cell("I3")]
        public string LSTNote { get; set; }

        [Cell("J3")]
        public string MaxBetHDP { get; set; }

        [Cell("K3")]
        public string SpreadHDP { get; set; }

        [Cell("L3")]
        public string SpreadOU { get; set; }

        [Cell("M3")]
        public string NoHDP { get; set; }

        [Cell("N3")]
        public string TraderHDP { get; set; }

        [Cell("O3")]
        public string TraderOU { get; set; }

        [Cell("P3")]
        public string Trader1HHDP { get; set; }

        [Cell("Q3")]
        public string Trader1HOU { get; set; }

        [Cell("AB3")]
        public string LA { get; set; }

        [Cell("AD3")]
        public string Provider { get; set; }

        [Cell("AE3")]
        public string OtherProvider { get; set; }

        [Cell("AF3")]
        public string Division { get; set; }

        [Cell("AG3")]
        public string Remark { get; set; }

        [Cell("AH3")]
        public string WB { get; set; }

        [Cell("AI3")]
        public string Round { get; set; }

        [Cell("AJ3")]
        public string TV { get; set; }

        [Cell("AK3")]
        public string Channel { get; set; }

        [Cell("AL3")]
        public string IPTV { get; set; }

        [Cell("AM3")]
        public string Agent { get; set; }

        [Cell("AN3")]
        public string ReporterPhone { get; set; }

        [EpplusIgnore]
        public int InfoSiteMatchId { get; set; }

        [Cell("AO3")]
        public int InfoSiteMatchIdExport => InfoSiteMatchId;

        [Cell("AP3")]
        public int? AdminSiteMatchId { get; set; }

        [Cell("AQ3")]
        public string Website1 { get; set; }

        [Cell("AR3")]
        public string Website2 { get; set; }

        [Cell("AS3")]
        public string Website3 { get; set; }

        [Cell("AT3")]
        public string Website4 { get; set; }

        [Cell("AY3")]
        public string MaxBetOU { get; set; }

        [Cell("AZ3")]
        public string MaxBet1HHDP { get; set; }

        [Cell("BA3")]
        public string MaxBet1HOU { get; set; }

        [Cell("BB3")]
        public string Spread1HHDP { get; set; }

        [Cell("BC3")]
        public string Spread1HOU { get; set; }

        [Cell("BD3")]
        public string MaxBet1X2 { get; set; }

        [Cell("BE3")]
        public string Spread1X2 { get; set; }

        [Cell("BF3")]
        public string MaxBet1H1X2 { get; set; }

    }
}

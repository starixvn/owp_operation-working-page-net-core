namespace OWP.OvertimeReport.Models
{
    using Epplus.Mapper.Annotations;
    using System.Collections.Generic;

    public class ExcelYearModel
    {
        public IEnumerable<ExcelYearData> HourByMonth { get; set; }

        public IEnumerable<decimal?> TotalHoursByMonth { get; set; }
    }

    public class ExcelYearData
    {
        [Cell("A3")]
        public int No { get; set; }

        [Cell("B3")]
        public string Location { get; set; }

        [Cell("C3")]
        public string Trader { get; set; }

        [Cell("D3")]
        public string Jan { get; set; }

        [Cell("E3")]
        public string Feb { get; set; }

        [Cell("F3")]
        public string Mar { get; set; }

        [Cell("G3")]
        public string Apr { get; set; }

        [Cell("H3")]
        public string May { get; set; }

        [Cell("I3")]
        public string Jun { get; set; }

        [Cell("J3")]
        public string Jul { get; set; }

        [Cell("K3")]
        public string Aug { get; set; }

        [Cell("L3")]
        public string Sep { get; set; }

        [Cell("M3")]
        public string Oct { get; set; }

        [Cell("N3")]
        public string Nov { get; set; }

        [Cell("O3")]
        public string Dec { get; set; }

        [Cell("P3")]
        public string Total { get; set; }
    }
}

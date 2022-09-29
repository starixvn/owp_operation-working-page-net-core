namespace OWP.Report.LAReport.Models
{
    using Epplus.Mapper.Annotations;

    public class LAExcelReportModel
    {
        [Cell("A7")]
        public string Username { get; set; }

        [Cell("B7")]
        public string Section { get; set; }

        [Cell("C7")]
        public int TotalMatches { get; set; }

        [Cell("D7")]
        public int Link2 { get; set; }

        [Cell("E7")]
        public int Link3 { get; set; }

        [Cell("F7")]
        public int Link4 { get; set; }

        [Cell("G7")]
        public int Link5 { get; set; }

        [Cell("H7")]
        public int Link6 { get; set; }

        [Cell("I7")]
        public int Link7 { get; set; }

        [Cell("J7")]
        public int Link8Plus { get; set; }

        [Cell("K7")]
        public int TotalMultiMatches { get; set; }

        [Cell("L7")]
        public int OnlyProvider { get; set; }

        [Cell("M7")]
        public int OnlyTv { get; set; }

        [Cell("N7")]
        public int OnlyReporter { get; set; }

        [Cell("O7")]
        public int MixedProviderTVReporter { get; set; }

        [Cell("P7")]
        public int TotalSingleMatches { get; set; }

        [Cell("Q7")]
        public int CornerPlus { get; set; }

        [Cell("R7")]
        public int TBookingPlus { get; set; }

        [Cell("S7")]
        public int Fantasy { get; set; }

        [Cell("T7")]
        public int GoalKick { get; set; }

        [Cell("U7")]
        public int OtherProducts { get; set; }

        [Cell("V7")]
        public int TotalSpecialProducts { get; set; }

        [Cell("W7")]
        public int FTCorner { get; set; }

        [Cell("X7")]
        public int FTCorner5ML { get; set; }

        [Cell("Y7")]
        public int TotalMixedProduct { get; set; }

        [Cell("Z7")]
        public int FiveML { get; set; }

        [Cell("AA7")]
        public int ESoccer5Mins { get; set; }

        [Cell("AB7")]
        public int ESoccer6Mins { get; set; }

        [Cell("AC7")]
        public int ESoccer15Mins { get; set; }

        [Cell("AD7")]
        public int ESoccer16Mins { get; set; }

        [Cell("AE7")]
        public int ESoccer20Mins { get; set; }

        [Cell("AF7")]
        public int TotalESoccer { get; set; }

        [Cell("AG7")]
        public int Others { get; set; }
    }
}
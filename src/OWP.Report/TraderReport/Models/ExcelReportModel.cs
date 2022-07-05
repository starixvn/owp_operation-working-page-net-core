namespace OWP.Report.TraderReport.Models
{
    using Epplus.Mapper.Annotations;

    public class ExcelReportModel
    {
        [Cell("A7")]
        public string Username { get; set; }

        [Cell("B7")]
        public string SectionName { get; set; }

        [Cell("C7")]
        public int TotalMatch { get; set; }

        [Cell("D7")]
        public int Type1_FT_HDP { get; set; }

        [Cell("E7")]
        public int Type1_FT_OU { get; set; }

        [Cell("F7")]
        public int Type1_FT_HDPOU { get; set; }

        [Cell("G7")]
        public int Type2_AUS_League { get; set; }

        [Cell("H7")]
        public int Type2_AUS_League_Corner { get; set; }

        [Cell("I7")]
        public int Type2_JAP_Divison1 { get; set; }

        [Cell("J7")]
        public int Type2_JAP_Divison2 { get; set; }

        [Cell("K7")]
        public int Robot3_X2 { get; set; }

        [Cell("L7")]
        public int Robot3_X3 { get; set; }

        [Cell("M7")]
        public int Robot3_X3_Plus { get; set; }

        [Cell("N7")]
        public int Type3_Solo_Major { get; set; }

        [Cell("O7")]
        public int Type4_Major { get; set; }

        [Cell("P7")]
        public int Type4_Minor1 { get; set; }

        [Cell("Q7")]
        public int Type4_Minor2 { get; set; }

        [Cell("R7")]
        public int Type4_Minor3 { get; set; }

        [Cell("S7")]
        public int Type4_1ST_Major { get; set; }

        [Cell("T7")]
        public int Type4_1ST_Other { get; set; }

        [Cell("U7")]
        public int Type4_Fantasy { get; set; }

        [Cell("V7")]
        public int Type4_Corner_NextCorner { get; set; }

        [Cell("W7")]
        public int Type4_15M { get; set; }

        [Cell("X7")]
        public int Type4_10M { get; set; }

        [Cell("Y7")]
        public int Type4_TotalBooking { get; set; }

        [Cell("Z7")]
        public int Type4_PKSO { get; set; }

        [Cell("AA7")]
        public int Type4_Winner { get; set; }

        [Cell("AB7")]
        public int PH_1X2_Major { get; set; }

        [Cell("AC7")]
        public int PH_1X2_Minor1 { get; set; }

        [Cell("AD7")]
        public int PH_1X2_Minor2 { get; set; }

        [Cell("AE7")]
        public int PH_1X2_Minor3 { get; set; }

        [Cell("AF7")]
        public int PH_1X2_Others { get; set; }

        [Cell("AG7")]
        public int X5 { get; set; }

        [Cell("AH7")]
        public int ESoccer_5Mins { get; set; }

        [Cell("AI7")]
        public int ESoccer_6Mins { get; set; }

        [Cell("AJ7")]
        public int ESoccer_15Mins { get; set; }

        [Cell("AK7")]
        public int ESoccer_16Mins { get; set; }

        [Cell("AL7")]
        public int ESoccer_20Mins { get; set; }

        [Cell("AM7")]
        public decimal TurnOver_FT_HDP { get; set; }

        [Cell("AN7")]
        public decimal TurnOver_FT_OU { get; set; }

        [Cell("AO7")]
        public decimal TurnOver_1H_HDP { get; set; }

        [Cell("AP7")]
        public decimal TurnOver_1H_OU { get; set; }

        [Cell("AQ7")]
        public decimal TurnOver_Others { get; set; }

        [Cell("AR7")]
        public decimal WinLoss_FT_HDP { get; set; }

        [Cell("AS7")]
        public decimal Winloss_FT_OU { get; set; }

        [Cell("AT7")]
        public decimal Winloss_1H_HDP { get; set; }

        [Cell("AU7")]
        public decimal Winloss_1H_OU { get; set; }

        [Cell("AV7")]
        public decimal Winloss_Others { get; set; }
    }
}

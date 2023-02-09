namespace OWP.Report.TraderReport.Models;

public class ReportModel
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public string SectionId { get; set; }

    public string SectionName { get; set; }

    public int TotalMatch { get; set; }

    public int Type1_FT_HDP { get; set; }

    public int Type1_FT_OU { get; set; }

    public int Type1_FT_HDPOU { get; set; }

    public int Type2_AUS_League { get; set; }

    public int Type2_AUS_League_Corner { get; set; }

    public int Type2_JAP_Divison1 { get; set; }

    public int Type2_JAP_Divison2 { get; set; }

    public int Type3_Solo_Major { get; set; }

    public int Type4_Major { get; set; }

    public int Type4_Minor1 { get; set; }

    public int Type4_Minor2 { get; set; }

    public int Type4_Minor3 { get; set; }

    public int Type4_1ST_Major { get; set; }

    public int Type4_1ST_Other { get; set; }

    public int Type4_Fantasy { get; set; }

    public int Type4_Corner_NextCorner { get; set; }

    public int Type4_15M { get; set; }

    public int Type4_10M { get; set; }

    public int Type4_TotalBooking { get; set; }

    public int Type4_PKSO { get; set; }

    public int Type4_Winner { get; set; }

    public int PH_1X2_Major { get; set; }

    public int PH_1X2_Minor1 { get; set; }

    public int PH_1X2_Minor2 { get; set; }

    public int PH_1X2_Minor3 { get; set; }

    public int PH_1X2_Others { get; set; }

    public int X5 { get; set; }

    public decimal TurnOver_FT_HDP { get; set; }

    public decimal TurnOver_FT_OU { get; set; }

    public decimal TurnOver_1H_HDP { get; set; }

    public decimal TurnOver_1H_OU { get; set; }

    public decimal TurnOver_Others { get; set; }

    public decimal WinLoss_FT_HDP { get; set; }

    public decimal Winloss_FT_OU { get; set; }

    public decimal Winloss_1H_HDP { get; set; }

    public decimal Winloss_1H_OU { get; set; }

    public decimal Winloss_Others { get; set; }

    public int Robot3_X2 { get; set; }

    public int Robot3_X3 { get; set; }

    public int Robot3_X3_Plus { get; set; }

    public int Robot5_X5 { get; set; }

    public int ESoccer_5Mins { get; set; }

    public int ESoccer_6Mins { get; set; }

    public int ESoccer_15Mins { get; set; }

    public int ESoccer_16Mins { get; set; }

    public int ESoccer_20Mins { get; set; }
}
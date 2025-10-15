using System.ComponentModel.DataAnnotations;
using OWP.AssignmentUpload.Validation.Annotations;
using OWP.AssignmentUpload.Validation.Constants;

namespace OWP.AssignmentUpload.Validation.Models;

public class AssignmentModelInExcel
{
    [Display(Name = "Agent")]
    [StringLength(EventUploadingMaxLength.Agent, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Agent { get; set; }

    [Display(Name = "Channel")]
    [StringLength(EventUploadingMaxLength.Channel, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Channel { get; set; }

    [Display(Name = "IPTV")]
    [StringLength(EventUploadingMaxLength.IPTV, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string IPTV { get; set; }

    [Display(Name = "Division")]
    [StringLength(EventUploadingMaxLength.Division, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Division { get; set; }

    [Display(Name = "Home")]
    [StringLength(EventUploadingMaxLength.HomeName,
        MinimumLength = 1,
        ErrorMessage = "[Column: {0}] - Error: The item's value must be not empty and must not exceed {1} characters")]
    public string Home { get; set; }

    [Display(Name = "Away")]
    [StringLength(EventUploadingMaxLength.AwayName,
        MinimumLength = 1,
        ErrorMessage = "[Column: {0}] - Error: The item's value must be not empty and must not exceed {1} characters")]
    public string Away { get; set; }

    [Display(Name = "League")]
    [StringLength(EventUploadingMaxLength.LeagueName,
        MinimumLength = 1,
        ErrorMessage = "[Column: {0}] - Error: The item's value must be not empty and must not exceed {1} characters")]
    public string League { get; set; }

    [Display(Name = "InfoSite ID")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
    public int InfoSiteMatchId { get; set; }

    [Display(Name = "AdminSite ID")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer number")]
    public int AdminSiteId { get; set; }

    [Display(Name = "No")]
    [StringLength(EventUploadingMaxLength.MatchNo, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string MatchNo { get; set; }

    [Display(Name = "Match Offer")]
    public string MatchOffer { get; set; }

    [Display(Name = "Match Status")]
    [RegularExpression(@"(?i:^(Live|Cancel|Postponed|Suspended|In-play|Completed)*$)",
        ErrorMessage = "[Column: {0}] - The match status must be Live (or empty), In-play, Completed, Cancel, Postponed, Suspended")]
    public string MatchStatus { get; set; }

    [Display(Name = "No.HDP")]
    [RegularExpression(@"^(1HDP|2HDP|2\+1H|3HDP|4HDP|2\+4 lines|2\+3 lines|3\+3 lines|3\+4 lines|4\+4 lines|1 line|2 lines)*$",
        ErrorMessage = "[Column: {0}] - The No.HDP must be 1HDP, 2HDP, 2+1H, 3HDP, 4HDP, 2+4 lines, 3+3 lines, 3+4 lines, 4+4 lines, 1 line or 2 lines")]
    public string NoHDP { get; set; }

    [Display(Name = "Time")]
    [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9](\s)*(AM|PM)$", ErrorMessage = "[Column: {0}] - Error: Invalid time format")]
    public string Time { get; set; }

    [Display(Name = "Other Time")]
    [RegularExpression(@"^(([0-1]?[0-9]|2[0-3]):[0-5][0-9](\s)*(AM|PM))*$", ErrorMessage = "[Column: {0}] - Error: Invalid time format")]
    public string OtherOfTime { get; set; }

    [Display(Name = "Reporter Phone")]
    [StringLength(EventUploadingMaxLength.ReporterPhone, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Phone { get; set; }

    [Display(Name = "Provider")]
    [StringLength(EventUploadingMaxLength.Provider, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Provider { get; set; }

    [Display(Name = "Agent for EST")]
    [StringLength(EventUploadingMaxLength.Remark, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string AgentEst { get; set; }

    [Display(Name = "Remark for EST")]
    [StringLength(EventUploadingMaxLength.Remark, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string RemarkEst { get; set; }

    [Display(Name = "Remark by")]
    [StringLength(EventUploadingMaxLength.Remark, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string RemarkBy { get; set; }

    [Display(Name = "Other Provider")]
    [StringLength(EventUploadingMaxLength.OtherProvider, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string OtherProvider { get; set; }

    [Display(Name = "Remark")]
    [StringLength(EventUploadingMaxLength.Remark, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Remark { get; set; }

    [Display(Name = "Round")]
    [StringLength(EventUploadingMaxLength.Round, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Round { get; set; }

    [Display(Name = "TV")]
    [StringLength(EventUploadingMaxLength.TV, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string TV { get; set; }

    [Display(Name = "Streaming Info(WB)")]
    [StringLength(EventUploadingMaxLength.WatchBet, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string WB { get; set; }

    [Display(Name = "Web 1")]
    [StringLength(EventUploadingMaxLength.Website1, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Website1 { get; set; }

    [Display(Name = "Web 2")]
    [StringLength(EventUploadingMaxLength.Website2, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Website2 { get; set; }

    [Display(Name = "Web 3")]
    [StringLength(EventUploadingMaxLength.Website3, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Website3 { get; set; }

    [Display(Name = "Web 4")]
    [StringLength(EventUploadingMaxLength.Website4, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    public string Website4 { get; set; }

    [Display(Name = "10mins")]
    [StringLength(EventUploadingMaxLength.Trader10Mins, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string Trader10MinsName { get; set; }

    [Display(Name = "15mins")]
    [StringLength(EventUploadingMaxLength.Trader15Mins, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string Trader15MinsName { get; set; }

    [Display(Name = "Trader (1H 1X2)")]
    [StringLength(EventUploadingMaxLength.Trader1H1X2, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string Trader1H1X2Name { get; set; }

    [Display(Name = "Trader (1H HDP)")]
    [StringLength(EventUploadingMaxLength.Trader1HHDP, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string Trader1HHDPName { get; set; }

    [Display(Name = "Trader (1H O/U)")]
    [StringLength(EventUploadingMaxLength.Trader1HOU, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string Trader1HOUName { get; set; }

    [Display(Name = "Trader (1X2)")]
    [StringLength(EventUploadingMaxLength.Trader1X2, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string Trader1X2Name { get; set; }

    [Display(Name = "AIO")]
    [StringLength(EventUploadingMaxLength.TraderAIO, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string TraderAIOName { get; set; }

    [Display(Name = "FM")]
    [StringLength(EventUploadingMaxLength.TraderFM, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string TraderFMName { get; set; }

    [Display(Name = "Trader (HDP)")]
    [StringLength(EventUploadingMaxLength.TraderHDP, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string TraderHDPName { get; set; }

    [Display(Name = "Trader (O/U)")]
    [StringLength(EventUploadingMaxLength.TraderOU, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string TraderOUName { get; set; }

    [Display(Name = "LA")]
    [StringLength(EventUploadingMaxLength.LA, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
    [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
    public string TraderLAName { get; set; }

    [Display(Name = "Other Traders (10 mins)")]
    public string OtherTraders10Mins { get; set; }

    [Display(Name = "Other Traders (15 mins)")]
    public string OtherTraders15Mins { get; set; }

    [Display(Name = "Other Traders (1H 1X2)")]
    public string OtherTraders1H1X2 { get; set; }

    [Display(Name = "Other Traders (1H HDP)")]
    public string OtherTraders1HHDP { get; set; }

    [Display(Name = "Other Traders (1H O/U)")]
    public string OtherTraders1HOU { get; set; }

    [Display(Name = "Other Traders (1X2)")]
    public string OtherTraders1X2 { get; set; }

    [Display(Name = "Other Traders (AIO)")]
    public string OtherTradersAIO { get; set; }

    [Display(Name = "Other Traders (FM)")]
    public string OtherTradersFM { get; set; }

    [Display(Name = "Other Traders (HDP)")]
    public string OtherTradersHDP { get; set; }

    [Display(Name = "Other LA")]
    public string OtherTradersLA { get; set; }

    [Display(Name = "Other Traders (O/U)")]
    public string OtherTradersOU { get; set; }

    [Display(Name = "Max Bet (1H 1X2)")]
    [Range(0, 999000, ErrorMessage = "[Column: {0}] - Error: Invalid max bet. The value must be less than or equal 999K")]
    [MaxBet]
    public decimal? MaxBet1H1X2 { get; set; }

    [Display(Name = "Max Bet (1H HDP)")]
    [Range(0, 999000, ErrorMessage = "[Column: {0}] - Error: Invalid max bet. The value must be less than or equal 999K")]
    [MaxBet]
    public decimal? MaxBet1HHDP { get; set; }

    [Display(Name = "Max Bet (1H O/U)")]
    [Range(0, 999000, ErrorMessage = "[Column: {0}] - Error: Invalid max bet. The value must be less than or equal 999K")]
    [MaxBet]
    public decimal? MaxBet1HOU { get; set; }

    [Display(Name = "Max Bet (1X2)")]
    [Range(0, 999000, ErrorMessage = "[Column: {0}] - Error: Invalid max bet. The value must be less than or equal 999K")]
    [MaxBet]
    public decimal? MaxBet1X2 { get; set; }

    [Display(Name = "Max Bet (FM)")]
    [Range(0, 999000, ErrorMessage = "[Column: {0}] - Error: Invalid max bet. The value must be less than or equal 999K")]
    [MaxBet]
    public decimal? MaxBetFM { get; set; }

    [Display(Name = "Max Bet (HDP)")]
    [Range(0, 999000, ErrorMessage = "[Column: {0}] - Error: Invalid max bet. The value must be less than or equal 999K")]
    [MaxBet]
    public decimal? MaxBetHDP { get; set; }

    [Display(Name = "Max Bet (O/U)")]
    [Range(0, 999000, ErrorMessage = "[Column: {0}] - Error: Invalid max bet. The value must be less than or equal 999K")]
    [MaxBet]
    public decimal? MaxBetOU { get; set; }

    [Display(Name = "Max Bet (x mins)")]
    [Range(0, 999000, ErrorMessage = "[Column: {0}] - Error: Invalid max bet. The value must be less than or equal 999K")]
    [MaxBet]
    public decimal? MaxBetXMins { get; set; }

    [Display(Name = "Spread (1H 1X2)")]
    [Spread]
    public decimal? Spread1H1X2 { get; set; }

    [Display(Name = "Spread (1H HDP)")]
    [Spread]
    public decimal? Spread1HHDP { get; set; }

    [Display(Name = "Spread (1H O/U)")]
    [Spread]
    public decimal? Spread1HOU { get; set; }

    [Display(Name = "Spread (1X2)")]
    [Spread]
    public decimal? Spread1X2 { get; set; }

    [Display(Name = "Spread (FM)")]
    [Spread]
    public decimal? SpreadFM { get; set; }

    [Display(Name = "Spread (HDP)")]
    [Spread]
    public decimal? SpreadHDP { get; set; }

    [Display(Name = "Spread (O/U)")]
    [Spread]
    public decimal? SpreadOU { get; set; }

    [Display(Name = "Spread (x mins)")]
    [Spread]
    public decimal? SpreadXMins { get; set; }

    [Display(Name = "AIO BT")]
    public string AIOBT { get; set; }

    [Display(Name = "SOP")]
    public string SOP { get; set; }
}
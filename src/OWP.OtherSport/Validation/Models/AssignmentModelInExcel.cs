namespace OWP.OtherSport.Validation.Models
{
    using System.ComponentModel.DataAnnotations;
    using OWP.OtherSport.Validation.Annotations;
    using OWP.OtherSport.Validation.Constants;

    public class AssignmentModelInExcel
    {
        [Display(Name = "Match No")]
        public string MatchNo { get; set; }

        [Display(Name = "Reporter")]
        [StringLength(EventUploadingPropertyMaxLength.Reporter, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string Reporter { get; set; }

        [Display(Name = "Match Status")]
        [StringLength(EventUploadingPropertyMaxLength.MatchStatus,
            MinimumLength = 1,
            ErrorMessage = "[Column: {0}] - The Match Status must be Live, Canceled or Postponed")]
        [RegularExpression(@"(?i:^(Live|Canceled|Postponed)$)",
            ErrorMessage = "[Column: {0}] - The Match Status must be Live, Canceled or Postponed")]
        public string MatchStatus { get; set; }

        [Display(Name = "Sport")]
        [Sport(ErrorMessage = "[Column: {0}] - Invalid sport")]
        public string Sport { get; set; }

        [Display(Name = "Date Time")]
        public string DateTime { get; set; }

        [Display(Name = "Home")]
        [StringLength(EventUploadingPropertyMaxLength.HomeName,
            MinimumLength = 1,
            ErrorMessage = "[Column: {0}] - Error: The item's value must be not empty and must not exceed {1} characters")]
        public string Home { get; set; }

        [Display(Name = "Away")]
        [StringLength(EventUploadingPropertyMaxLength.AwayName,
            MinimumLength = 1,
            ErrorMessage = "[Column: {0}] - Error: The item's value must be not empty and must not exceed {1} characters")]
        public string Away { get; set; }

        [Display(Name = "League")]
        [StringLength(EventUploadingPropertyMaxLength.LeagueName,
            MinimumLength = 1,
            ErrorMessage = "[Column: {0}] - Error: The item's value must be not empty and must not exceed {1} characters")]
        public string League { get; set; }

        [Display(Name = "Category")]
        [StringLength(EventUploadingPropertyMaxLength.Category, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string Category { get; set; }

        [Display(Name = "No. HDP")]
        [StringLength(EventUploadingPropertyMaxLength.NoHDP, ErrorMessage = "[Column: {0}] - The item's value must not exceed {1} characters")]
        public string NoHDP { get; set; }

        [Display(Name = "Match Offer")]
        [StringLength(EventUploadingPropertyMaxLength.MatchOffer, ErrorMessage = "[Column: {0}] - The item's value must not exceed {1} characters")]
        public string MatchOffer { get; set; }

        [Display(Name = "Auto Link")]
        [StringLength(EventUploadingPropertyMaxLength.AutoLink,
            ErrorMessage = "[Column: {0}] - The item's value must not exceed {1} characters")]
        public string AutoLink { get; set; }

        [Display(Name = "Provider")]
        [StringLength(EventUploadingPropertyMaxLength.Provider, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string Provider { get; set; }

        [Display(Name = "Other Provider")]
        [StringLength(EventUploadingPropertyMaxLength.OtherProvider, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string OtherProvider { get; set; }

        [Display(Name = "Service")]
        [RegularExpression(@"(?i:^(InPlay|Stadium|Latent)*$)",
            ErrorMessage = "[Column: {0}] - The Service must be InPlay, Stadium, Latent")]
        public string Service { get; set; }

        [Display(Name = "IPTV")]
        [StringLength(EventUploadingPropertyMaxLength.IPTV, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string IPTV { get; set; }

        [Display(Name = "Streaming Info (WB)")]
        [StringLength(EventUploadingPropertyMaxLength.WatchBet, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string WB { get; set; }

        [Display(Name = "Remark")]
        [StringLength(EventUploadingPropertyMaxLength.Remark, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string Remark { get; set; }

        // Basketball
        [Display(Name = "Trader Basketball FT (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketballFTHDP { get; set; }

        [Display(Name = "Trader Basketball 1H (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketball1HHDP { get; set; }

        [Display(Name = "Trader Basketball 1Q & 3Q (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketball1Q3QHDP { get; set; }

        [Display(Name = "Trader Basketball FT (OU/OE)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketballFTOUOE { get; set; }

        [Display(Name = "Trader Basketball 1H (OU/OE)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketball1HOUOE { get; set; }

        [Display(Name = "Trader Basketball 1Q & 3Q (OU/OE)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketball1Q3QOUOE { get; set; }

        // Football
        [Display(Name = "Trader Football FT (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootballFTHDP { get; set; }

        [Display(Name = "Trader Football 1H (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootball1HHDP { get; set; }

        [Display(Name = "Trader Football 1Q & 3Q (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootball1Q3QHDP { get; set; }

        [Display(Name = "Trader Football FT (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootballFTOU { get; set; }

        [Display(Name = "Trader Football 1H (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootball1HOU { get; set; }

        [Display(Name = "Trader Football 1Q & 3Q (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootball1Q3QOU { get; set; }

        // Field Hockey
        [Display(Name = "Trader Field Hockey FT (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockeyFTHDP { get; set; }

        [Display(Name = "Trader Field Hockey 1H (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockey1HHDP { get; set; }

        [Display(Name = "Trader Field Hockey 1Q (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockey1QHDP { get; set; }

        [Display(Name = "Trader Field Hockey FT (O/U)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockeyFTOU { get; set; }

        [Display(Name = "Trader Field Hockey 1H (O/U)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockey1HOU { get; set; }

        [Display(Name = "Trader Field Hockey 1Q (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockey1QOU { get; set; }

        // Field Hockey
        [Display(Name = "Trader Ice Hockey FT (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderIceHockeyFTHDP { get; set; }

        [Display(Name = "Trader Ice Hockey 1st-2nd Periods (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderIceHockey1st2ndPeriodsHDP { get; set; }

        [Display(Name = "Trader Ice Hockey FT (O/U)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderIceHockeyFTOU { get; set; }

        [Display(Name = "Trader Ice Hockey 1st-2nd Periods (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderIceHockey1st2ndPeriodsOU { get; set; }

        // Other sport
        [Display(Name = "Trader Other Sports Match Winner")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderOtherSportsMatchWinner { get; set; }

        [Display(Name = "Trader Other Sports FT (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderOtherSportsFTHDP { get; set; }

        [Display(Name = "Trader Other Sports 1H (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderOtherSports1HHDP { get; set; }

        [Display(Name = "Trader Other Sports FT (O/U)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderOtherSportsFTOU { get; set; }

        [Display(Name = "Trader Other Sports 1H (O/U)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderOtherSports1HOU { get; set; }

        // Darts Legs
        [Display(Name = "Trader Darts Legs (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderDartsLegsHDP { get; set; }

        [Display(Name = "Trader Darts Most 180s (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderDartsMost180sHDP { get; set; }

        // Pool Match
        [Display(Name = "Trader Pool Match Winner (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderPoolMatchWinnerHDP { get; set; }

        [Display(Name = "Trader Pool Racks Winner (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderPoolRacksWinnerHDP { get; set; }

        // Table Tennis
        [Display(Name = "Trader Table Tennis Set Winner (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderTableTennisSetWinnerHDP { get; set; }

        [Display(Name = "Trader Table Tennis Set x Winner (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderTableTennisSetXWinnerHDP { get; set; }

        // Volleyball
        [Display(Name = "Trader Volleyball Set Winner (HDP/OE)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderVolleyballSetWinnerHDPOE { get; set; }

        [Display(Name = "Trader Volleyball Sets x Point Winner (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderVolleyballSetsXPointWinnerHDP { get; set; }

        // Handball
        [Display(Name = "Trader Handball FT (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderHandballFTHDP { get; set; }

        [Display(Name = "Trader Handball 1H (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderHandball1HHDP { get; set; }

        [Display(Name = "Trader Handball FT (O/U)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderHandballFTOU { get; set; }

        [Display(Name = "Trader Handball 1H (O/U)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderHandball1HOU { get; set; }

        [Display(Name = "Trader Netball FT (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderNetballFTHDP { get; set; }

        [Display(Name = "Trader Netball 1H (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderNetball1HHDP { get; set; }

        [Display(Name = "Trader Netball 1Q & 3Q (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderNetball1Q3QHDP { get; set; }

        [Display(Name = "Trader Netball FT (OU/OE)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderNetballFTOUOE { get; set; }

        [Display(Name = "Trader Netball 1H (OU/OE)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderNetball1HOUOE { get; set; }

        [Display(Name = "Trader Netball 1Q & 3Q (OU/OE)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderNetball1Q3QOUOE { get; set; }

        [Display(Name = "Trader Cricket Match Winner & Innings (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderCricketMatchWinnerInningsOU { get; set; }

        [Display(Name = "Trader Cricket Next Over (OU) & Player (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderCricketNextOverOUPlayerOU { get; set; }

        [Display(Name = "Trader Cricket FT 1X2")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderCricketFT1X2 { get; set; }

        [Display(Name = "Trader Snooker Match Winner (HDP/OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderSnookerMatchWinnerHDPOU { get; set; }

        [Display(Name = "Trader Snooker Frames Winner (HDP/OU/OE)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderSnookerFramesWinnerHDPOUOE { get; set; }

        [Display(Name = "Trader Snooker First 5 Frames (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderSnookerFirst5FramesHDP { get; set; }

        [Display(Name = "Trader Snooker First 8 Frames (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderSnookerFirst8FramesHDP { get; set; }

        [Display(Name = "Trader Snooker First 9 Frames (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderSnookerFirst9FramesHDP { get; set; }

        [Display(Name = "Trader Baseball FT Run Line (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBaseballFTRunLineHDP { get; set; }

        [Display(Name = "Trader Baseball 1H 1st 5 innings (HDP)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBaseball1H1st5inningsHDP { get; set; }

        [Display(Name = "Trader Baseball FT Total Run (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBaseballFTTotalRunOU { get; set; }

        [Display(Name = "Trader Baseball 1H 1st 5 innings (OU)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBaseball1H1st5inningsOU { get; set; }

        [Display(Name = "Trader Baseball Inning Hits & Runs")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBaseballInningHitsRuns { get; set; }

        [Display(Name = "Trader Badminton All Product (Set Winner)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBadmintonAllProductSetWinner { get; set; }

        [Display(Name = "Trader Badminton All Product (Point Winner)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBadmintonAllProductPointWinner { get; set; }

        [Display(Name = "Trader Badminton Race to Points (7, 9, 11)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBadmintonRacetoPoints7911 { get; set; }

        [Display(Name = "Trader Badminton Next Point Winner")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBadmintonNextPointWinner { get; set; }

        [Display(Name = "Trader Badminton Match Handicap")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBadmintonMatchHandicap { get; set; }

        [Display(Name = "Trader Tennis All Product (LS)")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderTennisAllProductLS { get; set; }

        [Display(Name = "Trader Tennis Next Game")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderTennisNextGame { get; set; }

        [Display(Name = "Trader Tennis Most Aces")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderTennisMostAces { get; set; }

        [Display(Name = "Trader Tennis Most Double Faults")]
        [StringLength(EventUploadingPropertyMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderTennisMostDoubleFaults { get; set; }

        [Display(Name = "Other Trader Basketball FT (HDP)")]
        public string OtherTraderBasketballFTHDP { get; set; }

        [Display(Name = "Other Trader Basketball 1H (HDP)")]
        public string OtherTraderBasketball1HHDP { get; set; }

        [Display(Name = "Other Trader Basketball 1Q & 3Q (HDP)")]
        public string OtherTraderBasketball1Q3QHDP { get; set; }

        [Display(Name = "Other Trader Basketball FT (OU/OE)")]
        public string OtherTraderBasketballFTOUOE { get; set; }

        [Display(Name = "Other Trader Basketball 1H (OU/OE)")]
        public string OtherTraderBasketball1HOUOE { get; set; }

        [Display(Name = "Other Trader Basketball 1Q & 3Q (OU/OE)")]
        public string OtherTraderBasketball1Q3QOUOE { get; set; }

        [Display(Name = "Other Trader Football FT (HDP)")]
        public string OtherTraderFootballFTHDP { get; set; }

        [Display(Name = "Other Trader Football 1H (HDP)")]
        public string OtherTraderFootball1HHDP { get; set; }

        [Display(Name = "Other Trader Football 1Q & 3Q (HDP)")]
        public string OtherTraderFootball1Q3QHDP { get; set; }

        [Display(Name = "Other Trader Football FT (OU)")]
        public string OtherTraderFootballFTOU { get; set; }

        [Display(Name = "Other Trader Football 1H (OU)")]
        public string OtherTraderFootball1HOU { get; set; }

        [Display(Name = "Other Trader Football 1Q & 3Q (OU)")]
        public string OtherTraderFootball1Q3QOU { get; set; }

        [Display(Name = "Other Trader Field Hockey FT (HDP)")]
        public string OtherTraderFieldHockeyFTHDP { get; set; }

        [Display(Name = "Other Trader Field Hockey 1H (HDP)")]
        public string OtherTraderFieldHockey1HHDP { get; set; }

        [Display(Name = "Other Trader Field Hockey 1Q (HDP)")]
        public string OtherTraderFieldHockey1QHDP { get; set; }

        [Display(Name = "Other Trader Field Hockey FT (O/U)")]
        public string OtherTraderFieldHockeyFTOU { get; set; }

        [Display(Name = "Other Trader Field Hockey 1H (O/U)")]
        public string OtherTraderFieldHockey1HOU { get; set; }

        [Display(Name = "Other Trader Field Hockey 1Q (OU)")]
        public string OtherTraderFieldHockey1QOU { get; set; }

        [Display(Name = "Other Trader Ice Hockey FT (HDP)")]
        public string OtherTraderIceHockeyFTHDP { get; set; }

        [Display(Name = "Other Trader Ice Hockey 1st-2nd Periods (HDP)")]
        public string OtherTraderIceHockey1st2ndPeriodsHDP { get; set; }

        [Display(Name = "Other Trader Ice Hockey FT (O/U)")]
        public string OtherTraderIceHockeyFTOU { get; set; }

        [Display(Name = "Other Trader Ice Hockey 1st-2nd Periods (OU)")]
        public string OtherTraderIceHockey1st2ndPeriodsOU { get; set; }

        [Display(Name = "Other Trader Other Sports Match Winner")]
        public string OtherTraderOtherSportsMatchWinner { get; set; }

        [Display(Name = "Other Trader Other Sports FT (HDP)")]
        public string OtherTraderOtherSportsFTHDP { get; set; }

        [Display(Name = "Other Trader Other Sports 1H (HDP)")]
        public string OtherTraderOtherSports1HHDP { get; set; }

        [Display(Name = "Other Trader Other Sports FT (O/U)")]
        public string OtherTraderOtherSportsFTOU { get; set; }

        [Display(Name = "Other Trader Other Sports 1H (O/U)")]
        public string OtherTraderOtherSports1HOU { get; set; }

        [Display(Name = "Other Trader Darts Legs (HDP)")]
        public string OtherTraderDartsLegsHDP { get; set; }

        [Display(Name = "Other Trader Darts Most 180s (HDP)")]
        public string OtherTraderDartsMost180sHDP { get; set; }

        [Display(Name = "Other Trader Pool Match Winner (HDP)")]
        public string OtherTraderPoolMatchWinnerHDP { get; set; }

        [Display(Name = "Other Trader Pool Racks Winner (HDP)")]
        public string OtherTraderPoolRacksWinnerHDP { get; set; }

        [Display(Name = "Other Trader Table Tennis Set Winner (HDP)")]
        public string OtherTraderTableTennisSetWinnerHDP { get; set; }

        [Display(Name = "Other Trader Table Tennis Set x Winner (HDP)")]
        public string OtherTraderTableTennisSetXWinnerHDP { get; set; }

        [Display(Name = "Other Trader Volleyball Set Winner (HDP/OE)")]
        public string OtherTraderVolleyballSetWinnerHDPOE { get; set; }

        [Display(Name = "Other Trader Volleyball Sets x Point Winner (HDP)")]
        public string OtherTraderVolleyballSetsXPointWinnerHDP { get; set; }

        [Display(Name = "Other Trader Handball FT (HDP)")]
        public string OtherTraderHandballFTHDP { get; set; }

        [Display(Name = "Other Trader Handball 1H (HDP)")]
        public string OtherTraderHandball1HHDP { get; set; }

        [Display(Name = "Other Trader Handball FT (O/U)")]
        public string OtherTraderHandballFTOU { get; set; }

        [Display(Name = "Other Trader Handball 1H (O/U)")]
        public string OtherTraderHandball1HOU { get; set; }

        [Display(Name = "Other Trader Netball FT (HDP)")]
        public string OtherTraderNetballFTHDP { get; set; }

        [Display(Name = "Other Trader Netball 1H (HDP)")]
        public string OtherTraderNetball1HHDP { get; set; }

        [Display(Name = "Other Trader Netball 1Q & 3Q (HDP)")]
        public string OtherTraderNetball1Q3QHDP { get; set; }

        [Display(Name = "Other Trader Netball FT (OU/OE)")]
        public string OtherTraderNetballFTOUOE { get; set; }

        [Display(Name = "Other Trader Netball 1H (OU/OE)")]
        public string OtherTraderNetball1HOUOE { get; set; }

        [Display(Name = "Other Trader Netball 1Q & 3Q (OU/OE)")]
        public string OtherTraderNetball1Q3QOUOE { get; set; }

        [Display(Name = "Other Trader Cricket Match Winner & Innings (OU)")]
        public string OtherTraderCricketMatchWinnerInningsOU { get; set; }

        [Display(Name = "Other Trader Cricket Next Over (OU) & Player (OU)")]
        public string OtherTraderCricketNextOverOUPlayerOU { get; set; }

        [Display(Name = "Other Trader Cricket FT 1X2")]
        public string OtherTraderCricketFT1X2 { get; set; }

        [Display(Name = "Other Trader Snooker Match Winner (HDP/OU)")]
        public string OtherTraderSnookerMatchWinnerHDPOU { get; set; }

        [Display(Name = "Other Trader Snooker Frames Winner (HDP/OU/OE)")]
        public string OtherTraderSnookerFramesWinnerHDPOUOE { get; set; }

        [Display(Name = "Other Trader Snooker First 5 Frames (HDP)")]
        public string OtherTraderSnookerFirst5FramesHDP { get; set; }

        [Display(Name = "Other Trader Snooker First 8 Frames (HDP)")]
        public string OtherTraderSnookerFirst8FramesHDP { get; set; }

        [Display(Name = "Other Trader Snooker First 9 Frames (HDP)")]
        public string OtherTraderSnookerFirst9FramesHDP { get; set; }

        [Display(Name = "Other Trader Baseball FT Run Line (HDP)")]
        public string OtherTraderBaseballFTRunLineHDP { get; set; }

        [Display(Name = "Other Trader Baseball 1H 1st 5 innings (HDP)")]
        public string OtherTraderBaseball1H1st5inningsHDP { get; set; }

        [Display(Name = "Other Trader Baseball FT Total Run (OU)")]
        public string OtherTraderBaseballFTTotalRunOU { get; set; }

        [Display(Name = "Other Trader Baseball 1H 1st 5 innings (OU)")]
        public string OtherTraderBaseball1H1st5inningsOU { get; set; }

        [Display(Name = "Other Trader Baseball Inning Hits & Runs")]
        public string OtherTraderBaseballInningHitsRuns { get; set; }

        [Display(Name = "Other Trader Badminton All Product (Set Winner)")]
        public string OtherTraderBadmintonAllProductSetWinner { get; set; }

        [Display(Name = "Other Trader Badminton All Product (Point Winner)")]
        public string OtherTraderBadmintonAllProductPointWinner { get; set; }

        [Display(Name = "Other Trader Badminton Race to Points (7, 9, 11)")]
        public string OtherTraderBadmintonRacetoPoints7911 { get; set; }

        [Display(Name = "Other Trader Badminton Next Point Winner")]
        public string OtherTraderBadmintonNextPointWinner { get; set; }

        [Display(Name = "Other Trader Badminton Match Handicap")]
        public string OtherTraderBadmintonMatchHandicap { get; set; }

        [Display(Name = "Other Trader Tennis All Product (LS)")]
        public string OtherTraderTennisAllProductLS { get; set; }

        [Display(Name = "Other Trader Tennis Next Game")]
        public string OtherTraderTennisNextGame { get; set; }

        [Display(Name = "Other Trader Tennis Most Aces")]
        public string OtherTraderTennisMostAces { get; set; }

        [Display(Name = "Other Trader Tennis Most Double Faults")]
        public string OtherTraderTennisMostDoubleFaults { get; set; }

        [Display(Name = "Max Bet Basketball FT (HDP)")]
        public string MaxBetBasketballFTHDP { get; set; }

        [Display(Name = "Max Bet Basketball FT (OU/OE)")]
        public string MaxBetBasketballFTOUOE { get; set; }

        [Display(Name = "Max Bet Football FT (HDP)")]
        public string MaxBetFootballFTHDP { get; set; }

        [Display(Name = "Max Bet Football 1H (HDP)")]
        public string MaxBetFootball1HHDP { get; set; }

        [Display(Name = "Max Bet Football 1Q & 3Q (HDP)")]
        public string MaxBetFootball1Q3QHDP { get; set; }

        [Display(Name = "Max Bet Football FT (OU)")]
        public string MaxBetFootballFTOU { get; set; }

        [Display(Name = "Max Bet Football 1H (OU)")]
        public string MaxBetFootball1HOU { get; set; }

        [Display(Name = "Max Bet Football 1Q & 3Q (OU)")]
        public string MaxBetFootball1Q3QOU { get; set; }

        [Display(Name = "Max Bet Field Hockey FT (HDP)")]
        public string MaxBetFieldHockeyFTHDP { get; set; }

        [Display(Name = "Max Bet Field Hockey 1H (HDP)")]
        public string MaxBetFieldHockey1HHDP { get; set; }

        [Display(Name = "Max Bet Field Hockey 1Q (HDP)")]
        public string MaxBetFieldHockey1QHDP { get; set; }

        [Display(Name = "Max Bet Field Hockey FT (O/U)")]
        public string MaxBetFieldHockeyFTOU { get; set; }

        [Display(Name = "Max Bet Field Hockey 1H (O/U)")]
        public string MaxBetFieldHockey1HOU { get; set; }

        [Display(Name = "Max Bet Field Hockey 1Q (OU)")]
        public string MaxBetFieldHockey1QOU { get; set; }

        [Display(Name = "Max Bet Ice Hockey FT (HDP)")]
        public string MaxBetIceHockeyFTHDP { get; set; }

        [Display(Name = "Max Bet Ice Hockey 1st-2nd Periods (HDP)")]
        public string MaxBetIceHockey1st2ndPeriodsHDP { get; set; }

        [Display(Name = "Max Bet Ice Hockey FT (O/U)")]
        public string MaxBetIceHockeyFTOU { get; set; }

        [Display(Name = "Max Bet Ice Hockey 1st-2nd Periods (OU)")]
        public string MaxBetIceHockey1st2ndPeriodsOU { get; set; }

        [Display(Name = "Max Bet Other Sports Match Winner")]
        public string MaxBetOtherSportsMatchWinner { get; set; }

        [Display(Name = "Max Bet Other Sports FT (HDP)")]
        public string MaxBetOtherSportsFTHDP { get; set; }

        [Display(Name = "Max Bet Other Sports 1H (HDP)")]
        public string MaxBetOtherSports1HHDP { get; set; }

        [Display(Name = "Max Bet Other Sports FT (O/U)")]
        public string MaxBetOtherSportsFTOU { get; set; }

        [Display(Name = "Max Bet Other Sports 1H (O/U)")]
        public string MaxBetOtherSports1HOU { get; set; }

        [Display(Name = "Max Bet Darts Legs (HDP)")]
        public string MaxBetDartsLegsHDP { get; set; }

        [Display(Name = "Max Bet Darts Most 180s (HDP)")]
        public string MaxBetDartsMost180sHDP { get; set; }

        [Display(Name = "Max Bet Pool Match Winner (HDP)")]
        public string MaxBetPoolMatchWinnerHDP { get; set; }

        [Display(Name = "Max Bet Pool Racks Winner (HDP)")]
        public string MaxBetPoolRacksWinnerHDP { get; set; }

        [Display(Name = "Max Bet Table Tennis Set Winner (HDP)")]
        public string MaxBetTableTennisSetWinnerHDP { get; set; }

        [Display(Name = "Max Bet Table Tennis Set x Winner (HDP)")]
        public string MaxBetTableTennisSetXWinnerHDP { get; set; }

        [Display(Name = "Max Bet Volleyball Set Winner (HDP/OE)")]
        public string MaxBetVolleyballSetWinnerHDPOE { get; set; }

        [Display(Name = "Max Bet Volleyball Sets x Point Winner (HDP)")]
        public string MaxBetVolleyballSetsXPointWinnerHDP { get; set; }

        [Display(Name = "Max Bet Handball FT (HDP)")]
        public string MaxBetHandballFTHDP { get; set; }

        [Display(Name = "Max Bet Handball 1H (HDP)")]
        public string MaxBetHandball1HHDP { get; set; }

        [Display(Name = "Max Bet Handball FT (O/U)")]
        public string MaxBetHandballFTOU { get; set; }

        [Display(Name = "Max Bet Handball 1H (O/U)")]
        public string MaxBetHandball1HOU { get; set; }

        [Display(Name = "Max Bet Netball FT (HDP)")]
        public string MaxBetNetballFTHDP { get; set; }

        [Display(Name = "Max Bet Netball 1H (HDP)")]
        public string MaxBetNetball1HHDP { get; set; }

        [Display(Name = "Max Bet Netball 1Q & 3Q (HDP)")]
        public string MaxBetNetball1Q3QHDP { get; set; }

        [Display(Name = "Max Bet Netball FT (OU/OE)")]
        public string MaxBetNetballFTOUOE { get; set; }

        [Display(Name = "Max Bet Netball 1H (OU/OE)")]
        public string MaxBetNetball1HOUOE { get; set; }

        [Display(Name = "Max Bet Netball 1Q & 3Q (OU/OE)")]
        public string MaxBetNetball1Q3QOUOE { get; set; }

        [Display(Name = "Max Bet Cricket Match Winner & Innings (OU)")]
        public string MaxBetCricketMatchWinnerInningsOU { get; set; }

        [Display(Name = "Max Bet Cricket Next Over (OU) & Player (OU)")]
        public string MaxBetCricketNextOverOUPlayerOU { get; set; }

        [Display(Name = "Max Bet Cricket FT 1X2")]
        public string MaxBetCricketFT1X2 { get; set; }

        [Display(Name = "Max Bet Snooker Match Winner (HDP/OU)")]
        public string MaxBetSnookerMatchWinnerHDPOU { get; set; }

        [Display(Name = "Max Bet Snooker Frames Winner (HDP/OU/OE)")]
        public string MaxBetSnookerFramesWinnerHDPOUOE { get; set; }

        [Display(Name = "Max Bet Snooker First 5 Frames (HDP)")]
        public string MaxBetSnookerFirst5FramesHDP { get; set; }

        [Display(Name = "Max Bet Snooker First 8 Frames (HDP)")]
        public string MaxBetSnookerFirst8FramesHDP { get; set; }

        [Display(Name = "Max Bet Snooker First 9 Frames (HDP)")]
        public string MaxBetSnookerFirst9FramesHDP { get; set; }

        [Display(Name = "Max Bet Baseball FT Run Line (HDP)")]
        public string MaxBetBaseballFTRunLineHDP { get; set; }

        [Display(Name = "Max Bet Baseball 1H 1st 5 innings (HDP)")]
        public string MaxBetBaseball1H1st5inningsHDP { get; set; }

        [Display(Name = "Max Bet Baseball FT Total Run (OU)")]
        public string MaxBetBaseballFTTotalRunOU { get; set; }

        [Display(Name = "Max Bet Baseball 1H 1st 5 innings (OU)")]
        public string MaxBetBaseball1H1st5inningsOU { get; set; }

        [Display(Name = "Max Bet Baseball Inning Hits & Runs")]
        public string MaxBetBaseballInningHitsRuns { get; set; }

        [Display(Name = "Max Bet Badminton All Product (Set Winner)")]
        public string MaxBetBadmintonAllProductSetWinner { get; set; }

        [Display(Name = "Max Bet Badminton All Product (Point Winner)")]
        public string MaxBetBadmintonAllProductPointWinner { get; set; }

        [Display(Name = "Max Bet Badminton Race to Points (7, 9, 11)")]
        public string MaxBetBadmintonRacetoPoints7911 { get; set; }

        [Display(Name = "Max Bet Badminton Next Point Winner")]
        public string MaxBetBadmintonNextPointWinner { get; set; }

        [Display(Name = "Max Bet Badminton Match Handicap")]
        public string MaxBetBadmintonMatchHandicap { get; set; }

        [Display(Name = "Max Bet Tennis All Product (LS)")]
        public string MaxBetTennisAllProductLS { get; set; }

        [Display(Name = "Max Bet Tennis Next Game")]
        public string MaxBetTennisNextGame { get; set; }

        [Display(Name = "Max Bet Tennis Most Aces")]
        public string MaxBetTennisMostAces { get; set; }

        [Display(Name = "Max Bet Tennis Most Double Faults")]
        public string MaxBetTennisMostDoubleFaults { get; set; }
    }
}
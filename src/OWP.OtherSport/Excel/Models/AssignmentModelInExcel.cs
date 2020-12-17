namespace OWP.OtherSport.Excel.Models
{
    using System.ComponentModel.DataAnnotations;
    using OWP.OtherSport.Excel.Annotations;
    using OWP.OtherSport.Excel.Constants;

    public class AssignmentModelInExcel
    {
        [Display(Name = "Match Status")]
        [RegularExpression(@"\b^(?:Live|Canceled|Postponed)*$\b",
            ErrorMessage = "[Column: {0}] - The Match Status must be Live, Canceled or Postponed")]
        public string MatchStatus { get; set; }

        [Display(Name = "Sport")]
        [Sport(ErrorMessage = "[Column: {0}] - Invalid sport")]
        public string Sport { get; set; }

        [Display(Name = "Date Time")]
        [StringLength(EventUploadingMaxLength.DateTime, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"[0-1]\d\/[0-3]\d\/\d{4} [0-1]\d:[0-5]\d [AP][M]", ErrorMessage = "[Column: {0}] - Error: Invalid date time format")]
        public string DateTime { get; set; }

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

        [Display(Name = "Category")]
        [StringLength(EventUploadingMaxLength.Category, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string Category { get; set; }

        [Display(Name = "No.HDP")]
        [StringLength(EventUploadingMaxLength.NoHDP, ErrorMessage = "[Column: {0}] - The item's value must not exceed {1} characters")]
        public string NoHDP { get; set; }

        [Display(Name = "Match Offer")]
        [StringLength(EventUploadingMaxLength.MatchOffer, ErrorMessage = "[Column: {0}] - The item's value must not exceed {1} characters")]
        public string MatchOffer { get; set; }

        [Display(Name = "Auto Link")]
        [StringLength(EventUploadingMaxLength.AutoLink,
            MinimumLength = 1,
            ErrorMessage = "[Column: {0}] - The item's value must be not empty and must not exceed {1} characters")]
        public string AutoLink { get; set; }

        [Display(Name = "Provider")]
        [StringLength(EventUploadingMaxLength.Provider, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string Provider { get; set; }

        [Display(Name = "Other Provider")]
        [StringLength(EventUploadingMaxLength.OtherProvider, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string OtherProvider { get; set; }

        [Display(Name = "Service")]
        [RegularExpression(@"^$|\b^(?:Live|Canceled|Postponed)*$\b",
            ErrorMessage = "[Column: {0}] - The Service must be empty or InPlay, Stadium, Latent")]
        public string Service { get; set; }

        [Display(Name = "IPTV")]
        [StringLength(EventUploadingMaxLength.IPTV, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string IPTV { get; set; }

        [Display(Name = "Streaming Info (WB)")]
        [StringLength(EventUploadingMaxLength.WatchBet, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string WB { get; set; }

        [Display(Name = "Remark")]
        [StringLength(EventUploadingMaxLength.Remark, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        public string Remark { get; set; }

        // Basketball
        [Display(Name = "Trader Basketball FT (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketballFTHDP { get; set; }

        [Display(Name = "Trader Basketball 1H (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketball1HHDP { get; set; }

        [Display(Name = "Trader Basketball 1Q & 3Q (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketball1Q3QHDP { get; set; }

        [Display(Name = "Trader Basketball FT (OU/OE)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketballFTOUOE { get; set; }

        [Display(Name = "Trader Basketball 1H (OU/OE)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketball1HOUOE { get; set; }

        [Display(Name = "Trader Basketball 1Q & 3Q (OU/OE)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderBasketball1Q3QOUOE { get; set; }

        // Football
        [Display(Name = "Trader Football FT (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootballFTHDP { get; set; }

        [Display(Name = "Trader Football 1H (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootball1HHDP { get; set; }

        [Display(Name = "Trader Football 1Q & 3Q (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootball1Q3QHDP { get; set; }

        [Display(Name = "Trader Football FT (OU)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootballFTOU { get; set; }

        [Display(Name = "Trader Football 1H (OU)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootball1HOU { get; set; }

        [Display(Name = "Trader Football 1Q & 3Q (OU)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFootball1Q3QOU { get; set; }

        // Field Hockey
        [Display(Name = "Trader Field Hockey FT (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockeyFTHDP { get; set; }

        [Display(Name = "Trader Field Hockey 1H (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockey1HHDP { get; set; }

        [Display(Name = "Trader Field Hockey 1Q (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockey1QHDP { get; set; }

        [Display(Name = "Trader Field Hockey FT (O/U)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockeyFTOU { get; set; }

        [Display(Name = "Trader Field Hockey 1H (O/U)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockey1HOU { get; set; }

        [Display(Name = "Trader Field Hockey 1Q (OU)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderFieldHockey1QOU { get; set; }

        // Field Hockey
        [Display(Name = "Trader Ice Hockey FT (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderIceHockeyFTHDP { get; set; }

        [Display(Name = "Trader Ice Hockey 1st-2nd Periods (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderIceHockey1st2ndHDP { get; set; }

        [Display(Name = "Trader Ice Hockey FT (O/U)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderIceHockeyFTOU { get; set; }

        [Display(Name = "Trader Ice Hockey 1st-2nd Periods (OU)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderIceHockey1st2ndOU { get; set; }

        // Other sport
        [Display(Name = "Trader Other Sports Match Winner")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderOtherSportsMatchWinner { get; set; }

        // Darts Legs
        [Display(Name = "Trader Darts Legs (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderDartsLegsHDP { get; set; }

        [Display(Name = "Trader Darts Most 180s (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderDartsMost180sHDP { get; set; }

        // Pool Match
        [Display(Name = "Trader Pool Match Winner (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderPoolMatchWinnerHDP { get; set; }

        [Display(Name = "Trader Pool Racks Winner (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderPoolRacksWinnerHDP { get; set; }

        // Table Tennis
        [Display(Name = "Trader Table Tennis Set Winner (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderTableTennisSetWinnerHDP { get; set; }

        [Display(Name = "Trader Table Tennis Set x Winner (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderTableTennisSetXWinnerHDP { get; set; }

        // Volleyball
        [Display(Name = "Trader Volleyball Set Winner (HDP/OE)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderVolleyballSetWinnerHDPOE { get; set; }

        [Display(Name = "Trader Volleyball Sets x Point Winner (HDP)")]
        [StringLength(EventUploadingMaxLength.Trader, ErrorMessage = "[Column: {0}] - Error: The item's value must not exceed {1} characters")]
        [RegularExpression(@"^[^,]*$", ErrorMessage = "[Column: {0}] - Error: Trader name must not contain character ','")]
        public string TraderVolleyballSetsXPointWinnerHDP { get; set; }

        [Display(Name = "Max Bet (1H 1X2)")]
        public decimal? MaxBet1H1X2 { get; set; }
    }
}
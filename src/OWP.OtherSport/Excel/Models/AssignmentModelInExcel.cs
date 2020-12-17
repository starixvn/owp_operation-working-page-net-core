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

        public string TraderHandballFTHDP { get; set; }
        public string TraderHandball1HHDP { get; set; }
        public string TraderHandballFTOU { get; set; }
        public string TraderHandball1HOU { get; set; }
        public string TraderNetballFTHDP { get; set; }
        public string TraderNetball1HHDP { get; set; }
        public string TraderNetball1Q3QHDP { get; set; }
        public string TraderNetballFTOUOE { get; set; }
        public string TraderNetball1HOUOE { get; set; }
        public string TraderNetball1Q3QOUOE { get; set; }
        public string TraderCricketMatchWinnerInningsOU { get; set; }
        public string TraderCricketNextOverOUPlayerOU { get; set; }
        public string TraderCricketFT1X2 { get; set; }
        public string TraderSnookerMatchWinnerHDPOU { get; set; }
        public string TraderSnookerFramesWinnerHDPOUOE { get; set; }
        public string TraderSnookerFirst5FramesHDP { get; set; }
        public string TraderSnookerFirst8FramesHDP { get; set; }
        public string TraderSnookerFirst9FramesHDP { get; set; }
        public string TraderBaseballFTRunLineHDP { get; set; }
        public string TraderBaseball1H1st5InningsHDP { get; set; }
        public string TraderBaseballFTTotalRunOU { get; set; }
        public string TraderBaseball1H1st5InningsOU { get; set; }
        public string TraderBaseballInningHitsRuns { get; set; }
        public string TraderBadmintonAllProductSetWinner { get; set; }
        public string TraderBadmintonAllProductPointWinner { get; set; }
        public string TraderBadmintonRacetoPoints7911 { get; set; }
        public string TraderBadmintonNextPointWinner { get; set; }
        public string TraderBadmintonMatchHandicap { get; set; }
        public string TraderTennisAllProductLS { get; set; }
        public string TraderTennisNextGame { get; set; }
        public string TraderTennisMostAces { get; set; }
        public string TraderTennisMostDoubleFaults { get; set; }

        public string OtherTraderBasketballFTHDP { get; set; }
        public string OtherTraderBasketball1HHDP { get; set; }
        public string OtherTraderBasketball1Q3QHDP { get; set; }
        public string OtherTraderBasketballFTOUOE { get; set; }
        public string OtherTraderBasketball1HOUOE { get; set; }
        public string OtherTraderBasketball1Q3QOUOE { get; set; }
        public string OtherTraderFootballFTHDP { get; set; }
        public string OtherTraderFootball1HHDP { get; set; }
        public string OtherTraderFootball1Q3QHDP { get; set; }
        public string OtherTraderFootballFTOU { get; set; }
        public string OtherTraderFootball1HOU { get; set; }
        public string OtherTraderFootball1Q3QOU { get; set; }
        public string OtherTraderFieldHockeyFTHDP { get; set; }
        public string OtherTraderFieldHockey1HHDP { get; set; }
        public string OtherTraderFieldHockey1QHDP { get; set; }
        public string OtherTraderFieldHockeyFTOU { get; set; }
        public string OtherTraderFieldHockey1HOU { get; set; }
        public string OtherTraderFieldHockey1QOU { get; set; }
        public string OtherTraderIceHockeyFTHDP { get; set; }
        public string OtherTraderIceHockey1st2ndPeriodsHDP { get; set; }
        public string OtherTraderIceHockeyFTOU { get; set; }
        public string OtherTraderIceHockey1st2ndPeriodsOU { get; set; }
        public string OtherTraderOtherSportsMatchWinner { get; set; }
        public string OtherTraderDartsLegsHDP { get; set; }
        public string OtherTraderDartsMost180sHDP { get; set; }
        public string OtherTraderPoolMatchWinnerHDP { get; set; }
        public string OtherTraderPoolRacksWinnerHDP { get; set; }
        public string OtherTraderTableTennisSetWinnerHDP { get; set; }
        public string OtherTraderTableTennisSetXWinnerHDP { get; set; }
        public string OtherTraderVolleyballSetWinnerHDPOU { get; set; }
        public string OtherTraderVolleyballSetsXPointWinnerHDP { get; set; }
        public string OtherTraderHandballFTHDP { get; set; }
        public string OtherTraderHandball1HHDP { get; set; }
        public string OtherTraderHandballFTOU { get; set; }
        public string OtherTraderHandball1HOU { get; set; }
        public string OtherTraderNetballFTHDP { get; set; }
        public string OtherTraderNetball1HHDP { get; set; }
        public string OtherTraderNetball1Q3QHDP { get; set; }
        public string OtherTraderNetballFTOUOE { get; set; }
        public string OtherTraderNetball1HOUOE { get; set; }
        public string OtherTraderNetball1Q3QOUOE { get; set; }
        public string OtherTraderCricketMatchWinnerInningsOU { get; set; }
        public string OtherTraderCricketNextOverOUPlayerOU { get; set; }
        public string OtherTraderCricketFT1X2 { get; set; }
        public string OtherTraderSnookerMatchWinnerHDPOU { get; set; }
        public string OtherTraderSnookerFramesWinnerHDPOUOE { get; set; }
        public string OtherTraderSnookerFirst5FramesHDP { get; set; }
        public string OtherTraderSnookerFirst8FramesHDP { get; set; }
        public string OtherTraderSnookerFirst9FramesHDP { get; set; }
        public string OtherTraderBaseballFTRunLineHDP { get; set; }
        public string OtherTraderBaseball1H1st5inningsHDP { get; set; }
        public string OtherTraderBaseballFTTotalRunOU { get; set; }
        public string OtherTraderBaseball1H1st5inningsOU { get; set; }
        public string OtherTraderBaseballInningHitsRuns { get; set; }
        public string OtherTraderBadmintonAllProductSetWinner { get; set; }
        public string OtherTraderBadmintonAllProductPointWinner { get; set; }
        public string OtherTraderBadmintonRacetoPoints7911 { get; set; }
        public string OtherTraderBadmintonNextPointWinner { get; set; }
        public string OtherTraderBadmintonMatchHandicap { get; set; }
        public string OtherTraderTennisAllProductLS { get; set; }
        public string OtherTraderTennisNextGame { get; set; }
        public string OtherTraderTennisMostAces { get; set; }
        public string OtherTraderTennisMostDoubleFaults { get; set; }
        public string MaxBetBasketballFTHDP { get; set; }
        public string MaxBetBasketballFTOE { get; set; }
        public string MaxBetFootballFTHDP { get; set; }
        public string MaxBetFootball1HHDP { get; set; }
        public string MaxBetFootball1Q3QHDP { get; set; }
        public string MaxBetFootballFTOU { get; set; }
        public string MaxBetFootball1HOU { get; set; }
        public string MaxBetFootball1Q3QOU { get; set; }
        public string MaxBetFieldHockeyFTHDP { get; set; }
        public string MaxBetFieldHockey1HHDP { get; set; }
        public string MaxBetFieldHockey1QHDP { get; set; }
        public string MaxBetFieldHockeyFTOU { get; set; }
        public string MaxBetFieldHockey1HOU { get; set; }
        public string MaxBetFieldHockey1QOU { get; set; }
        public string MaxBetIceHockeyFTHDP { get; set; }
        public string MaxBetIceHockey1st2ndPeriodsHDP { get; set; }
        public string MaxBetIceHockeyFTOU { get; set; }
        public string MaxBetIceHockey1st2ndPeriodsOU { get; set; }
        public string MaxBetOtherSportsHDP { get; set; }
        public string MaxBetDartsLegsHDP { get; set; }
        public string MaxBetDartsMost180sHDP { get; set; }
        public string MaxBetPoolMatchWinnerHDP { get; set; }
        public string MaxBetPoolRacksWinnerHDP { get; set; }
        public string MaxBetTableTennisHDP { get; set; }
        public string MaxBetVolleyballSetWinnerHDPOU { get; set; }
        public string MaxBetVolleyballSetsXPointWinnerHDP { get; set; }
        public string MaxBetHandballFTHDP { get; set; }
        public string MaxBetHandball1HHDP { get; set; }
        public string MaxBetHandballFTOU { get; set; }
        public string MaxBetHandball1HOU { get; set; }
        public string MaxBetNetballFTHDP { get; set; }
        public string MaxBetNetball1HHDP { get; set; }
        public string MaxBetNetball1Q3QHDP { get; set; }
        public string MaxBetNetballFTOUOE { get; set; }
        public string MaxBetNetball1HOUOE { get; set; }
        public string MaxBetNetball1Q3QOUOE { get; set; }
        public string MaxBetCricketMatchWinnerInningsOU { get; set; }
        public string MaxBetCricketNextOverOUPlayerOU { get; set; }
        public string MaxBetCricketFT1X2 { get; set; }
        public string MaxBetSnookerMatchWinnerHDPOU { get; set; }
        public string MaxBetSnookerFramesWinnerHDPOUOE { get; set; }
        public string MaxBetSnookerFirst5FramesHDP { get; set; }
        public string MaxBetSnookerFirst8FramesHDP { get; set; }
        public string MaxBetSnookerFirst9FramesHDP { get; set; }
        public string MaxBetBaseballFTRunLineHDP { get; set; }
        public string MaxBetBaseball1H1st5inningsHDP { get; set; }
        public string MaxBetBaseballFTTotalRunOU { get; set; }
        public string MaxBetBaseball1H1st5inningsOU { get; set; }
        public string MaxBetBaseballInningHitsRuns { get; set; }
        public string MaxBetBadmintonAllProductSetWinner { get; set; }
        public string MaxBetBadmintonAllProductPointWinner { get; set; }
        public string MaxBetBadmintonRacetoPoints7911 { get; set; }
        public string MaxBetBadmintonNextPointWinner { get; set; }
        public string MaxBetBadmintonMatchHandicap { get; set; }
        public string MaxBetTennisAllProductLS { get; set; }
        public string MaxBetTennisNextGame { get; set; }
        public string MaxBetTennisMostAces { get; set; }
        public string MaxBetTennisMostDoubleFaults { get; set; }
    }
}
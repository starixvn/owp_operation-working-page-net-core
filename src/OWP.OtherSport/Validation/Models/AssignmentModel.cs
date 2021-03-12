namespace OWP.OtherSport.Validation.Models
{
    using System;

    public class AssignmentModel
    {
        public AssignmentModel()
        {
            MatchStatus = string.Empty;
            Home = string.Empty;
            Away = string.Empty;
            League = string.Empty;
            AutoLink = string.Empty;
            OtherTraderBadmintonAllProductPointWinner = string.Empty;
            OtherTraderBadmintonAllProductSetWinner = string.Empty;
            OtherTraderBadmintonMatchHandicap = string.Empty;
            OtherTraderBadmintonNextPointWinner = string.Empty;
            OtherTraderBadmintonRacetoPoints7911 = string.Empty;
            OtherTraderBaseball1H1st5inningsHDP = string.Empty;
            OtherTraderBaseball1H1st5inningsOU = string.Empty;
            OtherTraderBaseballFTRunLineHDP = string.Empty;
            OtherTraderBaseballFTTotalRunOU = string.Empty;
            OtherTraderBaseballInningHitsRuns = string.Empty;
            OtherTraderBasketball1HHDP = string.Empty;
            OtherTraderBasketball1HOUOE = string.Empty;
            OtherTraderBasketball1Q3QHDP = string.Empty;
            OtherTraderBasketball1Q3QOUOE = string.Empty;
            OtherTraderBasketballFTHDP = string.Empty;
            OtherTraderBasketballFTOUOE = string.Empty;
            OtherTraderCricketFT1X2 = string.Empty;
            OtherTraderCricketMatchWinnerInningsOU = string.Empty;
            OtherTraderCricketNextOverOUPlayerOU = string.Empty;
            OtherTraderDartsLegsHDP = string.Empty;
            OtherTraderDartsMost180sHDP = string.Empty;
            OtherTraderFieldHockey1HHDP = string.Empty;
            OtherTraderFieldHockey1HOU = string.Empty;
            OtherTraderFieldHockey1QHDP = string.Empty;
            OtherTraderFieldHockey1QOU = string.Empty;
            OtherTraderFieldHockeyFTHDP = string.Empty;
            OtherTraderFieldHockeyFTOU = string.Empty;
            OtherTraderFootball1HHDP = string.Empty;
            OtherTraderFootball1HOU = string.Empty;
            OtherTraderFootball1Q3QHDP = string.Empty;
            OtherTraderFootball1Q3QOU = string.Empty;
            OtherTraderFootballFTHDP = string.Empty;
            OtherTraderFootballFTOU = string.Empty;
            OtherTraderHandball1HHDP = string.Empty;
            OtherTraderHandball1HOU = string.Empty;
            OtherTraderHandballFTHDP = string.Empty;
            OtherTraderHandballFTOU = string.Empty;
            OtherTraderIceHockey1st2ndPeriodsHDP = string.Empty;
            OtherTraderIceHockey1st2ndPeriodsOU = string.Empty;
            OtherTraderIceHockeyFTHDP = string.Empty;
            OtherTraderIceHockeyFTOU = string.Empty;
            OtherTraderNetball1HHDP = string.Empty;
            OtherTraderNetball1HOUOE = string.Empty;
            OtherTraderNetball1Q3QHDP = string.Empty;
            OtherTraderNetball1Q3QOUOE = string.Empty;
            OtherTraderNetballFTHDP = string.Empty;
            OtherTraderNetballFTOUOE = string.Empty;
            OtherTraderOtherSportsMatchWinner = string.Empty;
            OtherTraderPoolMatchWinnerHDP = string.Empty;
            OtherTraderPoolRacksWinnerHDP = string.Empty;
            OtherTraderSnookerFirst5FramesHDP = string.Empty;
            OtherTraderSnookerFirst8FramesHDP = string.Empty;
            OtherTraderSnookerFirst9FramesHDP = string.Empty;
            OtherTraderSnookerFramesWinnerHDPOUOE = string.Empty;
            OtherTraderSnookerMatchWinnerHDPOU = string.Empty;
            OtherTraderTableTennisSetWinnerHDP = string.Empty;
            OtherTraderTableTennisSetXWinnerHDP = string.Empty;
            OtherTraderTennisAllProductLS = string.Empty;
            OtherTraderTennisMostAces = string.Empty;
            OtherTraderTennisMostDoubleFaults = string.Empty;
            OtherTraderTennisNextGame = string.Empty;
            OtherTraderVolleyballSetsXPointWinnerHDP = string.Empty;
            OtherTraderVolleyballSetWinnerHDPOE = string.Empty;
        }
        public double MatchNo { get; set; }

        public int OSMatchId { get; set; }

        public string MatchStatus { get; set; }

        public string Sport { get; set; }

        public DateTime DateTime { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public string League { get; set; }

        public string Category { get; set; }

        public string NoHDP { get; set; }

        public string MatchOffer { get; set; }

        public string AutoLink { get; set; }

        public string Provider { get; set; }

        public string OtherProvider { get; set; }

        public string Service { get; set; }

        public string IPTV { get; set; }

        public string WB { get; set; }

        public string Remark { get; set; }

        public string HashKey { get; set; }

        // Basketball
        public int TraderBasketballFTHDP { get; set; }

        public int TraderBasketball1HHDP { get; set; }

        public int TraderBasketball1Q3QHDP { get; set; }

        public int TraderBasketballFTOUOE { get; set; }

        public int TraderBasketball1HOUOE { get; set; }

        public int TraderBasketball1Q3QOUOE { get; set; }

        // Football
        public int TraderFootballFTHDP { get; set; }

        public int TraderFootball1HHDP { get; set; }

        public int TraderFootball1Q3QHDP { get; set; }

        public int TraderFootballFTOU { get; set; }

        public int TraderFootball1HOU { get; set; }

        public int TraderFootball1Q3QOU { get; set; }

        // Field Hockey
        public int TraderFieldHockeyFTHDP { get; set; }

        public int TraderFieldHockey1HHDP { get; set; }

        public int TraderFieldHockey1QHDP { get; set; }

        public int TraderFieldHockeyFTOU { get; set; }

        public int TraderFieldHockey1HOU { get; set; }

        public int TraderFieldHockey1QOU { get; set; }

        // Field Hockey
        public int TraderIceHockeyFTHDP { get; set; }

        public int TraderIceHockey1st2ndPeriodsHDP { get; set; }

        public int TraderIceHockeyFTOU { get; set; }

        public int TraderIceHockey1st2ndPeriodsOU { get; set; }

        // Other sport
        public int TraderOtherSportsMatchWinner { get; set; }

        // Darts Legs
        public int TraderDartsLegsHDP { get; set; }

        public int TraderDartsMost180sHDP { get; set; }

        // Pool Match
        public int TraderPoolMatchWinnerHDP { get; set; }

        public int TraderPoolRacksWinnerHDP { get; set; }

        // Table Tennis
        public int TraderTableTennisSetWinnerHDP { get; set; }

        public int TraderTableTennisSetXWinnerHDP { get; set; }

        // Volleyball
        public int TraderVolleyballSetWinnerHDPOE { get; set; }

        public int TraderVolleyballSetsXPointWinnerHDP { get; set; }

        // Handball
        public int TraderHandballFTHDP { get; set; }

        public int TraderHandball1HHDP { get; set; }

        public int TraderHandballFTOU { get; set; }

        public int TraderHandball1HOU { get; set; }

        public int TraderNetballFTHDP { get; set; }

        public int TraderNetball1HHDP { get; set; }

        public int TraderNetball1Q3QHDP { get; set; }

        public int TraderNetballFTOUOE { get; set; }

        public int TraderNetball1HOUOE { get; set; }

        public int TraderNetball1Q3QOUOE { get; set; }

        public int TraderCricketMatchWinnerInningsOU { get; set; }

        public int TraderCricketNextOverOUPlayerOU { get; set; }

        public int TraderCricketFT1X2 { get; set; }

        public int TraderSnookerMatchWinnerHDPOU { get; set; }

        public int TraderSnookerFramesWinnerHDPOUOE { get; set; }

        public int TraderSnookerFirst5FramesHDP { get; set; }

        public int TraderSnookerFirst8FramesHDP { get; set; }

        public int TraderSnookerFirst9FramesHDP { get; set; }

        public int TraderBaseballFTRunLineHDP { get; set; }

        public int TraderBaseball1H1st5inningsHDP { get; set; }

        public int TraderBaseballFTTotalRunOU { get; set; }

        public int TraderBaseball1H1st5inningsOU { get; set; }

        public int TraderBaseballInningHitsRuns { get; set; }

        public int TraderBadmintonAllProductSetWinner { get; set; }

        public int TraderBadmintonAllProductPointWinner { get; set; }

        public int TraderBadmintonRacetoPoints7911 { get; set; }

        public int TraderBadmintonNextPointWinner { get; set; }

        public int TraderBadmintonMatchHandicap { get; set; }

        public int TraderTennisAllProductLS { get; set; }

        public int TraderTennisNextGame { get; set; }

        public int TraderTennisMostAces { get; set; }

        public int TraderTennisMostDoubleFaults { get; set; }

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

        public string OtherTraderVolleyballSetWinnerHDPOE { get; set; }

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

        public decimal? MaxBetBasketballFTHDP { get; set; }

        public decimal? MaxBetBasketballFTOUOE { get; set; }

        public decimal? MaxBetFootballFTHDP { get; set; }

        public decimal? MaxBetFootball1HHDP { get; set; }

        public decimal? MaxBetFootball1Q3QHDP { get; set; }

        public decimal? MaxBetFootballFTOU { get; set; }

        public decimal? MaxBetFootball1HOU { get; set; }

        public decimal? MaxBetFootball1Q3QOU { get; set; }

        public decimal? MaxBetFieldHockeyFTHDP { get; set; }

        public decimal? MaxBetFieldHockey1HHDP { get; set; }

        public decimal? MaxBetFieldHockey1QHDP { get; set; }

        public decimal? MaxBetFieldHockeyFTOU { get; set; }

        public decimal? MaxBetFieldHockey1HOU { get; set; }

        public decimal? MaxBetFieldHockey1QOU { get; set; }

        public decimal? MaxBetIceHockeyFTHDP { get; set; }

        public decimal? MaxBetIceHockey1st2ndPeriodsHDP { get; set; }

        public decimal? MaxBetIceHockeyFTOU { get; set; }

        public decimal? MaxBetIceHockey1st2ndPeriodsOU { get; set; }

        public decimal? MaxBetOtherSportsMatchWinner { get; set; }

        public decimal? MaxBetDartsLegsHDP { get; set; }

        public decimal? MaxBetDartsMost180sHDP { get; set; }

        public decimal? MaxBetPoolMatchWinnerHDP { get; set; }

        public decimal? MaxBetPoolRacksWinnerHDP { get; set; }

        public decimal? MaxBetTableTennisSetWinnerHDP { get; set; }

        public decimal? MaxBetTableTennisSetXWinnerHDP { get; set; }

        public decimal? MaxBetVolleyballSetWinnerHDPOE { get; set; }

        public decimal? MaxBetVolleyballSetsXPointWinnerHDP { get; set; }

        public decimal? MaxBetHandballFTHDP { get; set; }

        public decimal? MaxBetHandball1HHDP { get; set; }

        public decimal? MaxBetHandballFTOU { get; set; }

        public decimal? MaxBetHandball1HOU { get; set; }

        public decimal? MaxBetNetballFTHDP { get; set; }

        public decimal? MaxBetNetball1HHDP { get; set; }

        public decimal? MaxBetNetball1Q3QHDP { get; set; }

        public decimal? MaxBetNetballFTOUOE { get; set; }

        public decimal? MaxBetNetball1HOUOE { get; set; }

        public decimal? MaxBetNetball1Q3QOUOE { get; set; }

        public decimal? MaxBetCricketMatchWinnerInningsOU { get; set; }

        public decimal? MaxBetCricketNextOverOUPlayerOU { get; set; }

        public decimal? MaxBetCricketFT1X2 { get; set; }

        public decimal? MaxBetSnookerMatchWinnerHDPOU { get; set; }

        public decimal? MaxBetSnookerFramesWinnerHDPOUOE { get; set; }

        public decimal? MaxBetSnookerFirst5FramesHDP { get; set; }

        public decimal? MaxBetSnookerFirst8FramesHDP { get; set; }

        public decimal? MaxBetSnookerFirst9FramesHDP { get; set; }

        public decimal? MaxBetBaseballFTRunLineHDP { get; set; }

        public decimal? MaxBetBaseball1H1st5inningsHDP { get; set; }

        public decimal? MaxBetBaseballFTTotalRunOU { get; set; }

        public decimal? MaxBetBaseball1H1st5inningsOU { get; set; }

        public decimal? MaxBetBaseballInningHitsRuns { get; set; }

        public decimal? MaxBetBadmintonAllProductSetWinner { get; set; }

        public decimal? MaxBetBadmintonAllProductPointWinner { get; set; }

        public decimal? MaxBetBadmintonRacetoPoints7911 { get; set; }

        public decimal? MaxBetBadmintonNextPointWinner { get; set; }

        public decimal? MaxBetBadmintonMatchHandicap { get; set; }

        public decimal? MaxBetTennisAllProductLS { get; set; }

        public decimal? MaxBetTennisNextGame { get; set; }

        public decimal? MaxBetTennisMostAces { get; set; }

        public decimal? MaxBetTennisMostDoubleFaults { get; set; }
    }
}
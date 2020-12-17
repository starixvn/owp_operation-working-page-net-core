namespace OWP.OtherSport.Validation.Models
{
    using System;

    public class AssignmentModel
    {
        public string MatchStatus { get; set; }

        public string Sport { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime EventDate { get; set; }

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

        public int TraderIceHockey1st2ndHDP { get; set; }

        public int TraderIceHockeyFTOU { get; set; }

        public int TraderIceHockey1st2ndOU { get; set; }

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

        public int TraderBaseball1H1st5InningsHDP { get; set; }

        public int TraderBaseballFTTotalRunOU { get; set; }

        public int TraderBaseball1H1st5InningsOU { get; set; }

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

        public int OtherTraderBasketballFTHDP { get; set; }

        public int OtherTraderBasketball1HHDP { get; set; }

        public int OtherTraderBasketball1Q3QHDP { get; set; }

        public int OtherTraderBasketballFTOUOE { get; set; }

        public int OtherTraderBasketball1HOUOE { get; set; }

        public int OtherTraderBasketball1Q3QOUOE { get; set; }

        public int OtherTraderFootballFTHDP { get; set; }

        public int OtherTraderFootball1HHDP { get; set; }

        public int OtherTraderFootball1Q3QHDP { get; set; }

        public int OtherTraderFootballFTOU { get; set; }

        public int OtherTraderFootball1HOU { get; set; }

        public int OtherTraderFootball1Q3QOU { get; set; }

        public int OtherTraderFieldHockeyFTHDP { get; set; }

        public int OtherTraderFieldHockey1HHDP { get; set; }

        public int OtherTraderFieldHockey1QHDP { get; set; }

        public int OtherTraderFieldHockeyFTOU { get; set; }

        public int OtherTraderFieldHockey1HOU { get; set; }

        public int OtherTraderFieldHockey1QOU { get; set; }

        public int OtherTraderIceHockeyFTHDP { get; set; }

        public int OtherTraderIceHockey1st2ndPeriodsHDP { get; set; }

        public int OtherTraderIceHockeyFTOU { get; set; }

        public int OtherTraderIceHockey1st2ndPeriodsOU { get; set; }

        public int OtherTraderOtherSportsMatchWinner { get; set; }

        public int OtherTraderDartsLegsHDP { get; set; }

        public int OtherTraderDartsMost180sHDP { get; set; }

        public int OtherTraderPoolMatchWinnerHDP { get; set; }

        public int OtherTraderPoolRacksWinnerHDP { get; set; }

        public int OtherTraderTableTennisSetWinnerHDP { get; set; }

        public int OtherTraderTableTennisSetXWinnerHDP { get; set; }

        public int OtherTraderVolleyballSetWinnerHDPOU { get; set; }

        public int OtherTraderVolleyballSetsXPointWinnerHDP { get; set; }

        public int OtherTraderHandballFTHDP { get; set; }

        public int OtherTraderHandball1HHDP { get; set; }

        public int OtherTraderHandballFTOU { get; set; }

        public int OtherTraderHandball1HOU { get; set; }

        public int OtherTraderNetballFTHDP { get; set; }

        public int OtherTraderNetball1HHDP { get; set; }

        public int OtherTraderNetball1Q3QHDP { get; set; }

        public int OtherTraderNetballFTOUOE { get; set; }

        public int OtherTraderNetball1HOUOE { get; set; }

        public int OtherTraderNetball1Q3QOUOE { get; set; }

        public int OtherTraderCricketMatchWinnerInningsOU { get; set; }

        public int OtherTraderCricketNextOverOUPlayerOU { get; set; }

        public int OtherTraderCricketFT1X2 { get; set; }

        public int OtherTraderSnookerMatchWinnerHDPOU { get; set; }

        public int OtherTraderSnookerFramesWinnerHDPOUOE { get; set; }

        public int OtherTraderSnookerFirst5FramesHDP { get; set; }

        public int OtherTraderSnookerFirst8FramesHDP { get; set; }

        public int OtherTraderSnookerFirst9FramesHDP { get; set; }

        public int OtherTraderBaseballFTRunLineHDP { get; set; }

        public int OtherTraderBaseball1H1st5inningsHDP { get; set; }

        public int OtherTraderBaseballFTTotalRunOU { get; set; }

        public int OtherTraderBaseball1H1st5inningsOU { get; set; }

        public int OtherTraderBaseballInningHitsRuns { get; set; }

        public int OtherTraderBadmintonAllProductSetWinner { get; set; }

        public int OtherTraderBadmintonAllProductPointWinner { get; set; }

        public int OtherTraderBadmintonRacetoPoints7911 { get; set; }

        public int OtherTraderBadmintonNextPointWinner { get; set; }

        public int OtherTraderBadmintonMatchHandicap { get; set; }

        public int OtherTraderTennisAllProductLS { get; set; }

        public int OtherTraderTennisNextGame { get; set; }

        public int OtherTraderTennisMostAces { get; set; }

        public int OtherTraderTennisMostDoubleFaults { get; set; }

        public decimal? MaxBetBasketballFTHDP { get; set; }

        public decimal? MaxBetBasketballFTOE { get; set; }

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

        public decimal? MaxBetOtherSportsHDP { get; set; }

        public decimal? MaxBetDartsLegsHDP { get; set; }

        public decimal? MaxBetDartsMost180sHDP { get; set; }

        public decimal? MaxBetPoolMatchWinnerHDP { get; set; }

        public decimal? MaxBetPoolRacksWinnerHDP { get; set; }

        public decimal? MaxBetTableTennisHDP { get; set; }

        public decimal? MaxBetVolleyballSetWinnerHDPOU { get; set; }

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
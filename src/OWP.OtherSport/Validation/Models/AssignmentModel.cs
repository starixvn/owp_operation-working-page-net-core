namespace OWP.OtherSport.Validation.Models
{
    using System;

    public class AssignmentModel
    {
        public double MatchNo { get; set; }

        public int OSMatchId { get; set; }

        public string Reporter { get; set; } = string.Empty;

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

        //Basketball
        public string TraderBasketballFTHDP { get; set; }

        public string TraderBasketball1HHDP { get; set; }

        public string TraderBasketball1Q3QHDP { get; set; }

        public string TraderBasketballFTOUOE { get; set; }

        public string TraderBasketball1HOUOE { get; set; }

        public string TraderBasketball1Q3QOUOE { get; set; }

        // Football
        public string TraderFootballFTHDP { get; set; }

        public string TraderFootball1HHDP { get; set; }

        public string TraderFootball1Q3QHDP { get; set; }

        public string TraderFootballFTOU { get; set; }

        public string TraderFootball1HOU { get; set; }

        public string TraderFootball1Q3QOU { get; set; }

        // Field Hockey
        public string TraderFieldHockeyFTHDP { get; set; }

        public string TraderFieldHockey1HHDP { get; set; }

        public string TraderFieldHockey1QHDP { get; set; }

        public string TraderFieldHockeyFTOU { get; set; }

        public string TraderFieldHockey1HOU { get; set; }

        public string TraderFieldHockey1QOU { get; set; }

        // Field Hockey
        public string TraderIceHockeyFTHDP { get; set; }

        public string TraderIceHockey1st2ndPeriodsHDP { get; set; }

        public string TraderIceHockeyFTOU { get; set; }

        public string TraderIceHockey1st2ndPeriodsOU { get; set; }

        // Other sport
        public string TraderOtherSportsMatchWinner { get; set; }

        public string TraderOtherSportsFTHDP { get; set; }

        public string TraderOtherSports1HHDP { get; set; }

        public string TraderOtherSportsFTOU { get; set; }

        public string TraderOtherSports1HOU { get; set; }

        // Darts Legs
        public string TraderDartsLegsHDP { get; set; }

        public string TraderDartsMost180sHDP { get; set; }

        //  Pool Match
        public string TraderPoolMatchWinnerHDP { get; set; }

        public string TraderPoolRacksWinnerHDP { get; set; }

        // Table Tennis
        public string TraderTableTennisSetWinnerHDP { get; set; }

        public string TraderTableTennisSetXWinnerHDP { get; set; }

        //Volleyball
        public string TraderVolleyballSetWinnerHDPOE { get; set; }

        public string TraderVolleyballSetsXPointWinnerHDP { get; set; }

        // Handball
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

        public string TraderBaseball1H1st5inningsHDP { get; set; }

        public string TraderBaseballFTTotalRunOU { get; set; }

        public string TraderBaseball1H1st5inningsOU { get; set; }

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

        public string OtherTraderOtherSportsFTHDP { get; set; }

        public string OtherTraderOtherSports1HHDP { get; set; }

        public string OtherTraderOtherSportsFTOU { get; set; }

        public string OtherTraderOtherSports1HOU { get; set; }

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

        public decimal? MaxBetOtherSportsMatchWinner { get; set; }

        public decimal? MaxBetOtherSportsFTHDP { get; set; }

        public decimal? MaxBetOtherSports1HHDP { get; set; }

        public decimal? MaxBetOtherSportsFTOU { get; set; }

        public decimal? MaxBetOtherSports1HOU { get; set; }

        public decimal? MaxBetDartsLegsHDP { get; set; }

        public decimal? MaxBetDartsMost180sHDP { get; set; }

        public decimal? MaxBetPoolMatchWinnerHDP { get; set; }

        public decimal? MaxBetPoolRacksWinnerHDP { get; set; }

        public decimal? MaxBetTableTennisSetWinnerHDP { get; set; }

        public decimal? MaxBetTableTennisSetXWinnerHDP { get; set; }

        public decimal? MaxBetVolleyballSetWinnerHDPOE { get; set; }

        public decimal? MaxBetVolleyballSetWinnerOE { get; set; }

        public decimal? MaxBetVolleyballSetsXPointWinnerHDP { get; set; }

        public decimal? MaxBetHandballFTHDP { get; set; }

        public decimal? MaxBetHandball1HHDP { get; set; }

        public decimal? MaxBetHandballFTOU { get; set; }

        public decimal? MaxBetHandball1HOU { get; set; }

        public decimal? MaxBetNetballFTHDP { get; set; }

        public decimal? MaxBetNetball1HHDP { get; set; }

        public decimal? MaxBetNetball1Q3QHDP { get; set; }

        public decimal? MaxBetNetballFTOUOE { get; set; }

        public decimal? MaxBetNetballFTOE { get; set; }

        public decimal? MaxBetNetball1HOUOE { get; set; }

        public decimal? MaxBetNetball1HOE { get; set; }

        public decimal? MaxBetNetball1Q3QOUOE { get; set; }

        public decimal? MaxBetNetball1Q3QOE { get; set; }

        public decimal? MaxBetCricketMatchWinnerInningsOU { get; set; }

        public decimal? MaxBetCricketNextOverOUPlayerOU { get; set; }

        public decimal? MaxBetCricketFT1X2 { get; set; }

        public decimal? MaxBetSnookerMatchWinnerHDPOU { get; set; }

        public decimal? MaxBetSnookerMatchWinnerOU { get; set; }

        public decimal? MaxBetSnookerFramesWinnerHDPOUOE { get; set; }

        public decimal? MaxBetSnookerFramesWinnerOU { get; set; }

        public decimal? MaxBetSnookerFramesWinnerOE { get; set; }

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

        public decimal? SpreadBasketballFTHDP { get; set; }

        public decimal? SpreadBasketballFTOUOE { get; set; }

        public decimal? SpreadBasketballFTOE { get; set; }

        public decimal? SpreadFootballFTHDP { get; set; }

        public decimal? SpreadFootball1HHDP { get; set; }

        public decimal? SpreadFootball1Q3QHDP { get; set; }

        public decimal? SpreadFootballFTOU { get; set; }

        public decimal? SpreadFootball1HOU { get; set; }

        public decimal? SpreadFootball1Q3QOU { get; set; }

        public decimal? SpreadFieldHockeyFTHDP { get; set; }

        public decimal? SpreadFieldHockey1HHDP { get; set; }

        public decimal? SpreadFieldHockey1QHDP { get; set; }

        public decimal? SpreadFieldHockeyFTOU { get; set; }

        public decimal? SpreadFieldHockey1HOU { get; set; }

        public decimal? SpreadFieldHockey1QOU { get; set; }

        public decimal? SpreadIceHockeyFTHDP { get; set; }

        public decimal? SpreadIceHockey1st2ndPeriodsHDP { get; set; }

        public decimal? SpreadIceHockeyFTOU { get; set; }

        public decimal? SpreadIceHockey1st2ndPeriodsOU { get; set; }

        public decimal? SpreadOtherSportsMatchWinner { get; set; }

        public decimal? SpreadOtherSportsFTHDP { get; set; }

        public decimal? SpreadOtherSports1HHDP { get; set; }

        public decimal? SpreadOtherSportsFTOU { get; set; }

        public decimal? SpreadOtherSports1HOU { get; set; }

        public decimal? SpreadDartsLegsHDP { get; set; }

        public decimal? SpreadDartsMost180sHDP { get; set; }

        public decimal? SpreadPoolMatchWinnerHDP { get; set; }

        public decimal? SpreadPoolRacksWinnerHDP { get; set; }

        public decimal? SpreadTableTennisSetWinnerHDP { get; set; }

        public decimal? SpreadTableTennisSetXWinnerHDP { get; set; }

        public decimal? SpreadVolleyballSetWinnerHDPOE { get; set; }

        public decimal? SpreadVolleyballSetWinnerOE { get; set; }

        public decimal? SpreadVolleyballSetsXPointWinnerHDP { get; set; }

        public decimal? SpreadHandballFTHDP { get; set; }

        public decimal? SpreadHandball1HHDP { get; set; }

        public decimal? SpreadHandballFTOU { get; set; }

        public decimal? SpreadHandball1HOU { get; set; }

        public decimal? SpreadNetballFTHDP { get; set; }

        public decimal? SpreadNetball1HHDP { get; set; }

        public decimal? SpreadNetball1Q3QHDP { get; set; }

        public decimal? SpreadNetballFTOUOE { get; set; }

        public decimal? SpreadNetballFTOE { get; set; }

        public decimal? SpreadNetball1HOUOE { get; set; }

        public decimal? SpreadNetball1HOE { get; set; }

        public decimal? SpreadNetball1Q3QOUOE { get; set; }

        public decimal? SpreadNetball1Q3QOE { get; set; }

        public decimal? SpreadCricketMatchWinnerInningsOU { get; set; }

        public decimal? SpreadCricketNextOverOUPlayerOU { get; set; }

        public decimal? SpreadCricketFT1X2 { get; set; }

        public decimal? SpreadSnookerMatchWinnerHDPOU { get; set; }

        public decimal? SpreadSnookerMatchWinnerOU { get; set; }

        public decimal? SpreadSnookerFramesWinnerHDPOUOE { get; set; }

        public decimal? SpreadSnookerFramesWinnerOU { get; set; }

        public decimal? SpreadSnookerFramesWinnerOE { get; set; }

        public decimal? SpreadSnookerFirst5FramesHDP { get; set; }

        public decimal? SpreadSnookerFirst8FramesHDP { get; set; }

        public decimal? SpreadSnookerFirst9FramesHDP { get; set; }

        public decimal? SpreadBaseballFTRunLineHDP { get; set; }

        public decimal? SpreadBaseball1H1st5inningsHDP { get; set; }

        public decimal? SpreadBaseballFTTotalRunOU { get; set; }

        public decimal? SpreadBaseball1H1st5inningsOU { get; set; }

        public decimal? SpreadBaseballInningHitsRuns { get; set; }

        public decimal? SpreadBadmintonAllProductSetWinner { get; set; }

        public decimal? SpreadBadmintonAllProductPointWinner { get; set; }

        public decimal? SpreadBadmintonRacetoPoints7911 { get; set; }

        public decimal? SpreadBadmintonNextPointWinner { get; set; }

        public decimal? SpreadBadmintonMatchHandicap { get; set; }

        public decimal? SpreadTennisAllProductLS { get; set; }

        public decimal? SpreadTennisNextGame { get; set; }

        public decimal? SpreadTennisMostAces { get; set; }

        public decimal? SpreadTennisMostDoubleFaults { get; set; }
    }
}
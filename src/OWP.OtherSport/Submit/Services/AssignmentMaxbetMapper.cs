namespace OWP.OtherSport.Submit.Services
{
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation.Models;
    using System.Collections.Generic;
    using System.Linq;

    public interface AssignmentMaxbetMapper
    {
        Dictionary<int, List<AssignmentMaxbet>> MapBySport(List<AssignmentModel> assignments);
    }

    public class AssignmentMaxbetMapperImp : AssignmentMaxbetMapper
    {
        public Dictionary<int, List<AssignmentMaxbet>> MapBySport(List<AssignmentModel> assignments)
            => assignments.ToDictionary(a => a.OSMatchId, a => MapBySport(a));

        public List<AssignmentMaxbet> MapBySport(AssignmentModel assignment)
        {
            var assignmentMaxBet = new List<AssignmentMaxbet>();
            assignmentMaxBet.AddRange(MapBasketball(assignment));
            assignmentMaxBet.AddRange(MapFootball(assignment));
            assignmentMaxBet.AddRange(MapOtherSports(assignment));

            return assignmentMaxBet;
        }

        public static List<AssignmentMaxbet> MapBasketball(AssignmentModel assignment)
        {
            return new List<AssignmentMaxbet>
            {
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderFootballFTHDP,
                    OtherTrader = assignment.OtherTraderFootballFTHDP,
                    MaxBet = assignment.MaxBetFootballFTHDP
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderFootball1HHDP,
                    OtherTrader = assignment.OtherTraderFootball1HHDP,
                    MaxBet = assignment.MaxBetFootball1HHDP
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    TraderId = assignment.TraderFootball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderFootball1Q3QHDP,
                    MaxBet = assignment.MaxBetFootball1Q3QHDP
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FTOU,
                    TraderId = assignment.TraderFootballFTOU,
                    OtherTrader = assignment.OtherTraderFootballFTOU,
                    MaxBet = assignment.MaxBetFootballFTOU
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstHOU,
                    TraderId = assignment.TraderFootball1HOU,
                    OtherTrader = assignment.OtherTraderFootball1HOU,
                    MaxBet = assignment.MaxBetFootball1HOU
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstQ3QOU,
                    TraderId = assignment.TraderFootball1Q3QOU,
                    OtherTrader = assignment.OtherTraderFootball1Q3QOU,
                    MaxBet = assignment.MaxBetFootball1Q3QOU
                },
            };
        }

        public List<AssignmentMaxbet> MapOtherSports(AssignmentModel assignment)
        {
            return new List<AssignmentMaxbet>
            {
                new AssignmentMaxbet
                {
                    Bettype = Bettype.MatchWinner,
                    TraderId = assignment.TraderOtherSportsMatchWinner,
                    OtherTrader = assignment.OtherTraderOtherSportsMatchWinner,
                    MaxBet = assignment.MaxBetOtherSportsMatchWinner
                }
            };
        }

        public List<AssignmentMaxbet> Basketball(AssignmentModel assignment)
        {
            if(!assignment.Sport.Equals(Sport.Basketball, System.StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                // FTHDP
                //FTHDP
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderBasketballFTHDP,
                    OtherTrader = assignment.OtherTraderBasketballFTHDP,
                    MaxBet = assignment.MaxBetBasketballFTHDP
                },
                //1H (HDP)
                //FirstHHDP
                //1HHDP
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderBasketball1HHDP,
                    OtherTrader = assignment.OtherTraderBasketball1HHDP,
                    MaxBet = null
                },
                //FirstQ3QHDP
                //Q3QHDP
                 new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    TraderId = assignment.TraderBasketball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderBasketball1Q3QHDP,
                    MaxBet = null
                },
                 //FT (OU/OE)
                 //FTOUOE
                 //FTOUOE
                  new AssignmentMaxbet
                {
                    Bettype = Bettype.FTOUOE,
                    TraderId = assignment.TraderBasketballFTOUOE,
                    OtherTrader = assignment.OtherTraderBasketballFTOUOE,
                    MaxBet = assignment.MaxBetBasketballFTOE
                },
                  //1H (OU/OE)
                  //FirstHOUOE
                  //1HOUOE
                   new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstHOUOE,
                    TraderId = assignment.TraderBasketball1HOUOE,
                    OtherTrader = assignment.OtherTraderBasketball1HOUOE,
                    MaxBet = null
                },
                   //1Q & 3Q (OU/OE)
                   //FirstQ3QOUOE
                   //1Q3QOUOE
                   new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstQ3QOUOE,
                    TraderId = assignment.TraderBasketball1Q3QOUOE,
                    OtherTrader = assignment.OtherTraderBasketball1Q3QOUOE,
                    MaxBet = null
                },
            };
        } 
        
        public static List<AssignmentMaxbet> MapFootball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Football, System.StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                // FTHDP
                //FTHDP
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderFieldHockeyFTHDP,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTHDP,
                    MaxBet = assignment.MaxBetFieldHockeyFTHDP
                },
                //1H (HDP)
                //FirstHHDP
                //1HHDP
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderFieldHockey1HHDP,
                    OtherTrader = assignment.OtherTraderFieldHockey1HHDP,
                    MaxBet = null
                },
                //FirstQHDP
                //1QHDP
                 new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstQHDP,
                    TraderId = assignment.TraderFieldHockey1QHDP,
                    OtherTrader = assignment.OtherTraderFieldHockey1QHDP,
                    MaxBet = null
                },
                 //FT (OU/OE)
                 //FTOUOE
                 //FTOUOE
                  new AssignmentMaxbet
                {
                    Bettype = Bettype.FTOUOE,
                    TraderId = assignment.TraderFieldHockeyFTOUOE,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTOUOE,
                    MaxBet = assignment.MaxBetFieldHockeyFTOE
                },
                  //1H (OU/OE)
                  //FirstHOUOE
                  //1HOUOE
                   new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstHOUOE,
                    TraderId = assignment.TraderFieldHockey1HOUOE,
                    OtherTrader = assignment.OtherTraderFieldHockeyl1HOUOE,
                    MaxBet = null
                },
                   //1Q & 3Q (OU/OE)
                   //FirstQ3QOUOE
                   //1Q3QOUOE
                   new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstQ3QOUOE,
                    TraderId = assignment.TraderFieldHockey1Q3QOUOE,
                    OtherTrader = assignment.OtherTraderFieldHockey1Q3QOUOE,
                    MaxBet = null
                },
            };
        }
    }
}

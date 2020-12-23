namespace OWP.OtherSport.Submit.Services
{
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation.Models;
    using System;
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
            assignmentMaxBet.AddRange(MapFieldHockey(assignment));
            assignmentMaxBet.AddRange(MapIceHockey(assignment));
            assignmentMaxBet.AddRange(MapOtherSports(assignment));

            return assignmentMaxBet;
        }

        public List<AssignmentMaxbet> MapBasketball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Basketball, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                new()
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderBasketballFTHDP,
                    OtherTrader = assignment.OtherTraderBasketballFTHDP,
                    MaxBet = assignment.MaxBetBasketballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderBasketball1HHDP,
                    OtherTrader = assignment.OtherTraderBasketball1HHDP,
                    MaxBet = null
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    TraderId = assignment.TraderBasketball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderBasketball1Q3QHDP,
                    MaxBet = null
                },
                new()
                {
                    Bettype = Bettype.FTOUOE,
                    TraderId = assignment.TraderBasketballFTOUOE,
                    OtherTrader = assignment.OtherTraderBasketballFTOUOE,
                    MaxBet = assignment.MaxBetBasketballFTOE
                },
                new()
                {
                    Bettype = Bettype.FirstHOUOE,
                    TraderId = assignment.TraderBasketball1HOUOE,
                    OtherTrader = assignment.OtherTraderBasketball1HOUOE,
                    MaxBet = null
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QOUOE,
                    TraderId = assignment.TraderBasketball1Q3QOUOE,
                    OtherTrader = assignment.OtherTraderBasketball1Q3QOUOE,
                    MaxBet = null
                }
            };
        }

        public static List<AssignmentMaxbet> MapFootball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Football, StringComparison.CurrentCultureIgnoreCase))
                return new List<AssignmentMaxbet>();

            return new List<AssignmentMaxbet>
            {
                new()
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderFootballFTHDP,
                    OtherTrader = assignment.OtherTraderFootballFTHDP,
                    MaxBet = assignment.MaxBetFootballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderFootball1HHDP,
                    OtherTrader = assignment.OtherTraderFootball1HHDP,
                    MaxBet = assignment.MaxBetFootball1HHDP
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    TraderId = assignment.TraderFootball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderFootball1Q3QHDP,
                    MaxBet = assignment.MaxBetFootball1Q3QHDP
                },
                new()
                {
                    Bettype = Bettype.FTOU,
                    TraderId = assignment.TraderFootballFTOU,
                    OtherTrader = assignment.OtherTraderFootballFTOU,
                    MaxBet = assignment.MaxBetFootballFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOU,
                    TraderId = assignment.TraderFootball1HOU,
                    OtherTrader = assignment.OtherTraderFootball1HOU,
                    MaxBet = assignment.MaxBetFootball1HOU
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QOU,
                    TraderId = assignment.TraderFootball1Q3QOU,
                    OtherTrader = assignment.OtherTraderFootball1Q3QOU,
                    MaxBet = assignment.MaxBetFootball1Q3QOU
                }
            };
        }

        public static List<AssignmentMaxbet> MapFieldHockey(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.FieldHockey, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                new()
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderFieldHockeyFTHDP,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTHDP,
                    MaxBet = assignment.MaxBetFieldHockeyFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderFieldHockey1HHDP,
                    OtherTrader = assignment.OtherTraderFieldHockey1HHDP,
                    MaxBet = assignment.MaxBetFieldHockey1HHDP
                },
                new()
                {
                    Bettype = Bettype.FirstQHDP,
                    TraderId = assignment.TraderFieldHockey1QHDP,
                    OtherTrader = assignment.OtherTraderFieldHockey1QHDP,
                    MaxBet = assignment.MaxBetFieldHockey1QHDP
                },
                new()
                {
                    Bettype = Bettype.FTOU,
                    TraderId = assignment.TraderFieldHockeyFTOU,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTOU,
                    MaxBet = assignment.MaxBetFieldHockeyFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOUOE,
                    TraderId = assignment.TraderFieldHockey1HOU,
                    OtherTrader = assignment.OtherTraderFieldHockey1HOU,
                    MaxBet = assignment.MaxBetFieldHockey1HOU
                },
                new()
                {
                    Bettype = Bettype.FirstQOU,
                    TraderId = assignment.TraderFieldHockey1QOU,
                    OtherTrader = assignment.OtherTraderFieldHockey1QOU,
                    MaxBet = assignment.MaxBetFieldHockey1QOU
                }
            };
        }

        public static List<AssignmentMaxbet> MapIceHockey(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.IceHockey, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                new()
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderFieldHockeyFTHDP,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTHDP,
                    MaxBet = assignment.MaxBetFieldHockeyFTHDP
                },
                new()
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderIceHockeyFTHDP,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTHDP,
                    MaxBet = assignment.MaxBetIceHockeyFTHDP
                },
                new()
                {
                    Bettype = Bettype.First2ndPeriodsHDP,
                    TraderId = assignment.TraderIceHockey1st2ndHDP,
                    OtherTrader = assignment.OtherTraderIceHockey1st2ndPeriodsHDP,
                    MaxBet = assignment.MaxBetIceHockey1st2ndPeriodsHDP,
                },
                new()
                {
                    Bettype = Bettype.FTOsU,
                    TraderId = assignment.TraderIceHockeyFTOU,
                    OtherTrader = assignment.OtherTraderIceHockeyFTOU,
                    MaxBet = assignment.MaxBetIceHockeyFTOU
                },
                new()
                {
                    Bettype = Bettype.First2ndPeriodsOU,
                    TraderId = assignment.TraderIceHockey1st2ndOU,
                    OtherTrader = assignment.OtherTraderIceHockey1st2ndPeriodsOU,
                    MaxBet = assignment.MaxBetIceHockey1st2ndPeriodsOU
                }

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
    }
}

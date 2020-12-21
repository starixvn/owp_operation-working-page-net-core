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

    public class AssignmentMaxbetMapperImp: AssignmentMaxbetMapper
    {
        public Dictionary<int, List<AssignmentMaxbet>> MapBySport(List<AssignmentModel> assignments)
            => assignments.ToDictionary(a => a.OSMatchId, a => MapBySport(a));

        public List<AssignmentMaxbet> MapBySport(AssignmentModel assignment)
        {
            var sportBetType = new Dictionary<string, List<AssignmentMaxbet>>
            {
                {Sport.Basketball, MapBasketball(assignment)},
                {Sport.Football, MapFootball(assignment)},
                //{Sport.FieldHockey, BasketballHelper.GetAllAssignmentsOfMatch(assignment)},
                //{Sport.IceHockey, BasketballHelper.GetAllAssignmentsOfMatch(assignment)},
                {Sport.OtherSports, MapOtherSports(assignment)},
                //{Sport.Darts, GetFieldHockeyAssignments(assignment)},
                //{Sport.Pool, GetBasketballInformation(assignment)},
                //{Sport.TableTennis, GetFootballAssignments(assignment)},
                //{Sport.Volleyball, GetFieldHockeyAssignments(assignment)},
                //{Sport.Handball, GetFieldHockeyAssignments(assignment)},
                //{Sport.Netball, GetFieldHockeyAssignments(assignment)},
                //{Sport.Cricket, GetFieldHockeyAssignments(assignment)},
                //{Sport.Baseball, GetFieldHockeyAssignments(assignment)},
                //{Sport.Badminton, GetFieldHockeyAssignments(assignment)},
                //{Sport.Tennis, GetFieldHockeyAssignments(assignment)}
            };
            return sportBetType[assignment.Sport];
        }

        public List<AssignmentMaxbet> MapBasketball(AssignmentModel assignment)
        {
            return new List<AssignmentMaxbet>
            {
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderBasketballFTHDP,
                    OtherTrader = assignment.OtherTraderBasketballFTHDP,
                    MaxBet = assignment.MaxBetBasketballFTHDP
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderBasketball1HHDP,
                    OtherTrader = assignment.OtherTraderBasketball1HHDP
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    TraderId = assignment.TraderBasketball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderBasketball1Q3QHDP
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FTOUOE,
                    TraderId = assignment.TraderBasketballFTOUOE,
                    OtherTrader = assignment.OtherTraderBasketballFTOUOE,
                    MaxBet = assignment.MaxBetBasketballFTOE
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstHOUOE,
                    TraderId = assignment.TraderBasketball1HOUOE,
                    OtherTrader = assignment.OtherTraderBasketball1HOUOE
                },
                new AssignmentMaxbet
                {
                    Bettype = Bettype.FirstQ3QOUOE,
                    TraderId = assignment.TraderBasketball1Q3QOUOE,
                    OtherTrader = assignment.OtherTraderBasketball1Q3QOUOE
                }
            };
        }

        public List<AssignmentMaxbet> MapFootball(AssignmentModel assignment)
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
    }
}

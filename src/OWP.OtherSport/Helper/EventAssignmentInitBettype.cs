using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.Helper
{
    public static class EventAssignmentInitBettype
    {
        private static Dictionary<string, List<string>> DicBettypesSpecificSport = new Dictionary<string, List<string>>
        {
            {"Tennis",new List<string>
            {
                "All Product (LS)",
                "Next Game",
                "Most Aces",
                "Most Double Faults"
            }
            },
            {"Badminton",new List<string>
            {
                "All Product (Set Winner)",
                "All Product (Point Winner)",
                "Race to Points (7, 9, 11)",
                "FT 1X2"
            }
            },
            {"Cricket",new List<string>
            {
                "Match Winner & Innings (OU)",
                "Next Over (OU) & Player (OU)",
                "Race to Points (7, 9, 11)",
                "Next Point Winner"
            }
            }
        };

        #region Init bettype

        public static void InitBettype(
            string employee,
            List<EventAssignmentModel> matchsAssignments,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet)
        {
            Parallel.ForEach(matchsAssignments, matchAssignment =>
            {
                var tradersAssignment = DicHelper.GetValueFromDicBykey(dicMatchAssignment, matchAssignment.OSMatchId);
                var maxbets = DicHelper.GetValueFromDicBykey(dicMatchMaxBet, matchAssignment.OSMatchId);

                var bettypes = BuildBettypeForMatch(
                    employee,
                    matchAssignment,
                    tradersAssignment,
                    maxbets);
                foreach (var bettype in bettypes)
                {
                    matchAssignment.TraderMaxbetModels.Add(new TraderMaxBetModel
                    {
                        BetTypeName = bettype,
                        TraderAssignment = new TraderAssignment
                        {
                            OSMatchId = matchAssignment.OSMatchId,
                            BetTypeName = bettype
                        },
                        Maxbet = new MatchMaxBet
                        {
                            OSMatchId = matchAssignment.OSMatchId,
                            BetTypeName = bettype
                        }
                    });
                }
            });
        }

        public static IList<string> BuildBettypeTrader(
            string sport,
            IList<MatchAssignment> matchsAssignment,
            IList<MatchMaxBet> matchsMaxBet)
        {
            List<string> bettypes;
            DicBettypesSpecificSport.TryGetValue(sport, out bettypes);
            if (bettypes != null)
            {
                return bettypes;
            }

            var bettypeAssignment = matchsAssignment.Select(a => a.BetTypeName).Distinct();
            var bettypeMaxbet = matchsMaxBet.Select(m => m.BetTypeName).Distinct();

            var bettypeSelected = bettypeAssignment.Count() > bettypeMaxbet.Count()
                ? bettypeAssignment
                : bettypeMaxbet;
            return bettypeSelected == null
                ? new List<string>()
                : bettypeSelected.ToList();
        }

        public static IList<string> BuildBettypeForMatch(
            string employee,
            EventAssignmentModel matchsAssignment,
            IList<MatchAssignment> assignments,
            IList<MatchMaxBet> maxbets)
        {
           
            if (string.IsNullOrWhiteSpace(employee))
            {
                return BuildBettypeTrader(matchsAssignment.Sport, assignments, maxbets);
            }

            var existsAssignment = assignments?.Any(match =>
                 matchsAssignment.OSMatchId == match.OSMatchId &&
                 match.Username.Contains(employee));
            if(existsAssignment.HasValue && existsAssignment.Value)
            {
                return BuildBettypeTrader(matchsAssignment.Sport, assignments, maxbets);
            }
            return new List<string>();
        }
        #endregion Init bettype
    }
}

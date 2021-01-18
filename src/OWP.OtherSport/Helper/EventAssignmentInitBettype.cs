using System.Collections.Generic;
using System.Linq;
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
            bool hasFilterEmployee,
            List<EventAssignmentModel> matchs,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet)
        {
            matchs.ForEach(match =>
            {
                var bettypes = BuildBettypeForMatch(
                    hasFilterEmployee,
                    match,
                    dicMatchAssignment,
                    dicMatchMaxBet);
                foreach (var bettype in bettypes)
                {
                    match.TraderMaxbetModels.Add(new TraderMaxBetModel
                    {
                        BetTypeName = bettype,
                        TraderAssignment = new TraderAssignment
                        {
                            OSMatchId = match.OSMatchId,
                            BetTypeName = bettype
                        },
                        Maxbet = new MatchMaxBet
                        {
                            OSMatchId = match.OSMatchId,
                            BetTypeName = bettype
                        }
                    });
                }
            });
        }

        public static IList<string> BuildBettypeForMatch(
            bool hasFilterEmployee,
            EventAssignmentModel match,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet)
        {
            List<string> bettypes;
            DicBettypesSpecificSport.TryGetValue(match.Sport, out bettypes);
            if (!hasFilterEmployee && bettypes != null)
            {
                return bettypes;
            }

            var assignments = DicHelper.GetValueFromDicBykey(dicMatchAssignment, match.OSMatchId);
            var maxbets = DicHelper.GetValueFromDicBykey(dicMatchMaxBet, match.OSMatchId);
            var bettypeAssignment = assignments.Select(a => a.BetTypeName).Distinct();
            var bettypeMaxbet = maxbets.Select(m => m.BetTypeName).Distinct();
            if (hasFilterEmployee)
            {
                return bettypeAssignment.ToList();
            }
            var bettypeSelected = bettypeAssignment.Count() > bettypeMaxbet.Count()
                ? bettypeAssignment
                : bettypeMaxbet;
            return bettypeSelected.ToList();
        }

        #endregion Init bettype
    }
}

using System.Collections.Generic;
using System.Linq;
using OWP.Extensions;
using OWP.OtherSport.EventAssignment.Helper;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.EventAssignment.Builder
{
    public interface BettypeBuilder
    {
        public void BuildBettype(
           string employee,
           List<EventAssignmentModel> matchsAssignments,
           Dictionary<int, List<MatchAssignment>> dicMatchAssignment,
           Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet);

        IList<string> BuildBettypeTrader(string sport);

        IList<string> BuildBettypeForMatch(string employee, EventAssignmentModel matchsAssignment);
    }

    public class BettypeBuilderImp : BettypeBuilder
    {
        private IList<MatchAssignment> assignments;

        private IList<MatchMaxBet> maxbets;

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

        public void BuildBettype(
            string employee, 
            List<EventAssignmentModel> matchsAssignments, 
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment, 
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet)
        {
            foreach (var matchAssignment in matchsAssignments)
            {
                assignments = DicHelper.GetValueFromDicBykey(dicMatchAssignment, matchAssignment.OSMatchId);
                maxbets = DicHelper.GetValueFromDicBykey(dicMatchMaxBet, matchAssignment.OSMatchId);

                var bettypes = BuildBettypeForMatch(
                    employee,
                    matchAssignment);

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
            }
        }

        public IList<string> BuildBettypeForMatch(
            string employee,
            EventAssignmentModel matchsAssignment)
        {

            if (string.IsNullOrWhiteSpace(employee))
            {
                return BuildBettypeTrader(matchsAssignment.Sport);
            }

            var existsAssignment = assignments?.Any(match => match.Username.IsContains(employee));

            if (existsAssignment.HasValue && existsAssignment.Value)
            {
                return BuildBettypeTrader(matchsAssignment.Sport);
            }

            return new List<string>();
        }

        public IList<string> BuildBettypeTrader(string sport)
        {
            DicBettypesSpecificSport.TryGetValue(sport, out List<string> bettypes);

            if (bettypes != null)
            {
                return bettypes;
            }

            var bettypeAssignment = assignments.Select(a => a.BetTypeName).Distinct();
            var bettypeMaxbet = maxbets.Select(m => m.BetTypeName).Distinct();

            var bettypeSelected = bettypeAssignment.Count() > bettypeMaxbet.Count()
                ? bettypeAssignment
                : bettypeMaxbet;

            return bettypeSelected == null
                ? new List<string>()
                : bettypeSelected.ToList();
        }
    }
}

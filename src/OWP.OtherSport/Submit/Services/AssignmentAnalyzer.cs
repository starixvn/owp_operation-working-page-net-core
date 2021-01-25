namespace OWP.OtherSport.Submit.Services
{
    using Models;
    using System.Collections.Generic;
    using OWP.OtherSport.Validation.Models;
    using Contants;
    using System.Linq;

    public interface AssignmentAnalyzer
    {
        List<MatchAssignment> GetAllAssignmentsOfMatch(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets);

        List<MatchMaxBet> GetMaxBets(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets);

        string DetectOldMatchIds(List<int> oldMatchIds, List<int> newMatchIds);

        void ReassignOSMatchId(List<MatchByHashKey> hashKeyMap, List<AssignmentModel> assignments);
    }

    public class AssignmentAnalyzerImp : AssignmentAnalyzer
    {
        public List<MatchAssignment> GetAllAssignmentsOfMatch(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets)
        {
            var matchId = assignment.OSMatchId;
            var matchAssignments = new List<MatchAssignment>();
            foreach (var assignmentMaxbet in assignmentMaxbets)
            {
                matchAssignments.Add(GetAssignmentByBettype(matchId, assignmentMaxbet.Bettype, assignmentMaxbet.TraderId));
                matchAssignments.Add(GetOtherAssignmentByBettype(matchId, assignmentMaxbet.Bettype, assignmentMaxbet.OtherTrader));
            }

            return matchAssignments.Where(a => a.UserId > 0 || !string.IsNullOrWhiteSpace(a.AssignmentNote)).ToList();
        }

        public List<MatchMaxBet> GetMaxBets(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets)
        {
            var matchMaxBets = new List<MatchMaxBet>();
            foreach (var assignmentMaxbet in assignmentMaxbets)
            {
                matchMaxBets.Add(GetMaxBetByBettype(assignment.OSMatchId, assignmentMaxbet.Bettype, assignmentMaxbet.MaxBet));
            }

            return matchMaxBets.Where(a => a.MaxBet != null && a.MaxBet != 0).ToList();
        }

        public static MatchAssignment GetAssignmentByBettype(int matchId, string bettype, int userId)
        {
            return new MatchAssignment
            {
                OSMatchId = matchId,
                AssignmentType = AssignmentType.Assignment,
                BetTypeName = bettype,
                UserId = userId
            };
        }

        public static MatchAssignment GetOtherAssignmentByBettype(int matchId, string bettype, string otherTraderNote)
        {
            var otherTrader = MapOtherTrader(otherTraderNote);
            return new MatchAssignment
            {
                OSMatchId = matchId,
                AssignmentType = AssignmentType.OtherAssignment,
                BetTypeName = bettype,
                UserId = otherTrader.UserId,
                AssignmentNote = otherTrader.AssignmentNote
            };
        }

        public static OtherTrader MapOtherTrader(string otherTraderNote)
        {
            if (string.IsNullOrEmpty(otherTraderNote))
            {
                return new OtherTrader();
            }
            var parts = otherTraderNote.Split(',');
            var traderId = int.Parse(parts[0]);
            var assignmentNote = parts[1];

            return new OtherTrader
            {
                UserId = traderId,
                AssignmentNote = assignmentNote
            };
        }

        public static MatchMaxBet GetMaxBetByBettype(int matchId, string bettype, decimal? maxbet)
        {
            return new MatchMaxBet
            {
                OSMatchId = matchId,
                BetTypeName = bettype,
                MaxBet = maxbet
            };
        }

        public string DetectOldMatchIds(List<int> oldMatchIds, List<int> newMatchIds)
        {
            var deletedMatchIds = oldMatchIds.Except(newMatchIds);
            var deletedMatchIdsInString = string.Join(',', deletedMatchIds);

            return deletedMatchIdsInString;
        }

        public void ReassignOSMatchId(List<MatchByHashKey> hashKeyMap, List<AssignmentModel> assignments)
        {
            var matchIdMap = hashKeyMap.ToDictionary(h => h.HashKey, h => h.OSMatchId);
            assignments.ForEach(a => a.OSMatchId = matchIdMap[a.HashKey]);
        }
    }
}
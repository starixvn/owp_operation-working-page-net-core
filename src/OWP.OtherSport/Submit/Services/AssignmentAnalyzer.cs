namespace OWP.OtherSport.Submit.Services
{
    using Models;
    using System.Collections.Generic;
    using OWP.OtherSport.Validation.Models;
    using Contants;

    public interface AssignmentAnalyzer
    {
        IEnumerable<MatchAssignment> GetAllAssignmentsOfMatch(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets);

        IEnumerable<MatchMaxBet> GetMaxBets(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets);
    }

    public class AssignmentAnalyzerImp : AssignmentAnalyzer
    {
        public IEnumerable<MatchAssignment> GetAllAssignmentsOfMatch(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets)
        {
            var matchId = assignment.OSMatchId;
            foreach (var assignmentMaxbet in assignmentMaxbets)
            {
                yield return GetAssignmentByBettype(matchId, assignmentMaxbet.Bettype, assignmentMaxbet.TraderId);
                yield return GetOtherAssignmentByBettype(matchId, assignmentMaxbet.Bettype, assignmentMaxbet.OtherTrader);
            }
        }

        public IEnumerable<MatchMaxBet> GetMaxBets(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets)
        {
            var matchId = assignment.OSMatchId;
            foreach (var assignmentMaxbet in assignmentMaxbets)
            {
                yield return GetMaxBetByBettype(matchId, assignmentMaxbet.Bettype, assignmentMaxbet.TraderId);
            }
        }

        public static MatchAssignment GetAssignmentByBettype(int matchId, string bettype, int userId)
        {
            return new MatchAssignment
            {
                OSMatchId = matchId,
                AssignmentType = (int)AssignmentType.Assignment,
                BetTypeName = bettype,
                UserId = userId
            };
        }

        public static MatchAssignment GetOtherAssignmentByBettype(int matchId, string bettype, string traderInString)
        {
            var otherTrader = MapOtherTrader(traderInString);
            return new MatchAssignment
            {
                OSMatchId = matchId,
                AssignmentType = (int)AssignmentType.OtherAssignment,
                BetTypeName = bettype,
                UserId = otherTrader.UserId,
                AssignmentNote = otherTrader.AssignmentNote
            };
        }

        public static OtherTrader MapOtherTrader(string traderInString)
        {
            return new OtherTrader();
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
    }
}
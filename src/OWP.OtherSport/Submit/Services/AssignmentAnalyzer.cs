namespace OWP.OtherSport.Submit.Services;

using Contants;
using Models;
using OWP.OtherSport.Validation.Models;

public interface AssignmentAnalyzer
{
    List<MatchAssignment> GetAllAssignmentsOfMatch(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets);

    List<MatchSetting> GetMaxBets(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets);

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
            matchAssignments.Add(GetAssignmentByBettype(matchId, assignmentMaxbet));
            matchAssignments.Add(GetOtherAssignmentByBettype(matchId, assignmentMaxbet));
        }

        return matchAssignments.Where(a => a.UserId > 0 || !string.IsNullOrWhiteSpace(a.AssignmentNote)).ToList();
    }

    public List<MatchSetting> GetMaxBets(AssignmentModel assignment, List<AssignmentMaxbet> assignmentMaxbets)
    {
        var matchMaxBets = new List<MatchSetting>();
        foreach (var assignmentMaxbet in assignmentMaxbets)
        {
            matchMaxBets.Add(GetMaxBetByBettype(assignment.OSMatchId, assignmentMaxbet));
        }

        return matchMaxBets.Where(a => (a.MaxBet != null && a.MaxBet != 0) ||
                                       (a.Spread.HasValue && a.Spread.Value != 0)).ToList();
    }

    public static MatchAssignment GetAssignmentByBettype(int matchId, AssignmentMaxbet assignmentMaxbet)
        => GetAssignmentWithNoteByBettype(
            matchId,
            assignmentMaxbet.Bettype,
            assignmentMaxbet.Trader,
            (int)AssignmentType.Assignment);

    public static MatchAssignment GetOtherAssignmentByBettype(int matchId, AssignmentMaxbet assignmentMaxbet)
        => GetAssignmentWithNoteByBettype(
            matchId,
            assignmentMaxbet.Bettype,
            assignmentMaxbet.OtherTrader,
            (int)AssignmentType.OtherAssignment);

    public static MatchAssignment GetAssignmentWithNoteByBettype(int matchId, string bettype, string traderNote, int assignmentType)
    {
        var otherTrader = MapOtherTrader(traderNote);
        return new MatchAssignment
        {
            OSMatchId = matchId,
            AssignmentType = assignmentType,
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

    public static MatchSetting GetMaxBetByBettype(int matchId, AssignmentMaxbet assignmentMaxbet)
    {
        return new MatchSetting
        {
            OSMatchId = matchId,
            BetTypeName = assignmentMaxbet.Bettype,
            MaxBet = assignmentMaxbet.MaxBet,
            Spread = assignmentMaxbet.Spread
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
using OWP.AssignmentUpload.Submit.Constants;
using OWP.AssignmentUpload.Submit.Models;
using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Submit.Services;

public interface AssignmentAnalyzer
{
    IEnumerable<MatchAgent> GetAgentOfMatch(AssignmentModel assignment);

    List<MatchAssignment> GetAssignmentOfMatch(AssignmentModel assignmentModel);

    IEnumerable<MatchNote> GetMatchNotes(AssignmentModel assignment);

    IEnumerable<MatchOffer> GetOffersOfMatch(AssignmentModel assignment);

    IEnumerable<MatchAssignment> GetOtherTraderHandicap(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTraderHandicap1H(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTraderLA(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTraderOneXTwo(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTraderOneXTwo1H(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTraderOverUnder(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTraderOverUnder1H(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTraderFastMarket(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTraderAllInOne(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTrader10Mins(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchAssignment> GetOtherTrader15Mins(AssignmentModel assignmentModel, int mainTrader);

    IEnumerable<MatchSetting> GetSettingsOfMatch(AssignmentModel assignmentModel);

    IEnumerable<MatchTelevision> GetTelevisionOfMatch(AssignmentModel assignment);

    IEnumerable<MatchAssignment> GetTraderAssignmentOfMatch(AssignmentModel assignmentModel);

    IEnumerable<OtherTrader> GetOtherTraders(string otherTradersInString, int mainTrader);

    void ReassignMatchId(List<AssignmentModel> assignments, List<MatchByHashKey> hashKeyMap);
}

public class AssignmentAnalyzerImp : AssignmentAnalyzer
{
    public const int MainAssignmentType = 1;

    public const int OtherAssignmentType = 2;

    public void ReassignMatchId(List<AssignmentModel> assignments, List<MatchByHashKey> hashKeyMap)
    {
        var matchIdMap = hashKeyMap.ToDictionary(h => h.HashKey, h => h.OWPMatchId);
        assignments.ForEach(a => a.OWPMatchId = matchIdMap[a.HashKey]);
    }

    public IEnumerable<MatchAgent> GetAgentOfMatch(AssignmentModel assignment)
    {
        var agents = assignment.Agent
            .Replace("/", ",")
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(a => a.Trim())
            .Distinct();
        return agents.Select(a => new MatchAgent { MatchId = assignment.OWPMatchId, Agent = a });
    }

    public List<MatchAssignment> GetAssignmentOfMatch(AssignmentModel assignmentModel)
    {
        var assignmentOfMatches = new List<MatchAssignment>();
        var traderAssignments = GetTraderAssignmentOfMatch(assignmentModel).ToList();
        assignmentOfMatches.AddRange(traderAssignments);
        foreach (var trader in traderAssignments)
        {
            GetAssignmentFullTimeOfMatch(assignmentModel, assignmentOfMatches, trader);
            GetAssignmentFirstHalfOfMatch(assignmentModel, assignmentOfMatches, trader);
            GetAssignmentMinorsOfMatch(assignmentModel, assignmentOfMatches, trader);
        }

        return assignmentOfMatches.Where(a => a.UserId > 0 || !string.IsNullOrWhiteSpace(a.AssignmentNote))
            .Where(a => a.BetType is > 0 or (int)MajorBetType.LA)
            .ToList();
    }

    private void GetAssignmentFullTimeOfMatch(AssignmentModel assignmentModel, List<MatchAssignment> assignmentOfMatches, MatchAssignment trader)
    {
        switch (trader.BetType)
        {
            case (int)MajorBetType.Handicap:
                var otherTraderHandicap = GetOtherTraderHandicap(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderHandicap);
                break;

            case (int)MajorBetType.OverUnder:
                var otherTraderOverUnder = GetOtherTraderOverUnder(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderOverUnder);
                break;

            case (int)MajorBetType.OneXTwo:
                var otherTraderOneXTwo = GetOtherTraderOneXTwo(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderOneXTwo);
                break;

            default:
                return;
        }
    }

    private void GetAssignmentFirstHalfOfMatch(AssignmentModel assignmentModel, List<MatchAssignment> assignmentOfMatches, MatchAssignment trader)
    {
        switch (trader.BetType)
        {
            case (int)MajorBetType.Handicap1H:
                var otherTraderHandicap1H = GetOtherTraderHandicap1H(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderHandicap1H);
                break;

            case (int)MajorBetType.OverUnder1H:
                var otherTraderOverUnder1H = GetOtherTraderOverUnder1H(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderOverUnder1H);
                break;

            case (int)MajorBetType.OneXTwo1H:
                var otherTraderOneXTwo1H = GetOtherTraderOneXTwo1H(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderOneXTwo1H);
                break;

            default:
                return;
        }
    }

    private void GetAssignmentMinorsOfMatch(AssignmentModel assignmentModel, List<MatchAssignment> assignmentOfMatches, MatchAssignment trader)
    {
        switch (trader.BetType)
        {
            case (int)MajorBetType.XMins:
                var otherTrader10Mins = GetOtherTrader10Mins(assignmentModel, trader.UserId);
                var otherTrader15Mins = GetOtherTrader15Mins(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTrader10Mins);
                assignmentOfMatches.AddRange(otherTrader15Mins);
                break;

            case (int)MajorBetType.LA:
                var otherTraderLA = GetOtherTraderLA(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderLA);
                break;

            case (int)MajorBetType.FastMarket:
                var otherTraderFM = GetOtherTraderFastMarket(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderFM);
                break;

            case (int)MajorBetType.AllInOne:
                var otherTraderAIO = GetOtherTraderAllInOne(assignmentModel, trader.UserId);
                assignmentOfMatches.AddRange(otherTraderAIO);
                break;

            default:
                return;
        }
    }

    public IEnumerable<MatchAssignment> GetTraderAssignmentOfMatch(AssignmentModel assignmentModel)
    {
        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.Handicap,
            UserId = assignmentModel.TraderHDP
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.Handicap1H,
            UserId = assignmentModel.Trader1HHDP
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.LA,
            UserId = assignmentModel.TraderLA
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.OneXTwo,
            UserId = assignmentModel.Trader1X2
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.OneXTwo1H,
            UserId = assignmentModel.Trader1H1X2
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.OverUnder,
            UserId = assignmentModel.TraderOU
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.OverUnder1H,
            UserId = assignmentModel.Trader1HOU
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.FastMarket,
            UserId = assignmentModel.TraderFM
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.AllInOne,
            UserId = assignmentModel.TraderAIO
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.XMins,
            UserId = assignmentModel.Trader10Mins
        };

        yield return new MatchAssignment
        {
            MatchId = assignmentModel.OWPMatchId,
            AssignmentType = MainAssignmentType,
            BetType = (int)MajorBetType.XMins,
            UserId = assignmentModel.Trader15Mins
        };
    }

    public IEnumerable<MatchAssignment> GetOtherTraderHandicap(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTraderHandicap = GetOtherTraders(assignmentModel.OtherTradersHDP, mainTrader);
        foreach (var user in listOtherTraderHandicap)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.Handicap,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTraderHandicap1H(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTraders1HHDP, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.Handicap1H,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTraderOverUnder(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTradersOU, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.OverUnder,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTraderOverUnder1H(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTraders1HOU, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.OverUnder1H,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTraderOneXTwo(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTraders1X2, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.OneXTwo,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTraderOneXTwo1H(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTraders1H1X2, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.OneXTwo1H,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTraderFastMarket(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTradersFM, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.FastMarket,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTraderAllInOne(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTradersAIO, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.AllInOne,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTrader10Mins(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTraders10Mins, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.XMins,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTrader15Mins(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTraders15Mins, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.XMins,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchAssignment> GetOtherTraderLA(AssignmentModel assignmentModel, int mainTrader)
    {
        var listOtherTrader = GetOtherTraders(assignmentModel.OtherTradersLA, mainTrader);
        foreach (var user in listOtherTrader)
        {
            yield return new MatchAssignment
            {
                MatchId = assignmentModel.OWPMatchId,
                AssignmentType = OtherAssignmentType,
                BetType = (int)MajorBetType.LA,
                UserId = user.UserId,
                AssignmentNote = user.AssignmentNote
            };
        }
    }

    public IEnumerable<MatchNote> GetMatchNotes(AssignmentModel assignment)
    {
        return assignment.Notes.Select(n => new MatchNote { MatchId = assignment.OWPMatchId, Note = n });
    }

    public IEnumerable<MatchOffer> GetOffersOfMatch(AssignmentModel assignment)
    {
        var offers = assignment.MatchOffer
            .Replace("/", ",")
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(a => a.Trim())
            .Distinct();
        return offers.Select(a => new MatchOffer { MatchId = assignment.OWPMatchId, Name = a });
    }

    public IEnumerable<MatchSetting> GetSettingsOfMatch(AssignmentModel assignmentModel)
    {
        yield return new MatchSetting
        {
            MatchId = assignmentModel.OWPMatchId,
            BetType = (int)MajorBetType.Handicap,
            MaxBet = assignmentModel.MaxBetHDP,
            Spread = assignmentModel.SpreadHDP
        };

        yield return new MatchSetting
        {
            MatchId = assignmentModel.OWPMatchId,
            BetType = (int)MajorBetType.OneXTwo,
            MaxBet = assignmentModel.MaxBet1X2,
            Spread = assignmentModel.Spread1H1X2
        };

        yield return new MatchSetting
        {
            MatchId = assignmentModel.OWPMatchId,
            BetType = (int)MajorBetType.OneXTwo1H,
            MaxBet = assignmentModel.MaxBet1H1X2,
            Spread = assignmentModel.Spread1H1X2
        };

        yield return new MatchSetting
        {
            MatchId = assignmentModel.OWPMatchId,
            BetType = (int)MajorBetType.Handicap1H,
            MaxBet = assignmentModel.MaxBet1HHDP,
            Spread = assignmentModel.Spread1HHDP
        };

        yield return new MatchSetting
        {
            MatchId = assignmentModel.OWPMatchId,
            BetType = (int)MajorBetType.OverUnder,
            MaxBet = assignmentModel.MaxBetOU,
            Spread = assignmentModel.SpreadOU
        };

        yield return new MatchSetting
        {
            MatchId = assignmentModel.OWPMatchId,
            BetType = (int)MajorBetType.OverUnder1H,
            MaxBet = assignmentModel.MaxBet1HOU,
            Spread = assignmentModel.Spread1HOU
        };
    }

    public IEnumerable<MatchTelevision> GetTelevisionOfMatch(AssignmentModel assignment)
    {
        var televisions = !string.IsNullOrEmpty(assignment.TV)
            ? assignment.TV
                .Replace("/", ",")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim())
                .Distinct()
            : new List<string>();
        return televisions.Select(a => new MatchTelevision { MatchId = assignment.OWPMatchId, TV = a });
    }

    public IEnumerable<OtherTrader> GetOtherTraders(string otherTradersInString, int mainTrader)
    {
        if (string.IsNullOrEmpty(otherTradersInString))
        {
            yield break;
        }

        var otherTraders = otherTradersInString.Split(new[] { "<o>" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var otherTrader in otherTraders)
        {
            var parts = otherTrader.Split(',');
            var traderId = int.Parse(parts[0]);
            var note = parts[1];

            var isETPKSame = IsETPKSame(note);

            var userId = isETPKSame && traderId == 0 ? mainTrader : traderId;
            var assignmentNote = parts[1];
            yield return new OtherTrader
            {
                UserId = userId,
                AssignmentNote = assignmentNote
            };
        }
    }

    private static bool IsETPKSame(string note)
    {
        var isETPK = note.IndexOf("ET", StringComparison.CurrentCultureIgnoreCase) >= 0
                     || note.IndexOf("PK", StringComparison.CurrentCultureIgnoreCase) >= 0;
        var isSame = note.IndexOf("Same", StringComparison.CurrentCultureIgnoreCase) >= 0;

        return isETPK && isSame;
    }
}
namespace OWP.NewLeagueSetting.Helper;

public static class StandardizedLeagueInfo
{
    private const int ThousandMultiplier = 1000;
    private const int PercentageMultiplier = 100;

    public static IList<LeagueSettingInfo> Execute(IList<LeagueSettingExcelInfo> excelInfo)
    {
        var leagueInfo = new List<LeagueSettingInfo>();
        foreach (var info in excelInfo)
        {
            var setting = new LeagueSettingInfo();
            BuildCommonInfo(setting, info);
            BuildCondition(setting, info);
            BuildSpread(setting, info.FTSpread);
            BuildMaxbet(setting, info.WD_HDP, info.WK_HDP);
            BuildNoHdp(setting, info.WD_NO_HDP, info.WK_NO_HDP);
            BuildRemark2(setting, info.Remark2);
            leagueInfo.Add(setting);
        }
        return leagueInfo;
    }

    public static void BuildCommonInfo(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        leagueInfo.ShortLeague = excelInfo.ShortLeague;
        leagueInfo.LeagueGroup = excelInfo.LeagueGroup;
        leagueInfo.League = excelInfo.League;
        leagueInfo.LstNote = excelInfo.LstNote;
    }

    public static void BuildSpread(LeagueSettingCore leagueInfo, string ftSpread)
    {
        var (spreadHdp, spreadOu, spreadSource) = AnalyzeSpreadFT(ftSpread?.Trim());
        leagueInfo.Spread_HDP_Value = spreadHdp;
        leagueInfo.Spread_OU_Value = spreadOu;
        leagueInfo.Spread_Note = spreadSource;
    }
    public static void BuildMaxbet(LeagueSettingCore leagueInfo, string WD_HDP, string WK_HDP)
    {
        var (maxbetWd, maxbetSourceWd) = AnalyzeMaxbet(WD_HDP?.Trim());
        leagueInfo.MaxbetWD_HDP_Value = maxbetWd;
        leagueInfo.MaxbetWD_HDP_Note = maxbetSourceWd;

        var (maxbetWk, maxbetSourceWk) = AnalyzeMaxbet(WK_HDP?.Trim());
        leagueInfo.MaxbetWK_HDP_Value = maxbetWk;
        leagueInfo.MaxbetWK_HDP_Note = maxbetSourceWk;
    }

    public static void BuildNoHdp(LeagueSettingCore leagueInfo, string WD_NO_HDP, string WK_NO_HDP)
    {
        var (noHdpWd, noHdpWdSource) = AnalyzeNoHdp(WD_NO_HDP?.Trim());
        leagueInfo.WD_NoOfHDP_Value = noHdpWd;
        leagueInfo.WD_NoOfHDP_Note = noHdpWdSource;

        var (noHdpWk, noHdpWkSource) = AnalyzeNoHdp(WK_NO_HDP?.Trim());
        leagueInfo.WK_NoOfHDP_Value = noHdpWk;
        leagueInfo.WK_NoOfHDP_Note = noHdpWkSource;
    }

    private static (decimal? maxbet, string maxbetSource) AnalyzeMaxbet(string maxbetSource)
    {
        var regex = Pattern.MaxbetRegex();
        if (!string.IsNullOrWhiteSpace(maxbetSource) && regex.IsMatch(maxbetSource))
        {
            var maxbet = RemoveLastCharacterAndParseToNumber(maxbetSource);

            return (maxbet * ThousandMultiplier, null);
        }
        return (null, maxbetSource);
    }
    private static (string noHdp, string noHdpSource) AnalyzeNoHdp(string noHdpSource)
    {
        var regex = Pattern.NoHDPRegex();
        return !string.IsNullOrWhiteSpace(noHdpSource) && regex.IsMatch(noHdpSource)
        ? (noHdpSource, null)
        : (null, noHdpSource);
    }

    public static (decimal? spreadHdp, decimal? spreadOu, string spreadSource) AnalyzeSpreadFT(string spreadSource)
    {
        var regex = Pattern.SpreadRegex();
        if (!string.IsNullOrWhiteSpace(spreadSource) && regex.IsMatch(spreadSource))
        {
            var spreadSplit = spreadSource.Split("-");
            var spreadHdp = RemoveLastCharacterAndParseToNumber(spreadSplit[0]);
            var spreadOu = RemoveLastCharacterAndParseToNumber(spreadSplit[1]);

            return (spreadHdp / PercentageMultiplier, spreadOu / PercentageMultiplier, null);
        }

        return (null, null, spreadSource);
    }

    private static decimal RemoveLastCharacterAndParseToNumber(string source)
    {
        decimal.TryParse(source?.Remove(source.Length - 1), out var spread);
        return spread;
    }
    public static void BuildCondition(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        var (hasTv, ipTv) = AnalyzeHasTv(excelInfo.LocalTv?.Trim());

        var remarkValue = BuildRemark(excelInfo);
        var shouldAnalyzeDivision = CanBuildDivision(excelInfo);

        leagueInfo.FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
        {
            HasTv = hasTv,
            HasIpTv = ipTv.Any(),
            IpTv = ipTv,
            Round = AnalyzeConditionString(excelInfo.Round?.Trim()),
            Division = shouldAnalyzeDivision
                ? AnalyzeDivisionString(excelInfo.LeagueDivision?.Trim())
                : [],
            Remark = remarkValue
        });
    }

    private static string BuildRemark(LeagueSettingExcelInfo excelInfo)
    {
        var remarkValue = excelInfo.Remark ?? string.Empty;
        var isSpecialDivision = excelInfo.LeagueDivision == "Club Team" || excelInfo.LeagueDivision == "National Team";

        if (isSpecialDivision && !string.IsNullOrWhiteSpace(excelInfo.Remark))
        {
            remarkValue = $"{excelInfo.LeagueDivision.Trim()},{remarkValue}";
        }

        return remarkValue;
    }

    public static void BuildRemark2(LeagueSettingInfo leagueInfo, string remark2Source)
    {
        if (remark2Source != null)
        {
            leagueInfo.Remark2WD = remark2Source.Replace("\n", "");
        }
    }

    private static bool CanBuildDivision(LeagueSettingExcelInfo excelInfo)
    {
        var isSpecialDivision = excelInfo.LeagueDivision is "Club Team" or "National Team";
        return !isSpecialDivision;
    }

    private static (bool hasTv, List<Condition> iptv) AnalyzeHasTv(string localTv)
    {
        var split = localTv?.Split('/') ?? [];
        var condition = new List<Condition>();
        var tv = split.Where(t => t.Trim().Equals("TV", StringComparison.OrdinalIgnoreCase));

        foreach (var iptv in split.Except(tv)) // NOSONAR
        {
            condition.Add(new(Type: "Contains", Value: iptv?.Trim()));
        }

        return (tv.Any(), condition);

    }

    public static List<Condition> AnalyzeDivisionString(string conditionSource)
    {
        var condition = new List<Condition>();
        if (string.IsNullOrWhiteSpace(conditionSource))
        {
            return condition;
        }

        if (conditionSource.Contains("-vs-"))
        {
            ProcessVsConditions(conditionSource, condition);
        }
        else
        {
            condition = AnalyzeConditionString(conditionSource);
        }

        return condition;
    }

    private static void ProcessVsConditions(string conditionSource, List<Condition> condition)
    {
        var vsConditions = conditionSource.Split(',');
        foreach (var vsCondition in vsConditions)
        {
            ProcessSingleVsCondition(vsCondition.Trim(), condition);
        }
    }

    private static void ProcessSingleVsCondition(string vsCondition, List<Condition> condition)
    {
        if (!vsCondition.Contains("-vs-"))
        {
            return;
        }

        var parts = vsCondition.Split("-vs-");
        if (parts.Length != 2)
        {
            return;
        }

        var leftSide = parts[0].Trim();
        var rightSide = parts[1].Trim();

        var leftValues = ExtractValuesFromCondition(leftSide);
        var rightValues = ExtractValuesFromCondition(rightSide);

        AddPairwiseConditions(leftValues, rightValues, condition);
    }

    private static void AddPairwiseConditions(List<string> leftValues, List<string> rightValues, List<Condition> condition)
    {
        foreach (var leftValue in leftValues)
        {
            foreach (var rightValue in rightValues)
            {
                var newCondition = new Condition("Pairwise Contains", $"{leftValue},{rightValue}");
                if (!IsDuplicateCondition(condition, newCondition))
                {
                    condition.Add(newCondition);
                }
            }
        }
    }

    private static bool IsDuplicateCondition(List<Condition> existingConditions, Condition newCondition)
    {
        foreach (var existing in existingConditions)
        {
            if (existing.Type == newCondition.Type && AreValueItemsEqual(existing.Value, newCondition.Value))
            {
                return true;
            }
        }
        return false;
    }

    private static bool AreValueItemsEqual(string value1, string value2)
    {
        if (value1 == value2)
        {
            return true;
        }
        var sortedItems1 = value1.Split(',').Select(x => x.Trim()).OrderBy(x => x).ToList();
        var sortedItems2 = value2.Split(',').Select(x => x.Trim()).OrderBy(x => x).ToList();

        if (sortedItems1.Count != sortedItems2.Count)
        {
            return false;
        }

        for (int i = 0; i < sortedItems1.Count; i++)
        {
            if (sortedItems1[i] != sortedItems2[i])
            {
                return false;
            }
        }

        return true;
    }

    public static string ExtractSingleValue(string conditionPart)
    {
        var trimmedPart = conditionPart.Trim();
        var previousLength = -1;

        // Keep removing symbols until no more changes
        while (trimmedPart.Length != previousLength)
        {
            previousLength = trimmedPart.Length;

            // Remove * symbols if present (at both ends)
            if (trimmedPart.StartsWith('*') && trimmedPart.Length > 1)
            {
                trimmedPart = trimmedPart[1..]; // Remove first character
            }
            if (trimmedPart.EndsWith('*') && trimmedPart.Length > 1)
            {
                trimmedPart = trimmedPart[..^1]; // Remove last character
            }

            // Remove parentheses if present
            if (trimmedPart.StartsWith('(') && trimmedPart.EndsWith(')'))
            {
                trimmedPart = trimmedPart[1..^1]; // Remove parentheses
            }
        }

        return trimmedPart.Trim();
    }

    private static List<string> ExtractValuesFromCondition(string conditionPart)
    {
        var values = new List<string>();

        // Split by / to handle multiple values (even if no / exists, this will return array with one element)
        var parts = conditionPart.Split('/');

        foreach (var part in parts)
        {
            var extractedValue = ExtractSingleValue(part);
            if (!string.IsNullOrWhiteSpace(extractedValue))
            {
                values.Add(extractedValue);
            }
        }

        return values;
    }

    private static List<Condition> AnalyzeConditionString(string conditionSource)
    {
        var condition = new List<Condition>();
        if (string.IsNullOrWhiteSpace(conditionSource))
        {
            return condition;
        }

        var splitConditionSource = conditionSource
            .Split("/")
            .Select(c => c.Trim())
            .Where(c => c.Length > 0);
        foreach (var source in splitConditionSource)
        {
            var type = "Equals";

            if (source.StartsWith('*') && source.EndsWith('*'))
            {
                type = "Contains";
            }

            // Always use ExtractSingleValue for consistent value extraction
            var value = ExtractSingleValue(source);

            condition.Add(new Condition(type, value));
        }
        return condition;
    }
}

namespace OWP.NewLeagueSetting.Helper;

public static partial class StandardizedLeagueInfo
{
    public static IList<LeagueSettingInfo> Execute(IList<LeagueSettingExcelInfo> excelInfo)
    {
        var leagueInfo = new List<LeagueSettingInfo>();
        foreach (var info in excelInfo)
        {
            var setting = new LeagueSettingInfo();
            BuildCommonInfo(setting, info);
            BuildCondition(setting, info);
            BuildSpread(setting, info.FTSpread);
            BuildMaxbet(setting, info);
            BuildNoHdp(setting, info);
            BuildComment(setting, info);
            leagueInfo.Add(setting);
        }
        return leagueInfo;
    }

    private static void BuildCommonInfo(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        leagueInfo.LeagueGroup = excelInfo.LeagueGroup;
        leagueInfo.League = excelInfo.League;
        leagueInfo.LstNote = excelInfo.LstNote;
        leagueInfo.Remark2 = excelInfo.Remark2;
    }
    private static void BuildCondition(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        (bool hasTv, List<Condition> ipTv) = AnalyzeHasTv(excelInfo.LocalTv?.Trim());
        leagueInfo.FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
        {
            HasTv = hasTv,
            IpTv = ipTv,
            Round = AnalyzeConditionString(excelInfo.Round?.Trim()),
            Division = AnalyzeConditionString(excelInfo.LeagueDivision?.Trim()),
            Remark = excelInfo.Remark
        });
    }

    private static void BuildSpread(LeagueSettingInfo leagueInfo, string ftSpread)
    {
        (decimal? spreadHdp, decimal? spreadOu, string spreadSource) = AnalyzeSpreadFT(ftSpread?.Trim());
        leagueInfo.Spread_HDP_value = spreadHdp;
        leagueInfo.Spread_OU_value = spreadOu;
        leagueInfo.Spread_Note = spreadSource;
    }
    private static void BuildMaxbet(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        (decimal? maxbetWd, string maxbetSourceWd) = AnalyzeMaxbet(excelInfo.WD_HDP?.Trim());
        leagueInfo.MaxbetWD_HDP_value = maxbetWd;
        leagueInfo.MaxbetWD_HDP_Note = maxbetSourceWd;

        (decimal? maxbetWk, string maxbetSourceWk) = AnalyzeMaxbet(excelInfo.WK_HDP?.Trim());
        leagueInfo.MaxbetWD_HDP_value = maxbetWk;
        leagueInfo.MaxbetWD_HDP_Note = maxbetSourceWk;
    }

    private static void BuildNoHdp(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        (string noHdpWd, string noHdpWdSource) = AnalyzeNoHdp(excelInfo.WD_NO_HDP?.Trim());
        leagueInfo.MaxbetWD_NoOfHDP_value = noHdpWd;
        leagueInfo.MaxbetWD_NoOfHDP_Note = noHdpWdSource;

        (string noHdpWk, string noHdpWkSource) = AnalyzeNoHdp(excelInfo.WK_NO_HDP?.Trim());
        leagueInfo.MaxbetWK_NoOfHDP_value = noHdpWk;
        leagueInfo.MaxbetWK_NoOfHDP_Note = noHdpWkSource;
    }

    private static void BuildComment(LeagueSettingInfo leagueInfo, LeagueSettingExcelInfo excelInfo)
    {
        var comment = string.Empty;
        if (!string.IsNullOrWhiteSpace(excelInfo.LeagueGroupNote))
        {
            comment = $"[League Group]{Environment.NewLine}{excelInfo.LeagueGroupNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.LeagueNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[League]{Environment.NewLine}{excelInfo.LeagueNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.LocalTvNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[Condition - TV]{Environment.NewLine}{excelInfo.LocalTvNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.RoundNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[Condition - Round]{Environment.NewLine}{excelInfo.RoundNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.LeagueDivisionNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[Condition- League Division]{Environment.NewLine}{excelInfo.LeagueDivisionNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.RemarkNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[Condition - Remark]{Environment.NewLine}{excelInfo.RemarkNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.FTSpreadNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[Spread(%)]{Environment.NewLine}{excelInfo.FTSpreadNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.WD_HDPNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[MB Weekday - HDP(K)]{Environment.NewLine}{excelInfo.WD_HDPNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.WD_NO_HDPNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[MB Weekday - NO.HDP]{Environment.NewLine}{excelInfo.WD_NO_HDPNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.WK_HDPNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[MB Weekend - HDP(K)]{Environment.NewLine}{excelInfo.WK_HDPNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.WK_NO_HDPNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[MB Weekend - NO.HDP]{Environment.NewLine}{excelInfo.WK_NO_HDPNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.LstNoteNote))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[Display in Mismatch file - LST Note (Show under League)]{Environment.NewLine}{excelInfo.LstNoteNote}";
        }
        if (!string.IsNullOrWhiteSpace(excelInfo.Remark2Note))
        {
            comment = $"{Environment.NewLine}{Environment.NewLine}[Display in Mismatch file - Remark 2 (Show under match)]{Environment.NewLine}{excelInfo.Remark2Note}`";
        }

        leagueInfo.Comment = comment;
    }


    private static (bool hasTv, List<Condition> iptv) AnalyzeHasTv(string localTv)
    {
        var split = localTv?.Split('/') ?? Array.Empty<string>();
        var condition = new List<Condition>();
        var tv = split?.Where(t => t.Trim().Equals("TV", StringComparison.OrdinalIgnoreCase)) ?? new List<string>();

        foreach (var iptv in split?.Except(tv))
        {
            condition.Add(new(Type: "Contains", Value: iptv?.Trim()));
        }

        return (tv.Any(), condition);

    }
    private static (decimal? maxbet, string maxbetSource) AnalyzeMaxbet(string maxbetSource)
    {
        var regex = Pattern.MaxbetRegex();
        if (!string.IsNullOrWhiteSpace(maxbetSource) && regex.IsMatch(maxbetSource))
        {
            var maxbet = RemoveLastCharacterAndParseToNumber(maxbetSource);

            return (maxbet * 1000, null);
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

    private static List<Condition> AnalyzeConditionString(string conditionSource)
    {
        var condition = new List<Condition>();
        if (string.IsNullOrWhiteSpace(conditionSource))
        {
            return condition;
        }

        var splitConditionSource = conditionSource.Split("/").Where(c => c.Trim().Length > 0);
        foreach (var value in splitConditionSource)
        {
            var type = value.StartsWith('*') && value.EndsWith('*') ?
            "Contains"
            : "Equals";

            condition.Add(new(type, value));
        }
        return condition;
    }

    private static (decimal? spreadHdp, decimal? spreadOu, string spreadSource) AnalyzeSpreadFT(string spreadSource)
    {
        var regex = Pattern.SpreadRegex();
        if (!string.IsNullOrWhiteSpace(spreadSource) && regex.IsMatch(spreadSource))
        {
            var spreadSplit = spreadSource.Split("-");
            var spreadHdp = RemoveLastCharacterAndParseToNumber(spreadSplit[0]);
            var spreadOu = RemoveLastCharacterAndParseToNumber(spreadSplit[1]);

            return (spreadHdp / 1000, spreadOu / 1000, null);
        }

        return (null, null, spreadSource);
    }

    private static decimal RemoveLastCharacterAndParseToNumber(string source)
    {
        decimal.TryParse(source?.Remove(source.Length - 1), out decimal spread);
        return spread;
    }

}
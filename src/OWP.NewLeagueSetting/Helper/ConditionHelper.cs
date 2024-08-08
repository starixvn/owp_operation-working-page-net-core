namespace OWP.NewLeagueSetting.Helper;

public class ConditionHelper
{
    public static IList<UpdateCondition> BuildCondition(IList<LeagueSettingExcelInfo> excelInfo)
    {
        var leagueInfo = new List<UpdateCondition>();
        foreach (var info in excelInfo)
        {
            var setting = new UpdateCondition()
            {
                League = info.League
            };
            setting.FullConditionDescription = BuildCondition(info);
            leagueInfo.Add(setting);
        }
        return leagueInfo;
    }

    public static string BuildCondition(LeagueSettingExcelInfo excelInfo)
    {
        var (hasTv, ipTv) = AnalyzeHasTv(excelInfo.LocalTv?.Trim());
        return JsonConvert.SerializeObject(new LeagueSettingCondition
        {
            HasTv = hasTv,
            HasIpTv = ipTv.Any(),
            IpTv = ipTv,
            Round = AnalyzeConditionString(excelInfo.Round?.Trim()),
            Division = AnalyzeConditionString(excelInfo.LeagueDivision?.Trim()),
            Remark = excelInfo.Remark ?? string.Empty
        });
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

    public static List<Condition> AnalyzeConditionString(string conditionSource)
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
            var value = source;

            if (source.StartsWith('*') && source.EndsWith('*'))
            {
                type = "Contains";
                value = RemoveContainSymbol(source);
            }

            condition.Add(new Condition(type, value));
        }
        return condition;
    }

    private static string RemoveContainSymbol(string originalString)
    {
        return originalString?.Length > 1
            ? originalString[1..^1]
            : originalString;
    }
}

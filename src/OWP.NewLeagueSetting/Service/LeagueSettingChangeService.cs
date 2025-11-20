using System.Reflection;

namespace OWP.NewLeagueSetting.Service;

public interface ILeagueSettingChangeService
{
    List<LogDetail> LogProcessingChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after);
}


public class LeagueSettingChangeService : ILeagueSettingChangeService
{
    private static readonly Dictionary<string, string> PropertyDisplayNames = new()
    {
        { "League", "League Name" },
        { "LeagueGroup", "League Group" },
        { "NoOfferLive", "No Offer Live" },
        { "HasTv", "Condition TV" },
        { "HasIpTv", "Condition IPTV" },
        { "IpTv", "Condition IPTV" },
        { "Round", "Condition Round" },
        { "Division", "Condition Division" },
        { "Remark", "Remark" },
        { "Spread", "FT Spread" },
        { "MaxbetWD_HDP", "MB Weekday HDP" },
        { "WD_NoOfHDP", "MB Weekday NO.HDP" },
        { "MaxbetWK_HDP", "MB Weekend HDP" },
        { "WK_NoOfHDP", "MB Weekend NO.HDP" },
        { "LstNote", "LST Note" },
        { "Remark2", "Remark 2" }
    };
    private static readonly Dictionary<string, string> NoOfferLiveDisplayNames = new()
    {
        { "0", "None" },
        { "1", "Weekday and Weekend" },
        { "2", "Weekend" },
        { "3", "Weekend PH" }
    };

    public List<LogDetail> LogProcessingChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after)
    {
        var changes = GetChanges(before, after);

        return changes;
    }

    public static List<LogDetail> GetChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after)
    {
        var changes = new List<LogDetail>();
        var properties = typeof(LeagueSettingChangeModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            var beforeValue = property.GetValue(before);
            var afterValue = property.GetValue(after);

            if (AreValuesEqual(beforeValue, afterValue))
            {
                continue;
            }

            var displayName = PropertyDisplayNames.TryGetValue(property.Name, out var name)
                ? name
                : property.Name;
                
            var oldValue = FormatValue(beforeValue);
            var newValue = FormatValue(afterValue);

            if (property.Name == "NoOfferLive")
            {
                oldValue = NoOfferLiveDisplayNames.TryGetValue(oldValue, out var value1) ? value1 : oldValue;

                newValue = NoOfferLiveDisplayNames.TryGetValue(newValue, out var value2) ? value2 : newValue;
            }

            changes.Add(new LogDetail
            {
                Field = displayName,
                OldValue = oldValue,
                NewValue = newValue
            });
        }
        
        return changes;
    }
    
   
    
    public static bool AreValuesEqual(object value1, object value2)
    {   
        if (value1 == null && value2 == null)
        {
            return true;
        }
        if (value1 is string || value2 is string)
        {
            var str1Value = value1 as string ?? "";
            var str2Value = value2 as string ?? "";
            return string.IsNullOrEmpty(str1Value) == string.IsNullOrEmpty(str2Value) && 
                   str1Value == str2Value;
        }
        
        if (value1 is System.Collections.IEnumerable || value2 is System.Collections.IEnumerable)
        {
            return AreEnumerableEqual(value1, value2);
        }
        
        return Equals(value1, value2);
    }
    
    private static bool AreEnumerableEqual(object value1, object value2)
    {
        var list1 = (value1 as System.Collections.IEnumerable)?.Cast<object>().ToList() ?? new List<object>();
        var list2 = (value2 as System.Collections.IEnumerable)?.Cast<object>().ToList() ?? new List<object>();

        if (list1.Count != list2.Count)
        {
            return false;
        }

        if (list1.Count > 0 && list1[0] is Condition)
        {
            var conditionList1 = list1.Cast<Condition>().ToList();
            var conditionList2 = list2.Cast<Condition>().ToList();
            var comparer = new ConditionComparer();
            
            return conditionList1.Count == conditionList2.Count && 
                   conditionList1.TrueForAll(c1 => conditionList2.Exists(c2 => comparer.Equals(c1, c2)));
        }
        
        return !list1.Except(list2).Any();
    }
    
    public static string FormatValue(object value)
    {
        if (value == null || (value is string str && string.IsNullOrEmpty(str)))
        {
            return "";
        }
        if (value is System.Collections.IEnumerable enumValue && !(value is string))
        {
            return FormatEnumerable(enumValue);
        }
        return value?.ToString() ?? "";
    }
    
    private static string FormatEnumerable(System.Collections.IEnumerable enumValue)
    {
        var list = enumValue.Cast<object>().ToList();
        if (list.Count == 0)
        {
            return "";
        }
        
        var formattedItems = new List<string>();
        foreach (var item in list)
        {
            formattedItems.Add(item is Condition condition 
                ? $"{condition.Type} - {condition.Value}"
                : $"{item?.GetType().Name ?? "null"}");
        }
        return string.Join("<br />", formattedItems);
    }
}

using OWP.NewLeagueSetting.Models;
using System.Reflection;
using Newtonsoft.Json;
using NPOI.SS.UserModel;

namespace OWP.NewLeagueSetting.Service;

public interface ILeagueSettingChangeService
{
    void LogProcessingChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after);
}


public class LeagueSettingChangeService : ILeagueSettingChangeService
{
    private static readonly Dictionary<string, string> PropertyDisplayNames = new Dictionary<string, string>
    {
        { "League", "League name" },
        { "NoOfferLive", "No OfferLive" },
        { "HasTv", "Condition HasTv" },
        { "HasIpTv", "Condition HasIpTv" },
        { "IpTv", "Condition IpTv" },
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

    public void LogProcessingChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after)
    {
        var changes = GetChanges(before, after);

        if (changes.Count != 0)
        {
            foreach (var change in changes)
            {

            }
        }
        else
        {
            Console.WriteLine("=== NO CHANGES DURING PROCESSING ===");
        }
    }

    public static List<object> GetChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after)
    {
        var changes = new List<object>();
        var properties = typeof(LeagueSettingChangeModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            var beforeValue = property.GetValue(before);
            var afterValue = property.GetValue(after);
            
            if (!AreValuesEqual(beforeValue, afterValue))
            {
                var displayName = PropertyDisplayNames.ContainsKey(property.Name) 
                    ? PropertyDisplayNames[property.Name] 
                    : property.Name;
                    
                changes.Add(new
                {
                    Key = displayName,
                    OldValue = FormatValue(beforeValue),
                    NewValue = FormatValue(afterValue)
                });
            }
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
        
        if (value1 is System.Collections.IEnumerable enum1 || value2 is System.Collections.IEnumerable enum2)
        {
            var list1 = (value1 as System.Collections.IEnumerable)?.Cast<object>().ToList() ?? new List<object>();
            var list2 = (value2 as System.Collections.IEnumerable)?.Cast<object>().ToList() ?? new List<object>();
            return list1.Count == list2.Count && !list1.Except(list2).Any();
        }
        
        return Equals(value1, value2);
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
            if (item is Condition condition)
            {
                formattedItems.Add($"{condition.Type} - {condition.Value}");
            }
            else
            {
                formattedItems.Add($"{item?.GetType().Name ?? "null"} - {JsonConvert.SerializeObject(item)}");
            }
        }
        return string.Join("\n", formattedItems);
    }
}

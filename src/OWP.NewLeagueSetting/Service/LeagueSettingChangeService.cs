using OWP.NewLeagueSetting.Models;
using System.Reflection;
using Newtonsoft.Json;
using NPOI.SS.UserModel;

namespace OWP.NewLeagueSetting.Service;

public interface ILeagueSettingChangeService
{
    List<object> GetChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after);
    void LogProcessingChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after);
    bool AreValuesEqual(object value1, object value2);
    string FormatValue(object value);
}


public class LeagueSettingChangeService : ILeagueSettingChangeService
{
    public List<object> GetChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after)
    {
        var changes = new List<object>();
        var properties = typeof(LeagueSettingChangeModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            var beforeValue = property.GetValue(before);
            var afterValue = property.GetValue(after);
            
            if (!AreValuesEqual(beforeValue, afterValue))
            {
                changes.Add(new
                {
                    Key = property.Name,
                    OldValue = FormatValue(beforeValue),
                    NewValue = FormatValue(afterValue)
                });
            }
        }
        
        return changes;
    }
    
    public void LogProcessingChanges(LeagueSettingChangeModel before, LeagueSettingChangeModel after)
    {
        var changes = GetChanges(before, after);
        
        if (changes.Any())
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
    
    public bool AreValuesEqual(object value1, object value2)
    {
        if (value1 == null && value2 == null) return true;
        
        if (value1 is string || value2 is string)
        {
            var str1Value = value1 as string ?? "";
            var str2Value = value2 as string ?? "";
            return string.IsNullOrEmpty(str1Value) == string.IsNullOrEmpty(str2Value) && 
                   (string.IsNullOrEmpty(str1Value) || str1Value == str2Value);
        }
        
        if (value1 is System.Collections.IEnumerable enum1 || value2 is System.Collections.IEnumerable enum2)
        {
            var list1 = (value1 as System.Collections.IEnumerable)?.Cast<object>().ToList() ?? new List<object>();
            var list2 = (value2 as System.Collections.IEnumerable)?.Cast<object>().ToList() ?? new List<object>();
            return list1.Count == list2.Count && !list1.Except(list2).Any();
        }
        
        return Equals(value1, value2);
    }
    
    public string FormatValue(object value)
    {
        if (value == null || (value is string str && string.IsNullOrEmpty(str)))
        {
            return "";
        }
        if (value is System.Collections.IEnumerable enumValue && !(value is string))
        {
            var list = enumValue.Cast<object>().ToList();
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
        return value?.ToString() ?? "";
    }
}

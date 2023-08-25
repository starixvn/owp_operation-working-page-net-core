using OWP.Kpi.LA.Models;
namespace OWP.Kpi.LA.Constant;

public static class ConditionConstant
{
    public static readonly List<ConditionInfo> ConditionInfos = new()
    {
        new()
        {
            Progress = "New (5%)",
            Level = "New",
            ConditionsFirst = new Condition
            {
                TotalVirtualMultipleMatches = 2050
            },
            TimeRange = "Last 1 year",
            Reward = "5%"
        },
        new()
        {
            Progress = "Level D (10%)",
            Level = "D",
            ConditionsFirst = new() { VirtualMatches = 3600, MultipleMatches = 500 },
            ConditionSecond = new() { VirtualMatches = 3200, MultipleMatches = 800 },
            TimeRange = "Last 2 year",
            Reward = "10%"
        },
        new()
        {
            Progress = "Level C (15%)",
            Level = "C",
            ConditionsFirst = new() { VirtualMatches = 3600, MultipleMatches = 500, SpecialProducts = 15 },
            ConditionSecond = new() { VirtualMatches = 3200, MultipleMatches = 800, SpecialProducts = 15 },
            TimeRange = "Last 2 year",
            Reward = "15%"
        },
        new()
        {
            Progress = "Level B (20%)",
            Level = "B",
            ConditionsFirst = new() { VirtualMatches = 2800, MultipleMatches = 600, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 350},
            ConditionSecond = new() { VirtualMatches = 2500, MultipleMatches = 800, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 450},
            TimeRange = "Last 2 year",
            Reward = "20%"
        },
        new()
        {
            Progress = "Level A (25%)",
            Level = "A",
            ConditionsFirst = new() { VirtualMatches = 2800, MultipleMatches = 600, SpecialProducts = 15, FiveMajorLeagues =  15, MultipleProducts = 450},
            ConditionSecond = new() { VirtualMatches = 2500, MultipleMatches = 800, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 550},
            TimeRange = "Last 2 year",
            Reward = "25%"
        }
    };
}


using OWP.Kpi.LA.Models;
namespace OWP.Kpi.LA.Constant;

public static class ConditionConstant
{
    public static readonly List<ConditionInfo> ConditionInfos = new()
    {
        new()
        {
            Progress = "New",
            Level = "New",
            ConditionFirst = new Condition
            {
                TotalVirtualMultipleMatches = 2050
            },
            TimeRange = "Last 1 year",
            Reward = "5%"
        },
        new()
        {
            Progress = "Level D",
            Level = "D",
            ConditionFirst = new() { VirtualMatches = 3600, MultipleMatches = 500 },
            ConditionSecond = new() { VirtualMatches = 3200, MultipleMatches = 800 },
            TimeRange = "Last 2 years",
            Reward = "10%"
        },
        new()
        {
            Progress = "Level C",
            Level = "C",
            ConditionFirst = new() { VirtualMatches = 3600, MultipleMatches = 500, SpecialProducts = 15 },
            ConditionSecond = new() { VirtualMatches = 3200, MultipleMatches = 800, SpecialProducts = 15 },
            TimeRange = "Last 2 years",
            Reward = "15%"
        },
        new()
        {
            Progress = "Level B",
            Level = "B",
            ConditionFirst = new() { VirtualMatches = 2800, MultipleMatches = 600, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 350},
            ConditionSecond = new() { VirtualMatches = 2500, MultipleMatches = 800, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 450},
            TimeRange = "Last 2 years",
            Reward = "20%"
        },
        new()
        {
            Progress = "Level A",
            Level = "A",
            ConditionFirst = new() { VirtualMatches = 2800, MultipleMatches = 600, SpecialProducts = 15, FiveMajorLeagues =  15, MultipleProducts = 450},
            ConditionSecond = new() { VirtualMatches = 2500, MultipleMatches = 800, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 550},
            TimeRange = "Last 2 years",
            Reward = "25%"
        }
    };
}


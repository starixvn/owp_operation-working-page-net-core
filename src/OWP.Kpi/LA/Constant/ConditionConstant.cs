using OWP.Kpi.LA.Models;
namespace OWP.Kpi.LA.Constant;

public static class ConditionConstant
{
    private const string LAST_TWO_YEAR = "Last 2 years";

    public static readonly ConditionInfo[] ConditionInfos =
    {
        new()
        {
            Progress = "New",
            Level = "New",
            LevelId = 1,
            ConditionFirst = new Condition
            {
                TotalVirtualMultipleMatches = 2050
            },
            TimeRange = "Last 1 year",
            Reward = "5%",
            TargetCondition = new Condition
            {
                VirtualMatches = 2000 ,
                MultipleMatches = 50
            }
        },
        new()
        {
            Progress = "Level D",
            Level = "D",
            LevelId = 2,
            ConditionFirst = new Condition { VirtualMatches = 3600, MultipleMatches = 500 },
            ConditionSecond = new Condition { VirtualMatches = 3200, MultipleMatches = 800 },
            TimeRange = LAST_TWO_YEAR,
            Reward = "10%",
            TargetCondition = new Condition
            {
                VirtualMatches = 3200,
                MultipleMatches = 500
            }
        },
        new()
        {
            Progress = "Level C",
            Level = "C",
            LevelId = 3,
            ConditionFirst = new Condition { VirtualMatches = 3600, MultipleMatches = 500, SpecialProducts = 15 },
            ConditionSecond = new Condition { VirtualMatches = 3200, MultipleMatches = 800, SpecialProducts = 15 },
            TimeRange = LAST_TWO_YEAR,
            Reward = "15%",
            TargetCondition = new Condition {VirtualMatches = 3200, MultipleMatches = 500, SpecialProducts = 15}
        },
        new()
        {
            Progress = "Level B",
            Level = "B",
            LevelId = 4,
            ConditionFirst = new Condition { VirtualMatches = 2800, MultipleMatches = 600, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 350},
            ConditionSecond = new Condition { VirtualMatches = 2500, MultipleMatches = 800, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 450},
            TimeRange = LAST_TWO_YEAR,
            Reward = "20%",
            TargetCondition = new Condition { VirtualMatches = 2500, MultipleMatches = 600, SpecialProducts = 15, FiveMajorLeagues = 15,MultipleProducts = 350}
        },
        new()
        {
            Progress = "Level A",
            Level = "A",
            LevelId = 5,
            ConditionFirst = new Condition { VirtualMatches = 2800, MultipleMatches = 600, SpecialProducts = 15, FiveMajorLeagues =  15, MultipleProducts = 450},
            ConditionSecond = new Condition { VirtualMatches = 2500, MultipleMatches = 800, SpecialProducts = 15, FiveMajorLeagues = 15, MultipleProducts = 550},
            TimeRange = LAST_TWO_YEAR,
            Reward = "25%",
            TargetCondition = new Condition { VirtualMatches = 2500, MultipleMatches = 600, SpecialProducts = 15, FiveMajorLeagues = 15,MultipleProducts = 450}
        }
    };
}

public static class ReportConstant
{
    private const int ProgressColumn = 3;
    private const int TargetColumn = 4;
    private const int VirtualMatchesGapColumn = 5;
    private const int MultipleMatchesGapColumn = 6;
    private const int SpecialProductsGapColumn = 7;
    private const int MajorProductsGapColumn = 8;
    private const int MultipleProductsGapColumn = 9;
    private const int RewardGapColumn = 15;

    public static readonly int[] DeleteColumns =
    {
        ProgressColumn,
        TargetColumn,
        VirtualMatchesGapColumn,
        MultipleMatchesGapColumn,
        SpecialProductsGapColumn,
        MajorProductsGapColumn,
        MultipleProductsGapColumn,
        RewardGapColumn
    };
}

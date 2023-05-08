namespace OWP.AssigneeSuggestionList.Constant;

public enum MixMatchCapacity
{
    MaxGame1G,
    MaxGame2G,
    MaxGame3G,
    None
}

public enum AssigneeCategory
{
    Free,
    Capacity2GDoing1G,
    Capacity3GDoing1G,
    Capacity3GDoing2G,
    PartiallyFree,
    Busy
}

public enum CategoryType
{
    FullyAvailable,
    PartiallyAvailable,
    Busy
}

public static class ExplanationConstants
{
    public const string FREE = "100% free from {0} to {1}";
    public const string CAPACITY_2G_DOING_1G = "doing 1 of 2 games in capacity";
    public const string CAPACITY_3G_DOING_1G = "doing 1 of 3 games in capacity";
    public const string CAPACITY_3G_DOING_2G = "doing 2 of 3 games in capacity";
    public const string PARTIALLY_AVAILABLE = "free in the first half of the match from {0} to {1}";
}

public static class CategoryConstants
{
    public const decimal MAX_BET_5K = 5000m;
}

namespace OWP.Maxbet.Constants;

using Enums;

public static class MaxBetSetting
{
    public const decimal SPREAD_HDP_PEN = (decimal)0.08;

    public const decimal SPREAD_OU_PEN = (decimal)0.1;

    public const decimal MAXBET_HDP = 70000;

    public const decimal MAXBET_ET = 35000;

    public const decimal SPREAD_SABA = (decimal)0.16;

    public const decimal MAXBET_SABA = 2000;

    public const decimal SPREAD_FUTSAL_PEN = (decimal)0.2;

    public const decimal FOLLOW_MAIN_MATCH = -1;

    public static readonly int[] IGNORE_MATCH_OFFER_ID = {
        (int)EnumMatchOffer.KO15,
        (int)EnumMatchOffer.FastMarket
    };
}
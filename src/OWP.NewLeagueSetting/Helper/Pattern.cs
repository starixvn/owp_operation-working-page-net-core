namespace OWP.NewLeagueSetting.Helper;

public static partial class Pattern
{
    [GeneratedRegex("^[0-9]+(\\.[0-9]+)?[%][-][0-9]+(\\.[0-9]+)?[%]*$")]
    public static partial Regex SpreadRegex();

    [GeneratedRegex("^[0-9]+(\\.[0-9]+)?[Kk]$")]
    public static partial Regex MaxbetRegex();

    [GeneratedRegex(@"^(1HDP|2HDP|2\+1H|3HDP|4HDP|2\+4 lines|3\+3 lines|3\+4 lines|4\+4 lines|1 line|2 lines)*$")]
    public static partial Regex NoHDPRegex();
}
namespace OWP.OtherSport.Validation.Models;

public class OSMatchInfo : IEquatable<OSMatchInfo>
{
    public int? Id { get; set; }
    public DateTime DateTime { get; set; }
    public string League { get; set; }
    public string Home { get; set; }
    public string Away { get; set; }
    public string Sport { get; set; }

    public bool Equals(OSMatchInfo other)
        => EqualMatch(other) && EqualLeague(other);

    private bool EqualMatch(OSMatchInfo other)
        => Home == other.Home && Away == other.Away && Sport == other.Sport;

    private bool EqualLeague(OSMatchInfo other)
         => DateTime == other.DateTime
            && League == other.League;

    public override string ToString() => $"(DateTime: {DateTime.ToString("yyyy-MM-dd H:mm:ss")}, League: {League}, Home: {Home}, Away: {Away}, Sport: {Sport})";
}
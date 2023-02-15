namespace OWP.LiveDetail.InfositeMatch;

using Agent;
using Television;

public class InfositeMatchModel
{
    public TimeSpan KOTime { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public string Round { get; set; }

    public string Division { get; set; }

    public string WB { get; set; }

    public string Channel { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public List<Agent> Agents { get; set; }

    public List<Television> Televisions { get; set; }
}
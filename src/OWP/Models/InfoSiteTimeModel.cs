using OWP.Utils;

namespace OWP.Models;
public class InfoSiteTimeModel
{
    public int AdminSiteMatchId { get; set; }

    public int ParentMatchId { get; set; }

    public DateTime EventDateTime { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public string KickOffTimeDisplay => KickOffTime.ToOWPTimeFormat();

}

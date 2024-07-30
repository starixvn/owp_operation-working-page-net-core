using System;
using OWP.Utils;

namespace OWP.OddsCompiler.Models;
public class InfoSiteTimeModel
{
    public int ParentMatchId { get; set; }

    public DateTime EventDateTime { get; set; }

    public TimeSpan KickOffTime { get; set; }

    public string KickOffTimeDisplay => KickOffTime.ToOWPTimeFormat();

}
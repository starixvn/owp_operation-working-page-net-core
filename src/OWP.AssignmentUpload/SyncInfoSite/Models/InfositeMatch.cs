using System;

namespace OWP.AssignmentUpload.SyncInfoSite.Models;

public class InfositeMatch
{
    public int InfoSiteMatchId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public DateTime EventDateTime { get; set; }

    public string Round { get; set; }

    public string Division { get; set; }

    public string Agent { get; set; }

    public string TV { get; set; }

    public string Channel { get; set; }

    public string WB { get; set; }

    public string Website1 { get; set; }

    public string Website2 { get; set; }

    public string Website3 { get; set; }

    public string Website4 { get; set; }

    public string IPTV { get; set; }

    public bool IsNeutral { get; set; }

    public int AdminSiteMatchId { get; set; }

    public string EtPk { get; set; }
}
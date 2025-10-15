namespace OWP.Event.LiveGameAssignment.Download.Models;

using OWP.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

[Serializable]
[XMLName("Row")]
public class EventModel
{
    private string _time;
    private string _otherTime;

    [ExcelIgnore]
    [XMLName("RowNum")]
    public int RowNum { get; set; }

    [ExcelIgnore]
    public string EventDate { get; set; }

    [Column(ColumnName.No)]
    public string MatchNo { get; set; }

    [Column(ColumnName.MatchStatus)]
    [XmlIgnore]
    public string MatchStatus
    {
        get; set;
    }

    [Column(ColumnName.InfositeId)]
    public string InfoSiteMatchId { get; set; }

    [Column(ColumnName.AdminSiteId)]
    public string AdminSiteId { get; set; }

    [XMLName("KickOffTime")]
    [Column(ColumnName.Time)]
    public string Time
    {
        get => _time;
        set =>
            _time = DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time)
                ? time.ToString("hh:mm tt", CultureInfo.InvariantCulture) : value;
    }

    [Column(ColumnName.OtherTime)]
    public string OtherTime
    {
        get => _otherTime;

        set =>
            _otherTime = DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time)
                ? time.ToString("hh:mm tt", CultureInfo.InvariantCulture) : value;
    }

    [Column(ColumnName.MatchOffer)]
    public string MatchOffer { get; set; }

    [ExcelIgnore]
    [XmlIgnore]
    public IEnumerable<MatchOffer> MatchOfferList { get; set; }

    [Column(ColumnName.LSTNote)]
    public string LSTNote { get; set; }

    [ExcelIgnore]
    [XmlIgnore]
    public TimeSpan KickOffTime { get; set; }

    [Column(ColumnName.League)]
    public string LeagueName { get; set; }

    [Column(ColumnName.Home)]
    public string HomeName { get; set; }

    [Column(ColumnName.Away)]
    public string AwayName { get; set; }

    [Column(ColumnName.Note)]
    public IEnumerable<string> Notes { get; set; }

    [ExcelIgnore]
    [XMLName("Note")]
    public string CombineNotes => string.Join("@#", Notes.Where(k => !string.IsNullOrEmpty(k)));

    [Column(ColumnName.Round)]
    public string Round { get; set; }

    [Column(ColumnName.TeamRound)]
    public string TeamRound { get; set; }

    [Column(ColumnName.Division)]
    public string OutSide { get; set; }

    [Column(ColumnName.Remark)]
    public string Remark { get; set; }

    [Column(ColumnName.NoOfHDP)]
    public string NoOfHDP { get; set; }

    [XmlIgnore]
    public AssignmentModel Assignments { get; set; }

    [Column(ColumnName.Agent)]
    public string Agent { get; set; }

    [Column(ColumnName.AgentEst)]
    public string AgentEst { get; set; }

    [Column(ColumnName.RemarkEst)]
    public string RemarkEst { get; set; }

    [Column(ColumnName.RemarkBy)]
    public string RemarkBy { get; set; }

    [Column(ColumnName.TV)]
    public string TVCode { get; set; }

    [Column(ColumnName.Channel)]
    public string Channel { get; set; }

    [Column(ColumnName.IPTV)]
    public string IPTV { get; set; }

    [Column(ColumnName.StreamingInfoWB)]
    public string WatchBet { get; set; }

    [Column(ColumnName.OtherProvider)]
    public string OtherProvider { get; set; }

    [Column(ColumnName.Provider)]
    public string Provider { get; set; }

    [Column(ColumnName.ReporterPhone)]
    public string ReporterPhone { get; set; }

    [Column(ColumnName.Web1)]
    public string Website { get; set; }

    [Column(ColumnName.Web2)]
    public string Website2 { get; set; }

    [Column(ColumnName.Web3)]
    public string Website3 { get; set; }

    [Column(ColumnName.Web4)]
    public string Website4 { get; set; }
    
    [Column(ColumnName.AIOBT)]
    public string AIOBT { get; set; } 
    
    [Column(ColumnName.SOP)]
    public string SOP { get; set; }

    [ExcelIgnore]
    public bool IsSelected => false;

    [ExcelIgnore]
    public bool IsSaBa => LeagueName.IsContains("SABA");

    [ExcelIgnore]
    public int SelectedBy => 0;

    [ExcelIgnore]
    public bool IsSoloMatch => false;

    [ExcelIgnore]
    public string ExtraTimeRemark => string.Empty;

    [ExcelIgnore]
    public string Venue => string.Empty;

    [ExcelIgnore]
    public string LocalTV => string.Empty;

    [ExcelIgnore]
    public string LiveTV => string.Empty;

    [ExcelIgnore]
    public int MinorBettypeId
    {
        get; set;
    }

    [ExcelIgnore]
    public DateTime EventDateTime
    {
        get; set;
    }

    [ExcelIgnore]
    public bool IsSyncLive
    {
        get;
        set;
    }

}

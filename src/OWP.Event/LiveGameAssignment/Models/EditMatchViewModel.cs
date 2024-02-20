namespace OWP.Event.LiveGameAssignment.Models;

using System;

public class EditMatchViewModel
{
    public int OWPMatchId { get; set; }

    public string MatchNo { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public TimeSpan KOTime { get; set; }
}
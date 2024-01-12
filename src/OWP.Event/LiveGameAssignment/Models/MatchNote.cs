namespace OWP.Event.LiveGameAssignment.Models;

using System;

public class MatchNote
{
    public int OWPMatchId { get; set; }

    public int MatchNoteId { get; set; }

    public string Note { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }

    public DateTime LastUpdated { get; set; }
}
namespace OWP.LiveDetail.MatchNote;

using System;

public class MatchNoteModel
{
    public int MatchNoteId { get; set; }

    public string Username { get; set; }

    public string Note { get; set; }

    public DateTime LastUpdated { get; set; }
}
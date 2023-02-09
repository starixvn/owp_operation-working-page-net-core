namespace OWP.LiveDetail.Assignment;

using System;

[Serializable]
public class ValidateModel
{
    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public int AssignmentType { get; set; }

    public string Username { get; set; }

    public int? UserId { get; set; }
}
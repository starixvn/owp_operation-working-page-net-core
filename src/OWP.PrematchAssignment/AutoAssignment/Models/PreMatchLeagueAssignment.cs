namespace OWP.PrematchAssignment.Models;

public class PreMatchLeagueAssignment
{
    public int PreMatchLeagueId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public int Bettype { get; set; }
    public bool IsTakeOver { get; set; }
    public byte AssignmentType { get; set; }
}
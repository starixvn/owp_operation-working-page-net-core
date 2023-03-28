using System.Text.Json.Serialization;

namespace OWP.AssigneeSuggestionList.Models;

public class UserInfo
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public string IsManagementRole { get; set; }

    public string Role { get; set; }

    public string Group { get; set; }

    public string Level { get; set; }

    public string LevelLabel { get; set; }

    public string MainBetType { get; set; }

    public string MaxMultipleGames { get; set; }

    public string Explanation { get; set; }

    public DateTime ShiftStartTime { get; set; }

    public DateTime ShiftEndTime { get; set; }
}

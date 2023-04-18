using Newtonsoft.Json;
using OWP.AssigneeSuggestionList.Constant;

namespace OWP.AssigneeSuggestionList.Models;

public class UserInfo
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public string Role { get; set; }

    public string Level { get; set; }

    public string LevelLabel { get; set; }

    public string MainBetType { get; set; }

    public string MaxMultipleGames { get; set; }

    [JsonIgnore]
    public MixMatchCapacity MixMatchCapacity => MaxMultipleGames switch
    {
        "1G" or "1G-no LA" => MixMatchCapacity.MaxGame1G,
        "2G" or "2G-1LA" => MixMatchCapacity.MaxGame2G,
        "3G" or "3G-2LAs" => MixMatchCapacity.MaxGame3G,
        _ => MixMatchCapacity.None
    };

    public string Explanation { get; set; }

    public DateTime ShiftStartTime { get; set; }

    public DateTime ShiftEndTime { get; set; }

    public string Shift => ShiftStartTime.ToString("HH:mm")
                           + " > " + ShiftEndTime.ToString("HH:mm");

    public bool IsManagementRole => WorkingShiftConst.ManagementRole.Any(x => x.Equals(Role));
}

using OWP.Models;
using OWP.ProductAssignmentReference.Models;

public class UserInfo
{
    public string Username { get; set; }

    public int UserId { get; set; }

    public int LevelId { get; set; }

    public int RoleId { get; set; }

    public string BetTypePreferencesId { get; set; }

    public int MultipleGamesId { get; set; }

    public string AssignmentRemark { get; set; }

    public int SectionId { get; set; }

    public List<MainBetTypePreferences> BetTypePreferencesValues { get; set; }
}
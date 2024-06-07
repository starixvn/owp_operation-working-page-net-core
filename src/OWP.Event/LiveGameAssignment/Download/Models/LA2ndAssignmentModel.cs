using OWP.Enums;

namespace OWP.Event.LiveGameAssignment.Download.Models;

[Serializable]
public class LA2ndAssignmentModel : LAAssignmentModel
{
    public LA2ndAssignmentModel()
    {
        Bettype = (int)MajorBetType.La2Nd;
    }

    [Column(ColumnName.LA)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherLA)]
    public string OtherTraders { get; set; }
}
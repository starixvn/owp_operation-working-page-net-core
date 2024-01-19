namespace OWP.Event.LiveGameAssignment.Download.Models.EventAssignment;

using OWP.Extensions;
using System;
using System.Xml.Serialization;

[Serializable]
public class AssignmentModel
{
    public AssignmentModel()
    {
        Handicap = new HandicapAssignmentModel();
        Handicap1st = new Handicap1stAssignmentModel();
        OverUnder = new OverUnderAssignmentModel();
        OverUnder1st = new OverUnder1stAssignmentModel();
        OneXTwo = new OneXTwoAssignmentModel();
        OneXTwo1st = new OneXTwo1stAssignmentModel();
        SpecificXMins = new SpecificXMinsAssignmentModel();
        Specific15Mins = new Specific15MinsAssignmentModel();
        Specific10Mins = new Specific10MinsAssignmentModel();
        LA2nd = new LA2ndAssignmentModel();
        FM = new FMAssignmentModel();
        AIO = new AIOAssignmentModel();
    }

    [ExcelIgnore]
    [XmlIgnore]
    public int MatchId { get; set; }

    public HandicapAssignmentModel Handicap { get; set; }

    public Handicap1stAssignmentModel Handicap1st { get; set; }

    public OverUnderAssignmentModel OverUnder { get; set; }

    public OverUnder1stAssignmentModel OverUnder1st { get; set; }

    public OneXTwoAssignmentModel OneXTwo { get; set; }

    public OneXTwo1stAssignmentModel OneXTwo1st { get; set; }

    public SpecificXMinsAssignmentModel SpecificXMins { get; set; }

    public Specific15MinsAssignmentModel Specific15Mins { get; set; }

    public Specific10MinsAssignmentModel Specific10Mins { get; set; }

    public LA2ndAssignmentModel LA2nd { get; set; }

    public FMAssignmentModel FM { get; set; }

    public AIOAssignmentModel AIO { get; set; }
}
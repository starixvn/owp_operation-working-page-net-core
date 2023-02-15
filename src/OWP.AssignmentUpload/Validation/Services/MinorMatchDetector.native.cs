namespace OWP.AssignmentUpload.Validation.Services;

using Models;

public interface MinorMatchDetector
{
    List<AssignmentModel> Detect(List<AssignmentModel> assignments);
}

public class MinorMatchDetectorImp : MinorMatchDetector
{
    private readonly ExplicitMinorMatchRule1 explicitMinorMatchRule1;
    private readonly ExplicitMinorMatchRule2 explicitMinorMatchRule2;

    public MinorMatchDetectorImp(
        ExplicitMinorMatchRule1 explicitMinorMatchRule1,
        ExplicitMinorMatchRule2 explicitMinorMatchRule2)
    {
        this.explicitMinorMatchRule1 = explicitMinorMatchRule1;
        this.explicitMinorMatchRule2 = explicitMinorMatchRule2;
    }

    public List<AssignmentModel> Detect(List<AssignmentModel> assignments)
    {
        foreach (var assignment in assignments)
        {
            DetectMinorMatch(assignment, assignments);
        }

        return assignments;
    }

    private void DetectMinorMatch(AssignmentModel assignment, List<AssignmentModel> assignments)
    {
        var detectedMinor = explicitMinorMatchRule1.DetectMinorBettypeIdRule(assignment, assignments);
        if (!detectedMinor)
        {
            explicitMinorMatchRule2.DetectMinorBettypeIdRule(assignment, assignments);
        }
    }
}
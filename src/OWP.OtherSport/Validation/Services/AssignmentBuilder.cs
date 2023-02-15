namespace OWP.OtherSport.Validation.Services;

using Models;

public interface AssignmentBuilder
{
    List<AssignmentModel> BuildAssignment(
        List<Trader> traders,
        List<AssignmentModelInExcel> assignmentInExcels);
}

public class AssignmentBuilderImp : AssignmentBuilder
{
    private readonly AssignmentMapper assignmentMapper;

    public AssignmentBuilderImp(AssignmentMapper assignmentMapper)
    {
        this.assignmentMapper = assignmentMapper;
    }

    public List<AssignmentModel> BuildAssignment(
        List<Trader> traders,
        List<AssignmentModelInExcel> assignmentInExcels)
    {
        assignmentMapper.Traders = traders;
        var assignmentModels = assignmentMapper.Map(assignmentInExcels);

        return assignmentModels;
    }
}
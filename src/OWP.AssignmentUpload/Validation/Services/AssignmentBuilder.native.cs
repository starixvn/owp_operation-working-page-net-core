using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Validation.Services;

public interface AssignmentBuilder
{
    List<AssignmentModel> BuildAssignment(
        List<Trader> traders,
        ValidationRequest request,
        List<AssignmentModelInExcel> assignmentInExcels);
}

public class AssignmentBuilderImp : AssignmentBuilder
{
    private readonly AssignmentMapper assignmentMapper;
    private readonly MinorMatchDetector minorMatchDetector;
        private readonly ImplicitMinorMatchGenerator minorMatchGenerator;

    public AssignmentBuilderImp(
        AssignmentMapper assignmentMapper,
        MinorMatchDetector minorMatchDetector,
             ImplicitMinorMatchGenerator minorMatchGenerator)
    {
        this.assignmentMapper = assignmentMapper;
        this.minorMatchDetector = minorMatchDetector;
            this.minorMatchGenerator = minorMatchGenerator;
    }

    public List<AssignmentModel> BuildAssignment(
        List<Trader> traders,
        ValidationRequest request,
        List<AssignmentModelInExcel> assignmentInExcels)
    {
        assignmentMapper.Traders = traders;
        var assignmentModels = assignmentMapper.Map(request, assignmentInExcels);
        assignmentModels = minorMatchDetector.Detect(assignmentModels);
            assignmentModels = minorMatchGenerator.GenerateMinorMatch(assignmentModels);

        return assignmentModels;
    }
}
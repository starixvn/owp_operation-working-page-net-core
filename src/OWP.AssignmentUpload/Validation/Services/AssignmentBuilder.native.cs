using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Validation.Services
{
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
        private readonly ImplicitMinorMatchGenerater minorMatchGenerater;

        public AssignmentBuilderImp(
            AssignmentMapper assignmentMapper,
             MinorMatchDetector minorMatchDetector,
             ImplicitMinorMatchGenerater minorMatchGenerater)
        {
            this.assignmentMapper = assignmentMapper;
            this.minorMatchDetector = minorMatchDetector;
            this.minorMatchGenerater = minorMatchGenerater;
        }

        public List<AssignmentModel> BuildAssignment(
            List<Trader> traders,
            ValidationRequest request,
            List<AssignmentModelInExcel> assignmentInExcels)
        {
            assignmentMapper.Traders = traders;
            var assignmentModels = assignmentMapper.Map(request, assignmentInExcels);
            assignmentModels = minorMatchDetector.Detect(assignmentModels);
            assignmentModels = minorMatchGenerater.GenerateMinorMatch(assignmentModels);

            return assignmentModels;
        }
    }
}

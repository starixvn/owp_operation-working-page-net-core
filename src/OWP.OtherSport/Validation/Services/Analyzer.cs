namespace OWP.OtherSport.Validation.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using OWP.OtherSport.Validation.Models;

    public interface Analyzer
    {
        Task<AnalyzedResult> Analyze(ValidationHandler.Request request);
    }

    public class AnalyzerImp : Analyzer
    {
        private readonly ExcelReader excelReader;
        private readonly Repository repository;
        private readonly AssignmentBuilder assignmentBuilder;
        private readonly AssignmentValidator assignmentValidator;
        private readonly DateTimeValidator dateTimeValidator;

        public AnalyzerImp(
            ExcelReader excelReader,
            Repository repository,
            AssignmentBuilder assignmentBuilder,
            AssignmentValidator assignmentValidator,
            DateTimeValidator dateTimeValidator)
        {
            this.excelReader = excelReader;
            this.repository = repository;
            this.assignmentBuilder = assignmentBuilder;
            this.assignmentValidator = assignmentValidator;
            this.dateTimeValidator = dateTimeValidator;
        }

        public async Task<AnalyzedResult> Analyze(ValidationHandler.Request request)
        {
            var rawDataFromExcel = excelReader.Read(request.Stream);

            var traders = await repository.GetTraders(rawDataFromExcel.Traders);

            var dateTimeValidationResult = dateTimeValidator.Validate(rawDataFromExcel.Assignments);
            var isMappingDateTime = !dateTimeValidationResult.Any();

            var assignments = assignmentBuilder.BuildAssignment(traders, rawDataFromExcel.Assignments, isMappingDateTime);

            assignmentValidator.Validate(rawDataFromExcel, traders, dateTimeValidationResult);

            return new AnalyzedResult
            {
                Assignments = assignments,
                OriginalRecordCount = rawDataFromExcel.Assignments.Count
            };
        }
    }
}
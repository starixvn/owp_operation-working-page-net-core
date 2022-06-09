namespace OWP.OtherSport.Validation.Services
{
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

        public AnalyzerImp(
            ExcelReader excelReader,
            Repository repository,
            AssignmentBuilder assignmentBuilder,
            AssignmentValidator assignmentValidator)
        {
            this.excelReader = excelReader;
            this.repository = repository;
            this.assignmentBuilder = assignmentBuilder;
            this.assignmentValidator = assignmentValidator;
        }

        public async Task<AnalyzedResult> Analyze(ValidationHandler.Request request)
        {
            var rawDataFromExcel = excelReader.Read(request.Stream);

            var traders = await repository.GetTraders(rawDataFromExcel.Traders);

            assignmentValidator.Validate(rawDataFromExcel, traders);

            var warningDeleteResults = await assignmentValidator.WarningDeleteMatch(rawDataFromExcel.Assignments);

            var assignments = assignmentBuilder.BuildAssignment(traders, rawDataFromExcel.Assignments);

            return new AnalyzedResult
            {
                Assignments = assignments,
                OriginalRecordCount = rawDataFromExcel.Assignments.Count,
                WarningDeleteResults = warningDeleteResults
            };
        }
    }
}
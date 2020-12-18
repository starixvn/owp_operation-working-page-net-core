namespace OWP.OtherSport.Validation.Services
{
    using System.Threading.Tasks;
    using OWP.OtherSport.Validation.Models;

    public interface Analyzer
    {
        Task<AnalyzedResult> Analyze(ValidationHandler.Request request);
    }

    public class AnalyzerImp : Analyzer
    {
        private readonly ExcelReader excelReader;

        public AnalyzerImp(
            ExcelReader excelReader)
        {
            this.excelReader = excelReader;
        }

        public async Task<AnalyzedResult> Analyze(ValidationHandler.Request request)
        {
            var rawDataFromExcel = excelReader.Read(request.Stream);

            //var traders = await repository.GetTraders(rawDataFromExcel.Traders);

            //var assignments = assignmentBuilder.BuildAssignment(traders, request, rawDataFromExcel.Assignments);

            //assignmentValidator.Validate(rawDataFromExcel, traders);

            return new AnalyzedResult();
            //{
                //Assignments = assignments,
                //OriginalRecordCount = rawDataFromExcel.Assignments.Count
            //};
        }
    }
}
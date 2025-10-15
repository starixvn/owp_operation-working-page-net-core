using OWP.AssignmentUpload.Validation.Models;
using OWP.Services;

namespace OWP.AssignmentUpload.Validation.Services;

public interface Analyzer
{
    Task<AnalyzedResult> Analyze(ValidationRequest request);
}

public class AnalyzerImp(
    AssignmentValidator assignmentValidator,
    InfoSiteMatchUpdater infoSiteMatchUpdater,
    ExcelReader excelReader,
    MatchStatusValidator matchStatusValidator,
    Repository repository,
    AssignmentBuilder assignmentBuilder,
    InfoSiteValidation infoSiteValidation,
    RefResourceService refResourceService)
    : Analyzer
{
    public async Task<AnalyzedResult> Analyze(ValidationRequest request)
    {
        var rawDataFromExcel = excelReader.Read(request.Stream);

        var traders = await repository.GetTraders(rawDataFromExcel.Traders);
        var matchOffers = (await refResourceService.GetRefResourcesByName(RefName.MatchOffer)).ToList();

        assignmentValidator.Validate(request.EventDate, rawDataFromExcel, traders, request.IsUploadFMAndAIO, matchOffers);

        var assignments = assignmentBuilder.BuildAssignment(traders, request, rawDataFromExcel.Assignments);

        await infoSiteMatchUpdater.UpdateInfoSiteMatchToOwpMatch(assignments, request.EventDate);

        matchStatusValidator.Validate(assignments);

        infoSiteValidation.Validate(assignments);

        return new AnalyzedResult
        {
            Assignments = assignments,
            OriginalRecordCount = rawDataFromExcel.Assignments.Count
        };
    }
}
using OWP.AssignmentUpload.Validation.Models;
using OWP.Services;

namespace OWP.AssignmentUpload.Validation.Services;

public interface Analyzer
{
    Task<AnalyzedResult> Analyze(ValidationRequest request);
}

public class AnalyzerImp : Analyzer
{
    private readonly AssignmentValidator assignmentValidator;
    private readonly ExcelReader excelReader;
    private readonly MatchStatusValidator matchStatusValidator;
    private readonly InfoSiteMatchUpdater infoSiteMatchUpdater;
    private readonly Repository repository;
    private readonly AssignmentBuilder assignmentBuilder;
    private readonly InfoSiteValidation infoSiteValidation;
    private readonly RefResourceService refResourceService;

    public AnalyzerImp(
        AssignmentValidator assignmentValidator,
        InfoSiteMatchUpdater infoSiteMatchUpdater,
        ExcelReader excelReader,
        MatchStatusValidator matchStatusValidator,
        Repository repository,
        AssignmentBuilder assignmentBuilder,
        InfoSiteValidation infoSiteValidation,
        RefResourceService refResourceService)
    {
        this.assignmentValidator = assignmentValidator;
        this.excelReader = excelReader;
        this.matchStatusValidator = matchStatusValidator;
        this.infoSiteMatchUpdater = infoSiteMatchUpdater;
        this.repository = repository;
        this.assignmentBuilder = assignmentBuilder;
        this.infoSiteValidation = infoSiteValidation;
        this.refResourceService = refResourceService;

    }

    public async Task<AnalyzedResult> Analyze(ValidationRequest request)
    {
        var rawDataFromExcel = excelReader.Read(request.Stream);

        var traders = await repository.GetTraders(rawDataFromExcel.Traders);
        var matchOffers = (await refResourceService.GetRefResourcesByName(RefName.MatchOffer)).ToList();

        var assignments = assignmentBuilder.BuildAssignment(traders, request, rawDataFromExcel.Assignments);

        await infoSiteMatchUpdater.UpdateInfoSiteMatchToOwpMatch(assignments, request.EventDate);

        assignmentValidator.Validate(request.EventDate, rawDataFromExcel, traders, request.IsUploadFMAndAIO, matchOffers);

        matchStatusValidator.Validate(assignments);

        infoSiteValidation.Validate(assignments);



        return new AnalyzedResult
        {
            Assignments = assignments,
            OriginalRecordCount = rawDataFromExcel.Assignments.Count
        };
    }
}
using NSubstitute;
using OWP.OtherSport.Validation;
using OWP.OtherSport.Validation.Models;
using OWP.OtherSport.Validation.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.OtherSport.UnitTests.Validation.Services;

public class AnalyzerUnitTests
{
    private readonly ExcelReader excelReader;
    private readonly Repository repository;
    private readonly AssignmentBuilder assignmentBuilder;
    private readonly AssignmentValidator assignmentValidator;

    public AnalyzerUnitTests()
    {
        excelReader = Substitute.For<ExcelReader>();
        repository = Substitute.For<Repository>();
        assignmentBuilder = Substitute.For<AssignmentBuilder>();
        assignmentValidator = Substitute.For<AssignmentValidator>();
    }

    [Fact]
    public async Task Analyzer_Always_ReturnAnalyzedResult()
    {
        var analyzer = new AnalyzerImp(excelReader, repository, assignmentBuilder, assignmentValidator);

        var rawModelInExcel = GetRawDataFromExcel();
        var traders = GetTraders();
        var assignmentModels = GetAssignmentModels();

        excelReader.Read(Arg.Compat.Any<Stream>(), Arg.Compat.Any<DateTime>(), Arg.Compat.Any<DateTime>()).Returns(rawModelInExcel);
        repository.GetTraders(Arg.Compat.Any<List<string>>()).Returns(traders);
        assignmentBuilder.BuildAssignment(Arg.Compat.Any<List<Trader>>(),
                Arg.Compat.Any<List<AssignmentModelInExcel>>())
            .Returns(assignmentModels);

        var request = new ValidationHandler.Request();
        var expected = GetAnalyzedResult();

        var actual = await analyzer.Analyze(request);

        DeepAssert.Equal(expected, actual);
    }

    public static UploadingDataFromExcel GetRawDataFromExcel()
    {
        var assignmentModelInExcels = new List<AssignmentModelInExcel>
        {
            new ()
            {
                Sport = "Basketball",
                Home = "Golden State Warrior",
                Away = "LA Lake",
                MatchStatus = "Completed",
                DateTime = "44771.5104166667",
                League = "NBA",
                AutoLink = "YES"
            },

            new ()
            {
                Sport = "Ice Hookey",
                Home = "Golden State Warrior",
                Away = "LA Lake",
                MatchStatus = "Completed",
                DateTime = "44771.5104166667",
                League = "Hookey",
                AutoLink = "YES"
            }
        };

        var traders = new List<string>
        {
            "Esther", "Mira", "Jack", "Tyler", "Carlos"
        };

        var rawModelInExcel = new UploadingDataFromExcel
        {
            Assignments = assignmentModelInExcels,
            Traders = traders
        };

        return rawModelInExcel;
    }

    public static List<Trader> GetTraders()
    {
        var traders = new List<Trader>
        {
            new() { UserId = 1, Username = "Esther"},
            new() { UserId = 10, Username = "Mira"},
            new() { UserId = 2, Username = "Jack"},
            new() { UserId = 10, Username = "Tyler"},
            new() { UserId = 2, Username = "Carlos"}
        };

        return traders;
    }

    public static List<AssignmentModel> GetAssignmentModels()
    {
        var model = new AssignmentModel
        {
            Sport = "Basketball",
            Home = "Golden State Warrior",
            Away = "LA Lake",
            MatchStatus = "Completed",
            DateTime = DateTime.Parse("12/10/2020 12:00 AM"),
            League = "NBA",
            AutoLink = "YES"
        };

        var model2 = new AssignmentModel
        {
            Sport = "Ice Hookey",
            Home = "Golden State Warrior",
            Away = "LA Lake",
            MatchStatus = "Completed",
            DateTime = DateTime.Parse("12/10/2020 12:00 AM"),
            League = "Hookey",
            AutoLink = "YES"
        };

        var assignmentModels = new List<AssignmentModel>
        {
            model, model2
        };

        return assignmentModels;
    }

    public static AnalyzedResult GetAnalyzedResult()
    {
        var rawModelInExcel = GetRawDataFromExcel();
        var assignments = GetAssignmentModels();
        var recordCount = rawModelInExcel.Assignments.Count;

        var analyzerResult = new AnalyzedResult
        {
            Assignments = assignments,
            OriginalRecordCount = recordCount
        };

        return analyzerResult;
    }
}
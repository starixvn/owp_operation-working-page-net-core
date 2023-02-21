using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using OfficeOpenXml;
using OWP.AssignmentUpload.Validation;
using OWP.AssignmentUpload.Validation.Exceptions;
using OWP.AssignmentUpload.Validation.Models;
using OWP.AssignmentUpload.Validation.Services;
using OWP.Services;
using Xunit;

namespace OWP.AssignmentUpload.UnitTests.Validation;

public class HandlerUnitTests
{
    private readonly ValidationRequest request;
    private readonly ExcelValidator excelValidator;
    private readonly ConfigurationService configurationService;
    private readonly CacheManipulator cacheManipulator;
    private readonly INapSession currentSession;
    private readonly Analyzer analyzer;
    private readonly string tempDirectory;

    public HandlerUnitTests()
    {
        request = new ValidationRequest();
        excelValidator = Substitute.For<ExcelValidator>();
        configurationService = Substitute.For<ConfigurationService>();
        cacheManipulator = Substitute.For<CacheManipulator>();
        currentSession = Substitute.For<INapSession>();
        analyzer = Substitute.For<Analyzer>();

        var emptyResult = new AnalyzedResult
        {
            Assignments = new List<AssignmentModel>(),
            OriginalRecordCount = 0
        };
        analyzer.Analyze(Arg.Compat.Any<ValidationRequest>()).Returns(Task.FromResult(emptyResult));
        tempDirectory = CreateTempDirectory();
        configurationService.CurrentDirectory.Returns(tempDirectory);
    }

    [Fact]
    public async Task Handle_ExcelHeaderIsInvalid_ResultIsException()
    {
        // Arrange
        excelValidator.When(x => x.Validate(request.Stream)).Do(call => { throw new InvalidExcelFormatException(); });
        var handler = new Handler(analyzer, excelValidator, configurationService, cacheManipulator, currentSession);

        // Act
        var actual = await handler.Handle(request, new CancellationToken(false));

        // Assert
        const string expect = "Incorrect template";
        Assert.Equal(expect, actual.Exception);
    }

    [Fact]
    public async Task Handle_AssignmentValidatorThrowDuplicatedMatchNoException_ResultIsException()
    {
        // Arrange
        var expected = new List<AssignmentValidationRowResult>
        {
            new()
            {
                Row = 1,
                Message = "Duplicate Match No: 'match1' of matches"
            },
            new()
            {
                Row = 2,
                Message = "Duplicate Match No: 'match1' of matches"
            }
        };
        analyzer.Analyze(Arg.Compat.Any<ValidationRequest>()).Throws(new InvalidAssignmentRowException(expected));
        var handler = new Handler(analyzer, excelValidator, configurationService, cacheManipulator, currentSession);

        // Act
        var actual = await handler.Handle(request, new CancellationToken(false));

        // Assert
        Assert.IsType<AssignmentValidationResult>(actual);
        Assert.Equal(expected, actual.ValidationResults);
    }

    [Fact]
    public async Task Handle_RequestStreamWithCapacityIsZero_ResultIsSuccessful()
    {
        // Arrange
        var newRequest = new ValidationRequest { Stream = new MemoryStream() };
        var handler = new Handler(analyzer, excelValidator, configurationService, cacheManipulator, currentSession);

        // Act
        var actual = await handler.Handle(request, new CancellationToken(false));

        // Assert
        var expected = new AssignmentValidationResult
        {
            SessionId = request.SessionId,
            Exception = "",
            ValidationResults = null
        };

        Assert.Equal(expected.SessionId, actual.SessionId);
        Assert.True(actual.IsSuccessful);
        Assert.Empty(actual.Exception);
        if (Directory.Exists(tempDirectory))
        {
            DeleteDirectory(tempDirectory);
        }
    }

    [Fact]
    public async Task Handle_RequestStreamWithByteArray_ResultIsSuccessful()
    {
        // Arrange
        var request = CreateStreamWithExcelPackageData();
        var handler = new Handler(analyzer, excelValidator, configurationService, cacheManipulator, currentSession);

        // Act
        var actual = await handler.Handle(request, new CancellationToken(false));

        // Assert
        var expected = new AssignmentValidationResult
        {
            SessionId = request.SessionId,
            Exception = "",
            ValidationResults = new List<AssignmentValidationRowResult>()
        };

        Assert.Equal(expected.SessionId, actual.SessionId);
        Assert.True(actual.IsSuccessful);
        Assert.Empty(actual.Exception);
        Assert.Equal(expected.ValidationResults, actual.ValidationResults);

        if (Directory.Exists(tempDirectory))
        {
            DeleteDirectory(tempDirectory);
        }
    }

    private static ValidationRequest CreateStreamWithExcelPackageData()
    {
        // Arrange
        var request = new ValidationRequest();
        using (var excelPackage = new ExcelPackage())
        {
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

            worksheet.Cells["A1"].Value = "Date";
            worksheet.Cells[1, 2].Value = new DateTime(2020, 3, 30);

            worksheet.Cells[2, 1].Value = "Match Status";
            worksheet.Cells[2, 2].Value = "InfoSite ID";
            worksheet.Cells[2, 3].Value = "No";
            worksheet.Cells[2, 4].Value = "Time";
            worksheet.Cells[2, 5].Value = "Other Time";
            worksheet.Cells[2, 6].Value = "Provider";
            worksheet.Cells[2, 7].Value = "Other Provider";
            worksheet.Cells[2, 8].Value = "Home";
            worksheet.Cells[2, 9].Value = "Away";
            worksheet.Cells[2, 10].Value = "League";
            worksheet.Cells[2, 11].Value = "Max Bet (HDP)";
            worksheet.Cells[2, 12].Value = "Spread (HDP)";
            worksheet.Cells[2, 13].Value = "Spread (O/U)";
            worksheet.Cells[2, 14].Value = "No.HDP";
            worksheet.Cells[2, 15].Value = "Trader (HDP)";
            worksheet.Cells[2, 16].Value = "Trader (O/U)";
            worksheet.Cells[2, 17].Value = "Trader (1H HDP)";
            worksheet.Cells[2, 18].Value = "Trader (1H O/U)";
            worksheet.Cells[2, 19].Value = "Other Traders (HDP)";
            worksheet.Cells[2, 20].Value = "Other Traders (O/U)";
            worksheet.Cells[2, 21].Value = "Other Traders (1H HDP)";
            worksheet.Cells[2, 22].Value = "Other Traders (1H O/U)";
            worksheet.Cells[2, 23].Value = "15mins";
            worksheet.Cells[2, 24].Value = "10mins";
            worksheet.Cells[2, 25].Value = "Trader (1X2)";
            worksheet.Cells[2, 26].Value = "Other Traders (1X2)";
            worksheet.Cells[2, 27].Value = "FM";
            worksheet.Cells[2, 28].Value = "AIO";
            worksheet.Cells[2, 29].Value = "LA";
            worksheet.Cells[2, 30].Value = "Other LA";
            worksheet.Cells[2, 31].Value = "Division";
            worksheet.Cells[2, 32].Value = "Remark";
            worksheet.Cells[2, 33].Value = "WB";
            worksheet.Cells[2, 34].Value = "Round";
            worksheet.Cells[2, 35].Value = "TV";
            worksheet.Cells[2, 36].Value = "Channel/IPTV";
            worksheet.Cells[2, 37].Value = "Agent";
            worksheet.Cells[2, 38].Value = "Phone";
            worksheet.Cells[2, 39].Value = "Match Offer";
            worksheet.Cells[2, 40].Value = "Web 1";
            worksheet.Cells[2, 41].Value = "Web 2";
            worksheet.Cells[2, 42].Value = "Web 3";
            worksheet.Cells[2, 43].Value = "Web 4";
            worksheet.Cells[2, 44].Value = "Other Traders (15 mins)";
            worksheet.Cells[2, 45].Value = "Other Traders (10 mins)";
            worksheet.Cells[2, 46].Value = "Other Traders (FM)";
            worksheet.Cells[2, 47].Value = "Other Traders (AIO)";
            worksheet.Cells[2, 48].Value = "Max Bet (O/U)";
            worksheet.Cells[2, 49].Value = "Max Bet (1H HDP)";
            worksheet.Cells[2, 50].Value = "Max Bet (1H O/U)";
            worksheet.Cells[2, 51].Value = "Spread (1H HDP)";
            worksheet.Cells[2, 52].Value = "Spread (1H O/U)";
            worksheet.Cells[2, 53].Value = "Max Bet (1X2)";
            worksheet.Cells[2, 54].Value = "Spread (1X2)";
            worksheet.Cells[2, 55].Value = "Max Bet (1H 1X2)";
            worksheet.Cells[2, 56].Value = "Spread (1H 1X2)";
            worksheet.Cells[2, 57].Value = "Trader (1H 1X2)";
            worksheet.Cells[2, 58].Value = "Other Traders (1H 1X2)";
            worksheet.Cells[2, 59].Value = "Max Bet (x mins)";
            worksheet.Cells[2, 60].Value = "Spread (x mins)";
            worksheet.Cells[2, 61].Value = "Max Bet (FM)";
            worksheet.Cells[2, 62].Value = "Spread (FM)";

            worksheet.Cells[3, 1].Value = "Live";
            worksheet.Cells[3, 4].Value = "01:00 PM";
            worksheet.Cells[3, 6].Value = "RB2";
            worksheet.Cells[3, 8].Value = "homemar27";
            worksheet.Cells[3, 9].Value = "awaymar27";
            worksheet.Cells[3, 10].Value = "leaguemar27";
            worksheet.Cells[3, 11].Value = "170K";
            worksheet.Cells[3, 15].Value = "starix1";
            worksheet.Cells[3, 29].Value = "starix14";

            var data = excelPackage.GetAsByteArray();
            request.Stream = new MemoryStream(data);
        }

        return request;
    }

    private static void DeleteDirectory(string directory)
    {
        new DirectoryInfo(directory).Delete(true);
    }

    private static string CreateTempDirectory()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(directory + @"\App_Data\Files");

        return directory;
    }
}
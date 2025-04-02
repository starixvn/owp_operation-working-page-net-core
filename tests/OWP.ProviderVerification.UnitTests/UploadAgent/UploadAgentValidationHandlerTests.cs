using NSubstitute;
using Xunit;
using OWP.ProviderVerification.ProviderVerification.UploadAgent;
using OfficeOpenXml;
using Fanex.Caching;
using Fanex.Data.Repository;
using Olympus.OWP.Log.Services;
using OWP.Models;
using Xunit.Asserts.Compare;
using Error = OWP.Models.Error;

namespace OWP.ProviderVerification.UnitTests.UploadAgent;
public class UploadAgentValidationHandlerTests
{
    private readonly UploadAgentValidationHandler _handler;

    public UploadAgentValidationHandlerTests()
    {
        var cache = Substitute.For<ICache>();
        var dynamicRepository = Substitute.For<IDynamicRepository>();
        var logWriterService= Substitute.For<LogWriterService>();
        _handler = new UploadAgentValidationHandler(cache, dynamicRepository);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    [Fact]
    public async Task Handle_ValidExcelFile_ReturnsSuccessResult()
    {
        // Arrange
        var stream = new MemoryStream();
        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");
            worksheet.Cells[1, 1].Value = "Provider";
            worksheet.Cells[1, 2].Value = "ProviderMatchID";
            worksheet.Cells[2, 1].Value = "RB2";
            worksheet.Cells[2, 2].Value = 12345;
            await package.SaveAsync();
        }
        stream.Position = 0;

        var request = new UploadAgentValidationRequest(stream, "sessionId");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("sessionId", result.Value.SessionId);
        Assert.Equal(1, result.Value.OriginalRecordCount);
    }

    [Fact]
    public async Task Handle_InvalidExcelFile_ReturnsFailureResult()
    {
        // Arrange
        var stream = new MemoryStream();
        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");
            worksheet.Cells[1, 1].Value = "Provider";
            worksheet.Cells[1, 2].Value = "ProviderMatchID";
            worksheet.Cells[2, 1].Value = "";
            worksheet.Cells[2, 2].Value = 12345;
            await package.SaveAsync();
        }
        stream.Position = 0;

        var request = new UploadAgentValidationRequest(stream, "sessionId");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        var errors = new List<Error> { Error.Validation(ErrorCode.PROVIDER_IS_REQUIRED, "Provider is required", 2) };

        DeepAssert.Equal(errors, result.Errors);
    }
}

namespace OWP.OtherSport.UnitTests.Validation
{
    using Fanex.NAP.Client.Helpers.Web;
    using NSubstitute;
    using OfficeOpenXml;
    using OWP.OtherSport.Validation;
    using OWP.OtherSport.Validation.Exceptions;
    using OWP.OtherSport.Validation.Services;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class ValidationHandlerUnitTests
    {
        [Fact]
        public async Task Handle_ExcelHeaderIsInvalid_ReturnsExceptionMessage()
        {
            // Arrange            
            var request = new ValidationHandler.Request { Stream = MockFileStream() };
            var handler = MockHandler(request);

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            const string expect = "Invalid header, should be Match Status";
            Assert.Equal(expect, actual.Exception);
        }

        private static Stream MockFileStream()
        {
            var stream = new MemoryStream();
            using var excelPackage = new ExcelPackage(stream);
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

            worksheet.Cells[1, 1].Value = "Match Status";
            worksheet.Cells[1, 2].Value = "Sport";

            worksheet.Cells[2, 1].Value = "Live";
            worksheet.Cells[2, 2].Value = "Soccer";

            var data = excelPackage.GetAsByteArray();

            return new MemoryStream(data);
        }

        private static ValidationHandler.Handler MockHandler(ValidationHandler.Request request)
        {
            var excelValidator = Substitute.For<ExcelValidator>();
            var cacheManipulator = Substitute.For<CacheManipulator>();
            var analyzer = Substitute.For<Analyzer>();
            var session = Substitute.For<INapSession>();
            excelValidator.When(x => x.Validate(request.Stream)).Do(call => { throw new InvalidExcelFormatException("Match Status"); });

            return new ValidationHandler.Handler(excelValidator, analyzer, session, cacheManipulator);
        }
    }
}
namespace OWP.OtherSport.UnitTests.Validation
{
    using Fanex.NAP.Client.Helpers.Web;
    using NSubstitute;
    using OfficeOpenXml;
    using OWP.OtherSport.Validation;
    using OWP.OtherSport.Validation.Exceptions;
    using OWP.OtherSport.Validation.Models;
    using OWP.OtherSport.Validation.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class ValidationHandlerUnitTests
    {
        [Fact]
        public async Task Handle_ExcelHeaderIsInvalid_ReturnsCriticalValidationExceptionMessage()
        {
            // Arrange            
            var request = new ValidationHandler.Request { Stream = MockFileStream() };
            var exception = new InvalidExcelFormatException("Match Status");
            var handler = MockHandler(request, exception);

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            const string expect = "Invalid header, should be Match Status";
            Assert.Equal(expect, actual.Exception);
        }

        [Fact]
        public async Task Handle_InvalidAssignment_ReturnsInvalidAssignmentRowExceptionMessage()
        {
            // Arrange            
            var request = new ValidationHandler.Request { Stream = MockFileStream() };
            var validationResult = new List<AssignmentValidationRowResult>{
                new AssignmentValidationRowResult
                {
                Message = "Invalid abc",
                Row = 1
                } 
            };
            var exception = new InvalidAssignmentRowException(validationResult);
            var handler = MockHandler(request, exception);

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            const string expectExceptionMessage = "Some rows have invalid data";
            Assert.Equal(expectExceptionMessage, actual.Exception);
            DeepAssert.Equal(validationResult, actual.ValidationResults);
        }

        [Fact]
        public async Task Handle_Assignment_ReturnsValidationResult()
        {
            // Arrange            
            var request = new ValidationHandler.Request { Stream = MockFileStream() };
            var handler = MockHandlerWithNoException(request);

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            DeepAssert.Equal(1, actual.RecordCount);
            Assert.Empty(actual.ValidationResults);
            Assert.True(actual.IsSuccessful);
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

        private static ValidationHandler.Handler MockHandlerWithNoException(ValidationHandler.Request request)
        {
            var excelValidator = Substitute.For<ExcelValidator>();
            var cacheManipulator = Substitute.For<CacheManipulator>();
            var analyzer = Substitute.For<Analyzer>();
            var session = Substitute.For<INapSession>();
            analyzer.Analyze(request).Returns(new AnalyzedResult
            {
                OriginalRecordCount = 1,
                Assignments = new List<AssignmentModel>
                {
                    new AssignmentModel
                    {
                        Sport = "Basketball",
                        Home = "Golden State Warrior",
                        Away = "LA Laker",
                        MatchStatus = "Completed",
                        DateTime = new DateTime(2020, 12, 29),
                        League = "NBA",
                        AutoLink = "YES"
                    }
                }
            });
            excelValidator.When(x => x.Validate(request.Stream)).Do(call => { });

            return new ValidationHandler.Handler(excelValidator, analyzer, session, cacheManipulator);
        }

        private static ValidationHandler.Handler MockHandler(ValidationHandler.Request request, Exception exception)
        {
            var excelValidator = Substitute.For<ExcelValidator>();
            var cacheManipulator = Substitute.For<CacheManipulator>();
            var analyzer = Substitute.For<Analyzer>();
            var session = Substitute.For<INapSession>();
            excelValidator.When(x => x.Validate(request.Stream)).Do(call => { throw exception; });

            return new ValidationHandler.Handler(excelValidator, analyzer, session, cacheManipulator);
        }
    }
}
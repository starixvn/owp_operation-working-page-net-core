using NSubstitute;
using OWP.OtherSport.Download;
using OWP.OtherSport.Download.Constants;
using OWP.OtherSport.Download.Models;
using OWP.OtherSport.Download.Services;
using OWP.OtherSport.Submit.Contants;
using OWP.OtherSport.Submit.Models;
using OWP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.OtherSport.UnitTests.Download.Services
{
    public class ExportExcelServiceUnitTests
    {
        private readonly ExportExcelServiceImp services;
        private readonly ConfigurationService configurationService;
        private readonly BuildExcelModelService buildExcelModelService;

        public ExportExcelServiceUnitTests()
        {
            configurationService = Substitute.For<ConfigurationService>();
            buildExcelModelService = Substitute.For<BuildExcelModelService>();

            services = new ExportExcelServiceImp(configurationService, buildExcelModelService);
        }

        [Fact]
        public async Task CreateExcelPackage()
        {
            var request = new DownloadAssignmentRequest
            {
                From = new DateTime(2021, 03, 25),
                To = new DateTime(2021, 03, 25),
                Employee = "sa",
                Sports = "Basketball",
                MatchStatuses = "Live"
            };
            var excelModel = GetExcelModel();
            buildExcelModelService.BuildExcelModel(request).Returns(excelModel);
            configurationService.CurrentDirectory.Returns("D:\\My folder\\OWP\\operation-working-page(Netcore)\\OWP.Websites");

            var actual = await services.CreateExcelPackage(request);
            var reportSheet = actual.Workbook.Worksheets[ExcelConst.SheetName];

            DeepAssert.Equal(reportSheet.Cells[3, 19].Value, "sa");
            DeepAssert.Equal(reportSheet.Cells[3, 83].Value, "#starix15 mynote");
            DeepAssert.Equal(reportSheet.Cells[3, 145].Value, "1234");
            DeepAssert.Equal(reportSheet.Cells[3, 146].Value, "222.05");
        }

        private static ExcelModel GetExcelModel()
        {
            return new ExcelModel
            {
                HeaderInfo = new ExcelHeaderInfoModel
                {
                    FromDate = "Mar 25, 2021",
                    ToDate = "Mar 25, 2021",
                    Employee = "sa",
                    Sports = "Basketball",
                    MatchStatus = "Live"
                },
                ExcelAssignmentModels = new ExcelAssignmentModel
                {
                    Matches = new List<MatchInfo>
                    {
                        new MatchInfo { OSMatchId = 111, Sport = Sport.Basketball, Home = "home111", League = "league111" }
                    },
                    MatchAssignments = new Dictionary<int, List<ExcelMatchAssignment>>
                    {
                        { 111, new List<ExcelMatchAssignment> {
                            new ExcelMatchAssignment { ColumnIndex=19,Value="sa", AssignmentType = 1, BetTypeName = Bettype.FTHDP, UserId = 8, Username="sa" },
                            new ExcelMatchAssignment { ColumnIndex=83,Value="#starix15 mynote", AssignmentType = 2, BetTypeName = Bettype.FirstHHDP,
                                UserId = 123, Username="starix15", AssignmentNote = "#<user> mynote" }
                        } }
                    },
                    MaxBetSpread = new Dictionary<int, List<ExcelMatchMaxBetSpread>>
                    {
                        { 111, new List<ExcelMatchMaxBetSpread> {
                            new ExcelMatchMaxBetSpread { ColumnIndexMaxbet=145,ValueMaxbet="1234", BetTypeName = Bettype.FTHDP, MaxBet = 1234 },
                            new ExcelMatchMaxBetSpread { ColumnIndexMaxbet=146,ValueMaxbet="222.05", BetTypeName = Bettype.FTOUOE, MaxBet = 222.05m }
                        } }
                    }
                }
            };
        }
    }
}

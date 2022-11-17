namespace OWP.AssignmentUpload.UnitTests.Validation.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using AssignmentUpload.Validation.Extensions;
    using AssignmentUpload.Validation.Models;
    using OfficeOpenXml;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class WorksheetExtensionsUnitTests
    {
        [Fact]
        public void GetHeaderMap_ExcelWithSomeHeader_ReturnSomeHeaderIndex()
        {
            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

                worksheet.Cells[2, 2].Value = "Match Status";
                worksheet.Cells[2, 41].Value = "InfoSite ID";
                worksheet.Cells[2, 1].Value = "No";
                worksheet.Cells[2, 3].Value = "Time";

                var expected = new Dictionary<string, int>
                {
                    {"Agent", 0},
                    {"Channel", 0},
                    {"IPTV", 0},
                    {"Division", 0},
                    {"Home", 0},
                    {"Away", 0},
                    {"League", 0},
                    {"InfoSite ID", 4},
                    {"AdminSite ID", 0},
                    {"No", 1},
                    {"Match Offer", 0},
                    {"Match Status", 2},
                    {"No.HDP", 0},
                    {"Time", 3},
                    {"Other Time", 0},
                    {"Reporter Phone", 0},
                    {"Provider", 0},
                    {"Other Provider", 0},
                    {"Remark", 0},
                    {"Round", 0},
                    {"TV", 0},
                    {"Streaming Info(WB)", 0},
                    {"Web 1", 0},
                    {"Web 2", 0},
                    {"Web 3", 0},
                    {"Web 4", 0},
                    {"10mins", 0},
                    {"15mins", 0},
                    {"Trader (1H 1X2)", 0},
                    {"Trader (1H HDP)", 0},
                    {"Trader (1H O/U)", 0},
                    {"Trader (1X2)", 0},
                    {"AIO", 0},
                    {"FM", 0},
                    {"Trader (HDP)", 0},
                    {"Trader (O/U)", 0},
                    {"LA", 0},
                    {"Other Traders (10 mins)", 0},
                    {"Other Traders (15 mins)", 0},
                    {"Other Traders (1H 1X2)", 0},
                    {"Other Traders (1H HDP)", 0},
                    {"Other Traders (1H O/U)", 0},
                    {"Other Traders (1X2)", 0},
                    {"Other Traders (AIO)", 0},
                    {"Other Traders (FM)", 0},
                    {"Other Traders (HDP)", 0},
                    {"Other LA", 0},
                    {"Other Traders (O/U)", 0},
                    {"Max Bet (1H 1X2)", 0},
                    {"Max Bet (1H HDP)", 0},
                    {"Max Bet (1H O/U)", 0},
                    {"Max Bet (1X2)", 0},
                    {"Max Bet (FM)", 0},
                    {"Max Bet (HDP)", 0},
                    {"Max Bet (O/U)", 0},
                    {"Max Bet (x mins)", 0},
                    {"Spread (1H 1X2)", 0},
                    {"Spread (1H HDP)", 0},
                    {"Spread (1H O/U)", 0},
                    {"Spread (1X2)", 0},
                    {"Spread (FM)", 0},
                    {"Spread (HDP)", 0},
                    {"Spread (O/U)", 0},
                    {"Spread (x mins)", 0},
                };

                var actual = worksheet.GetHeaderMap();
                DeepAssert.Equal(expected, actual);
            }
        }

        [Fact]
        public void GetHeaderMap_ExcelWithFullHeader_ReturnFullHeaderIndex()
        {
            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

                worksheet.Cells["A1"].Value = "Date";
                worksheet.Cells[1, 2].Value = "";

                worksheet.Cells["D1"].Value = "Employee";
                worksheet.Cells[1, 5].Value = "";

                worksheet.Cells["G1"].Value = "MatchStatus";
                worksheet.Cells[1, 8].Value = "";

                worksheet.Cells["J1"].Value = "LastUpdated";
                worksheet.Cells[1, 11].Value = "";

                worksheet.Cells[2, 1].Value = "No";
                worksheet.Cells[2, 2].Value = "Match Status";
                worksheet.Cells[2, 3].Value = "Time";
                worksheet.Cells[2, 4].Value = "Other Time";
                worksheet.Cells[2, 5].Value = "Home";
                worksheet.Cells[2, 6].Value = "Away";
                worksheet.Cells[2, 7].Value = "League";
                worksheet.Cells[2, 8].Value = "Match Offer";
                worksheet.Cells[2, 9].Value = "Max Bet (HDP)";
                worksheet.Cells[2, 10].Value = "Spread (HDP)";
                worksheet.Cells[2, 11].Value = "Spread (O/U)";
                worksheet.Cells[2, 12].Value = "No.HDP";
                worksheet.Cells[2, 13].Value = "Trader (HDP)";
                worksheet.Cells[2, 14].Value = "Trader (O/U)";
                worksheet.Cells[2, 15].Value = "Trader (1H HDP)";
                worksheet.Cells[2, 16].Value = "Trader (1H O/U)";
                worksheet.Cells[2, 17].Value = "Other Traders (HDP)";
                worksheet.Cells[2, 18].Value = "Other Traders (O/U)";
                worksheet.Cells[2, 19].Value = "Other Traders (1H HDP)";
                worksheet.Cells[2, 20].Value = "Other Traders (1H O/U)";
                worksheet.Cells[2, 21].Value = "15mins";
                worksheet.Cells[2, 22].Value = "10mins";
                worksheet.Cells[2, 23].Value = "Trader (1X2)";
                worksheet.Cells[2, 24].Value = "Other Traders (1X2)";
                worksheet.Cells[2, 25].Value = "FM";
                worksheet.Cells[2, 26].Value = "AIO";
                worksheet.Cells[2, 27].Value = "LA";
                worksheet.Cells[2, 28].Value = "Other LA";
                worksheet.Cells[2, 29].Value = "Provider";
                worksheet.Cells[2, 30].Value = "Other Provider";
                worksheet.Cells[2, 31].Value = "Division";
                worksheet.Cells[2, 32].Value = "Remark";
                worksheet.Cells[2, 33].Value = "Streaming Info(WB)";
                worksheet.Cells[2, 34].Value = "Round";
                worksheet.Cells[2, 35].Value = "TV";
                worksheet.Cells[2, 36].Value = "Channel";
                worksheet.Cells[2, 37].Value = "IPTV";
                worksheet.Cells[2, 38].Value = "Agent";
                worksheet.Cells[2, 39].Value = "Reporter Phone";
                worksheet.Cells[2, 40].Value = "InfoSite ID";
                worksheet.Cells[2, 41].Value = "AdminSite ID";
                worksheet.Cells[2, 42].Value = "Web 1";
                worksheet.Cells[2, 43].Value = "Web 2";
                worksheet.Cells[2, 44].Value = "Web 3";
                worksheet.Cells[2, 45].Value = "Web 4";
                worksheet.Cells[2, 46].Value = "Other Traders (15 mins)";
                worksheet.Cells[2, 47].Value = "Other Traders (10 mins)";
                worksheet.Cells[2, 48].Value = "Other Traders (FM)";
                worksheet.Cells[2, 49].Value = "Other Traders (AIO)";
                worksheet.Cells[2, 50].Value = "Max Bet (O/U)";
                worksheet.Cells[2, 51].Value = "Max Bet (1H HDP)";
                worksheet.Cells[2, 52].Value = "Max Bet (1H O/U)";
                worksheet.Cells[2, 53].Value = "Spread (1H HDP)";
                worksheet.Cells[2, 54].Value = "Spread (1H O/U)";
                worksheet.Cells[2, 55].Value = "Max Bet (1X2)";
                worksheet.Cells[2, 56].Value = "Spread (1X2)";
                worksheet.Cells[2, 57].Value = "Max Bet (1H 1X2)";
                worksheet.Cells[2, 58].Value = "Spread (1H 1X2)";
                worksheet.Cells[2, 59].Value = "Trader (1H 1X2)";
                worksheet.Cells[2, 60].Value = "Other Traders (1H 1X2)";
                worksheet.Cells[2, 61].Value = "Max Bet (x mins)";
                worksheet.Cells[2, 62].Value = "Spread (x mins)";
                worksheet.Cells[2, 63].Value = "Max Bet (FM)";
                worksheet.Cells[2, 64].Value = "Spread (FM)";
                worksheet.Cells[2, 65].Value = "Note";

                var expected = new Dictionary<string, int>
                {
                    {"No", 1},
                    {"Match Status", 2},
                    {"Time", 3},
                    {"Other Time", 4},
                    {"Home", 5},
                    {"Away", 6},
                    {"League", 7},
                    {"Match Offer", 8},
                    {"Max Bet (HDP)", 9},
                    { "Spread (HDP)", 10},
                    {"Spread (O/U)", 11},
                    { "No.HDP", 12},
                    { "Trader (HDP)", 13},
                    { "Trader (O/U)", 14},
                    {"Trader (1H HDP)", 15},
                    { "Trader (1H O/U)", 16},
                    { "Other Traders (HDP)", 17},
                    {"Other Traders (O/U)", 18},
                    { "Other Traders (1H HDP)", 19},
                    { "Other Traders (1H O/U)", 20},
                    {"15mins", 21},
                    { "10mins", 22},
                    { "Trader (1X2)", 23},
                    { "Other Traders (1X2)", 24},
                    { "FM", 25},
                    {"AIO", 26},
                    { "LA", 27},
                    { "Other LA", 28},
                    { "Provider", 29},
                    { "Other Provider", 30},
                    {"Division", 31},
                    { "Remark", 32},
                    { "Streaming Info(WB)", 33},
                    { "Round", 34},
                    { "TV", 35},
                    {"Channel", 36},
                    { "IPTV", 37},
                    { "Agent", 38},
                    { "Reporter Phone", 39},
                    { "InfoSite ID", 40},
                    {"AdminSite ID", 41},
                    { "Web 1", 42},
                    { "Web 2", 43},
                    { "Web 3", 44},
                    { "Web 4", 45},
                    {"Other Traders (15 mins)", 46},
                    { "Other Traders (10 mins)", 47},
                    { "Other Traders (FM)", 48},
                    {"Other Traders (AIO)", 49},
                    { "Max Bet (O/U)", 50},
                    { "Max Bet (1H HDP)", 51},
                    {"Max Bet (1H O/U)", 52},
                    { "Spread (1H HDP)", 53},
                    { "Spread (1H O/U)", 54},
                    { "Max Bet (1X2)", 55},
                    {"Spread (1X2)", 56},
                    { "Max Bet (1H 1X2)", 57},
                    { "Spread (1H 1X2)", 58},
                    { "Trader (1H 1X2)", 59},
                    {"Other Traders (1H 1X2)", 60},
                    { "Max Bet (x mins)", 61},
                    { "Spread (x mins)", 62},
                    {"Max Bet (FM)", 63},
                    { "Spread (FM)", 64},
                };

                var actual = worksheet.GetHeaderMap();
                DeepAssert.Equal(expected, actual);
            }
        }

        [Fact]
        public void ReadAssignment_ValidExcelPackage_ReturnAssignmentModelInExcel()
        {
            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

                worksheet.Cells["A1"].Value = "Date";
                worksheet.Cells[1, 2].Value = "";

                worksheet.Cells["D1"].Value = "Employee";
                worksheet.Cells[1, 5].Value = "";

                worksheet.Cells["G1"].Value = "MatchStatus";
                worksheet.Cells[1, 8].Value = "";

                worksheet.Cells["J1"].Value = "LastUpdated";
                worksheet.Cells[1, 11].Value = "";

                worksheet.Cells[2, 1].Value = "No";
                worksheet.Cells[2, 2].Value = "Match Status";
                worksheet.Cells[2, 3].Value = "Time";
                worksheet.Cells[2, 4].Value = "Other Time";
                worksheet.Cells[2, 5].Value = "Home";
                worksheet.Cells[2, 6].Value = "Away";
                worksheet.Cells[2, 7].Value = "League";
                worksheet.Cells[2, 8].Value = "Match Offer";
                worksheet.Cells[2, 9].Value = "LST Note";
                worksheet.Cells[2, 10].Value = "Max Bet (HDP)";
                worksheet.Cells[2, 11].Value = "Spread (HDP)";
                worksheet.Cells[2, 12].Value = "Spread (O/U)";
                worksheet.Cells[2, 13].Value = "No.HDP";
                worksheet.Cells[2, 14].Value = "Trader (HDP)";
                worksheet.Cells[2, 15].Value = "Trader (O/U)";
                worksheet.Cells[2, 16].Value = "Trader (1H HDP)";
                worksheet.Cells[2, 17].Value = "Trader (1H O/U)";
                worksheet.Cells[2, 18].Value = "Other Traders (HDP)";
                worksheet.Cells[2, 19].Value = "Other Traders (O/U)";
                worksheet.Cells[2, 20].Value = "Other Traders (1H HDP)";
                worksheet.Cells[2, 21].Value = "Other Traders (1H O/U)";
                worksheet.Cells[2, 22].Value = "15mins";
                worksheet.Cells[2, 23].Value = "10mins";
                worksheet.Cells[2, 24].Value = "Trader (1X2)";
                worksheet.Cells[2, 25].Value = "Other Traders (1X2)";
                worksheet.Cells[2, 26].Value = "FM";
                worksheet.Cells[2, 27].Value = "AIO";
                worksheet.Cells[2, 28].Value = "LA";
                worksheet.Cells[2, 29].Value = "Other LA";
                worksheet.Cells[2, 30].Value = "Provider";
                worksheet.Cells[2, 31].Value = "Other Provider";
                worksheet.Cells[2, 32].Value = "Division";
                worksheet.Cells[2, 33].Value = "Remark";
                worksheet.Cells[2, 34].Value = "Streaming Info(WB)";
                worksheet.Cells[2, 35].Value = "Round";
                worksheet.Cells[2, 36].Value = "TV";
                worksheet.Cells[2, 37].Value = "Channel";
                worksheet.Cells[2, 38].Value = "IPTV";
                worksheet.Cells[2, 39].Value = "Agent";
                worksheet.Cells[2, 40].Value = "Reporter Phone";
                worksheet.Cells[2, 41].Value = "InfoSite ID";
                worksheet.Cells[2, 42].Value = "AdminSite ID";
                worksheet.Cells[2, 43].Value = "Web 1";
                worksheet.Cells[2, 44].Value = "Web 2";
                worksheet.Cells[2, 45].Value = "Web 3";
                worksheet.Cells[2, 46].Value = "Web 4";
                worksheet.Cells[2, 47].Value = "Other Traders (15 mins)";
                worksheet.Cells[2, 48].Value = "Other Traders (10 mins)";
                worksheet.Cells[2, 49].Value = "Other Traders (FM)";
                worksheet.Cells[2, 50].Value = "Other Traders (AIO)";
                worksheet.Cells[2, 51].Value = "Max Bet (O/U)";
                worksheet.Cells[2, 52].Value = "Max Bet (1H HDP)";
                worksheet.Cells[2, 53].Value = "Max Bet (1H O/U)";
                worksheet.Cells[2, 54].Value = "Spread (1H HDP)";
                worksheet.Cells[2, 55].Value = "Spread (1H O/U)";
                worksheet.Cells[2, 56].Value = "Max Bet (1X2)";
                worksheet.Cells[2, 57].Value = "Spread (1X2)";
                worksheet.Cells[2, 58].Value = "Max Bet (1H 1X2)";
                worksheet.Cells[2, 59].Value = "Spread (1H 1X2)";
                worksheet.Cells[2, 60].Value = "Trader (1H 1X2)";
                worksheet.Cells[2, 61].Value = "Other Traders (1H 1X2)";
                worksheet.Cells[2, 62].Value = "Max Bet (x mins)";
                worksheet.Cells[2, 63].Value = "Spread (x mins)";
                worksheet.Cells[2, 64].Value = "Max Bet (FM)";
                worksheet.Cells[2, 65].Value = "Spread (FM)";
                worksheet.Cells[2, 64].Value = "Note";

                worksheet.Cells[3, 2].Value = "Live";
                worksheet.Cells[3, 3].Value = "01:00 PM";
                worksheet.Cells[3, 30].Value = "RB2";
                worksheet.Cells[3, 5].Value = "homemar27";
                worksheet.Cells[3, 6].Value = "awaymar27";
                worksheet.Cells[3, 7].Value = "leaguemar27";
                worksheet.Cells[3, 58].Value = "0.01K";
                worksheet.Cells[3, 14].Value = "starix1";
                worksheet.Cells[3, 28].Value = "starix14";

                var expected = new AssignmentModelInExcel
                {
                    Time = "01:00 PM",
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27",
                    MatchStatus = "Live",
                    MaxBet1H1X2 = 10,
                    Provider = "RB2",
                    TraderHDPName = "starix1",
                    TraderLAName = "starix14",
                    Agent = "",
                    Channel = "",
                    IPTV = "",
                    Division = "",
                    InfoSiteMatchId = 0,
                    AdminSiteId = 0,
                    MatchNo = "",
                    MatchOffer = "",
                    MaxBetHDP = null,
                    MaxBet1HHDP = null,
                    MaxBet1HOU = null,
                    MaxBet1X2 = null,
                    MaxBetFM = null,
                    MaxBetOU = null,
                    MaxBetXMins = null,
                    NoHDP = "",
                    Notes = new List<string> { string.Empty },
                    OtherOfTime = "",
                    OtherProvider = "",
                    OtherTraders10Mins = "",
                    OtherTraders15Mins = "",
                    OtherTraders1H1X2 = "",
                    OtherTraders1HHDP = "",
                    OtherTraders1HOU = "",
                    OtherTraders1X2 = "",
                    OtherTradersAIO = "",
                    OtherTradersFM = "",
                    OtherTradersHDP = "",
                    OtherTradersLA = "",
                    OtherTradersOU = "",
                    Phone = "",
                    Remark = "",
                    Round = "",
                    Spread1H1X2 = null,
                    Spread1HHDP = null,
                    Spread1HOU = null,
                    Spread1X2 = null,
                    SpreadFM = null,
                    SpreadHDP = null,
                    SpreadOU = null,
                    SpreadXMins = null,
                    TV = "",
                    Trader10MinsName = "",
                    Trader15MinsName = "",
                    Trader1H1X2Name = "",
                    Trader1HHDPName = "",
                    Trader1HOUName = "",
                    Trader1X2Name = "",
                    TraderAIOName = "",
                    TraderFMName = "",
                    TraderOUName = "",
                    WB = "",
                    Website1 = "",
                    Website2 = "",
                    Website3 = "",
                    Website4 = ""
                };

                var actual = worksheet.ReadAllAssignments(3).ToList();
                DeepAssert.Equal(expected, actual[0]);
            }
        }

        [Theory]
        [InlineData("0.01k", 10)]
        [InlineData("0.01K", 10)]
        [InlineData("170k", 170000)]
        [InlineData("170K", 170000)]
        [InlineData("8", 8)]
        [InlineData("0k", 0)]
        [InlineData("0.5k", 500)]
        public void GetMaxbetValue(string maxbetValue, decimal expect)
        {
            var actual = WorksheetExtensions.GetMaxbetValue(maxbetValue);
            Assert.Equal(expect, actual);
        }

        [Fact]
        public void ReadAssignment_MissingHeader_ReturnEmpty()
        {
            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

                worksheet.Cells[2, 1].Value = "Match Status";
                worksheet.Cells[2, 2].Value = "InfoSite ID";
                worksheet.Cells[2, 3].Value = "No";
                worksheet.Cells[2, 4].Value = "Time";
                worksheet.Cells[3, 4].Value = null;

                var actual = worksheet.ReadAllAssignments(3).ToList();
                Assert.Empty(actual);
            }
        }
    }
}
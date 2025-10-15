using OfficeOpenXml;
using OWP.AssignmentUpload.Validation.Exceptions;
using OWP.AssignmentUpload.Validation.Extensions;
using OWP.AssignmentUpload.Validation.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace OWP.AssignmentUpload.UnitTests.Validation.Extensions;

using Xunit.Asserts.Compare;

public class WorksheetExtensionsUnitTests
{
    [Fact]
    public void GetHeaderMap_ExcelWithSomeHeader_ReturnSomeHeaderIndex()
    {
        using var excelPackage = new ExcelPackage();
        var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

        // Set up some specific headers
        worksheet.Cells[2, 2].Value = "Match Status";
        worksheet.Cells[2, 41].Value = "InfoSite ID";
        worksheet.Cells[2, 1].Value = "No";
        worksheet.Cells[2, 3].Value = "Time";

        // Generate expected dictionary dynamically based on the actual model
        var type = typeof(AssignmentModelInExcel);
        var properties = type.GetProperties();
        var propertiesWithoutNotes = properties.Where(x => x.Name != "Notes").ToList();

        var expected = new Dictionary<string, int>();
        for (int i = 0; i < propertiesWithoutNotes.Count; i++)
        {
            var property = propertiesWithoutNotes[i];
            var displayName = GetDisplayName(property);
            expected[displayName] = 0; // Default to 0 for missing headers
        }

        // Update the specific headers that are present
        expected["No"] = 1;
        expected["Match Status"] = 2;
        expected["Time"] = 3;
        expected["InfoSite ID"] = 4; // InfoSite ID is at column 4 because it's the 4th non-empty header

        var actual = worksheet.GetHeaderMap();
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void GetHeaderMap_ExcelWithFullHeader_ReturnFullHeaderIndex()
    {
        using var excelPackage = new ExcelPackage();
        var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

        // Generate headers dynamically based on the actual model
        var type = typeof(AssignmentModelInExcel);
        var properties = type.GetProperties();
        var propertiesWithoutNotes = properties.Where(x => x.Name != "Notes").ToList();

        // Set up headers in row 2
        for (int i = 0; i < propertiesWithoutNotes.Count; i++)
        {
            var property = propertiesWithoutNotes[i];
            var displayName = GetDisplayName(property);
            worksheet.Cells[2, i + 1].Value = displayName;
        }

        // Generate expected dictionary dynamically
        var expected = new Dictionary<string, int>();
        for (int i = 0; i < propertiesWithoutNotes.Count; i++)
        {
            var property = propertiesWithoutNotes[i];
            var displayName = GetDisplayName(property);
            expected[displayName] = i + 1;
        }

        var actual = worksheet.GetHeaderMap();
        DeepAssert.Equal(expected, actual);
    }

    private static string GetDisplayName(PropertyInfo property)
    {
        var attribute = property.GetCustomAttributes(typeof(DisplayAttribute), true)
            .Cast<DisplayAttribute>()
            .FirstOrDefault();
        return attribute == null
            ? string.Empty
            : attribute.Name;
    }

    [Fact]
    public void ReadAssignment_ValidExcelPackage_ReturnAssignmentModelInExcel()
    {
        using var excelPackage = new ExcelPackage();
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

    [Theory]
    [InlineData("0.01k", 10)]
    [InlineData("0.01K", 10)]
    [InlineData("170k", 170000)]
    [InlineData("170K", 170000)]
    [InlineData("0k", 0.0)]
    [InlineData("0.5k", 500.0)]
    public void GetMaxbetValue(string maxbetValue, decimal expect)
    {
        var actual = WorksheetExtensions.GetMaxbetValue(maxbetValue, 2, "Max Bet(HDP)");
        Assert.Equal(expect, actual);
    }

    [Fact]
    public void GetMaxbetValueWithNull()
    {
        var actual = WorksheetExtensions.GetMaxbetValue("", 2, "Max Bet(HDP)");
        Assert.Null(actual);
    }

    [Fact]
    public void GetMaxBetValueShouldThrowError()
    {
        Assert.Throws<InvalidAssignmentRowException>(() => WorksheetExtensions.ThrowValidateMaxBetError(2, "Max Bet(HDP)"));
    }

    [Fact]
    public void ReadAssignment_MissingHeader_ReturnEmpty()
    {
        using var excelPackage = new ExcelPackage();
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
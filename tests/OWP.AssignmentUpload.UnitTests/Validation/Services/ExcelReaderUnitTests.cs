using OfficeOpenXml;
using OWP.AssignmentUpload.Validation.Models;
using OWP.AssignmentUpload.Validation.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssignmentUpload.UnitTests.Validation.Services;

public class ExcelReaderImpUnitTests
{
    private readonly ExcelReaderImp excelReader;
    private Stream stream;

    public ExcelReaderImpUnitTests()
    {
        excelReader = new ExcelReaderImp();
        stream = new MemoryStream();
    }

    [Fact]
    public void ExtractOtherTraders_ReturnValueCorrect()
    {
        var actual = ExcelReaderImp.ExtractOtherTraders("#sa  #     sa");

        var expected = new List<string>
        {
            "sa"
        };

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void ExtractOtherTraders_EmptyText_ReturnEmpty()
    {
        var actual = ExcelReaderImp.ExtractOtherTraders("");
        Assert.Empty(actual);
    }

    [Fact]
    public void ExtractOtherTraders_CannotCast_ReturnEmpty()
    {
        var actual = ExcelReaderImp.ExtractOtherTraders("sa");
        Assert.Empty(actual);
    }

    [Fact]
    public void ExtractOtherTraders_TraderLengthIs51_ReturnSubstringEqualMaxLength50()
    {
        var actual = ExcelReaderImp.ExtractOtherTraders("#saaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaakk");
        var expected = new List<string>
        {
            "saaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaak"
        };
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void GetMainTraders_EmptyAssignments_ReturnEmpty()
    {
        var assignments = new List<AssignmentModelInExcel>();
        var actual = ExcelReaderImp.GetMainTraders(assignments);
        Assert.Empty(actual);
    }

    [Fact]
    public void GetOtherTraders_EmptyAssignments_ReturnEmpty()
    {
        var assignments = new List<AssignmentModelInExcel>();
        var actual = ExcelReaderImp.GetOtherTraders(assignments);
        Assert.Empty(actual);
    }

    [Fact]
    public void GetOtherTraders_ManyAssignments_ReturnEmpty()
    {
        var assignments = new List<AssignmentModelInExcel>
        {
            new()
            {
                OtherTraders10Mins = "#starix1",
                OtherTraders15Mins = "",
                OtherTraders1H1X2 = "",
                OtherTraders1HHDP = "#sa",
                OtherTraders1HOU = "#starix1",
                OtherTraders1X2 = "",
                OtherTradersAIO = "",
                OtherTradersFM = "",
                OtherTradersHDP = "",
                OtherTradersOU = "",
                OtherTradersLA = ""
            }
        };
        var expected = new List<string>
        {
            "starix1",
            "sa"
        };
        var actual = ExcelReaderImp.GetOtherTraders(assignments);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void GetTraders_EmptyMainTrader_ReturnEmpty()
    {
        var assignments = new List<AssignmentModelInExcel>
        {
            new()
            {
                OtherTraders10Mins = "#starix1",
                OtherTraders15Mins = "",
                OtherTraders1H1X2 = "",
                OtherTraders1HHDP = "#sa",
                OtherTraders1HOU = "#starix1",
                OtherTraders1X2 = "",
                OtherTradersAIO = "",
                OtherTradersFM = "",
                OtherTradersHDP = "",
                OtherTradersOU = "",
                OtherTradersLA = "",
                Trader10MinsName = "",
                Trader15MinsName = "",
                Trader1H1X2Name = "",
                Trader1HHDPName = "",
                Trader1HOUName = "",
                Trader1X2Name = "",
                TraderAIOName = "",
                TraderFMName = "",
                TraderHDPName = "",
                TraderOUName = "",
                TraderLAName = ""
            }
        };
        var expected = new List<string>
        {
            "starix1",
            "sa"
        };
        var actual = ExcelReaderImp.GetTraders(assignments);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void Read_ValidStream_ReturnUploadingDataFromExcel()
    {
        using (var excelPackage = new ExcelPackage(stream))
        {
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

            SetHeader(worksheet);

            worksheet.Cells[3, 2].Value = "Live";
            worksheet.Cells[3, 3].Value = "01:00 PM";
            worksheet.Cells[3, 30].Value = "RB2";
            worksheet.Cells[3, 5].Value = "homemar27";
            worksheet.Cells[3, 6].Value = "awaymar27";
            worksheet.Cells[3, 7].Value = "leaguemar27";
            worksheet.Cells[3, 10].Value = "170K";
            worksheet.Cells[3, 14].Value = "starix1";
            worksheet.Cells[3, 28].Value = "starix14";
            worksheet.Cells[3, 18].Value = "#starix14";

            var data = excelPackage.GetAsByteArray();

            stream = new MemoryStream(data);
        }
        var expected = new UploadingDataFromExcel
        {
            EventDate = new DateTime(2020, 3, 30),
            Assignments = new List<AssignmentModelInExcel>
            {
                new()
                {
                    Time = "01:00 PM",
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27",
                    MatchStatus = "Live",
                    MaxBetHDP = 170000,
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
                    MaxBet1H1X2 = null,
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
                    OtherTradersHDP = "#starix14",
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
                }
            },
            Traders = new List<string> { "starix1", "starix14" }
        };

        var actual = excelReader.Read(stream);
        DeepAssert.Equal(expected, actual);
    }

    private static void SetHeader(ExcelWorksheet worksheet)
    {
        worksheet.Cells["A1"].Value = "Date";
        worksheet.Cells[1, 2].Value = new DateTime(2020, 3, 30);

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
        worksheet.Cells[2, 66].Value = "Note";
    }
}
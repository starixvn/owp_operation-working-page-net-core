using OfficeOpenXml;
using OWP.AssignmentUpload.Validation.Exceptions;
using OWP.AssignmentUpload.Validation.Services;
using Xunit;

namespace OWP.AssignmentUpload.UnitTests.Validation.Services;

public class ExcelValidatorImpUnitTests
{
    private readonly ExcelValidatorImp excelValidator;
    private Stream stream;

    public ExcelValidatorImpUnitTests()
    {
        excelValidator = new ExcelValidatorImp();
        stream = new MemoryStream();
    }

    [Fact]
    public void Validate_MissingHeader_ThrowsInvalidExcelFormatException()
    {
        using (var excelPackage = new ExcelPackage(stream))
        {
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

            worksheet.Cells["A1"].Value = "Date";
            worksheet.Cells[1, 2].Value = new DateTime(2020, 3, 30);

            worksheet.Cells[2, 1].Value = "Match Status";
            worksheet.Cells[2, 2].Value = "InfoSite ID";
            worksheet.Cells[2, 3].Value = "No";
            worksheet.Cells[2, 4].Value = "Time";

            stream = new MemoryStream(excelPackage.GetAsByteArray());
        }

        Assert.Throws<InvalidExcelFormatException>(() => excelValidator.Validate(stream));
    }

    [Fact]
    public void Validate_MissingNoteHeader_ThrowsInvalidExcelFormatException()
    {
        using (var excelPackage = new ExcelPackage(stream))
        {
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

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

            stream = new MemoryStream(excelPackage.GetAsByteArray());
        }
        Assert.Throws<InvalidExcelFormatException>(() => excelValidator.Validate(stream));
    }

    [Fact]
    public void Validate_ValidHeader_NotReturn()
    {
        using (var excelPackage = new ExcelPackage(stream))
        {
            var worksheet = GetValidHeaderSheet(excelPackage);

            worksheet.Cells[3, 2].Value = "Live";
            worksheet.Cells[3, 3].Value = "01:00 PM";
            worksheet.Cells[3, 30].Value = "RB2";
            worksheet.Cells[3, 5].Value = "homemar27";
            worksheet.Cells[3, 6].Value = "awaymar27";
            worksheet.Cells[3, 7].Value = "leaguemar27";
            worksheet.Cells[3, 10].Value = "170K";
            worksheet.Cells[3, 14].Value = "starix1";
            worksheet.Cells[3, 28].Value = "starix14";

            worksheet.Cells[4, 2].Value = 5;
            worksheet.Cells[4, 3].Value = 0;
            worksheet.Cells[4, 30].Value = "RB2";
            worksheet.Cells[4, 5].Value = "homemar27";
            worksheet.Cells[4, 6].Value = "awaymar27";
            worksheet.Cells[4, 7].Value = "leaguemar27";
            worksheet.Cells[4, 10].Value = "170K";
            worksheet.Cells[4, 14].Value = 5;
            worksheet.Cells[4, 28].Value = 0;

            var data = excelPackage.GetAsByteArray();
            stream = new MemoryStream(data);
        }

        var exception = Record.Exception(() => excelValidator.Validate(stream));
        Assert.Null(exception);
    }

    [Fact]
    public void Validate_InvalidValue_ThrowInvalidExcelCellFormatException()
    {
        using (var excelPackage = new ExcelPackage(stream))
        {
            var worksheet = GetValidHeaderSheet(excelPackage);

            worksheet.Cells[4, 2].Value = 5;
            worksheet.Cells[4, 3].Value = 0;
            worksheet.Cells[4, 41].Formula = "B4/C4";
            worksheet.Cells[4, 41].Calculate();

            var data = excelPackage.GetAsByteArray();
            stream = new MemoryStream(data);
        }

        Assert.Throws<InvalidExcelCellFormatException>(() => excelValidator.Validate(stream));
    }

    private static ExcelWorksheet GetValidHeaderSheet(ExcelPackage excelPackage)
    {
        var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

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
        worksheet.Cells[2, 67].Value = "Note2";

        return worksheet;
    }
}
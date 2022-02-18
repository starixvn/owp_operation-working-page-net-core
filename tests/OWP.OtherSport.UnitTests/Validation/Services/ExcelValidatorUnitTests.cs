namespace OWP.OtherSport.UnitTests.Validation.Services
{
    
    using OfficeOpenXml;
    using OWP.OtherSport.Validation.Exceptions;
    using OWP.OtherSport.Validation.Services;
    using Xunit;

    public class ExcelValidatorUnitTests
    {
        [Fact]
        public void Validate_WithExcelStreamExceeds5000Records_ThrowDataLengthExceedsMaximumException()
        {
            var excelValidator = new ExcelValidatorImp();
            using var stream = new MemoryStream();

            using var excelPackage = new ExcelPackage(stream);
            using var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

            worksheet.Cells[1, 1].Value = "Match Status";
            worksheet.Cells[1, 2].Value = "Sport";
            worksheet.Cells[1, 3].Value = "Date Time";
            worksheet.Cells[1, 4].Value = "League";
            worksheet.Cells[1, 5].Value = "Home";
            worksheet.Cells[1, 6].Value = "Away";

            for (var i = 1; i <= 5002; i++)
            {
                worksheet.Cells[i, 1].Value = "Live";
                worksheet.Cells[i, 2].Value = "Soccer";
                worksheet.Cells[i, 3].Value = "44167.645833333336";
                worksheet.Cells[i, 4].Value = "CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)";
                worksheet.Cells[i, 5].Value = "Dalian Pro FC (N)";
                worksheet.Cells[i, 6].Value = "Guangzhou R&F";
            }

            var data = excelPackage.GetAsByteArray();
            using var excelMemoryStream = new MemoryStream(data);

            Assert.Throws<DataLengthExceedsMaximumException>(() => excelValidator.Validate(excelMemoryStream));
        }

        [Fact]
        public void Validate_WithNotEnoughHeaders_ThrowInvalidExcelFormatException()
        {
            var excelValidator = new ExcelValidatorImp();
            using var stream = new MemoryStream();

            using var excelPackage = new ExcelPackage(stream);
            using var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

            worksheet.Cells[1, 1].Value = "Match Status";
            worksheet.Cells[1, 2].Value = "Sport";
            worksheet.Cells[1, 3].Value = "Date Time";
            worksheet.Cells[1, 4].Value = "League";
            worksheet.Cells[1, 5].Value = "Home";
            worksheet.Cells[1, 6].Value = "Away";

            var data = excelPackage.GetAsByteArray();
            using var excelMemoryStream = new MemoryStream(data);

            Assert.Throws<InvalidExcelFormatException>(() => excelValidator.Validate(excelMemoryStream));
        }

        [Fact]
        public void Validate_WithErrorCellValue_ThrowInvalidExcelCellFormatException()
        {
            var excelValidator = new ExcelValidatorImp();
            using var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var excelPackage = new ExcelPackage(stream);
            using var worksheet = GetValidHeaderWorksheet(excelPackage);

            worksheet.Cells[2, 41].Formula = "B4/C4";
            worksheet.Cells[2, 41].Calculate();

            var data = excelPackage.GetAsByteArray();
            using var excelMemoryStream = new MemoryStream(data);

            Assert.Throws<InvalidExcelCellFormatException>(() => excelValidator.Validate(excelMemoryStream));
        }

        [Fact]
        public void Validate_WithAllValidCellValue_DoesNotThrowException()
        {
            var excelValidator = new ExcelValidatorImp();
            using var stream = new MemoryStream();
            using var excelPackage = new ExcelPackage(stream);
            using var worksheet = GetValidHeaderWorksheet(excelPackage);

            worksheet.Cells[2, 2].Value = "Live";
            worksheet.Cells[2, 3].Value = "Soccer";
            worksheet.Cells[2, 4].Value = "44167.645833333336";
            worksheet.Cells[2, 5].Value = "CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)";
            worksheet.Cells[2, 6].Value = "Dalian Pro FC (N)";
            worksheet.Cells[2, 7].Value = "Guangzhou R&F";
            worksheet.Cells[2, 8].Value = "Asian 4";
            worksheet.Cells[2, 9].Value = "3HDP";
            worksheet.Cells[2, 10].Value = "15s Accept";
            worksheet.Cells[2, 11].Value = "YES";
            worksheet.Cells[2, 12].Value = "TV/BGTV/Glive53";
            worksheet.Cells[2, 13].Value = "BGtv/RB2tv/BETtv/";
            worksheet.Cells[2, 14].Value = "InPlay";
            worksheet.Cells[2, 15].Value = "Euro40";
            worksheet.Cells[2, 19].Value = "starix1";
            worksheet.Cells[2, 20].Value = "starix2";
            worksheet.Cells[2, 142].Value = "#starix14 other";
            worksheet.Cells[2, 159].Value = "50000";

            var data = excelPackage.GetAsByteArray();
            using var excelMemoryStream = new MemoryStream(data);

            var exception = Record.Exception(() => excelValidator.Validate(excelMemoryStream));
            Assert.Null(exception);
        }

        public static ExcelWorksheet GetValidHeaderWorksheet(ExcelPackage excelPackage)
        {
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");
            worksheet.Cells[1, 1].Value = "Match No";
            worksheet.Cells[1, 2].Value = "Match Status";
            worksheet.Cells[1, 3].Value = "Sport";
            worksheet.Cells[1, 4].Value = "Date Time";
            worksheet.Cells[1, 5].Value = "League";
            worksheet.Cells[1, 6].Value = "Home";
            worksheet.Cells[1, 7].Value = "Away";
            worksheet.Cells[1, 8].Value = "Category";
            worksheet.Cells[1, 9].Value = "No. HDP";
            worksheet.Cells[1, 10].Value = "Match Offer";
            worksheet.Cells[1, 11].Value = "Auto Link";
            worksheet.Cells[1, 12].Value = "Provider";
            worksheet.Cells[1, 13].Value = "Other Provider";
            worksheet.Cells[1, 14].Value = "Service";
            worksheet.Cells[1, 15].Value = "IPTV";
            worksheet.Cells[1, 16].Value = "Streaming Info (WB)";
            worksheet.Cells[1, 17].Value = "Reporter";
            worksheet.Cells[1, 18].Value = "Remark";
            worksheet.Cells[1, 19].Value = "Trader Basketball FT (HDP)";
            worksheet.Cells[1, 20].Value = "Trader Basketball 1H (HDP)";
            worksheet.Cells[1, 21].Value = "Trader Basketball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 22].Value = "Trader Basketball FT (OU/OE)";
            worksheet.Cells[1, 23].Value = "Trader Basketball 1H (OU/OE)";
            worksheet.Cells[1, 24].Value = "Trader Basketball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 25].Value = "Trader Football FT (HDP)";
            worksheet.Cells[1, 26].Value = "Trader Football 1H (HDP)";
            worksheet.Cells[1, 27].Value = "Trader Football 1Q & 3Q (HDP)";
            worksheet.Cells[1, 28].Value = "Trader Football FT (OU)";
            worksheet.Cells[1, 29].Value = "Trader Football 1H (OU)";
            worksheet.Cells[1, 30].Value = "Trader Football 1Q & 3Q (OU)";
            worksheet.Cells[1, 31].Value = "Trader Field Hockey FT (HDP)";
            worksheet.Cells[1, 32].Value = "Trader Field Hockey 1H (HDP)";
            worksheet.Cells[1, 33].Value = "Trader Field Hockey 1Q (HDP)";
            worksheet.Cells[1, 34].Value = "Trader Field Hockey FT (O/U)";
            worksheet.Cells[1, 35].Value = "Trader Field Hockey 1H (O/U)";
            worksheet.Cells[1, 36].Value = "Trader Field Hockey 1Q (OU)";
            worksheet.Cells[1, 37].Value = "Trader Ice Hockey FT (HDP)";
            worksheet.Cells[1, 38].Value = "Trader Ice Hockey 1st-2nd Periods (HDP)";
            worksheet.Cells[1, 39].Value = "Trader Ice Hockey FT (O/U)";
            worksheet.Cells[1, 40].Value = "Trader Ice Hockey 1st-2nd Periods (OU)";
            worksheet.Cells[1, 41].Value = "Trader Other Sports Match Winner";
            worksheet.Cells[1, 42].Value = "Trader Other Sports FT (HDP)";
            worksheet.Cells[1, 43].Value = "Trader Other Sports 1H (HDP)";
            worksheet.Cells[1, 44].Value = "Trader Other Sports FT (O/U)";
            worksheet.Cells[1, 45].Value = "Trader Other Sports 1H (O/U)";
            worksheet.Cells[1, 46].Value = "Trader Darts Most 180s (HDP)";
            worksheet.Cells[1, 47].Value = "Trader Darts Legs (HDP)";
            worksheet.Cells[1, 48].Value = "Trader Pool Match Winner (HDP)";
            worksheet.Cells[1, 49].Value = "Trader Pool Racks Winner (HDP)";
            worksheet.Cells[1, 50].Value = "Trader Table Tennis Set Winner (HDP)";
            worksheet.Cells[1, 51].Value = "Trader Table Tennis Set x Winner (HDP)";
            worksheet.Cells[1, 52].Value = "Trader Volleyball Set Winner (HDP/OE)";
            worksheet.Cells[1, 53].Value = "Trader Volleyball Sets x Point Winner (HDP)";
            worksheet.Cells[1, 54].Value = "Trader Handball FT (HDP)";
            worksheet.Cells[1, 55].Value = "Trader Handball 1H (HDP)";
            worksheet.Cells[1, 56].Value = "Trader Handball FT (O/U)";
            worksheet.Cells[1, 57].Value = "Trader Handball 1H (O/U)";
            worksheet.Cells[1, 58].Value = "Trader Netball FT (HDP)";
            worksheet.Cells[1, 59].Value = "Trader Netball 1H (HDP)";
            worksheet.Cells[1, 60].Value = "Trader Netball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 61].Value = "Trader Netball FT (OU/OE)";
            worksheet.Cells[1, 62].Value = "Trader Netball 1H (OU/OE)";
            worksheet.Cells[1, 63].Value = "Trader Netball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 64].Value = "Trader Cricket Match Winner & Innings (OU)";
            worksheet.Cells[1, 65].Value = "Trader Cricket Next Over (OU) & Player (OU)";
            worksheet.Cells[1, 66].Value = "Trader Cricket FT 1X2";
            worksheet.Cells[1, 67].Value = "Trader Snooker Match Winner (HDP/OU)";
            worksheet.Cells[1, 68].Value = "Trader Snooker Frames Winner (HDP/OU/OE)";
            worksheet.Cells[1, 69].Value = "Trader Snooker First 5 Frames (HDP)";
            worksheet.Cells[1, 70].Value = "Trader Snooker First 8 Frames (HDP)";
            worksheet.Cells[1, 71].Value = "Trader Snooker First 9 Frames (HDP)";
            worksheet.Cells[1, 72].Value = "Trader Baseball FT Run Line (HDP)";
            worksheet.Cells[1, 73].Value = "Trader Baseball 1H 1st 5 innings (HDP)";
            worksheet.Cells[1, 74].Value = "Trader Baseball FT Total Run (OU)";
            worksheet.Cells[1, 75].Value = "Trader Baseball 1H 1st 5 innings (OU)";
            worksheet.Cells[1, 76].Value = "Trader Baseball Inning Hits & Runs";
            worksheet.Cells[1, 77].Value = "Trader Badminton All Product (Set Winner)";
            worksheet.Cells[1, 78].Value = "Trader Badminton All Product (Point Winner)";
            worksheet.Cells[1, 79].Value = "Trader Badminton Race to Points (7, 9, 11)";
            worksheet.Cells[1, 80].Value = "Trader Badminton Next Point Winner";
            worksheet.Cells[1, 81].Value = "Trader Badminton Match Handicap";
            worksheet.Cells[1, 82].Value = "Trader Tennis All Product (LS)";
            worksheet.Cells[1, 83].Value = "Trader Tennis Next Game";
            worksheet.Cells[1, 84].Value = "Trader Tennis Most Aces";
            worksheet.Cells[1, 85].Value = "Trader Tennis Most Double Faults";
            worksheet.Cells[1, 86].Value = "Other Trader Basketball FT (HDP)";
            worksheet.Cells[1, 87].Value = "Other Trader Basketball 1H (HDP)";
            worksheet.Cells[1, 88].Value = "Other Trader Basketball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 89].Value = "Other Trader Basketball FT (OU/OE)";
            worksheet.Cells[1, 90].Value = "Other Trader Basketball 1H (OU/OE)";
            worksheet.Cells[1, 91].Value = "Other Trader Basketball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 92].Value = "Other Trader Football FT (HDP)";
            worksheet.Cells[1, 93].Value = "Other Trader Football 1H (HDP)";
            worksheet.Cells[1, 94].Value = "Other Trader Football 1Q & 3Q (HDP)";
            worksheet.Cells[1, 95].Value = "Other Trader Football FT (OU)";
            worksheet.Cells[1, 96].Value = "Other Trader Football 1H (OU)";
            worksheet.Cells[1, 97].Value = "Other Trader Football 1Q & 3Q (OU)";
            worksheet.Cells[1, 98].Value = "Other Trader Field Hockey FT (HDP)";
            worksheet.Cells[1, 99].Value = "Other Trader Field Hockey 1H (HDP)";
            worksheet.Cells[1, 100].Value = "Other Trader Field Hockey 1Q (HDP)";
            worksheet.Cells[1, 101].Value = "Other Trader Field Hockey FT (O/U)";
            worksheet.Cells[1, 102].Value = "Other Trader Field Hockey 1H (O/U)";
            worksheet.Cells[1, 103].Value = "Other Trader Field Hockey 1Q (OU)";
            worksheet.Cells[1, 104].Value = "Other Trader Ice Hockey FT (HDP)";
            worksheet.Cells[1, 105].Value = "Other Trader Ice Hockey 1st-2nd Periods (HDP)";
            worksheet.Cells[1, 106].Value = "Other Trader Ice Hockey FT (O/U)";
            worksheet.Cells[1, 107].Value = "Other Trader Ice Hockey 1st-2nd Periods (OU)";
            worksheet.Cells[1, 108].Value = "Other Trader Other Sports Match Winner";
            worksheet.Cells[1, 109].Value = "Other Trader Other Sports FT (HDP)";
            worksheet.Cells[1, 110].Value = "Other Trader Other Sports 1H (HDP)";
            worksheet.Cells[1, 111].Value = "Other Trader Other Sports FT (O/U)";
            worksheet.Cells[1, 112].Value = "Other Trader Other Sports 1H (O/U)";
            worksheet.Cells[1, 113].Value = "Other Trader Darts Legs (HDP)";
            worksheet.Cells[1, 114].Value = "Other Trader Darts Most 180s (HDP)";
            worksheet.Cells[1, 115].Value = "Other Trader Pool Match Winner (HDP)";
            worksheet.Cells[1, 116].Value = "Other Trader Pool Racks Winner (HDP)";
            worksheet.Cells[1, 117].Value = "Other Trader Table Tennis Set Winner (HDP)";
            worksheet.Cells[1, 118].Value = "Other Trader Table Tennis Set x Winner (HDP)";
            worksheet.Cells[1, 119].Value = "Other Trader Volleyball Set Winner (HDP/OE)";
            worksheet.Cells[1, 120].Value = "Other Trader Volleyball Sets x Point Winner (HDP)";
            worksheet.Cells[1, 121].Value = "Other Trader Handball FT (HDP)";
            worksheet.Cells[1, 122].Value = "Other Trader Handball 1H (HDP)";
            worksheet.Cells[1, 123].Value = "Other Trader Handball FT (O/U)";
            worksheet.Cells[1, 124].Value = "Other Trader Handball 1H (O/U)";
            worksheet.Cells[1, 125].Value = "Other Trader Netball FT (HDP)";
            worksheet.Cells[1, 126].Value = "Other Trader Netball 1H (HDP)";
            worksheet.Cells[1, 127].Value = "Other Trader Netball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 128].Value = "Other Trader Netball FT (OU/OE)";
            worksheet.Cells[1, 129].Value = "Other Trader Netball 1H (OU/OE)";
            worksheet.Cells[1, 130].Value = "Other Trader Netball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 131].Value = "Other Trader Cricket Match Winner & Innings (OU)";
            worksheet.Cells[1, 132].Value = "Other Trader Cricket Next Over (OU) & Player (OU)";
            worksheet.Cells[1, 133].Value = "Other Trader Cricket FT 1X2";
            worksheet.Cells[1, 134].Value = "Other Trader Snooker Match Winner (HDP/OU)";
            worksheet.Cells[1, 135].Value = "Other Trader Snooker Frames Winner (HDP/OU/OE)";
            worksheet.Cells[1, 136].Value = "Other Trader Snooker First 5 Frames (HDP)";
            worksheet.Cells[1, 137].Value = "Other Trader Snooker First 8 Frames (HDP)";
            worksheet.Cells[1, 138].Value = "Other Trader Snooker First 9 Frames (HDP)";
            worksheet.Cells[1, 139].Value = "Other Trader Baseball FT Run Line (HDP)";
            worksheet.Cells[1, 140].Value = "Other Trader Baseball 1H 1st 5 innings (HDP)";
            worksheet.Cells[1, 141].Value = "Other Trader Baseball FT Total Run (OU)";
            worksheet.Cells[1, 142].Value = "Other Trader Baseball 1H 1st 5 innings (OU)";
            worksheet.Cells[1, 143].Value = "Other Trader Baseball Inning Hits & Runs";
            worksheet.Cells[1, 144].Value = "Other Trader Badminton All Product (Set Winner)";
            worksheet.Cells[1, 145].Value = "Other Trader Badminton All Product (Point Winner)";
            worksheet.Cells[1, 146].Value = "Other Trader Badminton Race to Points (7, 9, 11)";
            worksheet.Cells[1, 147].Value = "Other Trader Badminton Next Point Winner";
            worksheet.Cells[1, 148].Value = "Other Trader Badminton Match Handicap";
            worksheet.Cells[1, 149].Value = "Other Trader Tennis All Product (LS)";
            worksheet.Cells[1, 150].Value = "Other Trader Tennis Next Game";
            worksheet.Cells[1, 151].Value = "Other Trader Tennis Most Aces";
            worksheet.Cells[1, 152].Value = "Other Trader Tennis Most Double Faults";
            worksheet.Cells[1, 153].Value = "Max Bet Basketball FT (HDP)";
            worksheet.Cells[1, 154].Value = "Max Bet Basketball FT (OU/OE)";
            worksheet.Cells[1, 155].Value = "Max Bet Basketball FT (OE)";
            worksheet.Cells[1, 156].Value = "Max Bet Football FT (HDP)";
            worksheet.Cells[1, 157].Value = "Max Bet Football 1H (HDP)";
            worksheet.Cells[1, 158].Value = "Max Bet Football 1Q & 3Q (HDP)";
            worksheet.Cells[1, 159].Value = "Max Bet Football FT (OU)";
            worksheet.Cells[1, 160].Value = "Max Bet Football 1H (OU)";
            worksheet.Cells[1, 161].Value = "Max Bet Football 1Q & 3Q (OU)";
            worksheet.Cells[1, 162].Value = "Max Bet Field Hockey FT (HDP)";
            worksheet.Cells[1, 163].Value = "Max Bet Field Hockey 1H (HDP)";
            worksheet.Cells[1, 164].Value = "Max Bet Field Hockey 1Q (HDP)";
            worksheet.Cells[1, 165].Value = "Max Bet Field Hockey FT (O/U)";
            worksheet.Cells[1, 166].Value = "Max Bet Field Hockey 1H (O/U)";
            worksheet.Cells[1, 167].Value = "Max Bet Field Hockey 1Q (OU)";
            worksheet.Cells[1, 168].Value = "Max Bet Ice Hockey FT (HDP)";
            worksheet.Cells[1, 169].Value = "Max Bet Ice Hockey 1st-2nd Periods (HDP)";
            worksheet.Cells[1, 170].Value = "Max Bet Ice Hockey FT (O/U)";
            worksheet.Cells[1, 171].Value = "Max Bet Ice Hockey 1st-2nd Periods (OU)";
            worksheet.Cells[1, 172].Value = "Max Bet Other Sports Match Winner";
            worksheet.Cells[1, 173].Value = "Max Bet Other Sports FT (HDP)";
            worksheet.Cells[1, 174].Value = "Max Bet Other Sports 1H (HDP)";
            worksheet.Cells[1, 175].Value = "Max Bet Other Sports FT (O/U)";
            worksheet.Cells[1, 176].Value = "Max Bet Other Sports 1H (O/U)";
            worksheet.Cells[1, 177].Value = "Max Bet Darts Legs (HDP)";
            worksheet.Cells[1, 178].Value = "Max Bet Darts Most 180s (HDP)";
            worksheet.Cells[1, 179].Value = "Max Bet Pool Match Winner (HDP)";
            worksheet.Cells[1, 180].Value = "Max Bet Pool Racks Winner (HDP)";
            worksheet.Cells[1, 181].Value = "Max Bet Table Tennis Set Winner (HDP)";
            worksheet.Cells[1, 182].Value = "Max Bet Table Tennis Set x Winner (HDP)";
            worksheet.Cells[1, 183].Value = "Max Bet Volleyball Set Winner (HDP/OE)";
            worksheet.Cells[1, 184].Value = "Max Bet Volleyball Set Winner (OE)";
            worksheet.Cells[1, 185].Value = "Max Bet Volleyball Sets x Point Winner (HDP)";
            worksheet.Cells[1, 186].Value = "Max Bet Handball FT (HDP)";
            worksheet.Cells[1, 187].Value = "Max Bet Handball 1H (HDP)";
            worksheet.Cells[1, 188].Value = "Max Bet Handball FT (O/U)";
            worksheet.Cells[1, 189].Value = "Max Bet Handball 1H (O/U)";
            worksheet.Cells[1, 190].Value = "Max Bet Netball FT (HDP)";
            worksheet.Cells[1, 191].Value = "Max Bet Netball 1H (HDP)";
            worksheet.Cells[1, 192].Value = "Max Bet Netball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 193].Value = "Max Bet Netball FT (OU/OE)";
            worksheet.Cells[1, 194].Value = "Max Bet Netball FT (OE)";
            worksheet.Cells[1, 195].Value = "Max Bet Netball 1H (OU/OE)";
            worksheet.Cells[1, 196].Value = "Max Bet Netball 1H (OE)";
            worksheet.Cells[1, 197].Value = "Max Bet Netball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 198].Value = "Max Bet Netball 1Q & 3Q (OE)";
            worksheet.Cells[1, 199].Value = "Max Bet Cricket Match Winner & Innings (OU)";
            worksheet.Cells[1, 200].Value = "Max Bet Cricket Next Over (OU) & Player (OU)";
            worksheet.Cells[1, 201].Value = "Max Bet Cricket FT 1X2";
            worksheet.Cells[1, 202].Value = "Max Bet Snooker Match Winner (HDP/OU)";
            worksheet.Cells[1, 203].Value = "Max Bet Snooker Match Winner (OU)";
            worksheet.Cells[1, 204].Value = "Max Bet Snooker Frames Winner (HDP/OU/OE)";
            worksheet.Cells[1, 205].Value = "Max Bet Snooker Frames Winner (OU)";
            worksheet.Cells[1, 206].Value = "Max Bet Snooker Frames Winner (OE)";
            worksheet.Cells[1, 207].Value = "Max Bet Snooker First 5 Frames (HDP)";
            worksheet.Cells[1, 208].Value = "Max Bet Snooker First 8 Frames (HDP)";
            worksheet.Cells[1, 209].Value = "Max Bet Snooker First 9 Frames (HDP)";
            worksheet.Cells[1, 210].Value = "Max Bet Baseball FT Run Line (HDP)";
            worksheet.Cells[1, 211].Value = "Max Bet Baseball 1H 1st 5 innings (HDP)";
            worksheet.Cells[1, 212].Value = "Max Bet Baseball FT Total Run (OU)";
            worksheet.Cells[1, 213].Value = "Max Bet Baseball 1H 1st 5 innings (OU)";
            worksheet.Cells[1, 214].Value = "Max Bet Baseball Inning Hits & Runs";
            worksheet.Cells[1, 215].Value = "Max Bet Badminton All Product (Set Winner)";
            worksheet.Cells[1, 216].Value = "Max Bet Badminton All Product (Point Winner)";
            worksheet.Cells[1, 217].Value = "Max Bet Badminton Race to Points (7, 9, 11)";
            worksheet.Cells[1, 218].Value = "Max Bet Badminton Next Point Winner";
            worksheet.Cells[1, 219].Value = "Max Bet Badminton Match Handicap";
            worksheet.Cells[1, 220].Value = "Max Bet Tennis All Product (LS)";
            worksheet.Cells[1, 221].Value = "Max Bet Tennis Next Game";
            worksheet.Cells[1, 222].Value = "Max Bet Tennis Most Aces";
            worksheet.Cells[1, 223].Value = "Max Bet Tennis Most Double Faults";
            worksheet.Cells[1, 224].Value = " Spread Basketball FT (HDP)";
            worksheet.Cells[1, 225].Value = " Spread Basketball FT (OU/OE)";
            worksheet.Cells[1, 226].Value = " Spread Basketball FT (OE)";
            worksheet.Cells[1, 227].Value = " Spread Football FT (HDP)";
            worksheet.Cells[1, 228].Value = " Spread Football 1H (HDP)";
            worksheet.Cells[1, 229].Value = " Spread Football 1Q & 3Q (HDP)";
            worksheet.Cells[1, 230].Value = " Spread Football FT (OU)";
            worksheet.Cells[1, 231].Value = " Spread Football 1H (OU)";
            worksheet.Cells[1, 232].Value = " Spread Football 1Q & 3Q (OU)";
            worksheet.Cells[1, 233].Value = " Spread Field Hockey FT (HDP)";
            worksheet.Cells[1, 234].Value = " Spread Field Hockey 1H (HDP)";
            worksheet.Cells[1, 235].Value = " Spread Field Hockey 1Q (HDP)";
            worksheet.Cells[1, 236].Value = " Spread Field Hockey FT (O/U)";
            worksheet.Cells[1, 237].Value = " Spread Field Hockey 1H (O/U)";
            worksheet.Cells[1, 238].Value = " Spread Field Hockey 1Q (OU)";
            worksheet.Cells[1, 239].Value = " Spread Ice Hockey FT (HDP)";
            worksheet.Cells[1, 240].Value = " Spread Ice Hockey 1st-2nd Periods (HDP)";
            worksheet.Cells[1, 241].Value = " Spread Ice Hockey FT (O/U)";
            worksheet.Cells[1, 242].Value = " Spread Ice Hockey 1st-2nd Periods (OU)";
            worksheet.Cells[1, 243].Value = " Spread Other Sports Match Winner";
            worksheet.Cells[1, 244].Value = " Spread Other Sports FT (HDP)";
            worksheet.Cells[1, 245].Value = " Spread Other Sports 1H (HDP)";
            worksheet.Cells[1, 246].Value = " Spread Other Sports FT (O/U)";
            worksheet.Cells[1, 247].Value = " Spread Other Sports 1H (O/U)";
            worksheet.Cells[1, 248].Value = " Spread Darts Legs (HDP)";
            worksheet.Cells[1, 249].Value = " Spread Darts Most 180s (HDP)";
            worksheet.Cells[1, 250].Value = " Spread Pool Match Winner (HDP)";
            worksheet.Cells[1, 251].Value = " Spread Pool Racks Winner (HDP)";
            worksheet.Cells[1, 252].Value = " Spread Table Tennis Set Winner (HDP)";
            worksheet.Cells[1, 253].Value = " Spread Table Tennis Set x Winner (HDP)";
            worksheet.Cells[1, 254].Value = " Spread Volleyball Set Winner (HDP/OE)";
            worksheet.Cells[1, 255].Value = " Spread Volleyball Set Winner (OE)";
            worksheet.Cells[1, 256].Value = " Spread Volleyball Sets x Point Winner (HDP)";
            worksheet.Cells[1, 257].Value = " Spread Handball FT (HDP)";
            worksheet.Cells[1, 258].Value = " Spread Handball 1H (HDP)";
            worksheet.Cells[1, 259].Value = " Spread Handball FT (O/U)";
            worksheet.Cells[1, 260].Value = " Spread Handball 1H (O/U)";
            worksheet.Cells[1, 261].Value = " Spread Netball FT (HDP)";
            worksheet.Cells[1, 262].Value = " Spread Netball 1H (HDP)";
            worksheet.Cells[1, 263].Value = " Spread Netball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 264].Value = " Spread Netball FT (OU/OE)";
            worksheet.Cells[1, 265].Value = " Spread Netball FT (OE)";
            worksheet.Cells[1, 266].Value = " Spread Netball 1H (OU/OE)";
            worksheet.Cells[1, 267].Value = " Spread Netball 1H (OE)";
            worksheet.Cells[1, 268].Value = " Spread Netball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 269].Value = " Spread Netball 1Q & 3Q (OE)";
            worksheet.Cells[1, 270].Value = " Spread Cricket Match Winner & Innings (OU)";
            worksheet.Cells[1, 271].Value = " Spread Cricket Next Over (OU) & Player (OU)";
            worksheet.Cells[1, 272].Value = " Spread Cricket FT 1X2";
            worksheet.Cells[1, 273].Value = " Spread Snooker Match Winner (HDP/OU)";
            worksheet.Cells[1, 274].Value = " Spread Snooker Match Winner (OU)";
            worksheet.Cells[1, 275].Value = " Spread Snooker Frames Winner (HDP/OU/OE)";
            worksheet.Cells[1, 276].Value = " Spread Snooker Frames Winner (OU)";
            worksheet.Cells[1, 277].Value = " Spread Snooker Frames Winner (OE)";
            worksheet.Cells[1, 278].Value = " Spread Snooker First 5 Frames (HDP)";
            worksheet.Cells[1, 279].Value = " Spread Snooker First 8 Frames (HDP)";
            worksheet.Cells[1, 280].Value = " Spread Snooker First 9 Frames (HDP)";
            worksheet.Cells[1, 281].Value = " Spread Baseball FT Run Line (HDP)";
            worksheet.Cells[1, 282].Value = " Spread Baseball 1H 1st 5 innings (HDP)";
            worksheet.Cells[1, 283].Value = " Spread Baseball FT Total Run (OU)";
            worksheet.Cells[1, 284].Value = " Spread Baseball 1H 1st 5 innings (OU)";
            worksheet.Cells[1, 285].Value = " Spread Baseball Inning Hits & Runs";
            worksheet.Cells[1, 286].Value = " Spread Badminton All Product (Set Winner)";
            worksheet.Cells[1, 287].Value = " Spread Badminton All Product (Point Winner)";
            worksheet.Cells[1, 288].Value = " Spread Badminton Race to Points (7, 9, 11)";
            worksheet.Cells[1, 289].Value = " Spread Badminton Next Point Winner";
            worksheet.Cells[1, 290].Value = " Spread Badminton Match Handicap";
            worksheet.Cells[1, 291].Value = " Spread Tennis All Product (LS)";
            worksheet.Cells[1, 292].Value = " Spread Tennis Next Game";
            worksheet.Cells[1, 293].Value = " Spread Tennis Most Aces";
            worksheet.Cells[1, 294].Value = " Spread Tennis Most Double Faults";

            return worksheet;
        }
    }
}
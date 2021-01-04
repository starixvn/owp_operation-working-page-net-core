namespace OWP.OtherSport.UnitTests.Validation.Extensions
{
    using System.Collections.Generic;
    using OfficeOpenXml;
    using Xunit;
    using Xunit.Asserts.Compare;
    using OWP.OtherSport.Validation.Extensions;
    using OWP.OtherSport.Validation.Models;
    using System.Linq;

    public class WorkSheetExtensionsUnitTests
    {
        [Fact]
        public void GetHeaderMap_ExcelWithSomeHeaders_ReturnSomeHeaderWithIndexes()
        {
            using var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

            worksheet.Cells[1, 1].Value = "Match Status";
            worksheet.Cells[1, 2].Value = "Sport";
            worksheet.Cells[1, 3].Value = "Date Time";
            worksheet.Cells[1, 4].Value = "League";
            worksheet.Cells[1, 5].Value = "";

            var expected = new Dictionary<string, int>
                {
                    {"Match Status", 1},
                    {"Sport", 2},
                    {"Date Time", 3},
                    {"Home", 0},
                    {"Away", 0},
                    {"League", 4},
                    {"Category", 0},
                    {"No. HDP", 0},
                    {"Match Offer", 0},
                    {"Auto Link", 0},
                    {"Provider", 0},
                    {"Other Provider", 0},
                    {"Service", 0},
                    {"IPTV", 0},
                    {"Streaming Info (WB)", 0},
                    {"Remark", 0},
                    {"Trader Basketball FT (HDP)", 0},
                    {"Trader Basketball 1H (HDP)", 0},
                    {"Trader Basketball 1Q & 3Q (HDP)", 0},
                    {"Trader Basketball FT (OU/OE)", 0},
                    {"Trader Basketball 1H (OU/OE)", 0},
                    {"Trader Basketball 1Q & 3Q (OU/OE)", 0},
                    {"Trader Football FT (HDP)", 0},
                    {"Trader Football 1H (HDP)", 0},
                    {"Trader Football 1Q & 3Q (HDP)", 0},
                    {"Trader Football FT (OU)", 0},
                    {"Trader Football 1H (OU)", 0},
                    {"Trader Football 1Q & 3Q (OU)", 0},
                    {"Trader Field Hockey FT (HDP)", 0},
                    {"Trader Field Hockey 1H (HDP)", 0},
                    {"Trader Field Hockey 1Q (HDP)", 0},
                    {"Trader Field Hockey FT (O/U)", 0},
                    {"Trader Field Hockey 1H (O/U)", 0},
                    {"Trader Field Hockey 1Q (OU)", 0},
                    {"Trader Ice Hockey FT (HDP)", 0},
                    {"Trader Ice Hockey 1st-2nd Periods (HDP)", 0},
                    {"Trader Ice Hockey FT (O/U)", 0},
                    {"Trader Ice Hockey 1st-2nd Periods (OU)", 0},
                    {"Trader Other Sports Match Winner", 0},
                    {"Trader Darts Legs (HDP)", 0},
                    {"Trader Darts Most 180s (HDP)", 0},
                    {"Trader Pool Match Winner (HDP)", 0},
                    {"Trader Pool Racks Winner (HDP)", 0},
                    {"Trader Table Tennis Set Winner (HDP)", 0},
                    {"Trader Table Tennis Set x Winner (HDP)", 0},
                    {"Trader Volleyball Set Winner (HDP/OE)", 0},
                    {"Trader Volleyball Sets x Point Winner (HDP)", 0},
                    {"Trader Handball FT (HDP)", 0},
                    {"Trader Handball 1H (HDP)", 0},
                    {"Trader Handball FT (O/U)", 0},
                    {"Trader Handball 1H (O/U)", 0},
                    {"Trader Netball FT (HDP)", 0},
                    {"Trader Netball 1H (HDP)", 0},
                    {"Trader Netball 1Q & 3Q (HDP)", 0},
                    {"Trader Netball FT (OU/OE)", 0},
                    {"Trader Netball 1H (OU/OE)", 0},
                    {"Trader Netball 1Q & 3Q (OU/OE)", 0},
                    {"Trader Cricket Match Winner & Innings (OU)", 0},
                    {"Trader Cricket Next Over (OU) & Player (OU)", 0},
                    {"Trader Cricket FT 1X2", 0},
                    {"Trader Snooker Match Winner (HDP/OU)", 0},
                    {"Trader Snooker Frames Winner (HDP/OU/OE)", 0},
                    {"Trader Snooker First 5 Frames (HDP)", 0},
                    {"Trader Snooker First 8 Frames (HDP)", 0},
                    {"Trader Snooker First 9 Frames (HDP)", 0},
                    {"Trader Baseball FT Run Line (HDP)", 0},
                    {"Trader Baseball 1H 1st 5 innings (HDP)", 0},
                    {"Trader Baseball FT Total Run (OU)", 0},
                    {"Trader Baseball 1H 1st 5 innings (OU)", 0},
                    {"Trader Baseball Inning Hits & Runs", 0},
                    {"Trader Badminton All Product (Set Winner)", 0},
                    {"Trader Badminton All Product (Point Winner)", 0},
                    {"Trader Badminton Race to Points (7, 9, 11)", 0},
                    {"Trader Badminton Next Point Winner", 0},
                    {"Trader Badminton Match Handicap", 0},
                    {"Trader Tennis All Product (LS)", 0},
                    {"Trader Tennis Next Game", 0},
                    {"Trader Tennis Most Aces", 0},
                    {"Trader Tennis Most Double Faults", 0},
                    {"Other Trader Basketball FT (HDP)", 0},
                    {"Other Trader Basketball 1H (HDP)", 0},
                    {"Other Trader Basketball 1Q & 3Q (HDP)", 0},
                    {"Other Trader Basketball FT (OU/OE)", 0},
                    {"Other Trader Basketball 1H (OU/OE)", 0},
                    {"Other Trader Basketball 1Q & 3Q (OU/OE)", 0},
                    {"Other Trader Football FT (HDP)", 0},
                    {"Other Trader Football 1H (HDP)", 0},
                    {"Other Trader Football 1Q & 3Q (HDP)", 0},
                    {"Other Trader Football FT (OU)", 0},
                    {"Other Trader Football 1H (OU)", 0},
                    {"Other Trader Football 1Q & 3Q (OU)", 0},
                    {"Other Trader Field Hockey FT (HDP)", 0},
                    {"Other Trader Field Hockey 1H (HDP)", 0},
                    {"Other Trader Field Hockey 1Q (HDP)", 0},
                    {"Other Trader Field Hockey FT (O/U)", 0},
                    {"Other Trader Field Hockey 1H (O/U)", 0},
                    {"Other Trader Field Hockey 1Q (OU)", 0},
                    {"Other Trader Ice Hockey FT (HDP)", 0},
                    {"Other Trader Ice Hockey 1st-2nd Periods (HDP)", 0},
                    {"Other Trader Ice Hockey FT (O/U)", 0},
                    {"Other Trader Ice Hockey 1st-2nd Periods (OU)", 0},
                    {"Other Trader Other Sports Match Winner", 0},
                    {"Other Trader Darts Legs (HDP)", 0},
                    {"Other Trader Darts Most 180s (HDP)", 0},
                    {"Other Trader Pool Match Winner (HDP)", 0},
                    {"Other Trader Pool Racks Winner (HDP)", 0},
                    {"Other Trader Table Tennis Set Winner (HDP)", 0},
                    {"Other Trader Table Tennis Set x Winner (HDP)", 0},
                    {"Other Trader Volleyball Set Winner (HDP/OE)", 0},
                    {"Other Trader Volleyball Sets x Point Winner (HDP)", 0},
                    {"Other Trader Handball FT (HDP)", 0},
                    {"Other Trader Handball 1H (HDP)", 0},
                    {"Other Trader Handball FT (O/U)", 0},
                    {"Other Trader Handball 1H (O/U)", 0},
                    {"Other Trader Netball FT (HDP)", 0},
                    {"Other Trader Netball 1H (HDP)", 0},
                    {"Other Trader Netball 1Q & 3Q (HDP)", 0},
                    {"Other Trader Netball FT (OU/OE)", 0},
                    {"Other Trader Netball 1H (OU/OE)", 0},
                    {"Other Trader Netball 1Q & 3Q (OU/OE)", 0},
                    {"Other Trader Cricket Match Winner & Innings (OU)", 0},
                    {"Other Trader Cricket Next Over (OU) & Player (OU)", 0},
                    {"Other Trader Cricket FT 1X2", 0},
                    {"Other Trader Snooker Match Winner (HDP/OU)", 0},
                    {"Other Trader Snooker Frames Winner (HDP/OU/OE)", 0},
                    {"Other Trader Snooker First 5 Frames (HDP)", 0},
                    {"Other Trader Snooker First 8 Frames (HDP)", 0},
                    {"Other Trader Snooker First 9 Frames (HDP)", 0},
                    {"Other Trader Baseball FT Run Line (HDP)", 0},
                    {"Other Trader Baseball 1H 1st 5 innings (HDP)", 0},
                    {"Other Trader Baseball FT Total Run (OU)", 0},
                    {"Other Trader Baseball 1H 1st 5 innings (OU)", 0},
                    {"Other Trader Baseball Inning Hits & Runs", 0},
                    {"Other Trader Badminton All Product (Set Winner)", 0},
                    {"Other Trader Badminton All Product (Point Winner)", 0},
                    {"Other Trader Badminton Race to Points (7, 9, 11)", 0},
                    {"Other Trader Badminton Next Point Winner", 0},
                    {"Other Trader Badminton Match Handicap", 0},
                    {"Other Trader Tennis All Product (LS)", 0},
                    {"Other Trader Tennis Next Game", 0},
                    {"Other Trader Tennis Most Aces", 0},
                    {"Other Trader Tennis Most Double Faults", 0},
                    {"Max Bet Basketball FT (HDP)", 0},
                    {"Max Bet Basketball FT (OE)", 0},
                    {"Max Bet Football FT (HDP)", 0},
                    {"Max Bet Football 1H (HDP)", 0},
                    {"Max Bet Football 1Q & 3Q (HDP)", 0},
                    {"Max Bet Football FT (OU)", 0},
                    {"Max Bet Football 1H (OU)", 0},
                    {"Max Bet Football 1Q & 3Q (OU)", 0},
                    {"Max Bet Field Hockey FT (HDP)", 0},
                    {"Max Bet Field Hockey 1H (HDP)", 0},
                    {"Max Bet Field Hockey 1Q (HDP)", 0},
                    {"Max Bet Field Hockey FT (O/U)", 0},
                    {"Max Bet Field Hockey 1H (O/U)", 0},
                    {"Max Bet Field Hockey 1Q (OU)", 0},
                    {"Max Bet Ice Hockey FT (HDP)", 0},
                    {"Max Bet Ice Hockey 1st-2nd Periods (HDP)", 0},
                    {"Max Bet Ice Hockey FT (O/U)", 0},
                    {"Max Bet Ice Hockey 1st-2nd Periods (OU)", 0},
                    {"Max Bet Other Sports Match Winner", 0},
                    {"Max Bet Darts Legs (HDP)", 0},
                    {"Max Bet Darts Most 180s (HDP)", 0},
                    {"Max Bet Pool Match Winner (HDP)", 0},
                    {"Max Bet Pool Racks Winner (HDP)", 0},
                    {"Max Bet Table Tennis Set Winner (HDP)", 0},
                    {"Max Bet Table Tennis Set x Winner (HDP)", 0},
                    {"Max Bet Volleyball Set Winner (HDP/OE)", 0},
                    {"Max Bet Volleyball Sets x Point Winner (HDP)", 0},
                    {"Max Bet Handball FT (HDP)", 0},
                    {"Max Bet Handball 1H (HDP)", 0},
                    {"Max Bet Handball FT (O/U)", 0},
                    {"Max Bet Handball 1H (O/U)", 0},
                    {"Max Bet Netball FT (HDP)", 0},
                    {"Max Bet Netball 1H (HDP)", 0},
                    {"Max Bet Netball 1Q & 3Q (HDP)", 0},
                    {"Max Bet Netball FT (OU/OE)", 0},
                    {"Max Bet Netball 1H (OU/OE)", 0},
                    {"Max Bet Netball 1Q & 3Q (OU/OE)", 0},
                    {"Max Bet Cricket Match Winner & Innings (OU)", 0},
                    {"Max Bet Cricket Next Over (OU) & Player (OU)", 0},
                    {"Max Bet Cricket FT 1X2", 0},
                    {"Max Bet Snooker Match Winner (HDP/OU)", 0},
                    {"Max Bet Snooker Frames Winner (HDP/OU/OE)", 0},
                    {"Max Bet Snooker First 5 Frames (HDP)", 0},
                    {"Max Bet Snooker First 8 Frames (HDP)", 0},
                    {"Max Bet Snooker First 9 Frames (HDP)", 0},
                    {"Max Bet Baseball FT Run Line (HDP)", 0},
                    {"Max Bet Baseball 1H 1st 5 innings (HDP)", 0},
                    {"Max Bet Baseball FT Total Run (OU)", 0},
                    {"Max Bet Baseball 1H 1st 5 innings (OU)", 0},
                    {"Max Bet Baseball Inning Hits & Runs", 0},
                    {"Max Bet Badminton All Product (Set Winner)", 0},
                    {"Max Bet Badminton All Product (Point Winner)", 0},
                    {"Max Bet Badminton Race to Points (7, 9, 11)", 0},
                    {"Max Bet Badminton Next Point Winner", 0},
                    {"Max Bet Badminton Match Handicap", 0},
                    {"Max Bet Tennis All Product (LS)", 0},
                    {"Max Bet Tennis Next Game", 0},
                    {"Max Bet Tennis Most Aces", 0},
                    {"Max Bet Tennis Most Double Faults", 0}
                };

            var actual = worksheet.GetHeaderMap();
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void ReadAllAssignments_ExcelWithSomeValues_ReturnAssignmentModel()
        {
            using var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet0");

            worksheet.Cells[1, 1].Value = "Match Status";
            worksheet.Cells[1, 2].Value = "Sport";
            worksheet.Cells[1, 3].Value = "Date Time";
            worksheet.Cells[1, 4].Value = "League";
            worksheet.Cells[1, 5].Value = "Home";
            worksheet.Cells[1, 6].Value = "Away";
            worksheet.Cells[1, 7].Value = "Category";
            worksheet.Cells[1, 8].Value = "No. HDP";
            worksheet.Cells[1, 9].Value = "Match Offer";
            worksheet.Cells[1, 10].Value = "Auto Link";
            worksheet.Cells[1, 11].Value = "Provider";
            worksheet.Cells[1, 12].Value = "Other Provider";
            worksheet.Cells[1, 13].Value = "Service";
            worksheet.Cells[1, 14].Value = "IPTV";
            worksheet.Cells[1, 15].Value = "Streaming Info (WB)";
            worksheet.Cells[1, 16].Value = "Remark";
            worksheet.Cells[1, 17].Value = "Trader Basketball FT (HDP)";
            worksheet.Cells[1, 18].Value = "Trader Basketball 1H (HDP)";
            worksheet.Cells[1, 19].Value = "Trader Basketball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 20].Value = "Trader Basketball FT (OU/OE)";
            worksheet.Cells[1, 21].Value = "Trader Basketball 1H (OU/OE)";
            worksheet.Cells[1, 22].Value = "Trader Basketball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 23].Value = "Trader Football FT (HDP)";
            worksheet.Cells[1, 24].Value = "Trader Football 1H (HDP)";
            worksheet.Cells[1, 25].Value = "Trader Football 1Q & 3Q (HDP)";
            worksheet.Cells[1, 26].Value = "Trader Football FT (OU)";
            worksheet.Cells[1, 27].Value = "Trader Football 1H (OU)";
            worksheet.Cells[1, 28].Value = "Trader Football 1Q & 3Q (OU)";
            worksheet.Cells[1, 29].Value = "Trader Field Hockey FT (HDP)";
            worksheet.Cells[1, 30].Value = "Trader Field Hockey 1H (HDP)";
            worksheet.Cells[1, 31].Value = "Trader Field Hockey 1Q (HDP)";
            worksheet.Cells[1, 32].Value = "Trader Field Hockey FT (O/U)";
            worksheet.Cells[1, 33].Value = "Trader Field Hockey 1H (O/U)";
            worksheet.Cells[1, 34].Value = "Trader Field Hockey 1Q (OU)";
            worksheet.Cells[1, 35].Value = "Trader Ice Hockey FT (HDP)";
            worksheet.Cells[1, 36].Value = "Trader Ice Hockey 1st-2nd Periods (HDP)";
            worksheet.Cells[1, 37].Value = "Trader Ice Hockey FT (O/U)";
            worksheet.Cells[1, 38].Value = "Trader Ice Hockey 1st-2nd Periods (OU)";
            worksheet.Cells[1, 39].Value = "Trader Other Sports Match Winner";
            worksheet.Cells[1, 40].Value = "Trader Darts Most 180s (HDP)";
            worksheet.Cells[1, 41].Value = "Trader Darts Legs (HDP)";
            worksheet.Cells[1, 42].Value = "Trader Pool Match Winner (HDP)";
            worksheet.Cells[1, 43].Value = "Trader Pool Racks Winner (HDP)";
            worksheet.Cells[1, 44].Value = "Trader Table Tennis Set Winner (HDP)";
            worksheet.Cells[1, 45].Value = "Trader Table Tennis Set x Winner (HDP)";
            worksheet.Cells[1, 46].Value = "Trader Volleyball Set Winner (HDP/OE)";
            worksheet.Cells[1, 47].Value = "Trader Volleyball Sets x Point Winner (HDP)";
            worksheet.Cells[1, 48].Value = "Trader Handball FT (HDP)";
            worksheet.Cells[1, 49].Value = "Trader Handball 1H (HDP)";
            worksheet.Cells[1, 50].Value = "Trader Handball FT (O/U)";
            worksheet.Cells[1, 51].Value = "Trader Handball 1H (O/U)";
            worksheet.Cells[1, 52].Value = "Trader Netball FT (HDP)";
            worksheet.Cells[1, 53].Value = "Trader Netball 1H (HDP)";
            worksheet.Cells[1, 54].Value = "Trader Netball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 55].Value = "Trader Netball FT (OU/OE)";
            worksheet.Cells[1, 56].Value = "Trader Netball 1H (OU/OE)";
            worksheet.Cells[1, 57].Value = "Trader Netball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 58].Value = "Trader Cricket Match Winner & Innings (OU)";
            worksheet.Cells[1, 59].Value = "Trader Cricket Next Over (OU) & Player (OU)";
            worksheet.Cells[1, 60].Value = "Trader Cricket FT 1X2";
            worksheet.Cells[1, 61].Value = "Trader Snooker Match Winner (HDP/OU)";
            worksheet.Cells[1, 62].Value = "Trader Snooker Frames Winner (HDP/OU/OE)";
            worksheet.Cells[1, 63].Value = "Trader Snooker First 5 Frames (HDP)";
            worksheet.Cells[1, 64].Value = "Trader Snooker First 8 Frames (HDP)";
            worksheet.Cells[1, 65].Value = "Trader Snooker First 9 Frames (HDP)";
            worksheet.Cells[1, 66].Value = "Trader Baseball FT Run Line (HDP)";
            worksheet.Cells[1, 67].Value = "Trader Baseball 1H 1st 5 innings (HDP)";
            worksheet.Cells[1, 68].Value = "Trader Baseball FT Total Run (OU)";
            worksheet.Cells[1, 69].Value = "Trader Baseball 1H 1st 5 innings (OU)";
            worksheet.Cells[1, 70].Value = "Trader Baseball Inning Hits & Runs";
            worksheet.Cells[1, 71].Value = "Trader Badminton All Product (Set Winner)";
            worksheet.Cells[1, 72].Value = "Trader Badminton All Product (Point Winner)";
            worksheet.Cells[1, 73].Value = "Trader Badminton Race to Points (7, 9, 11)";
            worksheet.Cells[1, 74].Value = "Trader Badminton Next Point Winner";
            worksheet.Cells[1, 75].Value = "Trader Badminton Match Handicap";
            worksheet.Cells[1, 76].Value = "Trader Tennis All Product (LS)";
            worksheet.Cells[1, 77].Value = "Trader Tennis Next Game";
            worksheet.Cells[1, 78].Value = "Trader Tennis Most Aces";
            worksheet.Cells[1, 79].Value = "Trader Tennis Most Double Faults";
            worksheet.Cells[1, 80].Value = "Other Trader Basketball FT (HDP)";
            worksheet.Cells[1, 81].Value = "Other Trader Basketball 1H (HDP)";
            worksheet.Cells[1, 82].Value = "Other Trader Basketball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 83].Value = "Other Trader Basketball FT (OU/OE)";
            worksheet.Cells[1, 84].Value = "Other Trader Basketball 1H (OU/OE)";
            worksheet.Cells[1, 85].Value = "Other Trader Basketball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 86].Value = "Other Trader Football FT (HDP)";
            worksheet.Cells[1, 87].Value = "Other Trader Football 1H (HDP)";
            worksheet.Cells[1, 88].Value = "Other Trader Football 1Q & 3Q (HDP)";
            worksheet.Cells[1, 89].Value = "Other Trader Football FT (OU)";
            worksheet.Cells[1, 90].Value = "Other Trader Football 1H (OU)";
            worksheet.Cells[1, 91].Value = "Other Trader Football 1Q & 3Q (OU)";
            worksheet.Cells[1, 92].Value = "Other Trader Field Hockey FT (HDP)";
            worksheet.Cells[1, 93].Value = "Other Trader Field Hockey 1H (HDP)";
            worksheet.Cells[1, 94].Value = "Other Trader Field Hockey 1Q (HDP)";
            worksheet.Cells[1, 95].Value = "Other Trader Field Hockey FT (O/U)";
            worksheet.Cells[1, 96].Value = "Other Trader Field Hockey 1H (O/U)";
            worksheet.Cells[1, 97].Value = "Other Trader Field Hockey 1Q (OU)";
            worksheet.Cells[1, 98].Value = "Other Trader Ice Hockey FT (HDP)";
            worksheet.Cells[1, 99].Value = "Other Trader Ice Hockey 1st-2nd Periods (HDP)";
            worksheet.Cells[1, 100].Value = "Other Trader Ice Hockey FT (O/U)";
            worksheet.Cells[1, 101].Value = "Other Trader Ice Hockey 1st-2nd Periods (OU)";
            worksheet.Cells[1, 102].Value = "Other Trader Other Sports Match Winner";
            worksheet.Cells[1, 103].Value = "Other Trader Darts Legs (HDP)";
            worksheet.Cells[1, 104].Value = "Other Trader Darts Most 180s (HDP)";
            worksheet.Cells[1, 105].Value = "Other Trader Pool Match Winner (HDP)";
            worksheet.Cells[1, 106].Value = "Other Trader Pool Racks Winner (HDP)";
            worksheet.Cells[1, 107].Value = "Other Trader Table Tennis Set Winner (HDP)";
            worksheet.Cells[1, 108].Value = "Other Trader Table Tennis Set x Winner (HDP)";
            worksheet.Cells[1, 109].Value = "Other Trader Volleyball Set Winner (HDP/OE)";
            worksheet.Cells[1, 110].Value = "Other Trader Volleyball Sets x Point Winner (HDP)";
            worksheet.Cells[1, 111].Value = "Other Trader Handball FT (HDP)";
            worksheet.Cells[1, 112].Value = "Other Trader Handball 1H (HDP)";
            worksheet.Cells[1, 113].Value = "Other Trader Handball FT (O/U)";
            worksheet.Cells[1, 114].Value = "Other Trader Handball 1H (O/U)";
            worksheet.Cells[1, 115].Value = "Other Trader Netball FT (HDP)";
            worksheet.Cells[1, 116].Value = "Other Trader Netball 1H (HDP)";
            worksheet.Cells[1, 117].Value = "Other Trader Netball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 118].Value = "Other Trader Netball FT (OU/OE)";
            worksheet.Cells[1, 119].Value = "Other Trader Netball 1H (OU/OE)";
            worksheet.Cells[1, 120].Value = "Other Trader Netball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 121].Value = "Other Trader Cricket Match Winner & Innings (OU)";
            worksheet.Cells[1, 122].Value = "Other Trader Cricket Next Over (OU) & Player (OU)";
            worksheet.Cells[1, 123].Value = "Other Trader Cricket FT 1X2";
            worksheet.Cells[1, 124].Value = "Other Trader Snooker Match Winner (HDP/OU)";
            worksheet.Cells[1, 125].Value = "Other Trader Snooker Frames Winner (HDP/OU/OE)";
            worksheet.Cells[1, 126].Value = "Other Trader Snooker First 5 Frames (HDP)";
            worksheet.Cells[1, 127].Value = "Other Trader Snooker First 8 Frames (HDP)";
            worksheet.Cells[1, 128].Value = "Other Trader Snooker First 9 Frames (HDP)";
            worksheet.Cells[1, 129].Value = "Other Trader Baseball FT Run Line (HDP)";
            worksheet.Cells[1, 130].Value = "Other Trader Baseball 1H 1st 5 innings (HDP)";
            worksheet.Cells[1, 131].Value = "Other Trader Baseball FT Total Run (OU)";
            worksheet.Cells[1, 132].Value = "Other Trader Baseball 1H 1st 5 innings (OU)";
            worksheet.Cells[1, 133].Value = "Other Trader Baseball Inning Hits & Runs";
            worksheet.Cells[1, 134].Value = "Other Trader Badminton All Product (Set Winner)";
            worksheet.Cells[1, 135].Value = "Other Trader Badminton All Product (Point Winner)";
            worksheet.Cells[1, 136].Value = "Other Trader Badminton Race to Points (7, 9, 11)";
            worksheet.Cells[1, 137].Value = "Other Trader Badminton Next Point Winner";
            worksheet.Cells[1, 138].Value = "Other Trader Badminton Match Handicap";
            worksheet.Cells[1, 139].Value = "Other Trader Tennis All Product (LS)";
            worksheet.Cells[1, 140].Value = "Other Trader Tennis Next Game";
            worksheet.Cells[1, 141].Value = "Other Trader Tennis Most Aces";
            worksheet.Cells[1, 142].Value = "Other Trader Tennis Most Double Faults";
            worksheet.Cells[1, 143].Value = "Max Bet Basketball FT (HDP)";
            worksheet.Cells[1, 144].Value = "Max Bet Basketball FT (OE)";
            worksheet.Cells[1, 145].Value = "Max Bet Football FT (HDP)";
            worksheet.Cells[1, 146].Value = "Max Bet Football 1H (HDP)";
            worksheet.Cells[1, 147].Value = "Max Bet Football 1Q & 3Q (HDP)";
            worksheet.Cells[1, 148].Value = "Max Bet Football FT (OU)";
            worksheet.Cells[1, 149].Value = "Max Bet Football 1H (OU)";
            worksheet.Cells[1, 150].Value = "Max Bet Football 1Q & 3Q (OU)";
            worksheet.Cells[1, 151].Value = "Max Bet Field Hockey FT (HDP)";
            worksheet.Cells[1, 152].Value = "Max Bet Field Hockey 1H (HDP)";
            worksheet.Cells[1, 153].Value = "Max Bet Field Hockey 1Q (HDP)";
            worksheet.Cells[1, 154].Value = "Max Bet Field Hockey FT (O/U)";
            worksheet.Cells[1, 155].Value = "Max Bet Field Hockey 1H (O/U)";
            worksheet.Cells[1, 156].Value = "Max Bet Field Hockey 1Q (OU)";
            worksheet.Cells[1, 157].Value = "Max Bet Ice Hockey FT (HDP)";
            worksheet.Cells[1, 158].Value = "Max Bet Ice Hockey 1st-2nd Periods (HDP)";
            worksheet.Cells[1, 159].Value = "Max Bet Ice Hockey FT (O/U)";
            worksheet.Cells[1, 160].Value = "Max Bet Ice Hockey 1st-2nd Periods (OU)";
            worksheet.Cells[1, 161].Value = "Max Bet Other Sports Match Winner";
            worksheet.Cells[1, 162].Value = "Max Bet Darts Legs (HDP)";
            worksheet.Cells[1, 163].Value = "Max Bet Darts Most 180s (HDP)";
            worksheet.Cells[1, 164].Value = "Max Bet Pool Match Winner (HDP)";
            worksheet.Cells[1, 165].Value = "Max Bet Pool Racks Winner (HDP)";
            worksheet.Cells[1, 166].Value = "Max Bet Table Tennis Set Winner (HDP)";
            worksheet.Cells[1, 167].Value = "Max Bet Table Tennis Set x Winner (HDP)";
            worksheet.Cells[1, 168].Value = "Max Bet Volleyball Set Winner (HDP/OE)";
            worksheet.Cells[1, 169].Value = "Max Bet Volleyball Sets x Point Winner (HDP)";
            worksheet.Cells[1, 170].Value = "Max Bet Handball FT (HDP)";
            worksheet.Cells[1, 171].Value = "Max Bet Handball 1H (HDP)";
            worksheet.Cells[1, 172].Value = "Max Bet Handball FT (O/U)";
            worksheet.Cells[1, 173].Value = "Max Bet Handball 1H (O/U)";
            worksheet.Cells[1, 174].Value = "Max Bet Netball FT (HDP)";
            worksheet.Cells[1, 175].Value = "Max Bet Netball 1H (HDP)";
            worksheet.Cells[1, 176].Value = "Max Bet Netball 1Q & 3Q (HDP)";
            worksheet.Cells[1, 177].Value = "Max Bet Netball FT (OU/OE)";
            worksheet.Cells[1, 178].Value = "Max Bet Netball 1H (OU/OE)";
            worksheet.Cells[1, 179].Value = "Max Bet Netball 1Q & 3Q (OU/OE)";
            worksheet.Cells[1, 180].Value = "Max Bet Cricket Match Winner & Innings (OU)";
            worksheet.Cells[1, 181].Value = "Trader Cricket Next Over (OU) & Player (OU)";
            worksheet.Cells[1, 182].Value = "Max Bet Cricket FT 1X2";
            worksheet.Cells[1, 183].Value = "Max Bet Snooker Match Winner (HDP/OU)";
            worksheet.Cells[1, 184].Value = "Max Bet Snooker Frames Winner (HDP/OU/OE)";
            worksheet.Cells[1, 185].Value = "Max Bet Snooker First 5 Frames (HDP)";
            worksheet.Cells[1, 186].Value = "Max Bet Snooker First 8 Frames (HDP)";
            worksheet.Cells[1, 187].Value = "Max Bet Snooker First 9 Frames (HDP)";
            worksheet.Cells[1, 188].Value = "Max Bet Baseball FT Run Line (HDP)";
            worksheet.Cells[1, 189].Value = "Max Bet Baseball 1H 1st 5 innings (HDP)";
            worksheet.Cells[1, 190].Value = "Max Bet Baseball FT Total Run (OU)";
            worksheet.Cells[1, 191].Value = "Max Bet Baseball 1H 1st 5 innings (OU)";
            worksheet.Cells[1, 192].Value = "Max Bet Baseball Inning Hits & Runs";
            worksheet.Cells[1, 193].Value = "Max Bet Badminton All Product (Set Winner)";
            worksheet.Cells[1, 194].Value = "Max Bet Badminton All Product (Point Winner)";
            worksheet.Cells[1, 195].Value = "Max Bet Badminton Race to Points (7, 9, 11)";
            worksheet.Cells[1, 196].Value = "Max Bet Badminton Next Point Winner";
            worksheet.Cells[1, 197].Value = "Max Bet Badminton Match Handicap";
            worksheet.Cells[1, 198].Value = "Max Bet Tennis All Product (LS)";
            worksheet.Cells[1, 199].Value = "Max Bet Tennis Next Game";
            worksheet.Cells[1, 200].Value = "Max Bet Tennis Most Aces";
            worksheet.Cells[1, 201].Value = "Max Bet Tennis Most Double Faults";

            worksheet.Cells[2, 1].Value = "Live";
            worksheet.Cells[2, 2].Value = "Soccer";
            worksheet.Cells[2, 3].Value = "12/2/2020  3:30:00 PM";
            worksheet.Cells[2, 4].Value = "CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)";
            worksheet.Cells[2, 5].Value = "Dalian Pro FC (N)";
            worksheet.Cells[2, 6].Value = "Guangzhou R&F";
            worksheet.Cells[2, 7].Value = "Asian 4";
            worksheet.Cells[2, 8].Value = "3HDP";
            worksheet.Cells[2, 9].Value = "15s Accept";
            worksheet.Cells[2, 10].Value = "YES";
            worksheet.Cells[2, 11].Value = "TV/BGTV/Glive53";
            worksheet.Cells[2, 12].Value = "BGtv/RB2tv/BETtv/";
            worksheet.Cells[2, 13].Value = "InPlay";
            worksheet.Cells[2, 14].Value = "Euro40";
            worksheet.Cells[2, 17].Value = "starix1";
            worksheet.Cells[2, 18].Value = "starix2";
            worksheet.Cells[2, 149].Value = "50000";

            worksheet.Cells[3, 1].Value = null;
            worksheet.Cells[3, 2].Value = null;
            worksheet.Cells[3, 3].Value = null;
            worksheet.Cells[3, 4].Value = null;
            worksheet.Cells[3, 5].Value = null;
            worksheet.Cells[3, 6].Value = null;
            worksheet.Cells[3, 7].Value = null;
            worksheet.Cells[3, 8].Value = null;
            worksheet.Cells[3, 9].Value = null;
            worksheet.Cells[3, 10].Value = null;
            worksheet.Cells[3, 11].Value = null;
            worksheet.Cells[3, 12].Value = null;
            worksheet.Cells[3, 13].Value = null;
            worksheet.Cells[3, 14].Value = null;
            worksheet.Cells[3, 17].Value = null;
            worksheet.Cells[3, 18].Value = null;
            worksheet.Cells[3, 149].Value = null;

            worksheet.Cells[4, 1].Value = "Live";
            worksheet.Cells[4, 2].Value = "Soccer";
            worksheet.Cells[4, 3].Value = "12/2/2020  3:30:00 PM";
            worksheet.Cells[4, 4].Value = "CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)";
            worksheet.Cells[4, 5].Value = "Dalian Pro FC (N)";
            worksheet.Cells[4, 6].Value = "Guangzhou R&F";
            worksheet.Cells[4, 7].Value = "Asian 4";
            worksheet.Cells[4, 8].Value = "3HDP";
            worksheet.Cells[4, 9].Value = "15s Accept";
            worksheet.Cells[4, 10].Value = "YES";
            worksheet.Cells[4, 11].Value = "TV/BGTV/Glive53";
            worksheet.Cells[4, 12].Value = "BGtv/RB2tv/BETtv/";
            worksheet.Cells[4, 13].Value = "InPlay";
            worksheet.Cells[4, 14].Value = "Euro40";
            worksheet.Cells[4, 17].Value = "starix1";
            worksheet.Cells[4, 18].Value = "starix2";
            worksheet.Cells[4, 149].Value = 50000;

            worksheet.Cells[4, 2].Value = "Basketball";
            worksheet.Cells[5, 2].Value = "Football";
            worksheet.Cells[6, 2].Value = "Ice Hockey";
            worksheet.Cells[7, 2].Value = "Darts";
            worksheet.Cells[8, 2].Value = "Pool";
            worksheet.Cells[9, 2].Value = "Table Tennis";
            worksheet.Cells[10, 2].Value = "Volleyball";
            worksheet.Cells[11, 2].Value = "Handball";
            worksheet.Cells[12, 2].Value = "Netball";
            worksheet.Cells[13, 2].Value = "Cricket";
            worksheet.Cells[14, 2].Value = "Snooker";
            worksheet.Cells[15, 2].Value = "Baseball";
            worksheet.Cells[16, 2].Value = "Badminton";
            worksheet.Cells[17, 2].Value = "Tennis";
            worksheet.Cells[18, 2].Value = "Other Sports";

            var expected = new AssignmentModelInExcel
            {
                MatchStatus = "Live",
                Sport = "Soccer",
                DateTime = "12/2/2020  3:30:00 PM",
                Home = "Dalian Pro FC (N)",
                Away = "Guangzhou R&F",
                League = "CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)",
                Category = "Asian 4",
                NoHDP = "3HDP",
                MatchOffer = "15s Accept",
                AutoLink = "YES",
                Provider = "TV/BGTV/Glive53",
                OtherProvider = "BGtv/RB2tv/BETtv/",
                Service = "InPlay",
                IPTV = "Euro40",
                WB = string.Empty,
                Remark = string.Empty,
                TraderBasketballFTHDP = "starix1",
                TraderBasketball1HHDP = "starix2",
                TraderBasketball1Q3QHDP = string.Empty,
                TraderBasketballFTOUOE = string.Empty,
                TraderBasketball1HOUOE = string.Empty,
                TraderBasketball1Q3QOUOE = string.Empty,
                TraderFootball1HHDP = string.Empty,
                TraderFootballFTHDP = string.Empty,
                TraderFootball1Q3QHDP = string.Empty,
                TraderFootballFTOU = string.Empty,
                TraderFootball1HOU = string.Empty,
                TraderFootball1Q3QOU = string.Empty,
                TraderFieldHockey1HHDP = string.Empty,
                TraderFieldHockey1QHDP = string.Empty,
                TraderFieldHockeyFTOU = string.Empty,
                TraderFieldHockey1HOU = string.Empty,
                TraderFieldHockey1QOU = string.Empty,
                TraderFieldHockeyFTHDP = string.Empty,
                TraderIceHockey1st2ndPeriodsHDP = string.Empty,
                TraderIceHockeyFTOU = string.Empty,
                TraderIceHockeyFTHDP = string.Empty,
                TraderIceHockey1st2ndPeriodsOU = string.Empty,
                TraderDartsMost180sHDP = string.Empty,
                TraderPoolRacksWinnerHDP = string.Empty,
                TraderTableTennisSetXWinnerHDP = string.Empty,
                TraderVolleyballSetsXPointWinnerHDP = string.Empty,
                TraderHandball1HHDP = string.Empty,
                TraderHandballFTOU = string.Empty,
                TraderHandball1HOU = string.Empty,
                TraderNetballFTHDP = string.Empty,
                TraderNetball1HHDP = string.Empty,
                TraderNetball1Q3QHDP = string.Empty,
                TraderNetballFTOUOE = string.Empty,
                TraderNetball1HOUOE = string.Empty,
                TraderNetball1Q3QOUOE = string.Empty,
                TraderCricketMatchWinnerInningsOU = string.Empty,
                TraderCricketNextOverOUPlayerOU = string.Empty,
                TraderCricketFT1X2 = string.Empty,
                TraderSnookerMatchWinnerHDPOU = string.Empty,
                TraderSnookerFramesWinnerHDPOUOE = string.Empty,
                TraderSnookerFirst5FramesHDP = string.Empty,
                TraderSnookerFirst8FramesHDP = string.Empty,
                TraderSnookerFirst9FramesHDP = string.Empty,
                TraderBaseballFTRunLineHDP = string.Empty,
                TraderBaseball1H1st5inningsHDP = string.Empty,
                TraderBaseballFTTotalRunOU = string.Empty,
                TraderBaseball1H1st5inningsOU = string.Empty,
                TraderBaseballInningHitsRuns = string.Empty,
                TraderBadmintonAllProductSetWinner = string.Empty,
                TraderBadmintonAllProductPointWinner = string.Empty,
                TraderBadmintonRacetoPoints7911 = string.Empty,
                TraderBadmintonNextPointWinner = string.Empty,
                TraderBadmintonMatchHandicap = string.Empty,
                TraderTennisAllProductLS = string.Empty,
                TraderTennisNextGame = string.Empty,
                TraderTennisMostAces = string.Empty,
                TraderTennisMostDoubleFaults = string.Empty,
                TraderOtherSportsMatchWinner = string.Empty,
                TraderDartsLegsHDP = string.Empty,
                TraderHandballFTHDP = string.Empty,
                TraderPoolMatchWinnerHDP = string.Empty,
                TraderTableTennisSetWinnerHDP = string.Empty,
                TraderVolleyballSetWinnerHDPOE = string.Empty,
                OtherTraderBasketballFTHDP = string.Empty,
                OtherTraderBasketball1HHDP = string.Empty,
                OtherTraderBasketball1Q3QHDP = string.Empty,
                OtherTraderBasketballFTOUOE = string.Empty,
                OtherTraderBasketball1HOUOE = string.Empty,
                OtherTraderBasketball1Q3QOUOE = string.Empty,
                OtherTraderFootballFTHDP = string.Empty,
                OtherTraderFootball1HHDP = string.Empty,
                OtherTraderFootball1Q3QHDP = string.Empty,
                OtherTraderFootballFTOU = string.Empty,
                OtherTraderFootball1HOU = string.Empty,
                OtherTraderFootball1Q3QOU = string.Empty,
                OtherTraderFieldHockeyFTHDP = string.Empty,
                OtherTraderFieldHockey1HHDP = string.Empty,
                OtherTraderFieldHockey1QHDP = string.Empty,
                OtherTraderFieldHockeyFTOU = string.Empty,
                OtherTraderFieldHockey1HOU = string.Empty,
                OtherTraderFieldHockey1QOU = string.Empty,
                OtherTraderIceHockeyFTHDP = string.Empty,
                OtherTraderIceHockey1st2ndPeriodsHDP = string.Empty,
                OtherTraderIceHockeyFTOU = string.Empty,
                OtherTraderIceHockey1st2ndPeriodsOU = string.Empty,
                OtherTraderOtherSportsMatchWinner = string.Empty,
                OtherTraderDartsLegsHDP = string.Empty,
                OtherTraderDartsMost180sHDP = string.Empty,
                OtherTraderPoolMatchWinnerHDP = string.Empty,
                OtherTraderPoolRacksWinnerHDP = string.Empty,
                OtherTraderTableTennisSetWinnerHDP = string.Empty,
                OtherTraderTableTennisSetXWinnerHDP = string.Empty,
                OtherTraderVolleyballSetWinnerHDPOE = string.Empty,
                OtherTraderVolleyballSetsXPointWinnerHDP = string.Empty,
                OtherTraderHandballFTHDP = string.Empty,
                OtherTraderHandball1HHDP = string.Empty,
                OtherTraderHandballFTOU = string.Empty,
                OtherTraderHandball1HOU = string.Empty,
                OtherTraderNetballFTHDP = string.Empty,
                OtherTraderNetball1HHDP = string.Empty,
                OtherTraderNetball1Q3QHDP = string.Empty,
                OtherTraderNetballFTOUOE = string.Empty,
                OtherTraderNetball1HOUOE = string.Empty,
                OtherTraderNetball1Q3QOUOE = string.Empty,
                OtherTraderCricketMatchWinnerInningsOU = string.Empty,
                OtherTraderCricketNextOverOUPlayerOU = string.Empty,
                OtherTraderCricketFT1X2 = string.Empty,
                OtherTraderSnookerMatchWinnerHDPOU = string.Empty,
                OtherTraderSnookerFramesWinnerHDPOUOE = string.Empty,
                OtherTraderSnookerFirst5FramesHDP = string.Empty,
                OtherTraderSnookerFirst8FramesHDP = string.Empty,
                OtherTraderSnookerFirst9FramesHDP = string.Empty,
                OtherTraderBaseballFTRunLineHDP = string.Empty,
                OtherTraderBaseball1H1st5inningsHDP = string.Empty,
                OtherTraderBaseballFTTotalRunOU = string.Empty,
                OtherTraderBaseball1H1st5inningsOU = string.Empty,
                OtherTraderBaseballInningHitsRuns = string.Empty,
                OtherTraderBadmintonAllProductSetWinner = string.Empty,
                OtherTraderBadmintonAllProductPointWinner = string.Empty,
                OtherTraderBadmintonRacetoPoints7911 = string.Empty,
                OtherTraderBadmintonNextPointWinner = string.Empty,
                OtherTraderBadmintonMatchHandicap = string.Empty,
                OtherTraderTennisAllProductLS = string.Empty,
                OtherTraderTennisNextGame = string.Empty,
                OtherTraderTennisMostAces = string.Empty,
                OtherTraderTennisMostDoubleFaults = string.Empty,
                MaxBetBasketballFTHDP = null,
                MaxBetBasketballFTOUOE = null,
                MaxBetFootballFTHDP = null,
                MaxBetFootball1HHDP = null,
                MaxBetFootball1Q3QHDP = null,
                MaxBetFootballFTOU = null,
                MaxBetFootball1HOU = 50000,
                MaxBetFootball1Q3QOU = null,
                MaxBetFieldHockeyFTHDP = null,
                MaxBetFieldHockey1HHDP = null,
                MaxBetFieldHockey1QHDP = null,
                MaxBetFieldHockeyFTOU = null,
                MaxBetFieldHockey1HOU = null,
                MaxBetFieldHockey1QOU = null,
                MaxBetIceHockeyFTHDP = null,
                MaxBetIceHockey1st2ndPeriodsHDP = null,
                MaxBetIceHockeyFTOU = null,
                MaxBetIceHockey1st2ndPeriodsOU = null,
                MaxBetOtherSportsMatchWinner = null,
                MaxBetDartsLegsHDP = null,
                MaxBetDartsMost180sHDP = null,
                MaxBetPoolMatchWinnerHDP = null,
                MaxBetPoolRacksWinnerHDP = null,
                MaxBetTableTennisSetWinnerHDP = null,
                MaxBetTableTennisSetXWinnerHDP = null,
                MaxBetVolleyballSetWinnerHDPOE = null,
                MaxBetVolleyballSetsXPointWinnerHDP = null,
                MaxBetHandballFTHDP = null,
                MaxBetHandball1HHDP = null,
                MaxBetHandballFTOU = null,
                MaxBetHandball1HOU = null,
                MaxBetNetballFTHDP = null,
                MaxBetNetball1HHDP = null,
                MaxBetNetball1Q3QHDP = null,
                MaxBetNetballFTOUOE = null,
                MaxBetNetball1HOUOE = null,
                MaxBetNetball1Q3QOUOE = null,
                MaxBetCricketMatchWinnerInningsOU = null,
                MaxBetCricketNextOverOUPlayerOU = null,
                MaxBetCricketFT1X2 = null,
                MaxBetSnookerMatchWinnerHDPOU = null,
                MaxBetSnookerFramesWinnerHDPOUOE = null,
                MaxBetSnookerFirst5FramesHDP = null,
                MaxBetSnookerFirst8FramesHDP = null,
                MaxBetSnookerFirst9FramesHDP = null,
                MaxBetBaseballFTRunLineHDP = null,
                MaxBetBaseball1H1st5inningsHDP = null,
                MaxBetBaseballFTTotalRunOU = null,
                MaxBetBaseball1H1st5inningsOU = null,
                MaxBetBaseballInningHitsRuns = null,
                MaxBetBadmintonAllProductSetWinner = null,
                MaxBetBadmintonAllProductPointWinner = null,
                MaxBetBadmintonRacetoPoints7911 = null,
                MaxBetBadmintonNextPointWinner = null,
                MaxBetBadmintonMatchHandicap = null,
                MaxBetTennisAllProductLS = null,
                MaxBetTennisNextGame = null,
                MaxBetTennisMostAces = null,
                MaxBetTennisMostDoubleFaults = null
            };

            var actual = worksheet.ReadAllAssignments(2).ToList();
            DeepAssert.Equal(expected, actual[0]);
        }
    }
}
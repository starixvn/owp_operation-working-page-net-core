using System.Drawing;

namespace OWP.OvertimeReport.Constants
{
    public static class ExcelConst
    {
        public const int DayOfWeekRowIndex = 1;
        public const int HeaderRowIndex = 2;
        public const int StartDataRowIndex = 3;
        public const int StartDataColIndex = 4;
        public const string YearSheetName = "Summary";
        public const string YearTemplateSheetName = "Template";
    }

    public static class UserColConst
    {
        public const int NoColIndex = 1;
        public const int LocationColIndex = 2;
        public const int TraderColIndex = 3;
        public const string NoColName = "No";
        public const string LocationColName = "Location";
        public const string TraderColName = "Trader";
        public const string TotalColName = "Total";
    }

    public static class ColorConst
    {
        public static Color Weekend = Color.LightBlue;
        public static Color Header = Color.DarkGray;
        public static Color TotalBackground = Color.NavajoWhite;
        public static Color OffDay = Color.LimeGreen;
        public static Color HeaderBorder = Color.Gray;
        public static Color TotalText = Color.DarkRed;
    }

    public static class YearConst
    {
        public const int StartDataRowIndex = 2;
        public const int TotalColIndex = 16;
    }
}
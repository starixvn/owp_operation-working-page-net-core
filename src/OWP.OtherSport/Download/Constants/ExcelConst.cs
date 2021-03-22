using OWP.OtherSport.Submit.Contants;
using System;
using System.Collections.Generic;

namespace OWP.OtherSport.Download.Constants
{
    public static class ExcelConst
    {
        public const string TemplateName = "Template";

        public const string SheetName = "Report";

        public const int StartRowIndex = 3;

        public static readonly Dictionary<Tuple<string, string>, int> TraderColumn = new Dictionary<Tuple<string, string>, int>
        {
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTHDP), 19 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstHHDP), 20 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QHDP), 21 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTOUOE), 22 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstHOUOE), 23 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QOUOE), 24 }
        };

        public static readonly Dictionary<Tuple<string, string>, int> OtherTraderColumn = new Dictionary<Tuple<string, string>, int>();

        public static readonly Dictionary<Tuple<string, string>, int> MaxbetColumn = new Dictionary<Tuple<string, string>, int>();
    }

    public static class MatchInfoConst
    {
        public const int MatchNoColIndex = 1;
        public const int MatchStatusColIndex = 2;
        public const int SportColIndex = 3;
        public const int DateTimeColIndex = 4;
        public const int LeagueColIndex = 5;
        public const int HomeColIndex = 6;
        public const int AwayColIndex = 7;
        public const int CategoryColIndex = 8;
        public const int NoHDPColIndex = 9;
        public const int MatchOfferColIndex = 10;
        public const int AutoLinkColIndex = 11;
        public const int ProviderColIndex = 12;
        public const int OtherProviderColIndex = 13;
        public const int ServiceColIndex = 14;
        public const int IPTVColIndex = 15;
        public const int WBColIndex = 16;
        public const int ReporterColIndex = 17;
        public const int RemarkColIndex = 18;
    }
}

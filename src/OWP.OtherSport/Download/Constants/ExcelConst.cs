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
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTHDP),               19},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstHHDP),           20},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QHDP),         21},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTOUOE),              22},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstHOUOE),          23},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QOUOE),        24},
            { new Tuple<string, string>(Sport.Football, Bettype.FTHDP),                 25},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHHDP),             26},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QHDP),           27},
            { new Tuple<string, string>(Sport.Football, Bettype.FTOU),                  28},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHOU),              29},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QOU),            30},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTHDP),              31},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHHDP),          32},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQHDP),          33},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTOsU),              34},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHOsU),          35},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQOU),           36},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTHDP),                37},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsHDP),   38},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTOsU),                39},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsOU),    40},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.MatchWinner),        41},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FTHDP),              42},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FirstHHDP),          43},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FTOsU),              44},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FirstHOsU),          45},
            { new Tuple<string, string>(Sport.Darts, Bettype.LegsHDP),                  46},
            { new Tuple<string, string>(Sport.Darts, Bettype.Most180sHDP),              47},
            { new Tuple<string, string>(Sport.Pool, Bettype.MatchWinnerHDP),            48},
            { new Tuple<string, string>(Sport.Pool, Bettype.RacksWinnerHDP),            49},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetWinnerHDP),       50},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetXWinnerHDP),      51},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetWinnerHDPOE),      52},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetsXPointWinnerHDP), 53},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTHDP),                 54},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHHDP),             55},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTOsU),                 56},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHOsU),             57},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTHDP),                  58},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHHDP),              59},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QHDP),            60},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTOUOE),                 61},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHOUOE),             62},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QOUOE),           63},
            { new Tuple<string, string>(Sport.Cricket, Bettype.MatchWinnerInningsOU),   64},
            { new Tuple<string, string>(Sport.Cricket, Bettype.NextOverOUPlayerOU),     65},
            { new Tuple<string, string>(Sport.Cricket, Bettype.FT1X2),                  66},
            { new Tuple<string, string>(Sport.Snooker, Bettype.MatchWinnerHDPOU),       67},
            { new Tuple<string, string>(Sport.Snooker, Bettype.FramesWinnerHDPOUOE),    68},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First5FramesHDP),        69},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First8FramesHDP),        70},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First9FramesHDP),        71},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTRunLineHDP),          72},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsHDP),  73},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTTotalRunOU),          74},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsOU),   75},
            { new Tuple<string, string>(Sport.Baseball, Bettype.InningHitsAndRuns),     76},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductSetWinner),  77},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductPointWinner),78},
            { new Tuple<string, string>(Sport.Badminton, Bettype.RacetoPoints7911),     79},
            { new Tuple<string, string>(Sport.Badminton, Bettype.NextPointWinner),      80},
            { new Tuple<string, string>(Sport.Badminton, Bettype.MatchHandicap),        81},
            { new Tuple<string, string>(Sport.Tennis, Bettype.AllProductLS),            82},
            { new Tuple<string, string>(Sport.Tennis, Bettype.NextGame),                83},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostAces),                84},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostDoubleFaults),        85},
        };

        public static readonly Dictionary<Tuple<string, string>, int> OtherTraderColumn = new Dictionary<Tuple<string, string>, int>
        {
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTHDP),               86},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstHHDP),           87},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QHDP),         88},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTOUOE),              89},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstHOUOE),          90},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QOUOE),        91},
            { new Tuple<string, string>(Sport.Football, Bettype.FTHDP),                 92},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHHDP),             93},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QHDP),           94},
            { new Tuple<string, string>(Sport.Football, Bettype.FTOU),                  95},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHOU),              96},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QOU),            97},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTHDP),              98},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHHDP),          99},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQHDP),          100},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTOsU),              101},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHOsU),          102},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQOU),           103},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTHDP),                104},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsHDP),   105},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTOsU),                106},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsOU),    107},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.MatchWinner),        108},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FTHDP),              109},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FirstHHDP),          110},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FTOsU),              111},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FirstHOsU),          112},
            { new Tuple<string, string>(Sport.Darts, Bettype.LegsHDP),                  113},
            { new Tuple<string, string>(Sport.Darts, Bettype.Most180sHDP),              114},
            { new Tuple<string, string>(Sport.Pool, Bettype.MatchWinnerHDP),            115},
            { new Tuple<string, string>(Sport.Pool, Bettype.RacksWinnerHDP),            116},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetWinnerHDP),       117},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetXWinnerHDP),      118},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetWinnerHDPOE),      119},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetsXPointWinnerHDP), 120},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTHDP),                 121},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHHDP),             122},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTOsU),                 123},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHOsU),             124},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTHDP),                  125},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHHDP),              126},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QHDP),            127},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTOUOE),                 128},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHOUOE),             129},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QOUOE),           130},
            { new Tuple<string, string>(Sport.Cricket, Bettype.MatchWinnerInningsOU),   131},
            { new Tuple<string, string>(Sport.Cricket, Bettype.NextOverOUPlayerOU),     132},
            { new Tuple<string, string>(Sport.Cricket, Bettype.FT1X2),                  133},
            { new Tuple<string, string>(Sport.Snooker, Bettype.MatchWinnerHDPOU),       134},
            { new Tuple<string, string>(Sport.Snooker, Bettype.FramesWinnerHDPOUOE),    135},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First5FramesHDP),        136},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First8FramesHDP),        137},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First9FramesHDP),        138},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTRunLineHDP),          139},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsHDP),  140},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTTotalRunOU),          141},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsOU),   142},
            { new Tuple<string, string>(Sport.Baseball, Bettype.InningHitsAndRuns),     143},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductSetWinner),  144},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductPointWinner),145},
            { new Tuple<string, string>(Sport.Badminton, Bettype.RacetoPoints7911),     146},
            { new Tuple<string, string>(Sport.Badminton, Bettype.NextPointWinner),      147},
            { new Tuple<string, string>(Sport.Badminton, Bettype.MatchHandicap),        148},
            { new Tuple<string, string>(Sport.Tennis, Bettype.AllProductLS),            149},
            { new Tuple<string, string>(Sport.Tennis, Bettype.NextGame),                150},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostAces),                151},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostDoubleFaults),        152}
        };

        public static readonly Dictionary<Tuple<string, string>, int> MaxbetColumn = new Dictionary<Tuple<string, string>, int>
        {
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTHDP),               153},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTOUOE),              154},
            { new Tuple<string, string>(Sport.Football, Bettype.FTHDP),                 155},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHHDP),             156},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QHDP),           157},
            { new Tuple<string, string>(Sport.Football, Bettype.FTOU),                  158},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHOU),              159},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QOU),            160},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTHDP),              161},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHHDP),          162},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQHDP),          163},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTOsU),              164},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHOsU),          165},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQOU),           166},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTHDP),                167},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsHDP),   168},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTOsU),                169},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsOU),    170},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.MatchWinner),        171},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FTHDP),              172},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FirstHHDP),          173},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FTOsU),              174},
            { new Tuple<string, string>(Sport.OtherSports, Bettype.FirstHOsU),          175},
            { new Tuple<string, string>(Sport.Darts, Bettype.LegsHDP),                  176},
            { new Tuple<string, string>(Sport.Darts, Bettype.Most180sHDP),              177},
            { new Tuple<string, string>(Sport.Pool, Bettype.MatchWinnerHDP),            178},
            { new Tuple<string, string>(Sport.Pool, Bettype.RacksWinnerHDP),            179},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetWinnerHDP),       180},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetXWinnerHDP),      181},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetWinnerHDPOE),      182},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetsXPointWinnerHDP), 183},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTHDP),                 184},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHHDP),             185},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTOsU),                 186},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHOsU),             187},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTHDP),                  188},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHHDP),              189},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QHDP),            190},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTOUOE),                 191},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHOUOE),             192},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QOUOE),           193},
            { new Tuple<string, string>(Sport.Cricket, Bettype.MatchWinnerInningsOU),   194},
            { new Tuple<string, string>(Sport.Cricket, Bettype.NextOverOUPlayerOU),     195},
            { new Tuple<string, string>(Sport.Cricket, Bettype.FT1X2),                  196},
            { new Tuple<string, string>(Sport.Snooker, Bettype.MatchWinnerHDPOU),       197},
            { new Tuple<string, string>(Sport.Snooker, Bettype.FramesWinnerHDPOUOE),    198},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First5FramesHDP),        199},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First8FramesHDP),        200},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First9FramesHDP),        201},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTRunLineHDP),          202},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsHDP),  203},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTTotalRunOU),          204},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsOU),   205},
            { new Tuple<string, string>(Sport.Baseball, Bettype.InningHitsAndRuns),     206},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductSetWinner),  207},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductPointWinner),208},
            { new Tuple<string, string>(Sport.Badminton, Bettype.RacetoPoints7911),     209},
            { new Tuple<string, string>(Sport.Badminton, Bettype.NextPointWinner),      210},
            { new Tuple<string, string>(Sport.Badminton, Bettype.MatchHandicap),        211},
            { new Tuple<string, string>(Sport.Tennis, Bettype.AllProductLS),            212},
            { new Tuple<string, string>(Sport.Tennis, Bettype.NextGame),                213},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostAces),                214},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostDoubleFaults),        215}
        };
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

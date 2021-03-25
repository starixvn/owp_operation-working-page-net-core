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
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QOUOE), 24 },

            { new Tuple<string, string>(Sport.Football, Bettype.FTHDP), 25},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHHDP), 26},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QHDP), 27},
            { new Tuple<string, string>(Sport.Football, Bettype.FTOU), 28},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHOU), 29},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QOU), 30},

            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTHDP), 31},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHHDP), 32},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQHDP), 33},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTOU), 34},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHOU), 35},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQOU), 36},

            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTHDP), 37},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsHDP), 38},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTOU), 39},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsOU), 40},

            { new Tuple<string, string>(Sport.OtherSports, Bettype.MatchWinner), 41},

            { new Tuple<string, string>(Sport.Darts, Bettype.LegsHDP), 42},
            { new Tuple<string, string>(Sport.Darts, Bettype.Most180sHDP), 43},

            { new Tuple<string, string>(Sport.Pool, Bettype.MatchWinnerHDP), 44},
            { new Tuple<string, string>(Sport.Pool, Bettype.RacksWinnerHDP), 45},

            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetWinnerHDP), 46},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetXWinnerHDP), 47},

            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetWinnerHDPOE), 48},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetsXPointWinnerHDP), 49},

            { new Tuple<string, string>(Sport.Handball, Bettype.FTHDP), 50},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHHDP), 51},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTOU), 52},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHOU), 53},

            { new Tuple<string, string>(Sport.Netball, Bettype.FTHDP), 54},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHHDP), 55},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QHDP), 56},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTOUOE), 57},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHOUOE), 58},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QOUOE), 59},

            { new Tuple<string, string>(Sport.Cricket, Bettype.MatchWinnerInningsOU), 60},
            { new Tuple<string, string>(Sport.Cricket, Bettype.NextOverOUPlayerOU), 61},
            { new Tuple<string, string>(Sport.Cricket, Bettype.FT1X2), 62},

            { new Tuple<string, string>(Sport.Snooker, Bettype.MatchWinnerHDPOU), 63},
            { new Tuple<string, string>(Sport.Snooker, Bettype.FramesWinnerHDPOUOE), 64},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First5FramesHDP), 65},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First8FramesHDP), 66},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First9FramesHDP), 67},

            { new Tuple<string, string>(Sport.Baseball, Bettype.FTRunLineHDP), 68},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsHDP), 69},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTTotalRunOU), 70},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsOU), 71},
            { new Tuple<string, string>(Sport.Baseball, Bettype.InningHitsAndRuns), 72},

            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductSetWinner), 73},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductPointWinner), 74},
            { new Tuple<string, string>(Sport.Badminton, Bettype.RacetoPoints7911), 75},
            { new Tuple<string, string>(Sport.Badminton, Bettype.NextPointWinner), 76},
            { new Tuple<string, string>(Sport.Badminton, Bettype.MatchHandicap), 77},

            { new Tuple<string, string>(Sport.Tennis, Bettype.AllProductLS), 78},
            { new Tuple<string, string>(Sport.Tennis, Bettype.NextGame), 79},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostAces), 80},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostDoubleFaults), 81},
        };

        public static readonly Dictionary<Tuple<string, string>, int> OtherTraderColumn = new Dictionary<Tuple<string, string>, int>
        {
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTHDP), 82 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstHHDP), 83 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QHDP), 84 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTOUOE), 85 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstHOUOE), 86 },
            { new Tuple<string, string>(Sport.Basketball, Bettype.FirstQ3QOUOE), 87 },

            { new Tuple<string, string>(Sport.Football, Bettype.FTHDP), 88},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHHDP), 89},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QHDP), 90},
            { new Tuple<string, string>(Sport.Football, Bettype.FTOU), 91},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHOU), 92},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QOU), 93},

            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTHDP), 94},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHHDP), 95},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQHDP), 96},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTOU), 97},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHOU), 98},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQOU), 99},

            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTHDP), 100},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsHDP), 101},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTOU), 102},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsOU), 103},

            { new Tuple<string, string>(Sport.OtherSports, Bettype.MatchWinner), 104},

            { new Tuple<string, string>(Sport.Darts, Bettype.LegsHDP), 105},
            { new Tuple<string, string>(Sport.Darts, Bettype.Most180sHDP), 106},

            { new Tuple<string, string>(Sport.Pool, Bettype.MatchWinnerHDP), 107},
            { new Tuple<string, string>(Sport.Pool, Bettype.RacksWinnerHDP), 108},

            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetWinnerHDP), 109},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetXWinnerHDP), 110},

            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetWinnerHDPOE), 111},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetsXPointWinnerHDP), 112},

            { new Tuple<string, string>(Sport.Handball, Bettype.FTHDP), 113},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHHDP), 114},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTOU), 115},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHOU), 116},

            { new Tuple<string, string>(Sport.Netball, Bettype.FTHDP), 117},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHHDP), 118},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QHDP), 119},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTOUOE), 120},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHOUOE), 121},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QOUOE), 122},

            { new Tuple<string, string>(Sport.Cricket, Bettype.MatchWinnerInningsOU), 123},
            { new Tuple<string, string>(Sport.Cricket, Bettype.NextOverOUPlayerOU), 124},
            { new Tuple<string, string>(Sport.Cricket, Bettype.FT1X2), 125},

            { new Tuple<string, string>(Sport.Snooker, Bettype.MatchWinnerHDPOU), 126},
            { new Tuple<string, string>(Sport.Snooker, Bettype.FramesWinnerHDPOUOE), 127},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First5FramesHDP), 128},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First8FramesHDP), 129},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First9FramesHDP), 130},

            { new Tuple<string, string>(Sport.Baseball, Bettype.FTRunLineHDP),  131},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsHDP),  132},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTTotalRunOU),  133},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsOU),  134},
            { new Tuple<string, string>(Sport.Baseball, Bettype.InningHitsAndRuns),  135},

            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductSetWinner),  136},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductPointWinner),  137},
            { new Tuple<string, string>(Sport.Badminton, Bettype.RacetoPoints7911),  138},
            { new Tuple<string, string>(Sport.Badminton, Bettype.NextPointWinner),  139},
            { new Tuple<string, string>(Sport.Badminton, Bettype.MatchHandicap), 140},

            { new Tuple<string, string>(Sport.Tennis, Bettype.AllProductLS), 141},
            { new Tuple<string, string>(Sport.Tennis, Bettype.NextGame), 142},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostAces), 143},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostDoubleFaults), 144},
        };

        public static readonly Dictionary<Tuple<string, string>, int> MaxbetColumn = new Dictionary<Tuple<string, string>, int>
        {
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTHDP), 145},
            { new Tuple<string, string>(Sport.Basketball, Bettype.FTOUOE), 146},

            { new Tuple<string, string>(Sport.Football, Bettype.FTHDP), 147},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHHDP), 148},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QHDP), 149},
            { new Tuple<string, string>(Sport.Football, Bettype.FTOU), 150},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstHOU), 151},
            { new Tuple<string, string>(Sport.Football, Bettype.FirstQ3QOU), 152},

            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTHDP), 153},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHHDP), 154},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQHDP), 155},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FTOU), 156},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstHOU), 157},
            { new Tuple<string, string>(Sport.FieldHockey, Bettype.FirstQOU), 158},

            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTHDP), 159},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsHDP), 160},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.FTOU), 161},
            { new Tuple<string, string>(Sport.IceHockey, Bettype.First2ndPeriodsOU), 162},

            { new Tuple<string, string>(Sport.OtherSports, Bettype.MatchWinner), 163},

            { new Tuple<string, string>(Sport.Darts, Bettype.LegsHDP), 164},
            { new Tuple<string, string>(Sport.Darts, Bettype.Most180sHDP), 165},

            { new Tuple<string, string>(Sport.Pool, Bettype.MatchWinnerHDP), 166},
            { new Tuple<string, string>(Sport.Pool, Bettype.RacksWinnerHDP), 167},

            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetWinnerHDP), 168},
            { new Tuple<string, string>(Sport.TableTennis, Bettype.SetXWinnerHDP), 169},

            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetWinnerHDPOE), 170},
            { new Tuple<string, string>(Sport.Volleyball, Bettype.SetsXPointWinnerHDP), 171},

            { new Tuple<string, string>(Sport.Handball, Bettype.FTHDP), 172},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHHDP), 173},
            { new Tuple<string, string>(Sport.Handball, Bettype.FTOU), 174},
            { new Tuple<string, string>(Sport.Handball, Bettype.FirstHOU), 175},

            { new Tuple<string, string>(Sport.Netball, Bettype.FTHDP), 176},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHHDP), 177},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QHDP), 178},
            { new Tuple<string, string>(Sport.Netball, Bettype.FTOUOE), 179},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstHOUOE), 180},
            { new Tuple<string, string>(Sport.Netball, Bettype.FirstQ3QOUOE), 181},

            { new Tuple<string, string>(Sport.Cricket, Bettype.MatchWinnerInningsOU), 182},
            { new Tuple<string, string>(Sport.Cricket, Bettype.NextOverOUPlayerOU), 183},
            { new Tuple<string, string>(Sport.Cricket, Bettype.FT1X2), 184},

            { new Tuple<string, string>(Sport.Snooker, Bettype.MatchWinnerHDPOU), 185},
            { new Tuple<string, string>(Sport.Snooker, Bettype.FramesWinnerHDPOUOE), 186},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First5FramesHDP), 187},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First8FramesHDP), 188},
            { new Tuple<string, string>(Sport.Snooker, Bettype.First9FramesHDP), 189},

            { new Tuple<string, string>(Sport.Baseball, Bettype.FTRunLineHDP), 190},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsHDP), 191},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FTTotalRunOU), 192},
            { new Tuple<string, string>(Sport.Baseball, Bettype.FirstH1st5inningsOU), 193},
            { new Tuple<string, string>(Sport.Baseball, Bettype.InningHitsAndRuns), 194},

            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductSetWinner), 195},
            { new Tuple<string, string>(Sport.Badminton, Bettype.AllProductPointWinner), 196},
            { new Tuple<string, string>(Sport.Badminton, Bettype.RacetoPoints7911), 197},
            { new Tuple<string, string>(Sport.Badminton, Bettype.NextPointWinner), 198},
            { new Tuple<string, string>(Sport.Badminton, Bettype.MatchHandicap), 199},

            { new Tuple<string, string>(Sport.Tennis, Bettype.AllProductLS), 200},
            { new Tuple<string, string>(Sport.Tennis, Bettype.NextGame), 201},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostAces), 202},
            { new Tuple<string, string>(Sport.Tennis, Bettype.MostDoubleFaults), 203},
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

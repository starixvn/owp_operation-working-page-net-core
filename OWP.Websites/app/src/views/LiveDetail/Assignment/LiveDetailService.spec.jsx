import LiveDetailService from './LiveDetailService';

describe('LiveDetailService', () => {
  it('getAll should work on data collection 1', () => {
    const matchSettings = [
      {
        OWPMatchId: 279047,
        Bettype: 1,
        MinorBettypeId: 0,
        DisplayName: 'HDP',
        Spread: 0.0012,
        Maxbet: 10000.0,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279047,
        Bettype: 3,
        MinorBettypeId: 0,
        DisplayName: 'O/U',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279047,
        Bettype: 5,
        MinorBettypeId: 0,
        DisplayName: 'FT.1X2',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279047,
        Bettype: 7,
        MinorBettypeId: 0,
        DisplayName: '1H HDP',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279047,
        Bettype: 8,
        MinorBettypeId: 0,
        DisplayName: '1H O/U',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279047,
        Bettype: 15,
        MinorBettypeId: 0,
        DisplayName: '1H 1X2',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279048,
        Bettype: 1,
        MinorBettypeId: -1,
        DisplayName: 'SPECIFIC 15 MINS',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279049,
        Bettype: 1,
        MinorBettypeId: -88,
        DisplayName: 'Fast Market',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279050,
        Bettype: 1,
        MinorBettypeId: -89,
        DisplayName: 'All In One',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279054,
        Bettype: 1,
        MinorBettypeId: -2,
        DisplayName: 'SPECIFIC 10 MINS',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279055,
        Bettype: 1,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (HDP)',
        Spread: 0.0012,
        Maxbet: 10000.0,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279055,
        Bettype: 3,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279055,
        Bettype: 5,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279055,
        Bettype: 7,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (1H HDP)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279055,
        Bettype: 8,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (1H OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279055,
        Bettype: 15,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (1H 1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279056,
        Bettype: 1,
        MinorBettypeId: -5,
        DisplayName: 'SPECIFIC 15 MINS NUMBER OF CORNERS ',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279057,
        Bettype: 1,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (HDP)',
        Spread: 0.0012,
        Maxbet: 10000.0,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279057,
        Bettype: 3,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279057,
        Bettype: 5,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279057,
        Bettype: 7,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (1H HDP)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279057,
        Bettype: 8,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (1H OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279057,
        Bettype: 15,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (1H 1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279058,
        Bettype: 1,
        MinorBettypeId: -8,
        DisplayName: 'SPECIFIC 15 MINS TOTAL BOOKINGS',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279063,
        Bettype: 1,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (HDP)',
        Spread: 0.0012,
        Maxbet: 10000.0,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279063,
        Bettype: 3,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279063,
        Bettype: 5,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279063,
        Bettype: 7,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (1H HDP)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279063,
        Bettype: 8,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (1H OU)',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279063,
        Bettype: 15,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (1H 1X2)',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      }
    ];

    const assignments = [
      {
        AssignmentId: 211959,
        OWPMatchId: 279047,
        Bettype: -1002,
        MinorBettypeId: 0,
        AssignmentType: 1,
        DisplayName: 'LA',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: null
      },
      {
        AssignmentId: 212077,
        OWPMatchId: 279047,
        Bettype: -1002,
        MinorBettypeId: 0,
        AssignmentType: 2,
        DisplayName: 'LA',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 211957,
        OWPMatchId: 279047,
        Bettype: 1,
        MinorBettypeId: 0,
        AssignmentType: 1,
        DisplayName: 'HDP',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212064,
        OWPMatchId: 279047,
        Bettype: 1,
        MinorBettypeId: 0,
        AssignmentType: 2,
        DisplayName: 'HDP',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 211958,
        OWPMatchId: 279047,
        Bettype: 3,
        MinorBettypeId: 0,
        AssignmentType: 1,
        DisplayName: 'O/U',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212072,
        OWPMatchId: 279047,
        Bettype: 3,
        MinorBettypeId: 0,
        AssignmentType: 2,
        DisplayName: 'O/U',
        UserId: 0,
        Username: null,
        AssignmentNote: 'test'
      },
      {
        AssignmentId: 211974,
        OWPMatchId: 279047,
        Bettype: 5,
        MinorBettypeId: 0,
        AssignmentType: 1,
        DisplayName: 'FT.1X2',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212073,
        OWPMatchId: 279047,
        Bettype: 5,
        MinorBettypeId: 0,
        AssignmentType: 2,
        DisplayName: 'FT.1X2',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 211972,
        OWPMatchId: 279047,
        Bettype: 7,
        MinorBettypeId: 0,
        AssignmentType: 1,
        DisplayName: '1H HDP',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212074,
        OWPMatchId: 279047,
        Bettype: 7,
        MinorBettypeId: 0,
        AssignmentType: 2,
        DisplayName: '1H HDP',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 211973,
        OWPMatchId: 279047,
        Bettype: 8,
        MinorBettypeId: 0,
        AssignmentType: 1,
        DisplayName: '1H O/U',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212075,
        OWPMatchId: 279047,
        Bettype: 8,
        MinorBettypeId: 0,
        AssignmentType: 2,
        DisplayName: '1H O/U',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 211975,
        OWPMatchId: 279047,
        Bettype: 15,
        MinorBettypeId: 0,
        AssignmentType: 1,
        DisplayName: '1H 1X2',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212076,
        OWPMatchId: 279047,
        Bettype: 15,
        MinorBettypeId: 0,
        AssignmentType: 2,
        DisplayName: '1H 1X2',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 211960,
        OWPMatchId: 279048,
        Bettype: 1,
        MinorBettypeId: -1,
        AssignmentType: 1,
        DisplayName: 'SPECIFIC 15 MINS',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: null
      },
      {
        AssignmentId: 212065,
        OWPMatchId: 279048,
        Bettype: 1,
        MinorBettypeId: -1,
        AssignmentType: 2,
        DisplayName: 'SPECIFIC 15 MINS',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 211965,
        OWPMatchId: 279049,
        Bettype: 1,
        MinorBettypeId: -88,
        AssignmentType: 1,
        DisplayName: 'Fast Market',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: null
      },
      {
        AssignmentId: 211966,
        OWPMatchId: 279050,
        Bettype: 1,
        MinorBettypeId: -89,
        AssignmentType: 1,
        DisplayName: 'All In One',
        UserId: 862400,
        Username: 'starix2',
        AssignmentNote: null
      },
      {
        AssignmentId: 211998,
        OWPMatchId: 279054,
        Bettype: 1,
        MinorBettypeId: -2,
        AssignmentType: 1,
        DisplayName: 'SPECIFIC 10 MINS',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: null
      },
      {
        AssignmentId: 212066,
        OWPMatchId: 279054,
        Bettype: 1,
        MinorBettypeId: -2,
        AssignmentType: 2,
        DisplayName: 'SPECIFIC 10 MINS',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279055,
        Bettype: -1002,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (LA)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279055,
        Bettype: 3,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279055,
        Bettype: 5,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279055,
        Bettype: 7,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (1H HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279055,
        Bettype: 8,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (1H OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279055,
        Bettype: 15,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (1H 1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 212014,
        OWPMatchId: 279055,
        Bettype: 1,
        MinorBettypeId: -3,
        AssignmentType: 1,
        DisplayName: 'No.of Corners (HDP)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212067,
        OWPMatchId: 279055,
        Bettype: 1,
        MinorBettypeId: -3,
        AssignmentType: 2,
        DisplayName: 'No.of Corners (HDP)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 212015,
        OWPMatchId: 279056,
        Bettype: 1,
        MinorBettypeId: -5,
        AssignmentType: 1,
        DisplayName: 'SPECIFIC 15 MINS NUMBER OF CORNERS ',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: null
      },
      {
        AssignmentId: 212068,
        OWPMatchId: 279056,
        Bettype: 1,
        MinorBettypeId: -5,
        AssignmentType: 2,
        DisplayName: 'SPECIFIC 15 MINS NUMBER OF CORNERS ',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279057,
        Bettype: -1002,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (LA)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279057,
        Bettype: 3,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279057,
        Bettype: 5,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279057,
        Bettype: 7,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (1H HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279057,
        Bettype: 8,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (1H OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279057,
        Bettype: 15,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (1H 1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 212028,
        OWPMatchId: 279057,
        Bettype: 1,
        MinorBettypeId: -6,
        AssignmentType: 1,
        DisplayName: 'Total Booking (HDP)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212069,
        OWPMatchId: 279057,
        Bettype: 1,
        MinorBettypeId: -6,
        AssignmentType: 2,
        DisplayName: 'Total Booking (HDP)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 212029,
        OWPMatchId: 279058,
        Bettype: 1,
        MinorBettypeId: -8,
        AssignmentType: 1,
        DisplayName: 'SPECIFIC 15 MINS TOTAL BOOKINGS',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: null
      },
      {
        AssignmentId: 212070,
        OWPMatchId: 279058,
        Bettype: 1,
        MinorBettypeId: -8,
        AssignmentType: 2,
        DisplayName: 'SPECIFIC 15 MINS TOTAL BOOKINGS',
        UserId: 360418,
        Username: 'starix1',
        AssignmentNote: '#<user> test'
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279063,
        Bettype: -1002,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (LA)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279063,
        Bettype: 3,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279063,
        Bettype: 5,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279063,
        Bettype: 7,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (1H HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279063,
        Bettype: 8,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (1H OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279063,
        Bettype: 15,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (1H 1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 212063,
        OWPMatchId: 279063,
        Bettype: 1,
        MinorBettypeId: -96,
        AssignmentType: 1,
        DisplayName: '1st Penalty Score (HDP)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 212071,
        OWPMatchId: 279063,
        Bettype: 1,
        MinorBettypeId: -96,
        AssignmentType: 2,
        DisplayName: '1st Penalty Score (HDP)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: '#<user> test'
      }
    ];

    const result = LiveDetailService.getAll(matchSettings, assignments);

    const expectedResult = [
      {
        Group: 'Major',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: 0,
            Name: 'HDP',
            OWPMatchId: 279047,
            MaxBetFT: {
              MaxBet: '10K',
              Spread: '(0.12%)',

            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null,

            },
            FT: {
              main: {
                AssignmentId: 211957,
                OWPMatchId: 279047,
                Bettype: 1,
                MinorBettypeId: 0,
                AssignmentType: 1,
                DisplayName: 'HDP',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212064,
                  OWPMatchId: 279047,
                  Bettype: 1,
                  MinorBettypeId: 0,
                  AssignmentType: 2,
                  DisplayName: 'HDP',
                  UserId: 8,
                  Username: 'superman',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: 211972,
                OWPMatchId: 279047,
                Bettype: 7,
                MinorBettypeId: 0,
                AssignmentType: 1,
                DisplayName: '1H HDP',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212074,
                  OWPMatchId: 279047,
                  Bettype: 7,
                  MinorBettypeId: 0,
                  AssignmentType: 2,
                  DisplayName: '1H HDP',
                  UserId: 8,
                  Username: 'superman',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {
                AssignmentId: 211959,
                OWPMatchId: 279047,
                Bettype: -1002,
                MinorBettypeId: 0,
                AssignmentType: 1,
                DisplayName: 'LA',
                UserId: 360418,
                Username: 'starix1',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212077,
                  OWPMatchId: 279047,
                  Bettype: -1002,
                  MinorBettypeId: 0,
                  AssignmentType: 2,
                  DisplayName: 'LA',
                  UserId: 360418,
                  Username: 'starix1',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            }
          },
          {
            BetTypeId: 3,
            MinorBettypeId: 0,
            Name: 'O/U',
            OWPMatchId: 279047,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,
            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null
            },
            FT: {
              main: {
                AssignmentId: 211958,
                OWPMatchId: 279047,
                Bettype: 3,
                MinorBettypeId: 0,
                AssignmentType: 1,
                DisplayName: 'O/U',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212072,
                  OWPMatchId: 279047,
                  Bettype: 3,
                  MinorBettypeId: 0,
                  AssignmentType: 2,
                  DisplayName: 'O/U',
                  UserId: 0,
                  Username: null,
                  AssignmentNote: 'test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: 211973,
                OWPMatchId: 279047,
                Bettype: 8,
                MinorBettypeId: 0,
                AssignmentType: 1,
                DisplayName: '1H O/U',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212075,
                  OWPMatchId: 279047,
                  Bettype: 8,
                  MinorBettypeId: 0,
                  AssignmentType: 2,
                  DisplayName: '1H O/U',
                  UserId: 8,
                  Username: 'superman',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: true
            }
          },
          {
            BetTypeId: 5,
            MinorBettypeId: 0,
            Name: 'FT.1X2',
            OWPMatchId: 279047,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,

            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null,

            },
            FT: {
              main: {
                AssignmentId: 211974,
                OWPMatchId: 279047,
                Bettype: 5,
                MinorBettypeId: 0,
                AssignmentType: 1,
                DisplayName: 'FT.1X2',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212073,
                  OWPMatchId: 279047,
                  Bettype: 5,
                  MinorBettypeId: 0,
                  AssignmentType: 2,
                  DisplayName: 'FT.1X2',
                  UserId: 8,
                  Username: 'superman',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: 211975,
                OWPMatchId: 279047,
                Bettype: 15,
                MinorBettypeId: 0,
                AssignmentType: 1,
                DisplayName: '1H 1X2',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212076,
                  OWPMatchId: 279047,
                  Bettype: 15,
                  MinorBettypeId: 0,
                  AssignmentType: 2,
                  DisplayName: '1H 1X2',
                  UserId: 8,
                  Username: 'superman',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: true
            }
          },
          {
            BetTypeId: 1,
            MinorBettypeId: -1,
            Name: 'SPECIFIC 15 MINS',
            OWPMatchId: 279048,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,

            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',

            },
            FT: {
              main: {
                AssignmentId: 211960,
                OWPMatchId: 279048,
                Bettype: 1,
                MinorBettypeId: -1,
                AssignmentType: 1,
                DisplayName: 'SPECIFIC 15 MINS',
                UserId: 360418,
                Username: 'starix1',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212065,
                  OWPMatchId: 279048,
                  Bettype: 1,
                  MinorBettypeId: -1,
                  AssignmentType: 2,
                  DisplayName: 'SPECIFIC 15 MINS',
                  UserId: 360418,
                  Username: 'starix1',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -1,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -1,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: false
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: false
            }
          },
          {
            BetTypeId: 1,
            MinorBettypeId: -2,
            Name: 'SPECIFIC 10 MINS',
            OWPMatchId: 279054,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: 211998,
                OWPMatchId: 279054,
                Bettype: 1,
                MinorBettypeId: -2,
                AssignmentType: 1,
                DisplayName: 'SPECIFIC 10 MINS',
                UserId: 360418,
                Username: 'starix1',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212066,
                  OWPMatchId: 279054,
                  Bettype: 1,
                  MinorBettypeId: -2,
                  AssignmentType: 2,
                  DisplayName: 'SPECIFIC 10 MINS',
                  UserId: 360418,
                  Username: 'starix1',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -2,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -2,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: false
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: false
            }
          }
        ],
        TotalRows: 5
      },
      {
        Group: 'NoOfCorners',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -3,
            Name: 'No.of Corners (HDP)',
            OWPMatchId: 279055,
            MaxBetFT: {
              MaxBet: '10K',
              Spread: '(0.12%)',
            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null,
            },
            FT: {
              main: {
                AssignmentId: 212014,
                OWPMatchId: 279055,
                Bettype: 1,
                MinorBettypeId: -3,
                AssignmentType: 1,
                DisplayName: 'No.of Corners (HDP)',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212067,
                  OWPMatchId: 279055,
                  Bettype: 1,
                  MinorBettypeId: -3,
                  AssignmentType: 2,
                  DisplayName: 'No.of Corners (HDP)',
                  UserId: 8,
                  Username: 'superman',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -3,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -3,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: -1002,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -3,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: -1002,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -3,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            }
          },
          {
            BetTypeId: 1,
            MinorBettypeId: -5,
            Name: 'SPECIFIC 15 MINS NUMBER OF CORNERS ',
            OWPMatchId: 279056,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: 212015,
                OWPMatchId: 279056,
                Bettype: 1,
                MinorBettypeId: -5,
                AssignmentType: 1,
                DisplayName: 'SPECIFIC 15 MINS NUMBER OF CORNERS ',
                UserId: 360418,
                Username: 'starix1',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212068,
                  OWPMatchId: 279056,
                  Bettype: 1,
                  MinorBettypeId: -5,
                  AssignmentType: 2,
                  DisplayName: 'SPECIFIC 15 MINS NUMBER OF CORNERS ',
                  UserId: 360418,
                  Username: 'starix1',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -5,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -5,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: false
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: false
            }
          }
        ],
        TotalRows: 2
      },
      {
        Group: 'TotalBooking',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -6,
            Name: 'Total Booking (HDP)',
            OWPMatchId: 279057,
            MaxBetFT: {
              MaxBet: '10K',
              Spread: '(0.12%)',
            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null,
            },
            FT: {
              main: {
                AssignmentId: 212028,
                OWPMatchId: 279057,
                Bettype: 1,
                MinorBettypeId: -6,
                AssignmentType: 1,
                DisplayName: 'Total Booking (HDP)',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212069,
                  OWPMatchId: 279057,
                  Bettype: 1,
                  MinorBettypeId: -6,
                  AssignmentType: 2,
                  DisplayName: 'Total Booking (HDP)',
                  UserId: 8,
                  Username: 'superman',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -6,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -6,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: -1002,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -6,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: -1002,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -6,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            }
          },
          {
            BetTypeId: 1,
            MinorBettypeId: -8,
            Name: 'SPECIFIC 15 MINS TOTAL BOOKINGS',
            OWPMatchId: 279058,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,

            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: 212029,
                OWPMatchId: 279058,
                Bettype: 1,
                MinorBettypeId: -8,
                AssignmentType: 1,
                DisplayName: 'SPECIFIC 15 MINS TOTAL BOOKINGS',
                UserId: 360418,
                Username: 'starix1',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212070,
                  OWPMatchId: 279058,
                  Bettype: 1,
                  MinorBettypeId: -8,
                  AssignmentType: 2,
                  DisplayName: 'SPECIFIC 15 MINS TOTAL BOOKINGS',
                  UserId: 360418,
                  Username: 'starix1',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -8,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -8,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: false
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: false
            }
          }
        ],
        TotalRows: 2
      },
      {
        Group: 'Minor0',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -96,
            Name: '1st Penalty Score (HDP)',
            OWPMatchId: 279063,
            MaxBetFT: {
              MaxBet: '10K',
              Spread: '(0.12%)',

            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null,

            },
            FT: {
              main: {
                AssignmentId: 212063,
                OWPMatchId: 279063,
                Bettype: 1,
                MinorBettypeId: -96,
                AssignmentType: 1,
                DisplayName: '1st Penalty Score (HDP)',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: 212071,
                  OWPMatchId: 279063,
                  Bettype: 1,
                  MinorBettypeId: -96,
                  AssignmentType: 2,
                  DisplayName: '1st Penalty Score (HDP)',
                  UserId: 8,
                  Username: 'superman',
                  AssignmentNote: '#<user> test'
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -96,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -96,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: -1002,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -96,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: -1002,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -96,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            }
          }
        ],
        TotalRows: 1
      },
      {
        Group: 'FM',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -88,
            Name: 'Fast Market',
            OWPMatchId: 279049,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: 211965,
                OWPMatchId: 279049,
                Bettype: 1,
                MinorBettypeId: -88,
                AssignmentType: 1,
                DisplayName: 'Fast Market',
                UserId: 360418,
                Username: 'starix1',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 1,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -88,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -88,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -88,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: false
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: false
            }
          }
        ],
        TotalRows: 1
      },
      {
        Group: 'AIO',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -89,
            Name: 'All In One',
            OWPMatchId: 279050,
            MaxBetFT: {
              MaxBet: '',
              Spread: '',
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: 211966,
                OWPMatchId: 279050,
                Bettype: 1,
                MinorBettypeId: -89,
                AssignmentType: 1,
                DisplayName: 'All In One',
                UserId: 862400,
                Username: 'starix2',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 1,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -89,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -89,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -89,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: false
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: false
            }
          }
        ],
        TotalRows: 1
      }
    ];

    expect(result).toEqual(expectedResult);
  });

  it('getAll should work on data collection 2', () => {
    const matchSettings = [
      {
        OWPMatchId: 279051,
        Bettype: 1,
        MinorBettypeId: 0,
        DisplayName: 'HDP',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279051,
        Bettype: 3,
        MinorBettypeId: 0,
        DisplayName: 'O/U',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279051,
        Bettype: 5,
        MinorBettypeId: 0,
        DisplayName: 'FT.1X2',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279051,
        Bettype: 7,
        MinorBettypeId: 0,
        DisplayName: '1H HDP',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279051,
        Bettype: 8,
        MinorBettypeId: 0,
        DisplayName: '1H O/U',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279051,
        Bettype: 15,
        MinorBettypeId: 0,
        DisplayName: '1H 1X2',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: true
      },
      {
        OWPMatchId: 279052,
        Bettype: 1,
        MinorBettypeId: -88,
        DisplayName: 'Fast Market',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279053,
        Bettype: 1,
        MinorBettypeId: -89,
        DisplayName: 'All In One',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279064,
        Bettype: 1,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (HDP)',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279064,
        Bettype: 3,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (OU)',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279064,
        Bettype: 5,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279064,
        Bettype: 7,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (1H HDP)',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279064,
        Bettype: 8,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (1H OU)',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279064,
        Bettype: 15,
        MinorBettypeId: -6,
        DisplayName: 'Total Booking (1H 1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279065,
        Bettype: 1,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (HDP)',
        Spread: null,
        Maxbet: null,

        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279065,
        Bettype: 3,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279065,
        Bettype: 5,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279065,
        Bettype: 7,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (1H HDP)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279065,
        Bettype: 8,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (1H OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279065,
        Bettype: 15,
        MinorBettypeId: -3,
        DisplayName: 'No.of Corners (1H 1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279066,
        Bettype: 1,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (HDP)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279066,
        Bettype: 3,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279066,
        Bettype: 5,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279066,
        Bettype: 7,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (1H HDP)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279066,
        Bettype: 8,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (1H OU)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      },
      {
        OWPMatchId: 279066,
        Bettype: 15,
        MinorBettypeId: -96,
        DisplayName: '1st Penalty Score (1H 1X2)',
        Spread: null,
        Maxbet: null,
        IsFromMaxbetSetting: false
      }
    ];

    const assignments = [
      {
        AssignmentId: 0,
        OWPMatchId: 279052,
        Bettype: 1,
        MinorBettypeId: -88,
        AssignmentType: 0,
        DisplayName: 'Fast Market',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279053,
        Bettype: 1,
        MinorBettypeId: -89,
        AssignmentType: 0,
        DisplayName: 'All In One',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279064,
        Bettype: 1,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279064,
        Bettype: 3,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279064,
        Bettype: 5,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279064,
        Bettype: 7,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (1H HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279064,
        Bettype: 8,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (1H OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279064,
        Bettype: 15,
        MinorBettypeId: -6,
        AssignmentType: 0,
        DisplayName: 'Total Booking (1H 1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 212124,
        OWPMatchId: 279064,
        Bettype: -1002,
        MinorBettypeId: -6,
        AssignmentType: 1,
        DisplayName: 'Total Booking (LA)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279065,
        Bettype: 1,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279065,
        Bettype: 3,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279065,
        Bettype: 5,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279065,
        Bettype: 7,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (1H HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279065,
        Bettype: 8,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (1H OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279065,
        Bettype: 15,
        MinorBettypeId: -3,
        AssignmentType: 0,
        DisplayName: 'No.of Corners (1H 1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 212154,
        OWPMatchId: 279065,
        Bettype: -1002,
        MinorBettypeId: -3,
        AssignmentType: 1,
        DisplayName: 'No.of Corners (LA)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279066,
        Bettype: 1,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279066,
        Bettype: 3,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279066,
        Bettype: 5,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279066,
        Bettype: 7,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (1H HDP)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279066,
        Bettype: 8,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (1H OU)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 0,
        OWPMatchId: 279066,
        Bettype: 15,
        MinorBettypeId: -96,
        AssignmentType: 0,
        DisplayName: '1st Penalty Score (1H 1X2)',
        UserId: 0,
        Username: null,
        AssignmentNote: null
      },
      {
        AssignmentId: 212171,
        OWPMatchId: 279066,
        Bettype: -1002,
        MinorBettypeId: -96,
        AssignmentType: 1,
        DisplayName: '1st Penalty Score (LA)',
        UserId: 8,
        Username: 'superman',
        AssignmentNote: null
      }
    ];
    const result = LiveDetailService.getAll(matchSettings, assignments);
    const expectedResult = [
      {
        Group: 'Major',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: 0,
            Name: 'HDP',
            OWPMatchId: 279051,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,
            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null,

            },
            FT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 1,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: 0,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 1,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: 0,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: 0,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: 0,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: -1002,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: 0,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: -1002,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: 0,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            }
          },
          {
            BetTypeId: 3,
            MinorBettypeId: 0,
            Name: 'O/U',
            OWPMatchId: 279051,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,
            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null,
            },
            FT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 3,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: 0,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 3,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: 0,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 8,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: 0,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 8,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: 0,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: true
            }
          },
          {
            BetTypeId: 5,
            MinorBettypeId: 0,
            Name: 'FT.1X2',
            OWPMatchId: 279051,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,
            },
            MaxBetHT: {
              MaxBet: null,
              Spread: null,
            },
            FT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 5,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: 0,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 5,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: 0,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 15,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: 0,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 15,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: 0,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: true
            }
          }
        ],
        TotalRows: 3
      },
      {
        Group: 'NoOfCorners',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -3,
            Name: 'No.of Corners (HDP)',
            OWPMatchId: 279065,
            MaxBetFT: {
              MaxBet: '',
              Spread: '',
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 1,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -3,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 1,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -3,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -3,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -3,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {
                AssignmentId: 212154,
                OWPMatchId: 279065,
                Bettype: -1002,
                MinorBettypeId: -3,
                AssignmentType: 1,
                DisplayName: 'No.of Corners (LA)',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: -1002,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -3,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            }
          }
        ],
        TotalRows: 1
      },
      {
        Group: 'TotalBooking',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -6,
            Name: 'Total Booking (HDP)',
            OWPMatchId: 279064,
            MaxBetFT: {
              MaxBet: '',
              Spread: '',
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 1,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -6,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 1,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -6,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -6,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -6,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {
                AssignmentId: 212124,
                OWPMatchId: 279064,
                Bettype: -1002,
                MinorBettypeId: -6,
                AssignmentType: 1,
                DisplayName: 'Total Booking (LA)',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: -1002,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -6,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            }
          }
        ],
        TotalRows: 1
      },
      {
        Group: 'Minor0',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -96,
            Name: '1st Penalty Score (HDP)',
            OWPMatchId: 279066,
            MaxBetFT: {
              MaxBet: '',
              Spread: '',
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 1,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -96,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 1,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -96,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -96,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -96,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            LA: {
              main: {
                AssignmentId: 212171,
                OWPMatchId: 279066,
                Bettype: -1002,
                MinorBettypeId: -96,
                AssignmentType: 1,
                DisplayName: '1st Penalty Score (LA)',
                UserId: 8,
                Username: 'superman',
                AssignmentNote: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: -1002,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -96,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            }
          }
        ],
        TotalRows: 1
      },
      {
        Group: 'FM',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -88,
            Name: 'Fast Market',
            OWPMatchId: 279052,
            MaxBetFT: {
              MaxBet: null,
              Spread: null,
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 1,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -88,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 1,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -88,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -88,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -88,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: false
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: false
            }
          }
        ],
        TotalRows: 1
      },
      {
        Group: 'AIO',
        DataSource: [
          {
            BetTypeId: 1,
            MinorBettypeId: -89,
            Name: 'All In One',
            OWPMatchId: 279053,
            MaxBetFT: {
              MaxBet: '',
              Spread: '',
            },
            MaxBetHT: {
              MaxBet: '',
              Spread: '',
            },
            FT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 1,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -89,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 1,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -89,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: true
            },
            HT: {
              main: {
                AssignmentId: -1,
                AssignmentNote: null,
                AssignmentType: 1,
                Bettype: 7,
                OWPMatchId: null,
                DisplayName: null,
                MinorBettypeId: -89,
                UserId: null,
                Username: null
              },
              others: [
                {
                  AssignmentId: -1,
                  AssignmentNote: null,
                  AssignmentType: 2,
                  Bettype: 7,
                  OWPMatchId: null,
                  DisplayName: null,
                  MinorBettypeId: -89,
                  UserId: null,
                  Username: null
                }
              ],
              isAllowAdd: false
            },
            LA: {
              main: {},
              others: [],
              isAllowAdd: false
            }
          }
        ],
        TotalRows: 1
      }
    ];
    expect(result).toEqual(expectedResult);
  });
});

import {
  getPrematchsHaveAssignment,
  executeBuilder,
  buildModelNightShiftSaver
} from './modelSaveNightShiftBuilder';

jest.mock('../../const/const', () => {
  return {
    URL: { SAVE_PREMATCH_ASSIGNMENT: 'url-save' },
    netCoreUrl: 'v1/test'
  }
});

describe('modelSaveNightShiftBuilder under test', () => {
  it('should return pre match assignments correct', () => {
    const entities = [
      {
        HDP: 'sa',
        HDPTakeOver: '',
        OU: '',
        OUTakeOver: ''
      },
      {
        HDP: '',
        HDPTakeOver: 'sa',
        OU: '',
        OUTakeOver: ''
      },
      {
        HDP: '',
        HDPTakeOver: '',
        OU: 'sa',
        OUTakeOver: ''
      },
      {
        HDP: '',
        HDPTakeOver: '',
        OU: '',
        OUTakeOver: 'sa'
      },
      {
        HDP: '',
        HDPTakeOver: '',
        OU: '',
        OUTakeOver: ''
      }
    ];

    const expectState = [
      {
        HDP: 'sa',
        HDPTakeOver: '',
        OU: '',
        OUTakeOver: ''
      },
      {
        HDP: '',
        HDPTakeOver: 'sa',
        OU: '',
        OUTakeOver: ''
      },
      {
        HDP: '',
        HDPTakeOver: '',
        OU: 'sa',
        OUTakeOver: ''
      },
      {
        HDP: '',
        HDPTakeOver: '',
        OU: '',
        OUTakeOver: 'sa'
      }
    ];
    const result = getPrematchsHaveAssignment(entities);

    expect(result).toEqual(expectState);
  });

  it('execute should return model correct', () => {
    const entities = [
      {
        Blacklist: false,
        HDP: null,
        HDPTakeOver: '',
        HasMessages: true,
        HasNotes: true,
        IsRead: true,
        LeagueId: 30984,
        LeagueName: 'AUSTRALIA FOOTBALL QUEENSLAND PREMIER LEAGUE',
        MatchCount: 1,
        Maxbet: null,
        No: 1,
        OU: '',
        OUTakeOver: null,
        OtherHDPNote: null,
        OtherHDPTakeOverNote: null,
        OtherOUNote: null,
        OtherOUTakeOverNote: null,
        PreMatchIssueId: 115,
        PreMatchLeagueId: 9211,
        PreMatchNoteId: 104,
        Remark: null,
        Spread: null,
        isExpand: false,
        isInputTyping: false
      }
    ];
    const props = {
      startTimeOriginal: '15:59:00',
      startTimeFilter: '15:00:00',
      entities,
      PrematchIdDeleted: [],
      PrematchUpdated: [],
      Date: 'October 9, 2020 9:51 AM'
    };

    const expected = {
      postModel: {
        url: 'url-save',
       
        PrematchIdDeleted: [],
        PrematchUpdated: [],
        Date: 'October 9, 2020 9:51 AM',
        StartTime: '15:00'
      },
      message: 'Change start time may cause some loss in your data. Continue?',
      startTimeChange: true
    };
    const result = executeBuilder(props);
    expect(result).toEqual(expected);
  });

  it('should build save model night-shift correct when no change start time and day shift not empty', () => {
    const dayShift = {
      entities: [
        {
          Blacklist: false,
          HDP: null,
          HDPTakeOver: '',
          HasMessages: true,
          HasNotes: true,
          IsRead: true,
          LeagueId: 30984,
          LeagueName: 'AUSTRALIA FOOTBALL QUEENSLAND PREMIER LEAGUE',
          MatchCount: 1,
          Maxbet: null,
          No: 1,
          OU: '',
          OUTakeOver: null,
          OtherHDPNote: null,
          OtherHDPTakeOverNote: null,
          OtherOUNote: null,
          OtherOUTakeOverNote: null,
          PreMatchIssueId: 115,
          PreMatchLeagueId: 9211,
          PreMatchNoteId: 104,
          Remark: null,
          Spread: null,
          isExpand: false,
          isInputTyping: false
        }
      ],
      prematchIdDeleted: [],
      prematchIdUpdated: [9211]
    };
    const header = {
      startTimeOriginal: '15:59:00',
      startTimeFilter: '15:59:00'
    };
    const filter = {
      filterDate: 'October 9, 2020 9:51 AM'
    };

    const expected = {
      postModel: {
        url: 'url-save',
        PrematchIdDeleted: [],
        PrematchUpdated: [
          {
            Blacklist: false,
            HDP: null,
            HDPTakeOver: '',
            HasMessages: true,
            HasNotes: true,
            IsRead: true,
            LeagueId: 30984,
            LeagueName: 'AUSTRALIA FOOTBALL QUEENSLAND PREMIER LEAGUE',
            MatchCount: 1,
            Maxbet: null,
            No: 1,
            OU: '',
            OUTakeOver: null,
            OtherHDPNote: null,
            OtherHDPTakeOverNote: null,
            OtherOUNote: null,
            OtherOUTakeOverNote: null,
            PreMatchIssueId: 115,
            PreMatchLeagueId: 9211,
            PreMatchNoteId: 104,
            Remark: null,
            Spread: null,
            isExpand: false,
            isInputTyping: false
          }
        ],
        Date: 'October 9, 2020 9:51 AM',
        StartTime: '15:59'
      },
      message: '',
      startTimeChange: false
    };
    const actual = buildModelNightShiftSaver(dayShift, header, filter);
    expect(actual).toEqual(expected);
  });
});

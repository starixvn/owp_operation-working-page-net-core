import { DataSource } from '../type/TTaskListTable';
import {
  getHeaderDateTime,
  filterMatchesByShowUnreadMsg,
  filterMatchesByHideCompletedMatches,
  filterMatchByCriteria,
  getToday
} from './tasklistHelper';
import { getCurrentDateGmtPlus8 } from '../../Shared/DateTime/dateTimeHelper';

jest.mock('../../Shared/DateTime/dateTimeHelper', () => ({
  getCurrentDateGmtPlus8: jest.fn()
}));

const createMockMatch = (overrides: Partial<DataSource> = {}): DataSource => ({
  rowIndex: 0,
  AdminSiteId: 1213,
  OWPMatchId: 1107092,
  ParentMatchId: 0,
  IsFantasy: false,
  MatchNo: '12',
  OtherTime: null,
  Home: 'B',
  Away: 'B',
  MatchCode: 'test',
  League: 'a',
  LeagueGroup: '',
  IsPES23: false,
  IsPES24: false,
  IsFC24: false,
  IsPESOther: false,
  IsFIFA: false,
  IsSaba: false,
  NoOfHDP: '23',
  NoOfHDPId: 0,
  NoOfHDPShort: '23',
  Channel: 'okie',
  OtherProvider: 'oke',
  Provider: 'okie',
  Phone: '03204234',
  EventDate: new Date(),
  KickOffTime: '12:00:00',
  WatchBet: 'Test',
  Round: 'okie',
  TeamRound: '',
  Division: 'hi',
  Remark: 'ET/PK',
  MinorBettypeId: 32,
  IPTV: 'okie',
  IPTVCopy: 'Tv',
  AIOBT: '',
  AIOBTShortName: '',
  IsAllowedToViewDetail: true,
  MatchStatus: 'Completed',
  IsTodayEvent: true,
  IsReady: false,
  IsCompletedMatch: false,
  Assignment: {
    Handicap: [],
    HandicapOthers: [],
    Handicap1st: [],
    Handicap1stOthers: [],
    OverUnder: [],
    OverUnderOthers: [],
    OverUnder1st: [],
    OverUnder1stOthers: [],
    OneXTwo1st: [],
    OneXTwo1stOthers: [],
    Specific15Mins: [],
    Specific15MinsOthers: [],
    Specific10Mins: [],
    Specific10MinsOthers: [],
    FastMarket: [],
    FastMarketOthers: [],
    AllInOne: [],
    AllInOneOthers: [],
    LA: [],
    OneXTwo: [],
    OneXTwoOthers: [],
    LAOthers: []
  },
  MatchSetting: {
    Handicap: {
      BettypeId: 1,
      Maxbet: 5000,
      Spread: 0.04,
      MaxbetSpreadToDisplay: '20k',
      IsHasMaxbetSpread: true
    },
    Handicap1st: {
      BettypeId: 0,
      Maxbet: null,
      Spread: null,
      MaxbetSpreadToDisplay: '20k',
      IsHasMaxbetSpread: false
    },
    OverUnder: {
      BettypeId: 3,
      Maxbet: 20000,
      Spread: null,
      MaxbetSpreadToDisplay: '20k',
      IsHasMaxbetSpread: true
    },
    OverUnder1st: {
      BettypeId: 0,
      Maxbet: null,
      Spread: null,
      MaxbetSpreadToDisplay: '20k',
      IsHasMaxbetSpread: false
    },
    OneXTwo: {
      BettypeId: 0,
      Maxbet: null,
      Spread: null,
      MaxbetSpreadToDisplay: '20k',
      IsHasMaxbetSpread: false
    },
    OneXTwo1st: {
      BettypeId: 0,
      Maxbet: null,
      Spread: null,
      MaxbetSpreadToDisplay: '20k',
      IsHasMaxbetSpread: false
    },
    Specific15Mins: {
      BettypeId: 0,
      Maxbet: null,
      Spread: null,
      MaxbetSpreadToDisplay: '20k',
      IsHasMaxbetSpread: false
    },
    Specific10Mins: {
      BettypeId: 0,
      Maxbet: null,
      Spread: null,
      MaxbetSpreadToDisplay: '20k',
      IsHasMaxbetSpread: false
    },
    FastMarket: {
      BettypeId: 1,
      Maxbet: null,
      Spread: null,
      MaxbetSpreadToDisplay: '',
      IsHasMaxbetSpread: false
    }
  },
  Televisions: [],
  MatchOffers: [],
  IsRead: true,
  HasTicket: true,
  EventDateTime: new Date(),
  Rowspan: 2,
  OtherTimeDisplay: null,
  KickOffTimeDisplay: '12:00 PM',
  EventDateDisplay: '04-May',
  ReadyDateTimeDisplay: 'hi',
  InfoSiteTime: null,
  InfoSiteTimeDisplay: null,
  IsShowAbsenceIcon: false,
  ...overrides
});

describe('getToday', () => {
  it('should return the current day when time is at or after 11:00 (timeLine)', () => {
    const mockDate = new Date('2023-05-05T12:00:00');
    (getCurrentDateGmtPlus8 as jest.Mock).mockReturnValue(mockDate);

    const result = getToday();
    expect(result).toEqual(mockDate);
  });

  it('should return previous day when time is before 11:00 (timeLine)', () => {
    const mockDate = new Date('2023-05-05T10:00:00');
    (getCurrentDateGmtPlus8 as jest.Mock).mockReturnValue(mockDate);

    const result = getToday();
    const expectedDate = new Date('2023-05-04T10:00:00');
    expect(result).toEqual(expectedDate);
  });

  it('should return current day when time is exactly 11:00 (timeLine)', () => {
    const mockDate = new Date('2023-05-05T11:00:00');
    (getCurrentDateGmtPlus8 as jest.Mock).mockReturnValue(mockDate);

    const result = getToday();
    expect(result).toEqual(mockDate);
  });
});

describe('getHeaderDateTime', () => {
  it('should return today and yesterday when current time is after 11:00', () => {
    const mockDate = new Date('2023-05-05T15:00:00');
    (getCurrentDateGmtPlus8 as jest.Mock).mockReturnValue(mockDate);

    const result = getHeaderDateTime();
    expect(result.today).toEqual(mockDate);
    expect(result.yesterday).toEqual(new Date('2023-05-04T15:00:00'));
  });

  it('should return adjusted dates when current time is before 11:00', () => {
    const mockDate = new Date('2023-05-05T08:00:00');
    (getCurrentDateGmtPlus8 as jest.Mock).mockReturnValue(mockDate);

    const result = getHeaderDateTime();
    const expectedToday = new Date('2023-05-04T08:00:00');
    const expectedYesterday = new Date('2023-05-03T08:00:00');
    expect(result.today).toEqual(expectedToday);
    expect(result.yesterday).toEqual(expectedYesterday);
  });
});

describe('filterMatchesByShowUnreadMsg', () => {
  it('should return all matches when isUnRead is false', () => {
    const matches = [
      createMockMatch({ IsRead: true }),
      createMockMatch({ IsRead: false })
    ];
    const result = filterMatchesByShowUnreadMsg(matches, false);
    expect(result).toEqual(matches);
    expect(result.length).toBe(2);
  });

  it('should return only unread matches when isUnRead is true', () => {
    const readMatch = createMockMatch({ IsRead: true });
    const unreadMatch = createMockMatch({ IsRead: false });
    const matches = [readMatch, unreadMatch];

    const result = filterMatchesByShowUnreadMsg(matches, true);
    expect(result).toEqual([unreadMatch]);
    expect(result.length).toBe(1);
  });

  it('should return empty array when all matches are read and isUnRead is true', () => {
    const matches = [
      createMockMatch({ IsRead: true }),
      createMockMatch({ IsRead: true })
    ];
    const result = filterMatchesByShowUnreadMsg(matches, true);
    expect(result).toEqual([]);
  });

  it('should return all unread matches when isUnRead is true', () => {
    const matches = [
      createMockMatch({ IsRead: false }),
      createMockMatch({ IsRead: false })
    ];
    const result = filterMatchesByShowUnreadMsg(matches, true);
    expect(result).toEqual(matches);
    expect(result.length).toBe(2);
  });
});

describe('filterMatchesByHideCompletedMatches', () => {
  it('should return all matches when isHideCompleted is false', () => {
    const matches = [
      createMockMatch({ IsCompletedMatch: true }),
      createMockMatch({ IsCompletedMatch: false })
    ];
    const result = filterMatchesByHideCompletedMatches(matches, false);
    expect(result).toEqual(matches);
    expect(result.length).toBe(2);
  });

  it('should return only incomplete matches when isHideCompleted is true', () => {
    const completedMatch = createMockMatch({ IsCompletedMatch: true });
    const incompleteMatch = createMockMatch({ IsCompletedMatch: false });
    const matches = [completedMatch, incompleteMatch];

    const result = filterMatchesByHideCompletedMatches(matches, true);
    expect(result).toEqual([incompleteMatch]);
    expect(result.length).toBe(1);
  });

  it('should return empty array when all matches are completed and isHideCompleted is true', () => {
    const matches = [
      createMockMatch({ IsCompletedMatch: true }),
      createMockMatch({ IsCompletedMatch: true })
    ];
    const result = filterMatchesByHideCompletedMatches(matches, true);
    expect(result).toEqual([]);
  });

  it('should return all incomplete matches when isHideCompleted is true', () => {
    const matches = [
      createMockMatch({ IsCompletedMatch: false }),
      createMockMatch({ IsCompletedMatch: false })
    ];
    const result = filterMatchesByHideCompletedMatches(matches, true);
    expect(result).toEqual(matches);
    expect(result.length).toBe(2);
  });
});

describe('filterMatchByCriteria', () => {
  it('should filter matches by unread status and hide completed status', () => {
    const matches = [
      createMockMatch({ IsRead: false, IsCompletedMatch: false }), // unread, incomplete
      createMockMatch({ IsRead: true, IsCompletedMatch: false }), // read, incomplete
      createMockMatch({ IsRead: false, IsCompletedMatch: true }), // unread, completed
      createMockMatch({ IsRead: true, IsCompletedMatch: true }) // read, completed
    ];

    const filterFn = filterMatchByCriteria(matches);
    const result = filterFn(true)(true);

    // Should only return unread and incomplete matches
    expect(result).toEqual([matches[0]]);
    expect(result.length).toBe(1);
  });

  it('should return all matches when both filters are false', () => {
    const matches = [
      createMockMatch({ IsRead: false, IsCompletedMatch: false }),
      createMockMatch({ IsRead: true, IsCompletedMatch: true })
    ];

    const filterFn = filterMatchByCriteria(matches);
    const result = filterFn(false)(false);

    expect(result).toEqual(matches);
    expect(result.length).toBe(2);
  });

  it('should filter only by unread when isHideCompleted is false', () => {
    const matches = [
      createMockMatch({ IsRead: false, IsCompletedMatch: false }),
      createMockMatch({ IsRead: false, IsCompletedMatch: true }),
      createMockMatch({ IsRead: true, IsCompletedMatch: false })
    ];

    const filterFn = filterMatchByCriteria(matches);
    const result = filterFn(true)(false);

    // Should return only unread matches regardless of completion status
    expect(result).toEqual([matches[0], matches[1]]);
    expect(result.length).toBe(2);
  });

  it('should filter only by completed when isUnRead is false', () => {
    const matches = [
      createMockMatch({ IsRead: false, IsCompletedMatch: false }),
      createMockMatch({ IsRead: true, IsCompletedMatch: false }),
      createMockMatch({ IsRead: true, IsCompletedMatch: true })
    ];

    const filterFn = filterMatchByCriteria(matches);
    const result = filterFn(false)(true);

    // Should return only incomplete matches regardless of read status
    expect(result).toEqual([matches[0], matches[1]]);
    expect(result.length).toBe(2);
  });
});

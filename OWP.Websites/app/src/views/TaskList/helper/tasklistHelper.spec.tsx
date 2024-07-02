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

const mockDataSource: Array<DataSource> = [
  {
    rowIndex: 0,
    AdminSiteId: 1213,
    OWPMatchId: 1107092,
    IsFantasy: false,
    MatchNo: '12',
    OtherTime: null,
    Home: 'B',
    Away: 'B',
    MatchCode: 'test',
    League: 'a',
    IsPES: true,
    IsFIFA: false,
    NoOfHDP: '23',
    NoOfHDPId: 0,
    Channel: 'okie',
    OtherProvider: 'oke',
    Provider: 'okie',
    Phone: '03204234',
    EventDate: new Date(),
    KickOffTime: '12:00:00',
    WatchBet: 'Test',
    Round: 'okie',
    Division: 'hi',
    Remark: 'ET/PK',
    MinorBettypeId: 32,
    IPTV: 'okie',
    IPTVCopy: 'Tv',
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
      OverUnder: [
        {
          Phone: '1111111111',
          Skype: 'comp1_skype',
          Extension: '111',
          AssignmentNote: 'Assign to Competitor 1',
          UserId: '1111',
          Username: 'comp1',
          Nickname: 'Competitor 1',
          IsCurrentUser: false,
          IsHasContactInfo: true
        }
      ],
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
      LA: [
        {
          UserId: '1111',
          Username: 'comp1',
          Nickname: 'Competitor 1',
          IsCurrentUser: false,
          IsHasContactInfo: true,
          Phone: '1111111111',
          Skype: 'comp1_skype',
          Extension: '111',
          AssignmentNote: 'Assign to Competitor 1'
        }
      ],
      OneXTwo: [],
      OneXTwoOthers: [],
      LAOthers: [
        {
          UserId: '1111',
          Username: 'comp1',
          Nickname: 'Competitor 1',
          IsCurrentUser: false,
          IsHasContactInfo: true,
          Phone: '1111111111',
          Skype: 'comp1_skype',
          Extension: '111',
          AssignmentNote: 'Assign to Competitor 1'
        },
        {
          UserId: '1111',
          Username: 'comp1',
          Nickname: 'Competitor 1',
          IsCurrentUser: false,
          IsHasContactInfo: true,
          Phone: '1111111111',
          Skype: 'comp1_skype',
          Extension: '111',
          AssignmentNote: 'Assign to Competitor 1'
        }
      ]
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
    IsShowAbsenceIcon: false
  }
];

describe('getHeaderDateTime', () => {
  it('should return correct date objects for today and yesterday', () => {
    // Set the current date and time to be May 5, 2023 at 10:00 GMT+8
    (getCurrentDateGmtPlus8 as unknown as jest.Mock).mockReturnValue(
      new Date('5-5-2023')
    );

    const expectedToday = new Date('2023-05-03T17:00:00.000Z');
    const expectedYesterday = new Date('2023-05-02T17:00:00.000Z');
    expect(getHeaderDateTime().today).toEqual(expectedToday);
    expect(getHeaderDateTime().yesterday).toEqual(expectedYesterday);
  });
});

describe('filterMatchesByShowUnreadMsg', () => {
  const matches = mockDataSource;
  it('should return all matches when isUnRead is false', () => {
    expect(filterMatchesByShowUnreadMsg(matches, false)).toEqual(matches);
  });
  it('should return only unread matches when isUnRead is true', () => {
    expect(filterMatchesByShowUnreadMsg(matches, true)).toEqual([]);
  });
});

describe('filterMatchesByHideCompletedMatches', () => {
  const matches = mockDataSource;
  it('should return all matches when isHideCompleted is false', () => {
    expect(filterMatchesByHideCompletedMatches(matches, false)).toEqual(
      matches
    );
  });
  it('should return only incomplete matches when isHideCompleted is true', () => {
    const expectedMatches = mockDataSource;
    expect(filterMatchesByHideCompletedMatches(matches, true)).toEqual(
      expectedMatches
    );
  });
});

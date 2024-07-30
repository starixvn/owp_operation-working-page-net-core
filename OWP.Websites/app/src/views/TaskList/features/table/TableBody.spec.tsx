import { render } from '@testing-library/react';
import TableBody from './TableBody';
import { DataSource, TBody } from '../../type/TTaskListTable';
import { Provider } from 'react-redux';
import store from '../../app/storeNative';
import { useAppSelector, useAppDispatch } from '../../app/hooks';

jest.mock('../../app/hooks', () => ({
  useAppSelector: jest.fn(),
  useAppDispatch: jest.fn()
}));

describe('TableBody component', () => {
  beforeEach(() => {
    (useAppSelector as unknown as jest.Mock).mockClear();
  });

  it('should render There is no data. when dataSources is empty', () => {
    const props = {
      dataSources: [],
      hasDetailInformationPermission: true
    };
    const { getByText } = render(<TableBody {...props} />);
    expect(getByText('There is no data.')).toBeInTheDocument();
  });

  it('should render RowContent component for each dataSource', () => {
    const props: TBody = {
      hasDetailInformationPermission: true,
      dataSources: mockDataSource
    };

    (useAppSelector as unknown as jest.Mock).mockReturnValue({
      matches: mockDataSource,
      popupState: { isOpen: true, matchId: 21 }
    });

    const { getByText } = render(
      <Provider store={store}>
        <TableBody {...props} />
      </Provider>
    );
    expect(useAppSelector).toBeCalled();
    expect(getByText('03204234')).toBeInTheDocument();
    expect(getByText('Test')).toBeInTheDocument();
    expect(getByText('03204235')).toBeInTheDocument();
  });
});

const mockDataSource: Array<DataSource> = [
  {
    rowIndex: 0,
    AdminSiteId: 1213,
    OWPMatchId: 1107092,
    IsFantasy: false,
    MatchNo: '12',
    EventDate: new Date(),
    KickOffTime: '12:00:00',
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
      OverUnderOthers: [],
      OverUnder1st: [],
      OverUnder1stOthers: [],
      OneXTwo: [],
      OneXTwoOthers: [],
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
  },
  {
    rowIndex: 0,
    AdminSiteId: 1213,
    OWPMatchId: 1107092,
    IsFantasy: false,
    MatchNo: '12',
    EventDate: new Date(),
    KickOffTime: '12:00:00',
    OtherTime: null,
    Home: 'B',
    Away: 'B',
    MatchCode: 'test',
    League: 'a',
    IsPES: false,
    IsFIFA: true,
    NoOfHDP: '23',
    NoOfHDPId: 0,
    Channel: 'okie',
    OtherProvider: 'oke',
    Provider: 'okie',
    Phone: '03204235',
    WatchBet: 'Test watch bet',
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
      OverUnderOthers: [],
      OverUnder1st: [],
      OverUnder1stOthers: [],
      OneXTwo: [],
      OneXTwoOthers: [],
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

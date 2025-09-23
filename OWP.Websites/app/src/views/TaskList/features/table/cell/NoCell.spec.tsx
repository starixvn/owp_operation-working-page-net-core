import { render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import { CellProps, DataSource } from '../../../type/TTaskListTable';
import NoCell from './NoCell';

const middlewares = [];
const mockStore = configureStore(middlewares);

const Wrapper = ({ children }) => {
  const store = mockStore({
    taskList: {
      matchesSelected: []
    }
  });
  return <Provider store={store}>{children}</Provider>;
};

// Mock the custom hook
jest.mock('../../../../Shared/customHook/useStableDispatch', () => {
  return jest.fn(() => jest.fn());
});

describe('NoCell under testing', () => {

  const dataSources: DataSource = {
    KickOffTime: new Date('2022-12-13T16:00:00'),
    KickOffTimeDisplay: '4:00PM 13-12-2022',
    OtherTime: new Date('2022-12-13T16:00:00'),
    OtherTimeDisplay: '',
    IsCompletedMatch: false,
    ReadyDateTimeDisplay: '13-12-2022',
    IsReady: false,
    IsTodayEvent: true,
    Rowspan: 1,
    AdminSiteId: 0,
    Assignment: {
      AllInOne: [],
      AllInOneOthers: [],
      FastMarket: [],
      FastMarketOthers: [],
      Handicap: [],
      Handicap1st: [],
      Handicap1stOthers: [],
      HandicapOthers: [],
      LA: [],
      LAOthers: [],
      OneXTwo: [],
      OneXTwo1st: [],
      OneXTwo1stOthers: [],
      OneXTwoOthers: [],
      OverUnder: [],
      OverUnder1st: [],
      OverUnder1stOthers: [],
      OverUnderOthers: [],
      Specific10Mins: [],
      Specific10MinsOthers: [],
      Specific15Mins: [],
      Specific15MinsOthers: []
    },
    Away: 'Delhi FC',
    Channel: 'K+ Sport',
    Division: 'M15',
    EventDateTime: new Date('2022-12-13T16:00:00'),
    HasTicket: false,
    Home: 'Mahar United',
    IPTV: '',
    IPTVCopy: '',
    IsAllowedToViewDetail: true,
    IsFIFA: true,
    IsFantasy: false,
    IsPES: true,
    IsRead: true,
    League: 'INDIA DELHI PREMIER LEAGUE',
    MatchCode: '',
    MatchNo: '003',
    MatchOffers: [
      {
        MatchOfferValue: '2s accept values'
      }
    ],
    MatchSetting: {
      FastMarket: {
        BettypeId: 1,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      },
      Handicap: {
        BettypeId: 2,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      },
      Handicap1st: {
        BettypeId: 3,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      },
      OneXTwo: {
        BettypeId: 1,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      },
      OneXTwo1st: {
        BettypeId: 1,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      },
      OverUnder: {
        BettypeId: 1,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      },
      OverUnder1st: {
        BettypeId: 3,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      },
      Specific10Mins: {
        BettypeId: 3,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      },
      Specific15Mins: {
        BettypeId: 3,
        Maxbet: 15,
        Spread: 20,
        MaxbetSpreadToDisplay: ' 15K (20%)',
        IsHasMaxbetSpread: true
      }
    },
    MatchStatus: 'Live',
    MinorBettypeId: 0,
    NoOfHDP: '2 lines',
    NoOfHDPId: 11,
    OWPMatchId: 930265,
    OtherProvider: 'BG/BET/',
    Phone: '012948124',
    Provider: 'BET/Refer SBO/SB/ISN',
    Remark: 'No 1x2_Refer SB/ISN/SBO/FUN88(IM)_Last Round MB -50%',
    Round: 'Last Round',
    Televisions: [
      {
        TVCode: 'Tv VTV',
        Color: 'red'
      }
    ],
    WatchBet: 'wb',
    EventDate: new Date('2022-12-13T16:00:00'),
    EventDateDisplay: '4:00PM 13-12-2022',
    rowIndex: 0
  };
  it('should render icon', () => {
    const props: CellProps = {
      dataSource: dataSources,
      rowIndex: 0
    };
    const { container } = render(<NoCell {...props} />, { wrapper: Wrapper });
    const icon = container.querySelectorAll('.live');
    expect(icon).toBeTruthy();
  });
});

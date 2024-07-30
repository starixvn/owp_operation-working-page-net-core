import { render } from '@testing-library/react';
import IpTvStreamingCell from './IpTvStreamingCell';
import { CellProps } from '../../../type/TableType';

const props: CellProps = {
  dataSource: [
    {
      RowSpan: 2,
      AdminSiteId: 0,
      Assignment: {
        AllInOne: [],
        AllInOneOthers: [],
        FastMarket: [],
        FastMarketOthers: [],
        Handicap: [
          {
            AssignmentNote: null,
            AssignmentType: 1,
            BettypeId: 15,
            Extension: '5649',
            IsCancelled: false,
            IsCurrentUser: false,
            IsHasContactInfo: true,
            Nickname: 'sa',
            Phone: '0999',
            Skype: '2312',
            UserId: 24072194,
            Username: 'sa'
          }
        ],
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
      Division: '',
      EventDate: new Date('2022-12-13T00:00:00'),
      EventDateTime: new Date('2022-12-13T16:00:00'),
      HasTicket: false,
      Home: 'Mahar United',
      IPTV: '',
      IPTVCopy: '',
      IsAllowedToViewDetail: true,
      IsFIFA: false,
      IsFantasy: false,
      IsMinorMatch: false,
      IsOneHourToKickOffTime: false,
      IsPES: false,
      IsRead: true,
      KickOffTime: '16:00:00',
      League: 'INDIA DELHI PREMIER LEAGUE',
      LengthIpTv: 0,
      LengthLeague: 26,
      LengthOtherProduct1HOneXTwo: 60,
      LengthOtherProductAllInOne: 0,
      LengthOtherProductFastMarket: 0,
      LengthOtherProductFifteenMinutes: 0,
      LengthOtherProductOneXTwo: 40,
      LengthOtherProductTenMinutes: 0,
      LengthProvider: 80,
      LengthRemark: 62,
      LengthTraderHdp: 40,
      LengthTraderOu: 40,
      MatchCode: '',
      MatchNo: '003',
      MatchOffers: [],
      MatchSetting: {
        FastMarket: {
          BettypeId: 1,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        },
        Handicap: {
          BettypeId: 2,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        },
        Handicap1st: {
          BettypeId: 3,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        },
        OneXTwo: {
          BettypeId: 1,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        },
        OneXTwo1st: {
          BettypeId: 1,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        },
        OverUnder: {
          BettypeId: 1,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        },
        OverUnder1st: {
          BettypeId: 3,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        },
        Specific10Mins: {
          BettypeId: 3,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        },
        Specific15Mins: {
          BettypeId: 3,
          Maxbet: 15,
          Spread: 20,
          MaxbetSpreadToDisplay: ' 15K (20%)'
        }
      },
      MatchStatus: 'Live',
      MinorBettypeId: 0,
      NoOfHDP: '2 lines',
      NoOfHDPId: 11,
      OWPMatchId: 930265,
      OtherProvider: 'BG/BET/',
      OtherTime: null,
      ParentMatchId: 930265,
      Phone: '',
      Provider: 'BET/Refer SBO/SB/ISN',
      Remark: 'No 1x2_Refer SB/ISN/SBO/FUN88(IM)_Last Round MB -50%',
      Round: 'Last Round',
      Televisions: [
        {
          OWPMatchId: 123154,
          TVId: 2542,
          TVCode: 'Tv VTV',
          TVName: 'K+',
          Decoder: 'K+',
          Country: 'K+',
          Color: 'red',
          IsSelected: true
        }
      ],
      TicketInfos: [],
      WatchBet: ''
    }
  ],
  rowIndex: 0
};

describe('IpTvStreamingCell under test', () => {
  it('Should render trader name', () => {
    const { container } = render(<IpTvStreamingCell {...props} />);
    const tv = container.querySelector('.tv-cell');
    const channel = container.querySelector('.icon-channel');
    expect(tv).toBeInTheDocument();
    expect(channel).toBeInTheDocument();
  });
});

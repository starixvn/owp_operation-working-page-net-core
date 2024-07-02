import { fireEvent, render, screen } from '@testing-library/react';
import SearchResultItem from './SearchResultItem';
import { AppContext } from '../Shared/Context';

jest.mock('../Shared/Context');

const openLiveDetail = jest.fn();

const mockRenderHighlightKeywords = jest
  .fn()
  .mockImplementation((searchKeywords, matchNo) => searchKeywords + matchNo);
const mockGetMatchNoText = jest
  .fn()
  .mockImplementation((isToday, matchNo) => `#${isToday}-${matchNo}`);

const searchResults = [
  {
    RowNumber: 1,
    OWPMatchId: 295966,
    ParentMatchId: null,
    MatchNo: '31',
    Event: 'LT0C -vs- LT0D',
    EventDate: '2018-12-12T15:00:00-04:00',
    LeagueName: 'LeagueTest-LT',
    EventDateString: 'Dec 12, 2018 - 03:00 PM',
    IsToday: true,
    IsAllowedToViewDetail: true,
    IsHasAssignee: false,
    MainTrader: null,
    OtherTrader: null,
    MainLA: null,
    OtherLA: null
  },
  {
    RowNumber: 2,
    OWPMatchId: 295971,
    ParentMatchId: null,
    MatchNo: '32',
    Event: 'LT0E -vs- LT0F',
    EventDate: '2018-12-12T15:00:00-04:00',
    LeagueName: 'LeagueTest-LT',
    EventDateString: 'Dec 12, 2018 - 03:00 PM',
    IsToday: true,
    IsAllowedToViewDetail: false,
    IsHasAssignee: false,
    MainTrader: null,
    OtherTrader: null,
    MainLA: null,
    OtherLA: null
  },
  {
    RowNumber: 3,
    OWPMatchId: 295976,
    ParentMatchId: 11,
    MatchNo: '33',
    Event: 'LT0G -vs- LT0H',
    EventDate: '2018-12-12T15:00:00-04:00',
    LeagueName: 'LeagueTest-LT',
    EventDateString: 'Dec 12, 2018 - 03:00 PM',
    IsToday: true,
    IsAllowedToViewDetail: true,
    IsHasAssignee: false,
    MainTrader: {},
    OtherTrader: [],
    MainLA: {},
    OtherLA: []
  }
];

describe('Test <SearchResultItem />', () => {
  beforeEach(() => {
    mockRenderHighlightKeywords.mockClear();
    mockGetMatchNoText.mockClear();
  });

  it('render correct <SearchResultItem /> when props.data is empty and props.keywords not valid', () => {
    const props = {
      data: [],
      keywords: 'd',
      isNumeric: false,
      openLiveDetail
    };
    const { container } = render(<SearchResultItem {...props} />);

    expect(container).toMatchSnapshot();
  });

  it('render correct <SearchResultItem /> when props.data is empty and props.keywords valid', () => {
    const props = {
      data: [],
      keywords: '1',
      isNumeric: true,
      openLiveDetail
    };
    const { container } = render(<SearchResultItem {...props} />);

    expect(container).toMatchSnapshot();
  });

  it('getMatchNoText return empty when isToday=false', () => {
    const props = {
      data: [
        {
          Event: 'LT0C -vs- LT0D',
          IsToday: false,
          MatchNo: '12',
          LeagueName: 'LeagueTest'
        }
      ],
      keywords: '1',
      isNumeric: true,
      openLiveDetail
    };

    render(<SearchResultItem {...props} />);

    expect(screen.queryByText(/Match No./)).toBeNull();
  });

  it('getMatchNoText return expect text when isToday=true', () => {
    const props = {
      data: [
        {
          Event: 'LT0C -vs- LT0D',
          IsToday: true,
          MatchNo: '12',
          LeagueName: 'LeagueTest'
        }
      ],
      keywords: '1',
      isNumeric: true,
      openLiveDetail
    };

    render(<SearchResultItem {...props} />);

    expect(screen.queryByText(/Match No./)).toBeInTheDocument();
  });

  it('renderMatchInfo return expect result when props.isNumeric=true', () => {
    const props = {
      data: [
        {
          Event: 'LT0C -vs- LT0D',
          IsToday: true,
          MatchNo: '12',
          LeagueName: 'LeagueTest'
        }
      ],
      keywords: '1',
      isNumeric: true,
      openLiveDetail: jest.fn()
    };

    const { container } = render(<SearchResultItem {...props} />);
    expect(screen.getByText('LT0C -vs- LT0D')).toBeInTheDocument();
    expect(container.querySelector('.highlight').innerHTML).toEqual('1');
  });

  it('renderMatchInfo return expect result when props.isNumeric=false', () => {
    document.execCommand = jest.fn();
    const props = {
      data: [
        {
          Event: 'LT0C -vs- LT0D',
          IsToday: true,
          MatchNo: '12',
          LeagueName: 'LeagueTest'
        }
      ],
      keywords: 'HiHi',
      isNumeric: false,
      openLiveDetail: jest.fn()
    };

    const { container } = render(<SearchResultItem {...props} />);

    expect(screen.getByText('LT0C -vs- LT0D')).toBeInTheDocument();
    expect(container.querySelector('.highlight')).toBeNull();
  });

  it('renderMatchInfo handle on mouseDown', () => {
    document.execCommand = jest.fn();
    const props = {
      data: [
        {
          Event: 'LT0C -vs- LT0D',
          IsToday: true,
          MatchNo: '12',
          LeagueName: 'LeagueTest',
          OtherLA: { PositionName: 'vip', AssigneesNameConjunction: 'la' },
          MainTrader: { PositionName: 'vip1', AssigneesNameConjunction: 'la' },
          MainLA: { PositionName: 'vip2', AssigneesNameConjunction: 'la' },
          OtherTrader: { PositionName: 'vip3', AssigneesNameConjunction: 'la' },
          IsHasAssignee: true,
          IsAllowedToViewDetail: true
        }
      ],
      keywords: 'HiHi',
      isNumeric: false,
      openLiveDetail: jest.fn()
    };

    const { container } = render(<SearchResultItem {...props} />);

    const element = container.querySelector('.cursordefault');

    fireEvent.mouseUp(element);
    fireEvent.mouseDown(element);
    fireEvent.click(element);

    expect(screen.getByText('LT0C -vs- LT0D')).toBeInTheDocument();
    expect(container.querySelector('.highlight')).toBeNull();
  });

  it('renderMatchInfo call openLiveDetail', () => {
    document.execCommand = jest.fn();
    const mockOpenLiveDetail = jest.fn();
    const props = {
      data: [
        {
          Event: 'LT0C -vs- LT0D',
          IsToday: true,
          MatchNo: '12',
          LeagueName: 'LeagueTest',
          OtherLA: { PositionName: 'vip', AssigneesNameConjunction: 'la' },
          MainTrader: { PositionName: 'vip1', AssigneesNameConjunction: 'la' },
          MainLA: { PositionName: 'vip2', AssigneesNameConjunction: 'la' },
          OtherTrader: { PositionName: 'vip3', AssigneesNameConjunction: 'la' },
          IsHasAssignee: true,
          IsAllowedToViewDetail: true
        }
      ],
      keywords: 'HiHi',
      isNumeric: false,
      openLiveDetail: mockOpenLiveDetail
    };
    const { container } = render(
      <AppContext.Provider
        value={{
          detailInformationPolicy: { HasRead: true, HasWrite: false }
        }}>
        <SearchResultItem {...props} />
      </AppContext.Provider>
    );
    const element = container.querySelector('.pointer');

    fireEvent.click(element);

    expect(screen.getByText('LT0C -vs- LT0D')).toBeInTheDocument();
    expect(container.querySelector('.highlight')).toBeNull();
    expect(mockOpenLiveDetail).toBeCalled();
  });
});

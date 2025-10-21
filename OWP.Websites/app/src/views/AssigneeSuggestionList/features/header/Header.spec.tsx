import React from 'react';
import { render, screen } from '@testing-library/react';
import Header from './Header';
import useStore from '../../app/useStoreNative';
import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';

jest.mock('../../app/useStoreNative');
jest.mock('../../../Shared/DateTime/dateTimeHelper');
jest.mock('./SearchAssignee', () => ({
  __esModule: true,
  default: () => <div data-testid='search-assignee'>SearchAssignee</div>
}));
jest.mock('./ColorExplain', () => ({
  __esModule: true,
  default: () => <div data-testid='color-explain'>ColorExplain</div>
}));
jest.mock('./Filter', () => ({
  __esModule: true,
  default: () => <div data-testid='filter'>Filter</div>
}));

describe('Header component', () => {
  const mockGetLocaleDateTime = getLocaleDateTime as jest.MockedFunction<
    typeof getLocaleDateTime
  >;

  const OriginalURLSearchParams = global.URLSearchParams;

  beforeEach(() => {
    jest.clearAllMocks();
  });

  afterEach(() => {
    global.URLSearchParams = OriginalURLSearchParams;
  });

  const mockLocationSearch = (search: string) => {
    global.URLSearchParams = jest.fn().mockImplementation(() => ({
      get: (key: string) => {
        const params = new OriginalURLSearchParams(search);
        return params.get(key);
      }
    })) as any;
  };

  test('renders header with correct match info and all child components', () => {
    mockLocationSearch('');

    (useStore as unknown as jest.Mock).mockReturnValue({
      workScheduleInfo: {
        matchInfo: {
          matchNo: '123',
          leagueName: 'Premier League',
          homeName: 'Arsenal',
          awayName: 'Manchester United',
          koTime: '2022-05-01T19:00:00Z',
          maxbetHDPInString: '1',
          bettypeName: 'Asian Handicap'
        }
      }
    });

    mockGetLocaleDateTime
      .mockReturnValueOnce('May 01, 2022')
      .mockReturnValueOnce('07:00 PM');

    render(<Header />);

    expect(
      screen.getByText('#123 Premier League | Arsenal -vs- Manchester United')
    ).toBeInTheDocument();
    expect(screen.getByText(/Event Date: May 01, 2022/)).toBeInTheDocument();
    expect(screen.getByText(/07:00 PM/)).toBeInTheDocument();
    expect(screen.getByText('1')).toBeInTheDocument();
    expect(screen.getByText('Asian Handicap')).toBeInTheDocument();

    // Verify all child components are rendered
    expect(screen.getByTestId('filter')).toBeInTheDocument();
    expect(screen.getByTestId('search-assignee')).toBeInTheDocument();
    expect(screen.getByTestId('color-explain')).toBeInTheDocument();

    // Verify getLocaleDateTime is called with correct parameters
    expect(mockGetLocaleDateTime).toHaveBeenCalledWith(
      '2022-05-01T19:00:00Z',
      'MMM DD, YYYY'
    );
    expect(mockGetLocaleDateTime).toHaveBeenCalledWith(
      '2022-05-01T19:00:00Z',
      'hh:mm A'
    );
  });

  test('renders with "Other time" text when otherTime query parameter is present', () => {
    mockLocationSearch('?otherTime=true');

    (useStore as unknown as jest.Mock).mockReturnValue({
      workScheduleInfo: {
        matchInfo: {
          matchNo: '456',
          leagueName: 'La Liga',
          homeName: 'Barcelona',
          awayName: 'Real Madrid',
          koTime: '2022-06-15T20:00:00Z',
          maxbetHDPInString: '2',
          bettypeName: 'Over/Under'
        }
      }
    });

    mockGetLocaleDateTime
      .mockReturnValueOnce('Jun 15, 2022')
      .mockReturnValueOnce('08:00 PM');

    render(<Header />);

    expect(
      screen.getByText('#456 La Liga | Barcelona -vs- Real Madrid')
    ).toBeInTheDocument();
    expect(
      screen.getByText(/Event Date: Jun 15, 2022 - Other time: 08:00 PM/)
    ).toBeInTheDocument();
    expect(screen.getByText('2')).toBeInTheDocument();
    expect(screen.getByText('Over/Under')).toBeInTheDocument();
  });

  test('does not render match info when leagueName is not available', () => {
    mockLocationSearch('');

    (useStore as unknown as jest.Mock).mockReturnValue({
      workScheduleInfo: {
        matchInfo: {
          matchNo: '123',
          homeName: 'Arsenal',
          awayName: 'Manchester United',
          koTime: '2022-05-01T19:00:00Z',
          maxbetHDPInString: '1',
          bettypeName: 'Asian Handicap'
        }
      }
    });

    render(<Header />);

    expect(screen.queryByText(/Premier League/)).not.toBeInTheDocument();
    expect(screen.queryByText(/Arsenal/)).not.toBeInTheDocument();
    expect(screen.queryByText(/Manchester United/)).not.toBeInTheDocument();
    expect(screen.queryByText(/Event Date:/)).not.toBeInTheDocument();
    expect(screen.queryByText('Asian Handicap')).not.toBeInTheDocument();

    // Child components should still be rendered
    expect(screen.getByTestId('filter')).toBeInTheDocument();
    expect(screen.getByTestId('search-assignee')).toBeInTheDocument();
    expect(screen.getByTestId('color-explain')).toBeInTheDocument();
  });

  test('does not render match info when matchInfo is empty', () => {
    mockLocationSearch('');

    (useStore as unknown as jest.Mock).mockReturnValue({
      workScheduleInfo: {
        matchInfo: {}
      }
    });

    render(<Header />);

    expect(screen.queryByText(/#\d+/)).not.toBeInTheDocument();
    expect(screen.queryByText(/Event Date:/)).not.toBeInTheDocument();

    // Child components should still be rendered
    expect(screen.getByTestId('filter')).toBeInTheDocument();
    expect(screen.getByTestId('search-assignee')).toBeInTheDocument();
    expect(screen.getByTestId('color-explain')).toBeInTheDocument();
  });

  test('renders match info with all fields populated', () => {
    mockLocationSearch('');

    (useStore as unknown as jest.Mock).mockReturnValue({
      workScheduleInfo: {
        matchInfo: {
          matchNo: '789',
          leagueName: 'Bundesliga',
          homeName: 'Bayern Munich',
          awayName: 'Borussia Dortmund',
          koTime: '2022-07-20T18:30:00Z',
          maxbetHDPInString: '3',
          bettypeName: 'Handicap'
        }
      }
    });

    mockGetLocaleDateTime
      .mockReturnValueOnce('Jul 20, 2022')
      .mockReturnValueOnce('06:30 PM');

    const { container } = render(<Header />);

    const matchInfo = container.querySelector('.match-info');
    expect(matchInfo).toBeInTheDocument();
    expect(matchInfo?.textContent).toContain('#789');
    expect(matchInfo?.textContent).toContain('Bundesliga');
    expect(matchInfo?.textContent).toContain('Bayern Munich');
    expect(matchInfo?.textContent).toContain('Borussia Dortmund');
    expect(matchInfo?.textContent).toContain('Event Date:');
    expect(matchInfo?.textContent).toContain('Jul 20, 2022');
    expect(matchInfo?.textContent).toContain('06:30 PM');
    expect(matchInfo?.textContent).toContain('3');
    expect(matchInfo?.textContent).toContain('Handicap');
  });

  test('renders filter__info container with child components', () => {
    mockLocationSearch('');

    (useStore as unknown as jest.Mock).mockReturnValue({
      workScheduleInfo: {
        matchInfo: {}
      }
    });

    const { container } = render(<Header />);

    const filterInfo = container.querySelector('.filter__info');
    expect(filterInfo).toBeInTheDocument();
    expect(filterInfo).toContainElement(screen.getByTestId('filter'));
    expect(filterInfo).toContainElement(screen.getByTestId('search-assignee'));
    expect(filterInfo).toContainElement(screen.getByTestId('color-explain'));
  });
});

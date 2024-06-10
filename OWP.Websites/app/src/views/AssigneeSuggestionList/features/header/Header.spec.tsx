import { render, screen } from '@testing-library/react';
import Header from './Header';
import useStore from '../../app/useStoreNative';

jest.mock('../../app/useStoreNative');

describe('Header component', () => {
  test('renders header with correct match info', () => {
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

    render(<Header />);

    expect(
      screen.getByText('#123 Premier League | Arsenal -vs- Manchester United')
    ).toBeInTheDocument();
    expect(
      screen.getByText('Event Date: May 02, 2022 02:00 AM')
    ).toBeInTheDocument();
    expect(screen.getByText('1')).toBeInTheDocument();
    expect(screen.getByText('Asian Handicap')).toBeInTheDocument();
  });

  test('does not render header when match info is not available', () => {
    (useStore as unknown as jest.Mock).mockReturnValue({
      workScheduleInfo: {
        matchInfo: {}
      }
    });

    render(<Header />);
    expect(screen.queryByText('Premier League')).not.toBeInTheDocument();
    expect(
      screen.queryByText('Event Date: May 02, 2022 02:00 AM')
    ).not.toBeInTheDocument();

    expect(screen.queryByText('Asian Handicap')).not.toBeInTheDocument();
  });
});

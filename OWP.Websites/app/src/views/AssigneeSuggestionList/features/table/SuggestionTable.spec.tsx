import { render, screen, act } from '@testing-library/react';
import useStore from '../../app/useStoreNative';
import SuggestionTable from './SuggestionTable';

jest.mock('../../app/useStoreNative', () => jest.fn());

describe('SuggestionTable', () => {
  beforeEach(() => {
    (useStore as unknown as jest.Mock).mockReturnValue({
      workScheduleInfo: {
        fullyAvailable: [
          {
            userInfo: {
              userId: 3,
              shift: 'Liam',
              username: 'Liam',
              isManagementRole: 'true',
              level: 'max',
              mainBetType: '1',
              maxMultipleGames: '3',
              explanation: 'hello world',
              levelLabel: 'ok'
            },
            yesterday_02h: [{
              text: 'hello liam',
              color: '#000',
              textColor: '#000'
            }]
          }
        ],
        partiallyAvailable: [],
        busy: [],
        matchInfo: {
          filterDate: '2023-04-20',
          koTime: '10:00',
          leagueName: "league A"
        }
      }
    });
  });

  it('should render table with correct header and body', () => {
    render(<SuggestionTable />);

    const tableHeader = screen.getByText(/Working Schedule - Apr 20, 2023/i);
    const tableBody = screen.getByRole('table');
    const yesterday_02hCell = screen.getByText('hello liam');

    expect(tableHeader).toBeInTheDocument();
    expect(tableBody).toBeInTheDocument();
    expect(yesterday_02hCell).toBeInTheDocument();
  });

  it('should update max height when window is resized', () => {
    act(() => {
      render(<SuggestionTable />);
    });

    const tableContent = screen.getByRole('table');
    const currentTimeline =
      tableContent.getElementsByClassName('.current-timeline');
    const originalOffsetLeft =
      currentTimeline instanceof HTMLElement && currentTimeline.offsetLeft;
    const newHeight = window.innerHeight - 200;

    Object.defineProperty(window, 'innerHeight', {
      writable: true,
      configurable: true,
      value: newHeight
    });

    act(() => {
      window.dispatchEvent(new Event('resize'));
    });

    expect(tableContent.scrollTop).toBe(0);
    expect(
      currentTimeline instanceof HTMLElement && currentTimeline.offsetLeft
    ).toBe(originalOffsetLeft);
  });
});

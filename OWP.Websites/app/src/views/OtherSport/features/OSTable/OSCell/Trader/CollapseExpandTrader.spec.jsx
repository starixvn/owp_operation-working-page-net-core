
import { useDispatch } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';
import '@testing-library/jest-dom';
import CollapseExpandTrader from './CollapseExpandTrader';


jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createSelectorHook: jest.fn(),
}));

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

describe('CollapseExpandTrader under test', () => {
  it('render CollapseExpandTrader correct ', () => {
    const props = {
      dataSource: [
        {
          osMatchId: 19,
          matchStatus: 'LIVe',
          sport: 'Basketball',
          dateTime: '2021-01-01T01:00:00',
          home: 'Dalian Pro FC (N)',
          away: 'Guangzhou R&F',
          league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
          category: 'Asian 4',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          service: 'InPlay',
          remark: ''
        }
      ],
      rowIndex: 0,
      collapseExpandRowState: {
        columnKey: '',
        rowHasModeCollapse: []
      }
    };
    const { container } = render(<CollapseExpandTrader {...props} />);
    const icon = container.querySelector('.icon-expand');
    expect(icon).toBeInTheDocument();
    fireEvent.click(icon);
    expect(mockedDispatch).toBeCalled();
  });

  it('render CollapseExpandTrader collapse correct ', () => {
    const props = {
      dataSource: [
        {
          osMatchId: 19,
          matchStatus: 'LIVe',
          sport: 'Basketball',
          dateTime: '2021-01-01T01:00:00',
          home: 'Dalian Pro FC (N)',
          away: 'Guangzhou R&F',
          league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
          category: 'Asian 4',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          service: 'InPlay',
          remark: ''
        }
      ],
      rowIndex: 0,
      collapseExpandRowState: {
        columnKey: '',
        rowHasModeCollapse: [19]
      }
    };
    const { container } = render(<CollapseExpandTrader {...props} />);
    const icon = container.querySelector('.icon-expand');
    fireEvent.click(icon);
    expect(mockedDispatch).toBeCalled();
  });
});

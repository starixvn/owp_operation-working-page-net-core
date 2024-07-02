import { fireEvent, render } from '@testing-library/react';
import { useSelector } from 'react-redux';
import ReportTable from './ReportTable';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockData = [
  {
    traderName: 'sa',
    leagueHdpMj: '3',
    leagueHdpM1: '20',
    leagueHdpTotal: '23',
    matchHdpMj: '3',
    matchHdpM1: '12',
    matchHdpUnknown: '4',
    matchHdpTotal: '19'
  },
  {
    traderName: 'starix1',
    leagueHdpMj: '3',
    leagueHdpM1: '20',
    leagueHdpTotal: '23',
    matchHdpMj: '3',
    matchHdpM1: '12',
    matchHdpUnknown: '4',
    matchHdpTotal: '19'
  }
];

const setup = (reportData, isOpenTable, status = 'idle') => {
  const initialState = {
    report: {
      reportData: reportData,
      isOpenTable: isOpenTable,
      columnOrder: {
        columnId: 'name',
        subColumnId: 'subname',
        orderType: 'asc'
      },
      status
    }
  };
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn(initialState)
  );
};

describe('test ReportTable', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render Loading, status=pending', () => {
    setup(mockData, false, 'pending');
    const { container } = render(<ReportTable />);
    const table = container.querySelector('.loading-process');
    expect(table).toBeInTheDocument;
  });

  it('should not render table, isOpenTable=false', () => {
    setup(mockData, false);
    const { container } = render(<ReportTable />);
    const table = container.querySelector('.table-content');
    expect(table).toBeNull;
  });

  it('should render table content, ReactTable', () => {
    setup(mockData, true);
    const { container } = render(<ReportTable />);
    const table = container.querySelector('.table-content');
    const total = container.querySelector('.total-records');

    expect(table).toBeInTheDocument;
    expect(total?.innerHTML).toEqual('Total records: 2');
  });
});

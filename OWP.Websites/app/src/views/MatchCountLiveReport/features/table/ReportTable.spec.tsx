import { fireEvent, render, waitFor } from '@testing-library/react';
import { useSelector } from 'react-redux';
import ReportTable from './ReportTable';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockData = [
  {
    betType: 'Real Soccer',
    total: 100,
    mj: 100,
    m1: 100,
    m2: 100,
    m3: 100,
    m4: 100,
    others: 100,
    others1: 100,
    others2: 100,
    others3: 100,
    others4: 100,
    others5: 100,
    others6: 100,
    unknown: 100,
    isSaba: false
  },
  {
    betType: 'Test1',
    total: 100,
    mj: 100,
    m1: 100,
    m2: 100,
    m3: 100,
    m4: 100,
    others: 100,
    others1: 100,
    others2: 100,
    others3: 100,
    others4: 100,
    others5: 100,
    others6: 100,
    unknown: 100,
    isSaba: false
  },
  {
    betType: 'Saba',
    total: 100,
    mj: 100,
    m1: 100,
    m2: 100,
    m3: 100,
    m4: 100,
    others: 100,
    others1: 100,
    others2: 100,
    others3: 100,
    others4: 100,
    others5: 100,
    others6: 100,
    unknown: 100,
    isSaba: true
  },
  {
    betType: 'Test2',
    total: 100,
    mj: 100,
    m1: 100,
    m2: 100,
    m3: 100,
    m4: 100,
    others: 100,
    others1: 100,
    others2: 100,
    others3: 100,
    others4: 100,
    others5: 100,
    others6: 100,
    unknown: 100,
    isSaba: true
  }
];

const setup = (reportData, isOpenTable, status = 'idle') => {
  const initialState = {
    report: {
      reportData: reportData,
      isOpenTable: isOpenTable,
      status,
      columnOrder: {
        columnId: 'name',
        subColumnId: 'subName',
        orderType: 'asc'
      }
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

  it('should render Loading, when status is pending', async () => {
    setup(mockData, true, 'pending');
    const { container } = render(<ReportTable />);
    await waitFor(() => {
      expect(container.querySelector('.loading-process')).toBeInTheDocument;
    });
  });

  it('should render table content, ReactTable', async () => {
    setup(mockData, true);
    const { container } = render(<ReportTable />);
    await waitFor(() => {
      expect(container.querySelector('.table-content')).toBeInTheDocument;
    });
  });
});

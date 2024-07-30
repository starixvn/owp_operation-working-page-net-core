import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import { DateColumnInfo, MonthOvertimeData } from './Helper/TypeNative';
import MonthTabContent from './MonthTabContent';
import { screen } from '@testing-library/react';
import '@testing-library/jest-dom';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

const mockDispatch = jest.fn();
const mockData: MonthOvertimeData = {
  dateColumns: [
    { date: '01-Oct', isWeekend: true, dayOfWeek: 'Sun' },
    { date: '02-Oct', isWeekend: false, dayOfWeek: 'Mon' },
    { date: '03-Oct', isWeekend: false, dayOfWeek: 'Tue' }
  ] as DateColumnInfo[],
  totalHours: 123,
  totalHoursByDate: [0, 1, 2.5],
  traderOvertime: [
    {
      location: 'CTI',
      trader: 'ClgCandy',
      total: 50.5,
      overtime: {
        '02-Oct': { hour: 0, isOffDay: false },
        '03-Oct': { hour: 2, isOffDay: true }
      }
    },
    { location: 'CTI', trader: 'sa', total: 0, overtime: {} }
  ]
};

const setUp = (isLoading: boolean, isFulfill: boolean) => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      monthOTReducer: {
        data: mockData,
        isLoading,
        isFulfill
      },
      filter: {
        status: 'idle'
      }
    })
  );
};

describe('test MonthTabContent', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('loading=true, should render Loading and Filter', () => {
    setUp(true, false);
    const { container } = render(<MonthTabContent />);
    const filterElement = container.querySelector('.filter');
    const loadingElement = container.querySelector('.loading-process');
    expect(filterElement).toBeInTheDocument();
    expect(loadingElement).toBeInTheDocument();
  });

  it('loading=false, isFullFill = true, should not render Loading, render Filter', () => {
    setUp(false, true);
    const { container } = render(<MonthTabContent />);
    const tableElement = container.querySelector('.ReactTable');
    expect(tableElement).toBeInTheDocument();
    expect(container.getElementsByClassName('filter').length).toBe(1);
  });

  it('click submit button, should call dispatch 1 time', () => {
    setUp(false, true);
    render(<MonthTabContent />);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
    const button = screen.getByText('Search');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(3);
  });

  it('click export button, should call dispatch 1 time', () => {
    setUp(false, true);
    global = Object.create(window);
    const url = 'http://dummy.com';
    Object.defineProperty(window, 'location', {
      value: { href: url }
    });
    Object.defineProperty(window, 'netCoreRootUrl', {
      value: 'v1/test'
    });
    render(<MonthTabContent />);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
    const button = screen.getByText('Export');
    fireEvent.click(button);
    const year = new Date().getFullYear();
    const month = new Date().getMonth() + 1;
    expect(window.location.href).toEqual(
      `/api/OvertimeReport/export-report-by-month?Location=all&Trader=&Month=${month}&Year=${year}`
    );
  });
});

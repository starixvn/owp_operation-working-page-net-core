import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import { OverTimeDataByYear } from '../typeNative';
import { screen } from '@testing-library/react';
import YearTabContent from './YearTabContent';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

const mockDispatch = jest.fn();
const mockData: OverTimeDataByYear[] = [
  {
    location: 'CTI',
    trader: 'ClgCandy',
    total: 50.5,
    overtimeData: [1, 2, 3, null, 1, 1, 2, 3.5, null, null, 2.99, 0]
  },
  {
    location: 'CTI',
    trader: 'sa',
    total: 0,
    overtimeData: [
      1,
      2,
      3,
      null,
      null,
      null,
      null,
      null,
      null,
      2.99,
      null,
      null
    ]
  }
];

const setUp = (status: string) => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      yearTab: {
        status,
        overtimeData: mockData
      },
      filter: {
        status: 'idle'
      }
    })
  );
};

describe('test YearTabContent', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('status=pending, should render Loading', () => {
    setUp('pending');
    const { container } = render(<YearTabContent />);
    const loadingElement = container.querySelector('.loading-process');
    expect(loadingElement).toBeInTheDocument();
  });

  it('status=idle, should not render Loading, render Filter, ReportTable', () => {
    setUp('idle');
    const { container } = render(<YearTabContent />);
    const tableElement = container.querySelector('.ReactTable');
    expect(tableElement).toBeInTheDocument();
    expect(container.getElementsByClassName('filter').length).toBe(1);
  });

  it('click submit button, should call dispatch 1 time', () => {
    setUp('idle');
    render(<YearTabContent />);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
    const button = screen.getByText('Search');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(3);
  });

  it('click export button, should call dispatch 1 time', () => {
    setUp('idle');
    global = Object.create(window);
    const url = 'http://dummy.com';
    Object.defineProperty(window, 'location', {
      value: { href: url }
    });
    Object.defineProperty(window, 'netCoreRootUrl', {
      value: 'v1/test'
    });
    const year = new Date().getFullYear();
    render(<YearTabContent />);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
    const button = screen.getByText('Export');
    fireEvent.click(button);
    expect(window.location.href).toEqual(
      `v1/test/api/OvertimeReport/export-report-by-year?Location=all&Trader=&Year=${year}`
    );
  });
});

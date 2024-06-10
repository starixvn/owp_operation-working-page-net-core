import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import { DateColumnInfo, MonthOvertimeData } from '../Helper/TypeNative';
import ReportTable from './ReportTable';

const mockData: MonthOvertimeData = {
  dateColumns: [
    { date: '01-Oct', isWeekend: true, dayOfWeek: 'Sun' },
    { date: '02-Oct', isWeekend: false, dayOfWeek: 'Mon' },
    { date: '03-Oct', isWeekend: false, dayOfWeek: 'Tue' },
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

describe('test ReportTable', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render table content, ReactTable', () => {
    const { container } = render(<ReportTable data={mockData} />);
    const tableElement = container.querySelector('.ReactTable');
    expect(tableElement).toBeInTheDocument();
  });
});

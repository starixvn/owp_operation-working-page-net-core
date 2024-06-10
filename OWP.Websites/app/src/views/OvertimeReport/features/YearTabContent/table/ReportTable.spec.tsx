import { render } from '@testing-library/react';
import { OverTimeDataByYear } from '../../typeNative';
import ReportTable from './ReportTable';

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

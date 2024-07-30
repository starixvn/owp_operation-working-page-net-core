import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import ReportTable from './ReportTable';
import { useDispatch, useSelector } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    reportReducer: {
      pending: false,
      isDisplayTable: true,
      isClearAll: false
    }
  })
);
const mockProps = {
  columnId: '',
  columnSelected: [
    {
      id: 'MIXED PRODUCTS',
      name: 'MIXED PRODUCTS',
      order: 1,
      subColumn: [
        { name: 'FT_Corner', id: 'FTCorner' },
        { name: 'FT_Corner (5ML)', id: 'FTCorner5ML' },
        { name: 'Total Matches', id: 'TotalMixedProduct' }
      ]
    }
  ],
  data: [
    {
      FTCorner: 1,
      FTCorner5ML: 1,
      TotalMixedProduct: 1,
      UserId: 360418,
      Username: 'starix1'
    }
  ],
  orderType: ''
};

describe('test ReportTable', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render table content, ReactTable', () => {
    const { container } = render(<ReportTable {...mockProps} />);
    const tableElement = container.querySelector('.ReactTable');
    expect(tableElement).toBeInTheDocument();
  });
});

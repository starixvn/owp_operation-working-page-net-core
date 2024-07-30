import { useSelector, useDispatch } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import OrderHeader from './OrderHeader';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    chatBoard: {
      modelByRowIndex: {}
    },
    filter: {
      filterDate: new Date()
    }
  })
);

describe('OrderHeader under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should call onOrder ASC', () => {
    const props = {
      headerName: 'LeagueName',
      sortState: {
        orderType: 'ORDER_ASC',
        columnKey: 'LeagueName'
      },
      orderType: 'ORDER_ASC',
      onOrder: jest.fn(),
      columnKey: '',
      sortFunc: jest.fn()
    };
    const { container } = render(<OrderHeader {...props} />);
    const button = container.querySelector('.sort-icon');

    fireEvent.click(button, { bubbles: true });
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });

  it('should call onOrder DESC', () => {
    const props = {
      headerName: 'LeagueName',
      sortState: {
        orderType: 'ORDER_DESC',
        columnKey: 'LeagueName'
      },
      orderType: 'ORDER_DESC',
      onOrder: jest.fn(),
      columnKey: 'LeagueName',
      sortFunc: jest.fn()
    };
    const { container } = render(<OrderHeader {...props} />);
    const button = container.querySelector('.sort-icon');

    fireEvent.click(button, { bubbles: true });
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });
});

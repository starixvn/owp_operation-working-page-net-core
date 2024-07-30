import { fireEvent, render } from '@testing-library/react';
import { useDispatch } from 'react-redux';
import OrderHeader from './OrderHeader';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

describe('OrderHeader under test', () => {
  it('UseDispatch should have called when click', () => {
    const props = {
      columnId: 'test',
      columnName: 'columnName',
      rest: {
        columnId: 'test',
        orderType: 'ORDER_ASC'
      }
    };
    const { container } = render(<OrderHeader {...props} />);
    const button = container.querySelector('.sort-icon');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(1);
  });
  it('Difference orderType', () => {
    const props = {
      columnId: 'testA',
      columnName: 'columnName',
      rest: {
        columnId: 'test',
        orderType: 'ORDER_ASC'
      }
    };
    const { container } = render(<OrderHeader {...props} />);
    const button = container.querySelector('.sort-icon');
    expect(button).toBeInTheDocument();
  });
});

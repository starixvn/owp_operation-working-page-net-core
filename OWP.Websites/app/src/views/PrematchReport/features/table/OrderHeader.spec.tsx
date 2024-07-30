import OrderHeader from './OrderHeader';
import { useDispatch, useSelector } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

const mockDispatch = jest.fn();

const setUp = () => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      report: {
        columnOrder: {
          columnId: 'name',
          subColumnId: 'subname',
          orderType: 'asc'
        }
      }
    })
  );
};

describe('test for OrderHeader', () => {
  beforeEach(() => {
    setUp();
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  const props = {
    columnId: 'name',
    subColName: 'subname',
    subColId: 'name'
  };

  it('should OrderHeader render correct', () => {
    const { container } = render(<OrderHeader {...props} />);
    const span = container.querySelector('.sort-icon');
    expect(span).toBeInTheDocument;
  });

  it('should handle sort when click icon', () => {
    const { container } = render(<OrderHeader {...props} />);
    const icon = container.querySelector('div');
    expect(fireEvent.click(icon!)).toHaveLength;
    expect(mockDispatch).toBeCalledTimes(1);
  });
});

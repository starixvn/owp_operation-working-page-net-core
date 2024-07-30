import OrderHeader from './OrderHeader';
import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';

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
          subColumnId: 'subName',
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
    subColName: 'subName',
    subColId: 'name'
  };

  it('should OrderHeader render correct', () => {
    const { container } = render(<OrderHeader {...props} />);
    const span = container.querySelector('span');
    expect(span).toBeInTheDocument();
  });

  it('should handle sort when click icon', () => {
    const { container } = render(<OrderHeader {...props} />);
    const div = container.querySelector('div');
    fireEvent.click(div!);
    expect(mockDispatch).toBeCalledTimes(1);
  });
});

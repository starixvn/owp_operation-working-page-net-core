import { useDispatch, useSelector } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';
import IconCreateMessage from './IconCreateMessage';

const mockDispatch = jest.fn();
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const setup = HasWrite => {
  const state = {
    policy: {
      permission: {
        HasWrite
      }
    }
  };
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
  useDispatch.mockReturnValue(mockDispatch);
  const dataSource = [1, 2, 3];
  const rowIndex = 0;
  return { dataSource, rowIndex };
};

describe('IconCreateMessage under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render PlusCircleIcon', () => {
    const props = setup(true);
    const { container } = render(<IconCreateMessage {...props} />);
    const element = container.querySelector('.icon-plus');
    expect(element).toBeInTheDocument();
  });

  it('when have permission and click Icon press, should dispatch 6 action', () => {
    const props = setup(true);
    const { container } = render(<IconCreateMessage {...props} />);
    const button = container.querySelector('.icon-plus');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(7);
  });

  it('when dont have permission, can not see Icon create', () => {
    const props = setup(false);
    const { container } = render(<IconCreateMessage {...props} />);
    const button = container.querySelector('.mdi-icon');
    expect(button).toBeNull();
  });
});

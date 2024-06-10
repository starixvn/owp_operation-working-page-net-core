import ExpandBtn from './ExpandBtn';
import { useDispatch } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);
jest.mock('../../app/store');

describe('test ExpandBtn', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render ExpandBtn', () => {
    const { container } = render(<ExpandBtn />);

    const element = container.querySelector('.btn-outline-primary');
    expect(element).toBeInTheDocument();
  });

  it('should dispatch when click button', () => {
    const { container } = render(<ExpandBtn />);

    const button = container.querySelector('.btn-outline-primary');
    fireEvent.click(button);
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });
});

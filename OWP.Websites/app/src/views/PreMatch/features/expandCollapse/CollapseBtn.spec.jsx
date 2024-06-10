import CollapseBtn from './CollapseBtn';
import { useDispatch } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));
jest.mock('../../app/store');

const mockedDispatch = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);

describe('test CollapseBtn', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render CollapseBtn', () => {
    const { container } = render(<CollapseBtn />);

    const element = container.querySelector('.btn-outline-primary');
    expect(element).toBeInTheDocument();
  });

  it('should dispatch when click button', () => {
    const { container } = render(<CollapseBtn />);

    const button = container.querySelector('.btn-outline-primary');
    fireEvent.click(button);
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });
});

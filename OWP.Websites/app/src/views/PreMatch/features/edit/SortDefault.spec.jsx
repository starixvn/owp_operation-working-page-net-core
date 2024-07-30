import { fireEvent, render, screen } from '@testing-library/react';
import { useDispatch } from 'react-redux';
import SortDefault from './SortDefault';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));
const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);

describe('SortDefault under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render button-submit', () => {
    const { container } = render(<SortDefault />);
    const element = container.querySelector('.button-sort');
    expect(element).toBeInTheDocument();
  });
  it('should dispatch 2 time, when click button-sort', () => {
    const { container } = render(<SortDefault />);
    const element = container.querySelector('.button-sort');
    fireEvent.click(element);
    expect(mockedDispatch).toHaveBeenCalledTimes(2);
  });
});

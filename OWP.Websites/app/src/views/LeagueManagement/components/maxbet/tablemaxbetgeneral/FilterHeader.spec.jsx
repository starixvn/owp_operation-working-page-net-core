import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import FilterHeader from './FilterHeader';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

// const setUp = isClearAll => {
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    showHidePopupFilterReducer: {
      filterKey: 'a',
      isDisplay: true
    }
  })
);
// };

describe('test FilterHeader component', () => {
  const props = {
    title: 'abc',
    filterKey: 'a',
    keyword: 'b'
  };

  beforeEach(() => {
    mockDispatch.mockReset();
  });

  it('render correct component', () => {
    const { container, getByText } = render(<FilterHeader {...props} />);

    expect(getByText('abc')).toBeInTheDocument();
  });

  it('should handle search when click button', () => {
    const { container } = render(<FilterHeader {...props} />);

    const btn = container.querySelector('.btn-search');

    fireEvent.click(btn);

    expect(mockDispatch).toBeCalled();
  });

  it('should handle clear when click clear all button', () => {
    const { container } = render(<FilterHeader {...props} />);

    const btn = container.querySelector('.clear-all');

    fireEvent.click(btn);

    expect(mockDispatch).toBeCalled();
  });

  it('should handle clear when click close-popup button', () => {
    const { container } = render(<FilterHeader {...props} />);

    const btn = container.querySelector('.close-popup');

    fireEvent.click(btn);

    expect(mockDispatch).toBeCalled();
  });

  it('should handle onHandleClickSubmit when enter input', () => {
    const { container } = render(<FilterHeader {...props} />);
    const input = container.querySelector('.input-field');
    fireEvent.change(input, { target: { value: 'bc' } });

    expect(input.getAttribute('value')).toEqual('bc');

    fireEvent.keyPress(input, { key: 'Enter', code: 13, charCode: 13 });
    expect(mockDispatch).toBeCalledTimes(2);
  });
});

import { fireEvent, getAllByText, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import Tv from './Tv';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
useSelector.mockImplementation(selectorFn => selectorFn({}));
describe('test Tv component', () => {
  const props = {
    hasTV: true,
    tv: 'abc',
    actionType: {}
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { getAllByText } = render(<Tv {...props} />);
    expect(getAllByText('Has TV')[0]).toBeInTheDocument();
  });

  it('should handle change checkbox ', () => {
    const { container, getByText } = render(<Tv {...props} />);
    const input = container.querySelector('.form-check-input');
    fireEvent.click(input, { target: { checked: true } });

    expect(mockDispatch).toBeCalled();
  });

  it('should handle blur input ', () => {
    const { container, getByText } = render(<Tv {...props} />);
    const input = container.querySelector('.input-field');
    fireEvent.change(input, { target: { value: 'bac' } });

    fireEvent.blur(input);

    expect(mockDispatch).toBeCalled();
  });
});

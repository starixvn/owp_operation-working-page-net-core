import { render, screen } from '@testing-library/react';
import { useDispatch } from 'react-redux';
import MarketFilter from './MarketFilter';
import SelectFilterFactory from './SelectFilterFactory';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));

jest.mock('./SelectFilterFactory', () => {
  return jest.fn(() => null);
});

const mockedDispatch = jest.fn();
const onSelectedChange = jest.fn();
useDispatch.mockReturnValue(mockedDispatch);
const props = {
  onSelectedChange: onSelectedChange
};
describe('test MarketFilter', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render SelectFilterFactory', () => {
    render(<MarketFilter props={props} />);

    expect(screen.getByText('Market')).toBeInTheDocument();
    expect(SelectFilterFactory).toBeCalledTimes(1);
  });
});

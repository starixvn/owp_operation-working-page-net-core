import { render, act, fireEvent } from '@testing-library/react';
import Category from './Category';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import useStore from '../../app/store';

jest.mock('../../../Shared/Service/Service');
jest.mock('../../hook/useQueryDropdownOption');


const mockUseQueryDropdownOption = useQueryDropdownOption as jest.MockedFunction<typeof useQueryDropdownOption>;
const setFilter = jest.fn();

describe('<Category/> under test', () => {
  afterEach(() => {
    jest.resetAllMocks();
  });
  beforeEach(() => {
    useStore.setState({ setFilter });
  });

  it('should show loading while query option', () => {
    mockUseQueryDropdownOption.mockReturnValue({
      loading: true,
      options: []
    });
    const { container } = render(<Category />);
    const element = container.querySelector('.loading-process');
    expect(element).toBeInTheDocument();
  });

  it('should call setFilter when initialize', () => {
    mockUseQueryDropdownOption.mockReturnValue({
      loading: false,
      options: [{ value: 1, label: 'ABC' }]
    });
    render(<Category />);
    expect(setFilter).toBeCalledTimes(1);
  });

  it('should call setFilter when change selected', () => {
    mockUseQueryDropdownOption.mockReturnValue({
      loading: false,
      options: [{ value: 1, label: 'ABC' }, { value: 2, label: 'ABC1' }]
    });
    const { container, getByText } = render(<Category />);
    const element = container.querySelector('.dropdown-heading-dropdown-arrow');
    element && fireEvent.click(element);
    act(() => {
      const itemTarget = getByText('ABC1');
      fireEvent.click(itemTarget);
    });
    element && fireEvent.click(element);
    expect(getByText('None, ABC')).toBeInTheDocument();
  });
});

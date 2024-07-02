import { render, act, fireEvent } from '@testing-library/react';
import Status from './Status';
import useStore from '../../app/store';

jest.mock('../../../Shared/Service/Service');
jest.mock('../../hook/useQueryDropdownOption');


const setFilter = jest.fn();

describe('<Status/> under test', () => {
  afterEach(() => {
    jest.resetAllMocks();
  });
  beforeEach(() => {
    useStore.setState({ setFilter });
  });


  it('should call setFilter when initialize', () => {
    render(<Status />);
    expect(setFilter).toBeCalledTimes(1);
  });

  it('should call setFilter when change selected', () => {
    const { container, getAllByText, getByText } = render(<Status />);
    const element = container.querySelector('.dropdown-heading-dropdown-arrow');
    element && fireEvent.click(element);
    act(() => {
      const itemTarget = getByText('Inactive');
      fireEvent.click(itemTarget);
    });
    element && fireEvent.click(element);
    expect(getByText('Active')).toBeInTheDocument();
  });
});

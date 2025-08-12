import { createRef } from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import EditComponent from './EditComponent';
import { REQUEST_STATUS } from '../../../Shared/Constants';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  createRef: jest.fn()
}));
const ref = { current: { timePickerRef: { picker: { value: '222' } } } };
const mockedDispatch = jest.fn();

jest.useFakeTimers().setSystemTime(new Date('2020-09-16'));

useDispatch.mockReturnValue(mockedDispatch);

const setUp = isSetFilterDate => {
  const props = {
    filter: {
      filterDate: isSetFilterDate
        ? new Date('2020-09-16T08:36:55.768Z')
        : undefined
    },
    header: {
      info: {
        UpdatedTime: new Date('2020-09-16T08:36:55.768Z'),
        UserName: 'Carlos'
      },
      startTimeOriginal: undefined,
      startTimeFilter: undefined
    },
    deletedLeague: {
      leagueDeleted: [],
      loading: REQUEST_STATUS.idle,
      error: null
    },
    policy: {
      permission: 'sa'
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};
describe('EditComponent under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render button-submit', () => {
    setUp(false);
    const { container } = render(<EditComponent />);
    const element = container.querySelector('.button-submit');
    expect(element).toBeInTheDocument();
  });

  it('should dispatch 1 time, when click btn submit', () => {
    setUp(true);
    const { container } = render(<EditComponent />);

    // Click on the dropdown toggle to open it
    const dropdownToggle = container.querySelector('.button-submit');
    fireEvent.click(dropdownToggle);

    // Click on the Manual Assign menu item
    const manualAssignItem = screen.getByText('Manual Assign');
    fireEvent.click(manualAssignItem);

    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });
});

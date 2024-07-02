import { createRef } from 'react';
import { fireEvent, render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import EditFilterNightShift from './EditFilterNightShift';
import { REQUEST_STATUS } from '../../../Shared/Constants';

jest.mock('../../app/store');
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

useDispatch.mockReturnValue(mockedDispatch);

const setUp = (isSetFilterDate, startTimeOriginal = undefined) => {
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
      startTimeOriginal,
      startTimeFilter: undefined
    },
    deletedLeague: {
      leagueDeleted: [],
      loading: REQUEST_STATUS.idle,
      error: null
    },
    policy: {
      permission: { Username: 'sa', HasRead: true, HasWrite: true }
    },
    dayShift: {
      isShowUnreadMessage: true
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};
describe('EditFilterNightShift under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when EditMode, should render DateTimeFilterCommon, ShowUnreadMessageCheckBox', () => {
    setUp(true);
    const { container } = render(<EditFilterNightShift />);
    const element = container.querySelector('.edit-mode');
    expect(element).toBeInTheDocument();
  });
  it('when EditMode, should handle submit click, dispatch 3 times', () => {
    setUp(true);
    const { container } = render(<EditFilterNightShift />);
    const element = container.querySelector('.button-submit');
    fireEvent.click(element);
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });

  it('when EditMode, startTimeRef has value, should handle submit click, dispatch 3 times', () => {
    setUp(true, '2022-06-21 16:20');
    const { container } = render(<EditFilterNightShift />);
    const element = container.querySelector('.button-submit');
    fireEvent.click(element);
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });
});

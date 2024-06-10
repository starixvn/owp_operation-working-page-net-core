import { createRef } from 'react';
import { fireEvent, render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import ShowUnreadMessageCheckBox from './ShowUnreadMessageCheckBox';
import { REQUEST_STATUS } from '../../../Shared/Constants';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('../../app/store');
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  createRef: jest.fn()
}));
const ref = { current: { timePickerRef: { picker: { value: '222' } } } };
const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);

const setUp = (isSetFilterDate, isShowUnreadMessage = true) => {
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
    },
    dayShift: {
      isShowUnreadMessage: isShowUnreadMessage
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

describe('ShowUnreadMessageCheckBox under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 checkbox', () => {
    setUp(false);
    const { container } = render(<ShowUnreadMessageCheckBox />);
    const element = container.querySelector('.checkbox');
    expect(element).toBeInTheDocument();
  });

  it('should render 1 correct `Show unread message only`', () => {
    setUp(false);
    const { container } = render(<ShowUnreadMessageCheckBox />);
    const element = container.querySelector('label');
    expect(element.textContent).toEqual('Show unread message only');
  });

  it('when uncheck checkbox, should dispatch unshowUnreadMessageOnly', () => {
    setUp(true);
    const { container } = render(<ShowUnreadMessageCheckBox />);
    const element = container.querySelector('Input');
    fireEvent.click(element);
    const dispatchAcion = {
      payload: undefined,
      type: 'dayShift/unShowUnreadMessageOnly'
    };

    expect(mockedDispatch).toHaveBeenCalledTimes(1);
    expect(mockedDispatch).toBeCalledWith(dispatchAcion);
  });

  it('when check checkbox, should dispatch showUnreadMessageOnly', () => {
    setUp(true, false);
    const { container } = render(<ShowUnreadMessageCheckBox />);
    const element = container.querySelector('Input');
    fireEvent.click(element);
    const dispatchAcion = {
      payload: undefined,
      type: 'dayShift/showUnreadMessageOnly'
    };
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
    expect(mockedDispatch).toBeCalledWith(dispatchAcion);
  });
});

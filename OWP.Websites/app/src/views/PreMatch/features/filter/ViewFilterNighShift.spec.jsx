import { act, render } from '@testing-library/react';
import axios from 'axios';
import { noop } from 'lodash';
import { createRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import Spinner from '../../../components/Spinner';
import ViewFilterNighShift from './ViewFilterNighShift';

jest.mock('../../app/store');
jest.mock('axios');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  createRef: jest.fn()
}));
jest.mock('../../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());
const promise = Promise.resolve();

const ref = { current: { timePickerRef: { picker: { value: '222' } } } };
const mockedDispatch = jest.fn();

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
        UserName: 'esther'
      },
      startTimeOriginal: undefined,
      startTimeFilter: undefined,
      startTimeRespond: '15:59:00'
    },
    deletedLeague: {
      leagueDeleted: [],
      loading: REQUEST_STATUS.idle,
      error: null
    },
    policy: {
      permission: { ActorId: 8, Username: 'sa', HasRead: true, HasWrite: true }
    },
    dayShift: {
      isShowUnreadMessage: false
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};
describe('ViewFilterNightShift under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when waiting query server, should render Loading', () => {
    setUp(false);
    const { container } = render(<ViewFilterNighShift />);
    const element = container.querySelector('.loading-process');
    expect(element).toBeInTheDocument();
  });

  it('should render FilterPrematchViewer', async () => {
    setUp(true);
    const responseData = {
      data: [{ StartTime: '15:59:00' }]
    };
    axios.post.mockResolvedValue(responseData);
    const { container } = render(<ViewFilterNighShift />);
    await act(async () => {
      await promise;
    });
    const element = container.querySelector('.left-filter-dayshift');
    expect(element).toBeInTheDocument();
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });

  it('responseData=null should call dispatch 3 times', async () => {
    setUp(true);
    const responseData = {
      data: []
    };
    axios.post.mockResolvedValue(responseData);
    render(<ViewFilterNighShift />);
    await act(async () => {
      await promise;
    });
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });
});

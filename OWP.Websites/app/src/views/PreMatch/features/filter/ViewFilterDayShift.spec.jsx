import { render } from '@testing-library/react';
import { createRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import ViewFilterDayShift from './ViewFilterDayShift';
import ToastrWrapper from '../../../components/ToastrWrapper';

jest.mock('../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  createRef: jest.fn()
}));
jest.mock('../../../components/ToastrWrapper');
const mockToastWrapper = jest.fn();
ToastrWrapper.showError.mockImplementation(() => mockToastWrapper);

const ref = { current: { timePickerRef: { picker: { value: '222' } } } };
const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);

const setUp = isSetFilterDate => {
  const props = {
    filter: {
      filterDate: isSetFilterDate
        ? new Date('2020-09-16T08:36:55.768Z')
        : undefined,
      matchOffers: [1, 2, 3]
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
      permission: { ActorId: 8, Username: 'sa', HasRead: true, HasWrite: true }
    },
    dayShift: {
      isShowUnreadMessage: false
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};
describe('ViewFilterDayShift under test', () => {
  beforeEach(() => {
    setUp(true);
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should call mockedDispatch', () => {
    render(<ViewFilterDayShift />);
    expect(mockedDispatch).toHaveBeenCalledTimes(4);
  });
});

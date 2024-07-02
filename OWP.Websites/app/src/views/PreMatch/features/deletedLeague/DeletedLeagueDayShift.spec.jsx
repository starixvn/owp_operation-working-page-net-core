import { createRef } from 'react';
import { render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import DeletedLeagueDayShift from './DeletedLeagueDayShift';
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

const setUp = data => {
  const props = {
    header: {
      info: {
        UpdatedTime: new Date('2020-09-16T08:36:55.768Z'),
        UserName: 'Carlos'
      },
      startTimeOriginal: undefined,
      startTimeFilter: undefined
    },
    deletedLeague: {
      leagueDeleted: data,
      loading: REQUEST_STATUS.idle,
      error: null
    },
    policy: {
      permission: 'sa'
    },
    dayShift: {
      isShowUnreadMessage: true
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

describe('DeletedLeagueDayShift under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 tables, when have deleted league', () => {
    setUp([
      { PreMatchLeagueId: 1 },
      { PreMatchLeagueId: 2 },
      { PreMatchLeagueId: 3 }
    ]);
    const { container } = render(<DeletedLeagueDayShift />);
    const element = container.querySelector('.table-content');
    expect(element).toBeInTheDocument();
  });
});

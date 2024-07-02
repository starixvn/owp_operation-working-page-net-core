import { createRef } from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import RecoverCell from './RecoverCell';
import RestoreIcon from 'mdi-react/RestoreIcon';
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
    },
    dayShift: {
      isShowUnreadMessage: true
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

describe('RecoverCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 RestoreIcon', () => {
    setUp(false);
    const { container } = render(<RecoverCell PreMatchLeagueId={1} />);
    const element = container.querySelector('.icon-restore');
    expect(element).toBeInTheDocument();
  });

  it('when click Restore Icon, should dispatch 3 time', () => {
    setUp(true);
    const { container } = render(<RecoverCell PreMatchLeagueId={1} />);
    const element = container.querySelector('.icon-restore');
    fireEvent.click(element);
    expect(mockedDispatch).toHaveBeenCalledTimes(3);
  });
});

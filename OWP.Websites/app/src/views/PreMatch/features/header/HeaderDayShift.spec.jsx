import { render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import HeaderDayShift from './HeaderDayShift';
import { REQUEST_STATUS } from '../../../Shared/Constants';

const createProps = isEdit => {
  return { isEdit, isDay: false };
};
jest.mock('../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

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
    }
  };
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

describe('HeaderDayShift under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when EditMode, should render 2 button View Deleted Leagues, View Change Log', () => {
    setUp(true);
    const props = createProps(true);
    const { container } = render(<HeaderDayShift {...props} />);
    const element = container.querySelector('.btn-right');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('View Deleted Leagues View Change Log');
  });

  it('when ViewMode, should not render 2 btn View Deleted Leagues, View Change Log', () => {
    setUp(true);
    const props = createProps(false);
    const { container } = render(<HeaderDayShift {...props} />);
    const element = container.querySelector('.btn-right');
    expect(element).toBeNull();
  });
});

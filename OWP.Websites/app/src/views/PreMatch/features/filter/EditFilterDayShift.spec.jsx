import { render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import EditFilterDayShift from './EditFilterDayShift';
import { REQUEST_STATUS } from '../../../Shared/Constants';

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
    },
    policy: {
      permission: { Username: 'sa', HasRead: true, HasWrite: true }
    },
    dayShift: {
      isShowUnreadMessage: true
    }
  };
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};
describe('EditFilterDayShift under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when EditMode, should render DateTimeFilterCommon, ShowUnreadMessageCheckBox', () => {
    setUp(true);
    const { container } = render(<EditFilterDayShift />);
    const element = container.querySelector('.edit-mode');
    expect(element).toBeInTheDocument();
    expect(container.querySelector('.filter')).toBeInTheDocument();
  });
});

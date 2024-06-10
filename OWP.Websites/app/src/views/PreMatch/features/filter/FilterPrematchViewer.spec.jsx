import { createRef } from 'react';
import { fireEvent, render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import FilterPrematchViewer from './FilterPrematchViewer';
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
const employeeRef = {
  employeeRef: jest.fn(),
  leagueRef: jest.fn()
};
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
    dayShift: {
      isShowUnreadMessage: true
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

const mockOnSubmit = jest.fn();
const mockOnClickMyAssignee = jest.fn();
const defaultProps = {
  ref: employeeRef,
  onSubmit: mockOnSubmit,
  onClickMyAssignee: mockOnClickMyAssignee,
  isDisableEmp: false
};

describe('FilterPrematchViewer under test', () => {
  beforeEach(() => {
    setUp(true);
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when wating query server, should render Loading', () => {
    const { container } = render(
      <FilterPrematchViewer {...defaultProps} model={{ filterDate: null }} />
    );
    const element = container.querySelector('.loading-process');
    expect(element).toBeInTheDocument();
  });
  it('should render left-filter-dayshift', () => {
    const { container } = render(
      <FilterPrematchViewer
        {...defaultProps}
        model={{ filterDate: new Date('2020-09-16T08:36:55.768Z') }}
      />
    );

    expect(mockOnSubmit).toHaveBeenCalledTimes(1);
    const element = container.querySelector('.left-filter-dayshift');
    expect(element).toBeInTheDocument();
  });

  it('should call OnClickMyAssignee, when click employee icon', () => {
    const { container } = render(
      <FilterPrematchViewer
        {...defaultProps}
        model={{ filterDate: new Date('2020-09-16T08:36:55.768Z') }}
      />
    );

    const element = container.querySelector('.emp-icon');
    expect(element).toBeInTheDocument();
    fireEvent.click(element);
    expect(mockOnClickMyAssignee).toHaveBeenCalledTimes(1);
  });
});

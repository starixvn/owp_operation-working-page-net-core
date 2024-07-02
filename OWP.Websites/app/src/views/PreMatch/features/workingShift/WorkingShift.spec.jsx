import { createRef, useState } from 'react';
import { render } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import WorkingShift from './WorkingShift';
import { REQUEST_STATUS } from '../../../Shared/Constants';

jest.mock('../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));

const setState = jest.fn();
const ref = { current: { timePickerRef: { picker: { value: '222' } } } };
const mockedDispatch = jest.fn();
const mockedEffect = jest.fn();

const setUp = dataChangedLog => {
  const props = {
    filter: {
      filterDate: new Date('2020-09-16T08:36:55.768Z')
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
    },
    changedLog: {
      dataWorkingShift: {
        Logs: null,
        IsLastPage: undefined
      },
      loading: REQUEST_STATUS.idle,
      error: null
    },
    workingShift: {
      workingShiftData: dataChangedLog,
      loading: REQUEST_STATUS.idle,
      error: null
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockedDispatch);
};

describe('WorkingShift under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 Loading, when workingShiftData = null || waitting  get data', () => {
    setUp(null);
    const { container } = render(<WorkingShift isDay />);
    const element = container.querySelector('.loading-process');
    expect(element).toBeInTheDocument();
  });
  it('should render 1 WorkingShiftTable, when have data', () => {
    setUp([1, 2, 3]);
    const { container } = render(<WorkingShift isDay />);
    const element = container.querySelector('.table-content');
    expect(element).toBeInTheDocument();
  });
  it('first render, should execute useEffect 1 time', () => {
    setUp([1, 2, 3]);
    render(<WorkingShift isDay={false} />);
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });
});

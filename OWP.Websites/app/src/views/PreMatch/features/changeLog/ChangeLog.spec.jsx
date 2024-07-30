import { fireEvent, render } from '@testing-library/react';
import { createRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import ChangeLog from './ChangeLog';

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

const mockSetState = jest.fn();
const ref = { current: { timePickerRef: { picker: { value: '222' } } } };
const mockDispatch = jest.fn();

const setUp = dataChangedLog => {
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
      dataChangeLog: {
        Logs: dataChangedLog,
        IsLastPage: undefined
      },
      loading: REQUEST_STATUS.idle,
      error: null
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
  useState.mockImplementation(init => [init, mockSetState]);
  useDispatch.mockReturnValue(mockDispatch);
};

describe('ChangeLog under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 Loading, when log = null || waitting  get data', () => {
    setUp(null);
    const { container } = render(<ChangeLog isDay />);
    const element = container.querySelector('.loading-process');
    expect(element).toBeInTheDocument();
  });

  it('should render 1 Pagination, when have data', () => {
    setUp([1, 2, 3]);
    const { container } = render(<ChangeLog isDay />);
    const element = container.querySelector('.paging-move-btn');
    expect(element).toBeInTheDocument();
    expect(mockDispatch).toHaveBeenCalledTimes(1);
  });

  it('when click PageChanged, should execute useEffect', () => {
    setUp([1, 2, 3]);
    const { getAllByRole } = render(<ChangeLog isDay={false} />);
    const element = getAllByRole('button');
    fireEvent.click(element[2]);
    expect(mockSetState).toHaveBeenCalledTimes(1);
  });

  it('when click PageSizeChange, should set new state 1 time', () => {
    setUp([1, 2, 3]);
    const { container, getByText } = render(<ChangeLog isDay={false} />);
    const element = container.querySelector('.basic-multi-select');
    fireEvent.focus(element);
    fireEvent.keyDown(container.querySelector('input'), {
      key: 'ArrowDown',
      code: 40
    });
    fireEvent.click(getByText('100'));
    expect(mockSetState).toHaveBeenCalledTimes(1);
  });
});

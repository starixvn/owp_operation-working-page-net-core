import { createRef, useState, useEffect } from 'react';
import { fireEvent, render } from '@testing-library/react';
import { useSelector, useDispatch, useStore } from 'react-redux';
import DayShiftSaver from './DayShiftSaver';
import Spinner from '../../../components/Spinner';
import { noop } from 'lodash';
import { post } from '../../ImportPrematchHelperNative';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { setImmediate } from 'timers';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useStore: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn(),
  useEffect: jest.fn()
}));
jest.mock('../../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

jest.mock('../../ImportPrematchHelperNative');
post.mockResolvedValue([]);
jest.mock('../../../components/ToastrWrapper');
ToastrWrapper.showInfo.mockImplementation(() => noop());

const setState = jest.fn();
const ref = { current: { timePickerRef: { picker: { value: '222' } } } };
const mockedDispatch = jest.fn();

const flushPromises = () => {
  return new Promise(resolve => setImmediate(resolve));
};

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
      dataDayShiftSaver: {
        Logs: dataChangedLog,
        IsLastPage: undefined
      },
      loading: REQUEST_STATUS.idle,
      error: null
    }
  };
  createRef.mockReturnValue(ref);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
  useStore.mockImplementation(() => {
    return {
      getState: jest.fn().mockImplementation(() => {
        return {
          policyReducer: {
            policyData: {
              generalPolicy: {
                HasWrite: true
              }
            }
          },
          dayShift: {
            isShowUnreadMessage: true,
            entities: [],
            prematchIdUpdated: 1,
            prematchIdDeleted: 2
          },
          filter: {
            filterDate: new Date()
          },
          auto: {
            autoMode: false,
            assignments: []
          }
        };
      })
    };
  });
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockedDispatch);
};

describe('DayShiftSaver under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 SaveComponent', () => {
    setUp(null);
    const { container } = render(<DayShiftSaver />);
    const element = container.querySelector('.button-cancel-save');
    expect(element).toBeInTheDocument();
  });

  it('should click cancel, should execute Dispatch 3 time', () => {
    setUp(null);
    const { container } = render(<DayShiftSaver />);
    const button = container.querySelector('.button-cancel');
    fireEvent.click(button);
    expect(mockedDispatch).toHaveBeenCalledTimes(7);
  });

  it('should click save, should execute Dispatch 4 time', async () => {
    setUp(null);
    const { container } = render(<DayShiftSaver />);
    const button = container.querySelector('.button-submit');
    await fireEvent.click(button);
    await flushPromises();
    expect(mockedDispatch).toBeCalledTimes(8);
  });
});

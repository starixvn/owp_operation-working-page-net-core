import { createRef, useState, useEffect } from 'react';
import { fireEvent, render } from '@testing-library/react';
/**
 * @jest-environment jsdom
 */

import { setImmediate } from 'timers';
import { useSelector, useDispatch, useStore } from 'react-redux';
import NightShiftSaver from './NightShiftSaver';
import axios from 'axios';
import { noop } from 'lodash';
import Spinner from '../../../components/Spinner';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { buildModelNightShiftSaver } from './modelSaveNightShiftBuilder';
import { REQUEST_STATUS } from '../../../Shared/Constants';

const flushPromises = () => {
  return new Promise(resolve => setImmediate(resolve));
};

jest.mock('../../app/store');
jest.mock('axios');
jest.mock('../../helper/preMatchHelper');
jest.mock('./modelSaveNightShiftBuilder');
jest.mock('../../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());
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
jest.mock('../../../components/ToastrWrapper');
const object = { value: true };
ToastrWrapper.confirmYesNo.mockImplementation(() => object);

const setState = jest.fn();
const ref = { current: { timePickerRef: { picker: { value: '222' } } } };
const mockedDispatch = jest.fn();
const mockedEffect = jest.fn();

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
      dataNightShiftSaver: {
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
          }
        };
      })
    };
  });
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockedDispatch);
  useEffect.mockImplementationOnce(mockedEffect);
};

describe('NightShiftSaver under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 SaveComponent', () => {
    setUp(null);
    const { container } = render(<NightShiftSaver />);
    const element = container.querySelector('.button-cancel-save');
    expect(element).toBeInTheDocument();
  });
  it('should click cancel, should dispatch', () => {
    setUp(null);
    const { container } = render(<NightShiftSaver />);
    const element = container.querySelector('.button-cancel');
    fireEvent.click(element);
    expect(mockedDispatch).toBeCalled();
  });

  it('message is empty, should call dispatch when clisk save', async () => {
    setUp(null);
    axios.post.mockImplementation(() => noop());
    buildModelNightShiftSaver.mockReturnValue({
      postModel: {
        url: 'api/Prematch-Assignment/save',
        PrematchIdDeleted: [],
        PrematchUpdated: [
          {
            Blacklist: false,
            LeagueName: 'AUSTRALIA FOOTBALL QUEENSLAND PREMIER LEAGUE'
          }
        ],
        Date: 'October 9, 2020 9:51 AM',
        StartTime: '15:50'
      },
      message: '',
      startTimeChange: true
    });
    const { container } = render(<NightShiftSaver />);
    const button = container.querySelector('.button-submit');
    fireEvent.click(button, { preventDefault: jest.fn() });
    await flushPromises();
    expect(mockedDispatch).toBeCalled();
  });

  it('message is valid, should call dispatch when clisk save', async () => {
    setUp(null);
    axios.post.mockImplementation(() => noop());
    buildModelNightShiftSaver.mockReturnValue({
      postModel: {
        url: 'api/Prematch-Assignment/save',
        PrematchIdDeleted: [],
        PrematchUpdated: [
          {
            Blacklist: false,
            LeagueName: 'AUSTRALIA FOOTBALL QUEENSLAND PREMIER LEAGUE'
          }
        ],
        Date: 'October 9, 2020 9:51 AM',
        StartTime: '15:50'
      },
      message: 'abc',
      startTimeChange: true
    });
    const { container } = render(<NightShiftSaver />);
    const button = container.querySelector('.button-submit');
    await fireEvent.click(button, { preventDefault: jest.fn() });
    await flushPromises();
    expect(mockedDispatch).toBeCalled();
  });
});

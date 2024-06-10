import { useState } from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import WorkingShiftComponent from './WorkingShiftComponent';
import { REQUEST_STATUS } from '../../../Shared/Constants';

jest.mock('../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));

const setState = jest.fn();
const mockedDispatch = jest.fn();

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
      dataWorkingShiftComponent: {
        Logs: null,
        IsLastPage: undefined
      },
      loading: REQUEST_STATUS.idle,
      error: null
    },
    WorkingShiftComponent: {
      WorkingShiftComponentData: dataChangedLog,
      loading: REQUEST_STATUS.idle,
      error: null
    }
  };
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockedDispatch);
};

describe('WorkingShiftComponent under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 1 working-shift', () => {
    setUp(null);
    const { container } = render(<WorkingShiftComponent isDay />);
    const element = container.querySelector('.working-shift');
    expect(element).toBeInTheDocument();
  });
  // it('when click button-close, should call set state ', () => {
  //   setUp([1, 2, 3]);
  //   const { container } = render(<WorkingShiftComponent isDay />);
  //   const element = container.querySelector('.button-close');
  //   fireEvent.click(element);
  //   expect(setState).toHaveBeenCalledTimes(1);
  // });

  it('when click button-working-shift, should call set state ', () => {
    setUp([1, 2, 3]);
    const { container } = render(<WorkingShiftComponent isDay />);
    const element = container.querySelector('.button-working-shift');
    fireEvent.click(element);
    // screen.getByText('button-close');
    expect(setState).toHaveBeenCalledTimes(1);
  });
});

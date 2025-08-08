import { useState } from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import { getByRefName, authorize } from '../service/Service';
import Filter from './Filter';
import { useSelector, useDispatch } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));
const mockDispatch = jest.fn();
const mockSetState = jest.fn();

jest.mock('../service/Service');

const setUp = state => {
  useDispatch.mockReturnValue(mockDispatch);
  useState.mockImplementation(init => [init, mockSetState]);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
};

const data = [{ Id: 1, Value: '1', RefName: 'abc' }];
const fullTime = {
  fullTimeView: {
    HasRead: true
  }
};
const timeRange = {
  timeRangeView: {
    HasRead: true
  }
};

describe('Filter', () => {
  beforeEach(() => {
    getByRefName.mockReturnValue(data);
    authorize.mockResolvedValue(fullTime);
    authorize.mockResolvedValue(timeRange);
  });
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should dispatch when click show unread only', () => {
    const state = {
      events: {
        isShowUnReadTicketOnly: true
      }
    };
    setUp(state);
    const { getByText, container } = render(<Filter />);
    const checkBox = screen.queryByText(/Show Unread Message Only/i);
    fireEvent.click(checkBox);
    fireEvent.click(container.querySelector('.icon-calendar'));
    fireEvent.click(getByText('15'));
    expect(mockDispatch).toHaveBeenCalledTimes(1);
    expect(mockSetState).toHaveBeenCalledTimes(6);
  });
  it('should setState when click', () => {
    const state = {
      events: {
        isShowUnReadTicketOnly: true
      }
    };
    setUp(state);
    render(<Filter />);
    const matchStatus = screen.queryByText(/Match Status/i);
    fireEvent.click(matchStatus);
    expect(mockSetState).toHaveBeenCalledTimes(5);
  });

  it('should handle submit when click', () => {
    const state = {
      events: {
        isShowUnReadTicketOnly: true
      }
    };
    setUp(state);
    render(<Filter />);
    const search = screen.queryByText(/Search/i);
    fireEvent.click(search);
    expect(mockDispatch).toHaveBeenCalledTimes(3);
  });
});

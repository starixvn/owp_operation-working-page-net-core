import { useState } from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import Tab from './Tab';
import { useDispatch, useSelector } from 'react-redux';
import {
  DateColumnInfo,
  MonthOvertimeData
} from '../ByMonthTab/Helper/TypeNative';

jest.mock('react', () => ({
  ...(jest.requireActual('react') as object),
  useState: jest.fn()
}));

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

const mockSetState = jest.fn();
const mockDispatch = jest.fn();
const mockData: MonthOvertimeData = {
  dateColumns: [
    { date: '01-Oct', isWeekend: true, dayOfWeek: 'Sun' },
    { date: '02-Oct', isWeekend: false, dayOfWeek: 'Mon' },
    { date: '03-Oct', isWeekend: false, dayOfWeek: 'Tue' }
  ] as DateColumnInfo[],
  totalHours: 123,
  totalHoursByDate: [0, 1, 2.5],
  traderOvertime: [
    {
      location: 'CTI',
      trader: 'ClgCandy',
      total: 50.5,
      overtime: {
        '02-Oct': { hour: 0, isOffDay: false },
        '03-Oct': { hour: 2, isOffDay: true }
      }
    },
    { location: 'CTI', trader: 'sa', total: 0, overtime: {} }
  ]
};

const setUp = (isLoading: boolean, isFulfill: boolean) => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useState as jest.Mock).mockImplementation(init => [init, mockSetState]);
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      monthOTReducer: {
        data: mockData,
        isLoading,
        isFulfill
      },
      filter: {
        status: 'idle'
      }
    })
  );
};
describe('test Tab', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('click tab active should call mockSetState', async () => {
    setUp(false, true);
    const { container } = render(<Tab />);
    const button = screen.getByText('By Month');
    fireEvent.click(button);
    expect(container.getElementsByClassName('active-tabs').length).toBe(1);
    await waitFor(() => {
      expect(mockSetState).toHaveBeenCalledTimes(1);
    });
  });
});

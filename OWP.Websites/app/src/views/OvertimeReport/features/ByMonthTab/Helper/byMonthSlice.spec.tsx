import reducer from './byMonthSlice';
import { getReportByMonth } from './monthTabThunkNative';
import { MonthOvertimeData } from './TypeNative';

describe('monthReportSlice under test', () => {
  it('should return the initial state', () => {
    const initState = {
      isLoading: true,
      data: {} as MonthOvertimeData,
      isFulfill: false
    };
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initState);
  });

  it('should handle getReportByMonth.pending', () => {
    const action = {
      type: getReportByMonth.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      isLoading: true,
      data: {} as MonthOvertimeData,
      isFulfill: false
    });
  });

  it('should handle getReportByMonth.fulfilled', () => {
    const data = {
      dateColumns: [
        { date: '1-Oct', isWeekend: true, dayOfWeek: 'Sun' }
      ],
      totalHours: 100
    };
    const action = {
      type: getReportByMonth.fulfilled.type,
      payload: data
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      isLoading: false,
      data: data,
      isFulfill: true
    });
  });

  it('should handle getReportByMonth.reject', () => {
    const action = {
      type: getReportByMonth.rejected.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      isLoading: false,
      data: {} as MonthOvertimeData,
      isFulfill: false
    });
  });
});

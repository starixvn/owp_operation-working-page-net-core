import reducer from './yearTabSlice';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { OverTimeDataByYear } from '../typeNative';
import { getReportByYear } from '../overTimeReportThunkNative';

describe('yearTabSlice under test', () => {
  it('should return the initial state', () => {
    const initState = {
      status: REQUEST_STATUS.pending,
      overtimeData: [] as OverTimeDataByYear[]
    };
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initState);
  });

  it('should handle getReportByYear.pending', () => {
    const action = {
      type: getReportByYear.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      status: REQUEST_STATUS.pending,
      overtimeData: [] as OverTimeDataByYear[]
    });
  });

  it('should handle getReportByYear.fulfilled', () => {
    const data = [{
      trader: 'sa',
      location: 'SMS',
      overtimeData: [],
      total: 20
    }];
    const action = {
      type: getReportByYear.fulfilled.type,
      payload: data
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      status: REQUEST_STATUS.idle,
      overtimeData: data
    });
  });

  it('should handle getReportByYear.reject', () => {
    const action = {
      type: getReportByYear.rejected.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      status: REQUEST_STATUS.idle,
      overtimeData: [] as OverTimeDataByYear[]
    });
  });
});

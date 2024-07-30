import reducer from './filterSlice';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { OverTimeDataByYear, SelectOption } from '../typeNative';
import { getLocationOptions } from '../overTimeReportThunkNative';

describe('filterSlice under test', () => {
  it('should return the initial state', () => {
    const initState = {
      status: REQUEST_STATUS.pending,
      locations: [] as SelectOption[]
    };
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initState);
  });

  it('should handle getLocationOptions.pending', () => {
    const action = {
      type: getLocationOptions.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      status: REQUEST_STATUS.pending,
      locations: [] as SelectOption[]
    });
  });

  it('should handle getLocationOptions.fulfilled', () => {
    const data: SelectOption[] = [
      { value: 1, label: 'sms' }
    ];
    const action = {
      type: getLocationOptions.fulfilled.type,
      payload: data
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      status: REQUEST_STATUS.idle,
      locations: data
    });
  });

  it('should handle getLocationOptions.reject', () => {
    const action = {
      type: getLocationOptions.rejected.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      status: REQUEST_STATUS.idle,
      locations: [] as SelectOption[]
    });
  });
});

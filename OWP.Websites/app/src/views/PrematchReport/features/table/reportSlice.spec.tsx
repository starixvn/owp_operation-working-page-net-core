import { REQUEST_STATUS } from '../../../Shared/Constants';
import { PreMatchReportData } from '../../models/report-data';
import { ColumnOrder } from '../../typeNative';
import reducer, { displayTable, sort } from './reportSlice';
import { getReportData } from './reportThunkNative';
describe('test for reportSlice', () => {
  const initialState = {
    status: REQUEST_STATUS.idle,
    reportData: [] as PreMatchReportData[],
    columnOrder: {} as ColumnOrder,
    isOpenTable: false
  };

  it('return init state', () => {
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initialState);
  });

  it('should handle when getReport pending', () => {
    const action = {
      type: getReportData.pending
    };

    expect(reducer(undefined, action)).toEqual({
      ...initialState,
      status: REQUEST_STATUS.pending
    });
  });

  it('should handle when getReport fulfilled', () => {
    const action = {
      type: getReportData.fulfilled.type,
      payload: []
    };

    expect(reducer(undefined, action)).toEqual({
      ...initialState,
      status: REQUEST_STATUS.idle
    });
  });

  it('should handle when getReport reject', () => {
    const action = {
      type: getReportData.rejected.type,
      payload: []
    };

    expect(reducer(undefined, action)).toEqual({
      ...initialState,
      status: REQUEST_STATUS.idle
    });
  });

  it('should handle when sort', () => {
    const action = {
      type: sort.type,
      payload: {
        columnId: '',
        subColumnId: '',
        orderType: 'asc'
      }
    };

    expect(reducer({ ...initialState, status: 'idle' }, action)).toEqual({
      ...initialState,
      status: REQUEST_STATUS.idle,
      columnOrder: {
        columnId: '',
        subColumnId: '',
        orderType: 'asc'
      }
    });
  });

  it('should handle when displayTable', () => {
    const action = {
      type: displayTable.type,
      payload: true
    };

    expect(reducer({ ...initialState }, action)).toEqual({
      ...initialState,
      isOpenTable: true
    });
  });
});

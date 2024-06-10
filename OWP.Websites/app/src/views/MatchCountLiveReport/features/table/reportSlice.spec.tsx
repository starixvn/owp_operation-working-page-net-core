import { REQUEST_STATUS } from '../../../Shared/Constants';
import { ReportData } from '../../models/report-data';
import { ColumnOrder } from '../../typeNative';
import reducer, { displayTable, sort } from './reportSlice';
import { getReportData } from './reportThunkNative';
describe('test for reportSlice', () => {
  const initialState = {
    status: REQUEST_STATUS.idle,
    reportData: [] as ReportData[],
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
        columnId: 'totalMatches',
        orderType: 'desc',
        subColumnId: 'm2'
      }
    };
    const fakeData = [
      {
        betType: 'Real Soccer',
        total: 100,
        no: null,
        mj: 100,
        m1: 100,
        m2: 100,
        m3: 100,
        m4: 100,
        others: 100,
        others1: 100,
        others2: 100,
        others3: 100,
        others4: 100,
        others5: 100,
        unknown: 100,
        isSaba: false
      },
      {
        betType: 'Test1',
        total: 100,
        no: 1,
        mj: 100,
        m1: 100,
        m2: 37,
        m3: 100,
        m4: 100,
        others: 100,
        others1: 100,
        others2: 100,
        others3: 100,
        others4: 100,
        others5: 100,
        unknown: 100,
        isSaba: false
      },
      {
        betType: 'Test2',
        total: 100,
        no: 2,
        mj: 100,
        m1: 100,
        m2: 63,
        m3: 100,
        m4: 100,
        others: 100,
        others1: 100,
        others2: 100,
        others3: 100,
        others4: 100,
        others5: 100,
        unknown: 100,
        isSaba: false
      },
      {
        betType: 'Saba',
        total: 100,
        no: null,
        mj: 100,
        m1: 100,
        m2: 100,
        m3: 100,
        m4: 100,
        others: 100,
        others1: 100,
        others2: 100,
        others3: 100,
        others4: 100,
        others5: 100,
        unknown: 100,
        isSaba: true
      },
      {
        betType: 'Test3',
        total: 100,
        no: 1,
        mj: 100,
        m1: 100,
        m2: 100,
        m3: 100,
        m4: 100,
        others: 100,
        others1: 100,
        others2: 100,
        others3: 100,
        others4: 100,
        others5: 100,
        unknown: 100,
        isSaba: true
      }
    ];
    const state = reducer(
      {
        reportData: fakeData as ReportData[],
        isOpenTable: false,
        columnOrder: {} as ColumnOrder,
        status: 'idle'
      },
      action
    );
    expect(state).toEqual({
      reportData: [
        {
          betType: 'Real Soccer',
          total: 100,
          no: null,
          mj: 100,
          m1: 100,
          m2: 100,
          m3: 100,
          m4: 100,
          others: 100,
          others1: 100,
          others2: 100,
          others3: 100,
          others4: 100,
          others5: 100,
          unknown: 100,
          isSaba: false
        },
        {
          betType: 'Test2',
          total: 100,
          no: 2,
          mj: 100,
          m1: 100,
          m2: 63,
          m3: 100,
          m4: 100,
          others: 100,
          others1: 100,
          others2: 100,
          others3: 100,
          others4: 100,
          others5: 100,
          unknown: 100,
          isSaba: false
        },
        {
          betType: 'Test1',
          total: 100,
          no: 1,
          mj: 100,
          m1: 100,
          m2: 37,
          m3: 100,
          m4: 100,
          others: 100,
          others1: 100,
          others2: 100,
          others3: 100,
          others4: 100,
          others5: 100,
          unknown: 100,
          isSaba: false
        },
        {
          betType: 'Saba',
          total: 100,
          no: null,
          mj: 100,
          m1: 100,
          m2: 100,
          m3: 100,
          m4: 100,
          others: 100,
          others1: 100,
          others2: 100,
          others3: 100,
          others4: 100,
          others5: 100,
          unknown: 100,
          isSaba: true
        },
        {
          betType: 'Test3',
          total: 100,
          no: 1,
          mj: 100,
          m1: 100,
          m2: 100,
          m3: 100,
          m4: 100,
          others: 100,
          others1: 100,
          others2: 100,
          others3: 100,
          others4: 100,
          others5: 100,
          unknown: 100,
          isSaba: true
        }
      ],
      status: REQUEST_STATUS.idle,
      columnOrder: {
        columnId: 'totalMatches',
        orderType: 'desc',
        subColumnId: 'm2'
      },
      isOpenTable: false
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

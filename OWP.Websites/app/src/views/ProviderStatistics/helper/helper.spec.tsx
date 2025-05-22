import { ColumnOrder } from '../typeNative';
import { sortDataReport, buildRequestURL, validDateRange } from './helper';

describe('test helper', () => {
  it('valid Date Range', () => {
    const fromDate = new Date(2021, 4, 12);
    const toDate = new Date(2021, 8, 12);
    const result = validDateRange(fromDate, toDate);
    expect(result).toEqual(false);
  });

  it('sortData', () => {
    const columnOrder = {
      columnId: 'totalMatches',
      orderType: 'desc',
      subColumnId: 'm2'
    } as ColumnOrder;
    const currentData = [
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
        isSaba: false
      }
    ];
    const result = sortDataReport(currentData, columnOrder);

    const expected = [
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
      }
    ];
    expect(result).toEqual(expected);
  });
});

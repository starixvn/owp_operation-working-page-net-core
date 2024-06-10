import {
  buildTimeRangeForFromDate,
  buildTimeRangeForToDate,
  getCurrentTimeGmtPlus7,
  buildFromDateToDateDefaultWithServerTime,
  buildFilterDate,
  convertFrom24hTo12h,
  addDays,
  buildDateString,
  formatTimeOsLastUpdate
} from './DateTimeHelper';

describe('DateTimeHelper', () => {
  describe('Build time range for from date correct', () => {
    it('add date', () => {
      const result = addDays(new Date('2018-12-17'), 1);
      const expectValue = new Date('2018-12-18');
      expect(expectValue).toEqual(result);
    });
    it('buildDateString', () => {
      const result = buildDateString(new Date('2018-12-17'));
      const expectValue = '12-17-2018';
      expect(expectValue).toEqual(result);
    });
    it('Build min date for from date correct', () => {
      const { minFromDate } = buildTimeRangeForFromDate('2019-12-17');
      const minFromDateExpect = new Date('2018-12-17');
      expect(minFromDate).toEqual(minFromDateExpect);
    });

    it('Build max date for from date correct', () => {
      const { maxFromDate } = buildTimeRangeForFromDate('2019-12-17');
      const maxFromDateExpect = new Date('2019-12-17');
      expect(maxFromDate).toEqual(maxFromDateExpect);
    });
  });

  it('getCurrentTimeGmtPlus7 should return time', () => {
    const dateTimeGmtPlus7String = new Date().toLocaleString('en-US', {
      timeZone: 'Asia/Ho_Chi_Minh'
    });
    const dateTimeGmtPlus7 = new Date(dateTimeGmtPlus7String);
    const expecttime = {
      hour: dateTimeGmtPlus7.getHours(),
      minute: dateTimeGmtPlus7.getMinutes()
    };
    const result = getCurrentTimeGmtPlus7();
    expect(result).toEqual(expecttime);
  });

  describe('Build time range for to date correct', () => {
    it('Build min date for to date correct', () => {
      const { minToDate } = buildTimeRangeForToDate('2019-12-17');
      const minToDateExpect = new Date('2019-12-17');
      expect(minToDate).toEqual(minToDateExpect);
    });

    it('Build max date for to date correct', () => {
      const { maxToDate } = buildTimeRangeForToDate('2019-12-17', new Date());
      const currentDate = new Date();
      const maxToDateExpect = new Date(currentDate);
      maxToDateExpect.setDate(currentDate.getDate());
      expect(maxToDate.getDate()).toEqual(maxToDateExpect.getDate());
    });
  });

  it('Should build FromDate ToDate correct', () => {
    const time = buildFromDateToDateDefaultWithServerTime(
      'Wed Sep 09 2020 10:57:47 GMT+0700 (Indochina Time)'
    );
    expect(JSON.stringify(time)).toEqual(
      JSON.stringify({
        fromDate: '2020-08-10T03:57:47.000Z',
        toDate: '2020-09-09T03:57:47.000Z'
      })
    );
  });

  it('Should build filter date correct', () => {
    const date = buildFilterDate();
    expect(date.getDate()).toEqual(new Date().getDate());
  });

  it('Should convert from 24h to 12h', () => {
    const timeFormat12h = convertFrom24hTo12h('22:12:59', true);
    expect(JSON.stringify(timeFormat12h)).toEqual(
      JSON.stringify('10:12:59 PM')
    );
  });
  it('Should formatTimeOsLastUpdate', () => {
    const timeFormat12h = formatTimeOsLastUpdate('2023-04-05T10:43:39.467624Z');
    expect(timeFormat12h).toEqual('April 05, 2023 10:43 AM');
  });
});

import DateTimeHelper from './DateTimeHelper';

const buildFromDateToDateDefault = () => {
  const currentDate = new Date();
  const fromDate = new Date(currentDate);
  const toDate = new Date(currentDate);
  fromDate.setDate(currentDate.getDate() - 31);
  toDate.setDate(currentDate.getDate() - 1);
  return {
    fromDate,
    toDate
  };
};

describe('DateTimeHelper', () => {
  describe('build from date, to date correct', () => {
    it('build from date correct', () => {
      let { fromDate } = DateTimeHelper.buildFromDateToDateDefault();
      const dateExpect = buildFromDateToDateDefault();
      fromDate = fromDate.toISOString().split('T')[0];
      const fromDateExpect = dateExpect.fromDate.toISOString().split('T')[0];
      expect(fromDate).toEqual(fromDateExpect);
    });
    it('build to date correct', () => {
      let { toDate } = DateTimeHelper.buildFromDateToDateDefault();
      const dateExpect = buildFromDateToDateDefault();
      toDate = toDate.toISOString().split('T')[0];
      const toDateExpect = dateExpect.toDate.toISOString().split('T')[0];
      expect(toDate).toEqual(toDateExpect);
    });
  });

  describe('Build time range for to date correct', () => {
    it('get YYYYMMDDString from datetime correct', () => {
      const dateToStringActual = DateTimeHelper.getMMDDYYYYStringFromDateTime(
        new Date('Thu Dec 19 2019 13:41:42 GMT+0700 (Indochina Time)')
      );
      expect(dateToStringActual).toEqual('12/19/2019');
    });
  });
});

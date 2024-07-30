import Utils from './Utils';

describe('Utils', () => {
  it('should have default rootUrl is empty', () => {
    expect(Utils.rootUrl).toEqual('');
  });

  it('should have formatDate is DD/MM/YYYY', () => {
    const date = new Date('2018-12-17T03:24:00');
    const expectedResult = '12/17/2018';
    expect(Utils.formatDate(date)).toEqual(expectedResult);
  });

  it('formatDateTime should return MM/DD/YYYY hh:ss A', () => {
    const date = new Date('2018-12-17T03:24:00');
    const expectedResult = '12/17/2018 03:00 AM';
    expect(Utils.formatDateTime(date)).toEqual(expectedResult);
  });

  it('formatDateTimeGMT8 should return MM/DD/YYYY hh:ss A', () => {
    const date = new Date('2018-12-17T03:24:00');
    const expectedResult = '12/17/2018 04:24 AM';
    expect(Utils.formatDateTimeGMT8(date)).toEqual(expectedResult);
  });

  it('isSameOrAfter should return true', () => {
    const today = new Date();
    const tomorrow = new Date(new Date().setDate(today.getDate() + 1));
    expect(Utils.isSameOrAfter(tomorrow, today)).toEqual(true);
  });

  it('isSameOrAfter should return true', () => {
    const today = new Date();
    expect(Utils.isSameOrAfter(new Date(), today)).toEqual(true);
  });

  it('isSameOrAfter should return false', () => {
    const today = new Date();
    const yesterday = new Date(new Date().setDate(today.getDate() - 1));
    expect(Utils.isSameOrAfter(yesterday, today)).toEqual(false);
  });

  it('toLowerCaseForFilter should return lowerCase', () => {
    const mock = 'AbcKeywordAAA';
    const actual = Utils.toLowerCaseForFilter(mock);
    const expected = 'abckeywordaaa';
    expect(actual).toEqual(expected);

    const actualNullValue = Utils.toLowerCaseForFilter(undefined);
    expect(actualNullValue).toEqual('');
  });

  it('convertThousandToK should return value if small than 1', () => {
    const mock = 0;
    const actual = Utils.convertThousandToK(mock, true);
    const expected = '';
    expect(actual).toEqual(expected);
  });

  it('convertThousandToK should return K if larger than 1', () => {
    const mock = 3500;
    const actual = Utils.convertThousandToK(mock, true);
    const expected = '3.5K';
    expect(actual).toEqual(expected);
  });

  it('convertSpreadToPercent should return empty if value null', () => {
    const mock = undefined;
    const actual = Utils.convertSpreadToPercent(mock, true);
    const expected = '';
    expect(actual).toEqual(expected);
  });

  it('convertSpreadToPercent should return percent if value larger than 0', () => {
    const mock = 0.15;
    const actual = Utils.convertSpreadToPercent(mock, true);
    const expected = '15%';
    expect(actual).toEqual(expected);
  });

  it('convertSpreadToPercent should return percent if value larger than 0', () => {
    const mock = 0.15;
    const actual = Utils.convertSpreadToPercent(mock, false);
    const expected = '15';
    expect(actual).toEqual(expected);
  });

  it('test convertJsonToObject success', () => {
    const testData = '{ "name": "John", "age": 30, "car": null }';
    const result = Utils.convertJsonToObject(testData);
    expect(result).toEqual({ name: 'John', age: 30, car: null });
  });

  it('test convertJsonToObject fail', () => {
    const testData = { name: 'John', age: 30, car: null };
    const result = Utils.convertJsonToObject(testData);
    expect(result).toEqual({});
  });

  it('test getDataFromLocalStore when localStorageKey invalid, expect return empty', () => {
    const resultForParamNull = Utils.getDataFromLocalStore(null);
    expect(resultForParamNull).toBeNull();
  });

  it('test getDataFromLocalStore success when localStorageKey valid', () => {
    const localStorageKey = 'key_1';
    const data = ' abc';
    localStorage.setItem(localStorageKey, data);
    const result = Utils.getDataFromLocalStore(localStorageKey);
    expect(result).toBe(data);
  });

  it('test formatNumberWithCommas return undefined when input is null', () => {
    const result = Utils.formatNumberWithCommas(null);
    expect(result).toBe(undefined);
  });
});

import {
  isSecondAcceptValue,
  isColumnKey,
  getGroupColumnId,
  formatMaxbetValue,
  validateMaxbet,
  covertToNumber,
  buildURL
} from './helper';

describe('test helper', () => {
  it('test isSecondAcceptValue', () => {
    expect(true).toEqual(isSecondAcceptValue('secondAccept_Main'));
    expect(false).toEqual(isSecondAcceptValue('id'));
  });

  it('test isColumnKey', () => {
    expect(true).toEqual(isColumnKey('fT_HDP'));
    expect(false).toEqual(isColumnKey('id'));
  });
});

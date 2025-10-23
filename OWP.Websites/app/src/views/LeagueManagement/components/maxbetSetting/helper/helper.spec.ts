import {
  isSecondAcceptValue,
  isFullTimeMaxbetReferColumn
} from './helper';

describe('test helper', () => {
  it('test isSecondAcceptValue', () => {
    expect(true).toEqual(isSecondAcceptValue('secondAccept_Main'));
    expect(false).toEqual(isSecondAcceptValue('id'));
  });

  it('test isFullTimeMaxbetReferColumn', () => {
    expect(true).toEqual(isFullTimeMaxbetReferColumn('fT_HDP'));
    expect(false).toEqual(isFullTimeMaxbetReferColumn('id'));
  });
});

import {
  calculateRowLevel,
  mapRowLevelToHight,
  calculateRowHight,
  calculateTableHeight,
  calculateTableWidth
} from './PrematchTableHelper';
import { DefaultTableStyle } from './constants/preMatchConst';

describe('PrematchTableHelper under test', () => {
  it('calculateRowLevel no initial level', () => {
    const result = calculateRowLevel('testString', 15);

    expect(result).toEqual(1);
  });

  it('calculateRowLevel have initial level', () => {
    const result = calculateRowLevel('testString', 15, 1);

    expect(result).toEqual(2);
  });

  it('calculateRowLevel have inputString is empty', () => {
    const result = calculateRowLevel('', 15, 1);

    expect(result).toEqual(1);
  });

  it('mapRowLevelToHight should map exactly rowLevel to Hight', () => {
    const listRowLevelTestCase = [1, 2, 3, 0, 5];

    const listExpectHightTestCase = [38, 68, 90, 38, 108];

    const listResultHightTestCase = [];

    listRowLevelTestCase.forEach(level =>
      listResultHightTestCase.push(mapRowLevelToHight(level))
    );

    expect(listResultHightTestCase).toEqual(listExpectHightTestCase);
  });

  it('calculateTableHeight with dataLength > 27', () => {
    const dataLength = 100;
    const result = calculateTableHeight(dataLength);
    expect(result).toEqual(750);
  });

  it('calculateTableHeight with dataLength =< 27', () => {
    const dataLength = 10;
    const result = calculateTableHeight(dataLength);
    const { rowHeight } = DefaultTableStyle;
    const expectedValue = rowHeight + dataLength * rowHeight + 20;
    expect(result).toEqual(expectedValue);
  });
});

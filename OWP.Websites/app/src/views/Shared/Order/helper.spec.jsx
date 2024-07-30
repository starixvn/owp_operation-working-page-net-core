import { getValue, compareNumbers } from './helper';

describe('helper under test', () => {
  it('method getValue, should get value from input', () => {
    const { valueSource, valueDest } = getValue('test1', 'test2');
    expect(valueSource).toEqual('test1');
    expect(valueDest).toEqual('test2');
  });
  it('method getValue, should return ``, when input = null', () => {
    const { valueSource, valueDest } = getValue(null, null);
    expect(valueSource).toEqual('');
    expect(valueDest).toEqual('');
  });
  it('method compareNumbers, compare 1 vs 2 ', () => {
    const result = compareNumbers(1, 2);
    expect(result).toEqual(-1);
  });
  it('method compareNumbers, compare 2 vs 1', () => {
    const result = compareNumbers(2, 1);
    expect(result).toEqual(1);
  });
  it('method compareNumbers, compare 2 vs 2 ', () => {
    const result = compareNumbers(2, 2);
    expect(result).toEqual(0);
  });
  it('method compareNumbers, compare null vs 1 ', () => {
    const result = compareNumbers(null, 1);
    expect(result).toEqual(-1);
  });
});

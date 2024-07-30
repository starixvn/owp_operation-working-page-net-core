import { isEqual } from 'lodash';
import { compareNumbers, getValue } from './helper';

const compareValues = (key, order = 'asc') => {
  return (source, dest) => {
    if (
      !Object.prototype.hasOwnProperty.call(source, key) ||
      !Object.prototype.hasOwnProperty.call(dest, key)
    ) {
      return 0;
    }

    const { valueSource, valueDest } = getValue(source[key], dest[key]);
    const isCompareString = isEqual(typeof valueSource, 'string');

    let comparison = 0;
    comparison = isCompareString
      ? valueSource.localeCompare(valueDest)
      : compareNumbers(valueSource, valueDest);

    return isEqual(order.toLowerCase(), 'desc') ? comparison * -1 : comparison;
  };
};

export default compareValues;

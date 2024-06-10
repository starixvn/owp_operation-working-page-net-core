import { isEqual } from 'lodash';

const getValue = (source, dest) => {
  let valueSource = isEqual(typeof data, 'string')
    ? source.toLowerCase()
    : source;

  let valueDest = isEqual(typeof dest, 'string') ? dest.toLowerCase() : dest;

  valueSource = valueSource ?? '';
  valueDest = valueDest ?? '';

  return { valueSource, valueDest };
};

const compareNumbers = (a, b) => {
  let comparison = 0;
  if (a > b) {
    comparison = 1;
  } else if (a < b) {
    comparison = -1;
  }
  return comparison;
};

export { getValue, compareNumbers };

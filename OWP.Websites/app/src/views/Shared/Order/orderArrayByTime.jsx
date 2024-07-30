import { isEqual } from 'lodash';
import { direction } from './orderConst.json';

const orderArray = (arraySource, orderFollowKey) =>
  (compareValue, directionArg) =>
    arraySource.sort((firstValue, secondValue) =>
      compareValue(firstValue[orderFollowKey], secondValue[orderFollowKey], directionArg));

const compareValue = (source, dest, directionArg) => {
  const { asc, desc } = direction;
  const schemaComparer = {
    [asc]: new Date(source) - new Date(dest),
    [desc]: new Date(dest) - new Date(source)
  };

  return schemaComparer[directionArg];
};

const orderArrayByTime = (arraySource, orderFollowKey, directionArg) => {
  const { asc, desc } = direction;
  const directionHasDetected = [asc, desc].some(item => isEqual(item, directionArg));
  return directionHasDetected
    ? orderArray(arraySource, orderFollowKey)(compareValue, directionArg)
    : arraySource;
};

export default orderArrayByTime;

import { isEqual } from 'lodash';
import compareValues from '../../Shared/Order/compareValues';

const executeOrder = (data, columnId, sortType) => {
  return isEqual(sortType, 'ORDER_ASC')
    ? data.sort(compareValues(columnId, 'asc'))
    : data.sort(compareValues(columnId, 'desc'));
};

export { executeOrder };

import { isEqual } from 'lodash';
import { KOTime, Sport } from './TableSchema.json';
import orderArrayByTime from '../../../Shared/Order/orderArrayByTime';
import compareValues from '../../../Shared/Order/compareValues';
import { DataSource } from './OSCell/Share/Type';

const orderBy = (dataSource: Required<Array<DataSource>>, columnKey: string, orderType: string): Required<Array<DataSource>> => {
  if (isEqual(columnKey, KOTime.columnKey)) {
    return orderArrayByTime(dataSource, 'dateTime', orderType);
  }
  if (isEqual(columnKey, Sport.columnKey)) {
    const type = isEqual(orderType, 'ORDER_ASC') ? 'asc' : 'desc';
    return dataSource.sort(compareValues('sport', type));
  }
  return dataSource;
};

export { orderBy };
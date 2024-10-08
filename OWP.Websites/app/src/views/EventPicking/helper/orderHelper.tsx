import { orderBy } from 'lodash';
import TDataTable, {
  TMultipleOrder,
  TOrderDirection,
  TOverArrayToObject
} from '../types/dataTableType';

const getOrderDirection = (direction?: TOrderDirection) => {
  if (!direction || direction === 'none') {
    return false;
  }

  return direction === 'asc' ? 'asc' : 'desc';
};

const convertTOverArrayToObject = (entity: TDataTable[]) => {
  const tOverArrayToObject: TOverArrayToObject = [];

  entity.forEach(element => {
    let turnoverObject = {};
    element.turnoverPeriod.forEach(t => {
      turnoverObject = {
        ...turnoverObject,
        [t.title]: t.value
      };
    });
    tOverArrayToObject.push({
      infoSiteMatchId: element.infoSiteMatchId,
      childMatchId: element.childMatchId,
      ...turnoverObject
    });
  });
  return tOverArrayToObject;
};

const rebuildEntity = (
  orderTOver: TOverArrayToObject,
  entity: TDataTable[]
) => {
  return orderTOver.reduce((previousValue, currentValue) => {
    const { infoSiteMatchId, childMatchId } = currentValue;
    const target = entity.find(
      e =>
        e.infoSiteMatchId === infoSiteMatchId && e.childMatchId === childMatchId
    );
    target && previousValue.push(target);
    return previousValue;
  }, [] as TDataTable[]);
};

const executeOrderTable = (entity: TDataTable[], orderInfo: TMultipleOrder) => {
  const tOverArrayToObject = convertTOverArrayToObject(entity);
  const orderTOver = orderBy(
    tOverArrayToObject,
    ['1m', '3m', '6m', '12m'],
    [
      getOrderDirection(orderInfo.one_m?.direction),
      getOrderDirection(orderInfo.three_m?.direction),
      getOrderDirection(orderInfo.six_m?.direction),
      getOrderDirection(orderInfo.twelve_m?.direction)
    ]
  );
  return rebuildEntity(orderTOver, entity);
};

export { executeOrderTable };

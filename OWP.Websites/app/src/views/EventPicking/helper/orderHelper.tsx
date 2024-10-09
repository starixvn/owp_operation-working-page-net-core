import { orderBy } from 'lodash';
import TDataTable, {
  TOrder,
  TOrderDirection,
  TOverArrayToObject
} from '../types/dataTableType';

const getOrderDirection = (direction?: TOrderDirection) => {
  if (!direction) {
    return false;
  }

  return direction === 'asc' ? 'asc' : 'desc';
};

const convertTOverArrayToObject = (entity: TDataTable[]) => {
  const tOverArrayToObject: TOverArrayToObject[] = [];

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
  orderTOver: TOverArrayToObject[],
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

const executeOrderTable = (entity: TDataTable[], orderInfo: TOrder) => {
  const entitiesHasTOver = entity.filter(
    e =>
      e.turnoverPeriod.length &&
      e.turnoverPeriod.some(t => t.title === orderInfo.key && t.value)
  );
  const entitiesNoTOver = entity.filter(
    e =>
      !e.turnoverPeriod.length ||
      e.turnoverPeriod.some(t => t.title === orderInfo.key && !t.value)
  );
  const tOverArrayToObject = convertTOverArrayToObject(entitiesHasTOver);
  const orderTOver = orderBy(
    tOverArrayToObject,
    [orderInfo.key],
    [getOrderDirection(orderInfo.direction)]
  );
  return rebuildEntity(orderTOver, entity).concat(entitiesNoTOver);
};

export { executeOrderTable };

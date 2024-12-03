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

const convertTOverArrayToObject = (entities: TDataTable[], orderInfo: TOrder) => {
    const items: TOverArrayToObject[] = [];

    entities.forEach(element => {
        let item = {};
        element[orderInfo.columnKey].forEach(t => {
            item = {
                ...item,
                [t.title]: t.value
            };
        });
        items.push({
            infoSiteMatchId: element.infoSiteMatchId,
            childMatchId: element.childMatchId,
            ...item
        });
    });
    return items;
};

const rebuildEntity = (
    orderTOver: TOverArrayToObject[],
    entities: TDataTable[]
) => {
    return orderTOver.reduce((previousValue, currentValue) => {
        const { infoSiteMatchId, childMatchId } = currentValue;
        const target = entities.find(e => e.infoSiteMatchId === infoSiteMatchId && e.childMatchId === childMatchId);
        if (target) {
            previousValue.push(target);
        }

        return previousValue;
    }, [] as TDataTable[]);
};

const executeOrderTable = (entities: TDataTable[], orderInfo: TOrder) => {
    if (!orderInfo.columnKey || orderInfo.columnKey.length === 0) {
        return entities;
    }

    const entitiesHasValue = entities.filter(
        e => e[orderInfo.columnKey].length && e[orderInfo.columnKey].some(t => t.title === orderInfo[orderInfo.columnKey].key && t.value !== null)
    );

    const entitiesNoValue = entities.filter(
        e => !e[orderInfo.columnKey].length || e[orderInfo.columnKey].some(t => t.title === orderInfo[orderInfo.columnKey].key && t.value === null)
    );

    const tOverArrayToObject = convertTOverArrayToObject(entitiesHasValue, orderInfo);
    const orderItems = orderBy(
        tOverArrayToObject,
        [orderInfo[orderInfo.columnKey].key],
        [getOrderDirection(orderInfo[orderInfo.columnKey].direction)]
    );

    return rebuildEntity(orderItems, entities).concat(entitiesNoValue);
};

export { executeOrderTable };

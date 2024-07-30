import handleNoOfferAllMatch from '../helper/noOfferAllMatchHelper';
import updateConfirmAllEvent from '../helper/confirmedHelper';
import { ORDER_ASC, ORDER_DESC } from './TableSchema.json';
import { isEqual } from 'lodash';
const buildColumnValueByColumnName = (
  columnKey,
  isNoOfferAll,
  isConfirmAll
) => {
  const columnMappings = {
    'No #': {
      checked: isNoOfferAll,
      onCheckChange: handleNoOfferAllMatch
    },
    'Checked': {
      checked: isConfirmAll,
      onCheckChange: updateConfirmAllEvent
    }
  };

  return columnMappings[columnKey] || {};
};

const sortData = (orderType, entities, columnKey) => {
  if (columnKey === '') {
    return entities;
  }
  const hasValue = entities.filter(
    x => x[capitalizeFirstLetter(columnKey)] !== null
  );
  const hasNullValue = entities.filter(
    x => x[capitalizeFirstLetter(columnKey)] === null
  );
  let hasValueSorted = [];
  if (isEqual(orderType, ORDER_ASC)) {
    hasValueSorted = sortDataAsc(hasValue, columnKey);
  }
  if (isEqual(orderType, ORDER_DESC)) {
    hasValueSorted = sortDataDesc(hasValue, columnKey);
  }

  return [...hasValueSorted, ...hasNullValue];
};

const sortDataAsc = (entities, columnKey) =>
  [...entities].sort(
    (src, dest) =>
      src[capitalizeFirstLetter(columnKey)] -
      dest[capitalizeFirstLetter(columnKey)]
  );

const sortDataDesc = (entities, columnKey) =>
  [...entities].sort(
    (src, dest) =>
      dest[capitalizeFirstLetter(columnKey)] -
      src[capitalizeFirstLetter(columnKey)]
  );
const capitalizeFirstLetter = value =>
  value.charAt(0).toLowerCase() + value.slice(1);
export { buildColumnValueByColumnName, sortDataAsc, sortDataDesc, sortData };

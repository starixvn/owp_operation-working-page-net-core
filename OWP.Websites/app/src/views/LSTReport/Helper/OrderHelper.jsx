import isEqual from 'lodash.isequal';

const orderMatchAsc = collection =>
  collection.sort(
    (firstValue, secondValue) => firstValue.Match - secondValue.Match
  );

const orderMatchDesc = collection =>
  collection.sort(
    (firstValue, secondValue) => secondValue.Match - firstValue.Match
  );

const preProcessBeforeCompare = value => (isEqual(value, '-') ? 0 : value);

const orderSubColumnAsc = (collection, type, subtype) =>
  collection.sort((firstItem, secondItem) => {
    const firstValue = preProcessBeforeCompare(firstItem[type][subtype]);
    const secondValue = preProcessBeforeCompare(secondItem[type][subtype]);
    return firstValue - secondValue;
  });

const orderSubColumnDesc = (collection, type, subtype) =>
  collection.sort((firstItem, secondItem) => {
    const firstValue = preProcessBeforeCompare(firstItem[type][subtype]);
    const secondValue = preProcessBeforeCompare(secondItem[type][subtype]);
    return secondValue - firstValue;
  });

const OrderHelper = {
  orderMatchAsc,
  orderMatchDesc,
  orderSubColumnAsc,
  orderSubColumnDesc
};

export default OrderHelper;

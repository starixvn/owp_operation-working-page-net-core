/* eslint-disable no-warning-comments */
import { tableSchema, defaultTableStyle } from '../../../Const.json';
import dynamicCell from './dynamicCell';
import { calculateCustomTableWidth } from '../../../../Shared/common/CustomTable/Helper';

const tableWidth = columnSchema => calculateCustomTableWidth(columnSchema, 16);
const handleRowClassNameGetter = (index, dataSource) => {
  const dataLength = dataSource.length;
  const classNames = [];
  dataLength > 7 && index > dataLength - 6 && classNames.push('is-last-row');

  return classNames.join(' ');
};

const getConfigTable = containerWidth => {
  const ratio = containerWidth <= 1000 ? 1000 / 1614 : containerWidth / 1614;

  return {
    columnSchema: tableSchema.map(x => {
      return { ...x, width: x.width * ratio };
    }),
    tableStyle: defaultTableStyle,
    dynamicCell,
    sortFunc: undefined, // NOSONAR
    onRowClassNameGetter: handleRowClassNameGetter,
    tableWidth
  };
};

export { getConfigTable };

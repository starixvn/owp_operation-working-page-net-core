import { isEquals } from 'immutability-helper';
import {
  DefaultCellTrader,
  DefaultTableStyle,
  DefaultTableLargePlusStyle,
  DefaultCellProvider,
  Trader,
  TableSize,
  No,
  KOTime,
  Match,
  League,
  Sport,
  Remark,
  MaxBet,
  Spread,
  IPTvStreaming,
  Provider
} from './TableSchema.json';
import {
  filterBySearchKey,
  clearFilter,
  collapseAllRow,
  expandAllRow,
  order
} from './OSTableSlice/reducer';

import { dynamicCell } from './ConfigNative';

import { calculateCustomTableWidth } from '../../../Shared/common/CustomTable/Helper';

const { largePlus } = TableSize;

const calculateRowHightByProvider = props => {
  const { provider, otherProvider, service } = props;
  if (provider && otherProvider && service) {
    return DefaultCellProvider.height;
  }
  return 0;
};

const calculateRowHightByTrader = traders => {
  if (!traders) {
    return 0;
  }
  return traders.length * DefaultCellTrader.height;
};

const calculateRowHight = osAssignment => {
  const heights = [DefaultTableStyle.rowHeight];
  if (!osAssignment) {
    return 0;
  }
  const { traderMaxbetModels, provider, otherProvider, service } = osAssignment;
  const traderMaxbetModelsCanDisplay = traderMaxbetModels.filter(
    traderMaxbet => !traderMaxbet.hiden || traderMaxbet.hiden === undefined
  );
  heights.push(calculateRowHightByTrader(traderMaxbetModelsCanDisplay));
  heights.push(
    calculateRowHightByProvider({ provider, otherProvider, service })
  );
  const result = Math.max(...heights);
  return result;
};

const buildClassNameHightLightMatch = props => {
  const matchStatus = props ? props.matchStatus : '';
  const statusFormatted = matchStatus.trim().toLowerCase();
  if (statusFormatted === 'postponed' || statusFormatted === 'canceled') {
    return 'yellow-row';
  }
  return '';
};

const filterHomeOrAwayByKeyword = (
  arraySource,
  { keyword = undefined, columnKey = undefined }
) => {
  if (!keyword) {
    return arraySource;
  }
  const keyWordFormat = keyword.toLowerCase();

  if (columnKey === 'League') {
    return arraySource.filter(item =>
      item.league.toLowerCase().includes(keyWordFormat)
    );
  }

  if (columnKey === 'Match') {
    return arraySource.filter(
      item =>
        item.homeName.toLowerCase().includes(keyWordFormat) ||
        item.away.toLowerCase().includes(keyWordFormat)
    );
  }
  return arraySource || [];
};

const updatePropHidenTranderMaxbet = (rowIndex, tranderMaxbets, isHiden) => {
  tranderMaxbets.forEach((tranderMaxbet, index) => {
    tranderMaxbet.hiden = index === rowIndex ? false : isHiden;
  });
};

const findAndUpdatePropHidenTranderMaxbetByosMatchId = (
  osMatchId,
  dataSource,
  isHiden
) => {
  for (let i = 0; i < dataSource.length; i++) {
    if (dataSource[i].osMatchId === osMatchId) {
      updatePropHidenTranderMaxbet(
        0,
        dataSource[i].traderMaxbetModels,
        isHiden
      );
      break;
    }
  }
};

const buildColumn = (width, column) => {
  const { headerName, columnKey, isCanFilter, isCanSort, isCanCollapseExpand } =
    column;
  return {
    headerName,
    columnKey,
    isCanFilter,
    isCanSort,
    isCanCollapseExpand,
    width
  };
};

const selectTable = widthWindow => {
  const minWidth = 880;
  const width = widthWindow < minWidth ? minWidth : widthWindow;
  return {
    columnSchema: [
      buildColumn(width * 0.06, No),
      buildColumn(width * 0.054, KOTime),
      buildColumn(width * 0.064, Sport),
      buildColumn(width * 0.13, League),
      buildColumn(width * 0.14, Match),
      buildColumn(width * 0.08, Remark),
      buildColumn(width * 0.19, Trader),
      buildColumn(width * 0.07, MaxBet),
      buildColumn(width * 0.07, Spread),
      buildColumn(width * 0.07, IPTvStreaming),
      buildColumn(width * 0.07, Provider)
    ],
    tableStyle: buildStyleSchema(widthWindow),

    dynamicCell,
    sortFunc: order,
    filterFunc: filterBySearchKey,
    clearFilterFunc: clearFilter,
    collapseAllRowFunc: collapseAllRow,
    expandAllRowFunc: expandAllRow,
    tableWidth: columnSchema => calculateCustomTableWidth(columnSchema, 15)
  };
};

const buildStyleSchema = size =>
  isEquals(size, largePlus) ? DefaultTableLargePlusStyle : DefaultTableStyle;

export {
  calculateRowHight,
  buildClassNameHightLightMatch,
  filterHomeOrAwayByKeyword,
  updatePropHidenTranderMaxbet,
  findAndUpdatePropHidenTranderMaxbetByosMatchId,
  selectTable,
  buildStyleSchema
};

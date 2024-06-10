import { includes, find, filter } from 'lodash';
import Utils from '../../Shared/Utils';
import isEqual from 'lodash.isequal';
import { getMMDDYYYYStringFromDateTime } from '../../Shared/DateTime/dateTimeHelper';
import ColumnConst from '../constants/ColumnConst';

const { toLowerCaseForFilter } = Utils;

const updateFilterKeyword = (filterInfo, filterKey, keyword) => {
  const item = find(filterInfo, column => column.filterKey === filterKey);
  if (item) {
    item.keyword = keyword;
  } else {
    filterInfo.push({ filterKey, keyword });
  }
  return filterInfo;
};

const filterColumn = (source, key, keyword) =>
  filter(
    source,
    item => item[key] && includes(toLowerCaseForFilter(item[key]), keyword)
  );

const filterLastUse = (source, keyword) =>
  filter(
    source,
    item =>
      item.LastUse &&
      includes(
        getMMDDYYYYStringFromDateTime(item.LastUse),
        keyword
      )
  );

const filterData = (dataSource, filterInfo) => {
  const keyword = toLowerCaseForFilter(filterInfo.keyword);
  if (!keyword.trim().length) {
    return dataSource;
  }
  const {
    leagueGroup,
    league,
    spread,
    hDPWeekday,
    hDPWeekend,
    fT1X2,
    lastUse
  } = ColumnConst.columnMaxbetGeneral;
  switch (filterInfo.filterKey) {
    case leagueGroup.columnKey:
      return filterColumn(dataSource, 'GroupLabel', keyword);
    case league.columnKey:
      return filterColumn(dataSource, 'LeagueName', keyword);
    case spread.subHeader.min.columnKey:
      return filterColumn(dataSource, 'MinSpread', keyword);
    case spread.subHeader.max.columnKey:
      return filterColumn(dataSource, 'MaxSpread', keyword);
    case hDPWeekday.subHeader.min.columnKey:
      return filterColumn(dataSource, 'MinMaxbetWD', keyword);
    case hDPWeekday.subHeader.max.columnKey:
      return filterColumn(dataSource, 'MaxMaxbetWD', keyword);
    case hDPWeekend.subHeader.min.columnKey:
      return filterColumn(dataSource, 'MinMaxbetWK', keyword);
    case hDPWeekend.subHeader.max.columnKey:
      return filterColumn(dataSource, 'MaxMaxbetWK', keyword);
    case fT1X2.subHeader.min.columnKey:
      return filterColumn(dataSource, 'MinFT1X2', keyword);
    case fT1X2.subHeader.max.columnKey:
      return filterColumn(dataSource, 'MaxFT1X2', keyword);
    case lastUse.columnKey:
      return filterLastUse(dataSource, keyword);
  }
  return undefined;
};

const getKeyword = (filterInfo, filterKey) => {
  const result = find(filterInfo, col => isEqual(col.filterKey, filterKey));
  return result ? result.keyword : '';
};

export { updateFilterKeyword, filterData, getKeyword };

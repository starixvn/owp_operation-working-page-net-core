import { filterDataTable } from '../../../../../Shared/Utils';
import {
  TCondition,
  TDataSource,
  TMultipleFilter,
  TColumnKey,
  TTableSlice,
  TColumnHasMultipleFilterKey
} from '../TableType';
import helper from './calculateFilterOptions';

const createFilterConfig = (
  key: string,
  dataSource: TDataSource[] | undefined,
  helperFunction: (data?: TDataSource[]) => any
) => ({
  selected: helperFunction(dataSource),
  options: helperFunction(dataSource),
  isFilter: false
});

const buildDefaultTableFilter = (
  dataSource: TDataSource[],
  tableFilter: TMultipleFilter,
  isSaveAction: boolean
) => {
  const filterConfigs = {
    League_Group: () => helper.League_Group(dataSource),
    Tv: () => helper.Tv(dataSource),
    Round: () => helper.Round(dataSource),
    League_Division: () => helper.League_Division(dataSource),
    Remark: () => helper.Remark(dataSource),
    Spread_HDP_OU: () => helper.Spread_HDP_OU(dataSource),
    MB_WD_HDP: () => helper.MB_WD_HDP(dataSource),
    MB_WD_NO_HDP: () => helper.MB_WD_NO_HDP(dataSource),
    MB_WK_HDP: () => helper.MB_WK_HDP(dataSource),
    MB_WK_NO_HDP: () => helper.MB_WK_NO_HDP(dataSource),
    LST_Note: () => helper.LST_Note(dataSource),
    Remark2: () => helper.Remark2(dataSource),
    Comment: () => helper.Comment(dataSource)
  };

  Object.entries(filterConfigs).forEach(([key, helperFn]) => {
    const newFilter = createFilterConfig(key, dataSource, helperFn);
    const currentFilter = tableFilter[key as TColumnHasMultipleFilterKey];
    const hasMatchingSelectedOptions = currentFilter?.isFilter && isSaveAction && 
      newFilter.options.some((option: any) => 
        currentFilter.selected.some((selected: any) => selected.label === option.label)
      );

    tableFilter[key as TColumnHasMultipleFilterKey] = {
      ...newFilter,
      selected: hasMatchingSelectedOptions ? currentFilter.selected : newFilter.selected,
      isFilter: hasMatchingSelectedOptions ? currentFilter.isFilter : newFilter.isFilter
    };
  });
};

const executeFilterTable = (
  state: TTableSlice,
  filterByKeyword: (
    arraySource: any, //NOSONAR
    { keyword, columnKey }: { keyword: undefined; columnKey: undefined }
  ) => any //NOSONAR
) => {
  const { entitiesOriginal, multipleFilter, filterState } = state;

  const {
    League_Group,
    Tv,
    Round,
    League_Division,
    Spread_HDP_OU,
    MB_WK_HDP,
    MB_WK_NO_HDP,
    MB_WD_HDP,
    MB_WD_NO_HDP,
    Comment
  } = multipleFilter;

  let result = filterDataTable(entitiesOriginal, filterState, filterByKeyword);
  result = result.filter(
    (
      e: TDataSource //NOSONAR
    ) =>
      (!League_Group.isFilter ||
        League_Group.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.leagueGroup)
        )) &&
      (!Tv.isFilter ||
        Tv.selected.some(
          (m: any) =>
            m.label ===
            buildLabelTvOption(e.condition.hasTv, e.condition.hasIpTv)
        )) &&
      (!Round.isFilter ||
        Round.selected.some((m: any) =>
          buildLabelItemForCondition(e.condition.round, m.label)
        )) &&
      (!League_Division.isFilter ||
        League_Division.selected.some((m: any) =>
          buildLabelItemForCondition(e.condition.division, m.label)
        )) &&
      (!Spread_HDP_OU.isFilter ||
        Spread_HDP_OU.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.spreadShow)
        )) &&
      (!MB_WD_HDP.isFilter ||
        MB_WD_HDP.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.maxbetWD_HDP_Show)
        )) &&
      (!MB_WD_NO_HDP.isFilter ||
        MB_WD_NO_HDP.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.wD_NoOfHDP_Show)
        )) &&
      (!MB_WK_HDP.isFilter ||
        MB_WK_HDP.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.maxbetWK_HDP_Show)
        )) &&
      (!MB_WK_NO_HDP.isFilter ||
        MB_WK_NO_HDP.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.wK_NoOfHDP_Show)
        )) &&
      (!Comment.isFilter ||
        Comment.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.comment)
        ))
  );
  return result;
};
const buildLabelTvOption = (hasTv: boolean, hasIpTv: boolean) => {
  return hasIpTv && hasTv
    ? 'TV/IPTV'
    : hasIpTv
    ? 'IPTV'
    : hasTv
    ? 'TV'
    : '(Blanks)';
};
const buildLabelItemOption = (value: string | undefined | null) =>
  value?.trim().length ? value : '(Blanks)';

const buildLabelItemForCondition = (
  conditions: TCondition[],
  selected: string | number | null
) => {
  if (conditions.length === 0 && selected === '(Blanks)') {
    return true;
  }
  return conditions.some(x => buildValue(x.value, x.type) === selected);
};

const buildValue = (value: string, type: string) =>
  value.length > 0 ? (type === 'Contains' ? `*${value}*` : value) : null;
export { buildDefaultTableFilter, executeFilterTable };

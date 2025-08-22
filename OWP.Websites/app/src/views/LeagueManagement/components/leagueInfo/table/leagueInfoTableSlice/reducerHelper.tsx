import { filterDataTable } from '../../../../../Shared/Utils';
import {
  TCondition,
  TDataSource,
  TMultipleFilter,
  TColumnKey,
  TTableSlice
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
  tableFilter: TMultipleFilter
) => {
  const filterConfigs = {
    League_Group: () => helper.League_Group(dataSource),
    League: () => helper.League(dataSource),
    Tv: () => helper.Tv(),
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
    tableFilter[key as TColumnKey] = createFilterConfig(
      key,
      dataSource,
      helperFn
    );
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
    League,
    Tv,
    Round,
    League_Division,
    Remark,
    Spread_HDP_OU,
    MB_WK_HDP,
    MB_WK_NO_HDP,
    MB_WD_HDP,
    MB_WD_NO_HDP,
    LST_Note,
    Remark2,
    Comment
  } = multipleFilter;

  let result = filterDataTable(entitiesOriginal, filterState, filterByKeyword);
  //console.log(JSON.stringify(result, null, 2));
  result = result.filter(
    (
      e: TDataSource //NOSONAR
    ) =>
      (!League_Group.isFilter ||
        League_Group.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.leagueGroup)
        )) &&
      (!League.isFilter ||
        League.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.league)
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
      (!Remark.isFilter ||
        Remark.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.condition.remark)
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
      (!LST_Note.isFilter ||
        LST_Note.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.lstNote)
        )) &&
      (!Remark2.isFilter ||
        Remark2.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.remark2)
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

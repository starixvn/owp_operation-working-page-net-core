import { filterDataTable } from '../../../../../Shared/Utils';
import {
  TCondition,
  TDataSource,
  TMultipleFilter,
  TTableSlice,
  TColumnHasMultipleFilterKey
} from '../TableType';
import { Option } from '../../../../../Shared/MultipleSelection/type';
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
    const hasMatchingSelectedOptions =
      currentFilter?.isFilter &&
      isSaveAction &&
      newFilter.options.some((option: any) =>
        currentFilter.selected.some(
          (selected: any) => selected.label === option.label
        )
      );

    tableFilter[key as TColumnHasMultipleFilterKey] = {
      ...newFilter,
      selected: hasMatchingSelectedOptions
        ? currentFilter.selected
        : newFilter.selected,
      isFilter: hasMatchingSelectedOptions
        ? currentFilter.isFilter
        : newFilter.isFilter
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
  const { entitiesOriginal = [], multipleFilter, filterState } = state;

  const {
    League_Group = { isFilter: false, selected: [] },
    Tv = { isFilter: false, selected: [] },
    Round = { isFilter: false, selected: [] },
    League_Division = { isFilter: false, selected: [] },
    Spread_HDP_OU = { isFilter: false, selected: [] },
    MB_WK_HDP = { isFilter: false, selected: [] },
    MB_WK_NO_HDP = { isFilter: false, selected: [] },
    MB_WD_HDP = { isFilter: false, selected: [] },
    MB_WD_NO_HDP = { isFilter: false, selected: [] },
    Comment = { isFilter: false, selected: [] }
  } = multipleFilter || {};

  let result = filterDataTable(
    entitiesOriginal,
    filterState || [],
    filterByKeyword
  );
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

const executeFilterTableExcludingColumn = (
  state: TTableSlice,
  excludeColumnKey: TColumnHasMultipleFilterKey,
  filterByKeyword: any
) => {
  const { entitiesOriginal = [], multipleFilter, filterState } = state;
  const {
    League_Group = { isFilter: false, selected: [] },
    Tv = { isFilter: false, selected: [] },
    Round = { isFilter: false, selected: [] },
    League_Division = { isFilter: false, selected: [] },
    Spread_HDP_OU = { isFilter: false, selected: [] },
    MB_WK_HDP = { isFilter: false, selected: [] },
    MB_WK_NO_HDP = { isFilter: false, selected: [] },
    MB_WD_HDP = { isFilter: false, selected: [] },
    MB_WD_NO_HDP = { isFilter: false, selected: [] },
    Comment = { isFilter: false, selected: [] }
  } = multipleFilter || {};

  let result = filterDataTable(
    entitiesOriginal,
    filterState || [],
    filterByKeyword
  );

  // Apply multiple filters, excluding the specified column
  result = result.filter(
    (e: TDataSource) =>
      // League_Group filter
      (excludeColumnKey === 'League_Group' ||
        !League_Group.isFilter ||
        League_Group.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.leagueGroup)
        )) &&
      // Tv filter
      (excludeColumnKey === 'Tv' ||
        !Tv.isFilter ||
        Tv.selected.some(
          (m: any) =>
            m.label ===
            buildLabelTvOption(e.condition.hasTv, e.condition.hasIpTv)
        )) &&
      // Round filter
      (excludeColumnKey === 'Round' ||
        !Round.isFilter ||
        Round.selected.some((m: any) =>
          buildLabelItemForCondition(e.condition.round, m.label)
        )) &&
      // League_Division filter
      (excludeColumnKey === 'League_Division' ||
        !League_Division.isFilter ||
        League_Division.selected.some((m: any) =>
          buildLabelItemForCondition(e.condition.division, m.label)
        )) &&
      // Spread_HDP_OU filter
      (excludeColumnKey === 'Spread_HDP_OU' ||
        !Spread_HDP_OU.isFilter ||
        Spread_HDP_OU.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.spreadShow)
        )) &&
      // MB_WD_HDP filter
      (excludeColumnKey === 'MB_WD_HDP' ||
        !MB_WD_HDP.isFilter ||
        MB_WD_HDP.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.maxbetWD_HDP_Show)
        )) &&
      // MB_WD_NO_HDP filter
      (excludeColumnKey === 'MB_WD_NO_HDP' ||
        !MB_WD_NO_HDP.isFilter ||
        MB_WD_NO_HDP.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.wD_NoOfHDP_Show)
        )) &&
      // MB_WK_HDP filter
      (excludeColumnKey === 'MB_WK_HDP' ||
        !MB_WK_HDP.isFilter ||
        MB_WK_HDP.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.maxbetWK_HDP_Show)
        )) &&
      // MB_WK_NO_HDP filter
      (excludeColumnKey === 'MB_WK_NO_HDP' ||
        !MB_WK_NO_HDP.isFilter ||
        MB_WK_NO_HDP.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.wK_NoOfHDP_Show)
        )) &&
      // Comment filter
      (excludeColumnKey === 'Comment' ||
        !Comment.isFilter ||
        Comment.selected.some(
          (m: any) => m.label === buildLabelItemOption(e.comment)
        ))
  );

  return result;
};

const buildTableMultipleFilterOptionsExceptColumn = (
  state: TTableSlice,
  filterByKeyword: any
) => {
  const columnsKeyArray: TColumnHasMultipleFilterKey[] = [
    'League_Group',
    'Tv',
    'Round',
    'League_Division',
    'Spread_HDP_OU',
    'MB_WK_HDP',
    'MB_WK_NO_HDP',
    'MB_WD_HDP',
    'MB_WD_NO_HDP',
    'Comment'
  ];

  const result: { [key in TColumnHasMultipleFilterKey]: Option[] } = {
    League_Group: [],
    Tv: [],
    Round: [],
    League_Division: [],
    Spread_HDP_OU: [],
    MB_WK_HDP: [],
    MB_WK_NO_HDP: [],
    MB_WD_HDP: [],
    MB_WD_NO_HDP: [],
    Comment: []
  };

  // Build options for each column by filtering data excluding that column
  for (const columnKey of columnsKeyArray) {
    const filteredData = executeFilterTableExcludingColumn(
      state,
      columnKey,
      filterByKeyword
    );
    const options = buildTableMultipleFilterOptions(filteredData);
    result[columnKey] = options[columnKey];
  }

  return result;
};

const buildTableMultipleFilterOptions = (entities: TDataSource[]) => {
  return {
    League_Group: helper.League_Group(entities),
    Tv: helper.Tv(entities),
    Round: helper.Round(entities),
    League_Division: helper.League_Division(entities),
    Spread_HDP_OU: helper.Spread_HDP_OU(entities),
    MB_WK_HDP: helper.MB_WK_HDP(entities),
    MB_WK_NO_HDP: helper.MB_WK_NO_HDP(entities),
    MB_WD_HDP: helper.MB_WD_HDP(entities),
    MB_WD_NO_HDP: helper.MB_WD_NO_HDP(entities),
    Comment: helper.Comment(entities)
  };
};

const updateMultipleFilterOptionsAndSelected = (
  state: TTableSlice,
  filterByKeyword: any
) => {
  // Build options cho tất cả cột bằng cách filter data trừ từng cột ra
  const options = buildTableMultipleFilterOptionsExceptColumn(
    state,
    filterByKeyword
  );

  // Update options cho tất cả cột
  state.multipleFilter.League_Group.options = options.League_Group;
  state.multipleFilter.Tv.options = options.Tv;
  state.multipleFilter.Round.options = options.Round;
  state.multipleFilter.League_Division.options = options.League_Division;
  state.multipleFilter.Spread_HDP_OU.options = options.Spread_HDP_OU;
  state.multipleFilter.MB_WK_HDP.options = options.MB_WK_HDP;
  state.multipleFilter.MB_WK_NO_HDP.options = options.MB_WK_NO_HDP;
  state.multipleFilter.MB_WD_HDP.options = options.MB_WD_HDP;
  state.multipleFilter.MB_WD_NO_HDP.options = options.MB_WD_NO_HDP;
  state.multipleFilter.Comment.options = options.Comment;

  // Update selected values theo Excel behavior:
  // - Cột được clear: set selected = all options (để user có thể filter lại)
  // - Các cột khác: giữ nguyên selected nếu đang active filter, hoặc set = all options nếu không active
  state.multipleFilter.League_Group.selected = state.multipleFilter.League_Group
    .isFilter
    ? state.multipleFilter.League_Group.selected
    : options.League_Group;
  state.multipleFilter.Tv.selected = state.multipleFilter.Tv.isFilter
    ? state.multipleFilter.Tv.selected
    : options.Tv;
  state.multipleFilter.Round.selected = state.multipleFilter.Round.isFilter
    ? state.multipleFilter.Round.selected
    : options.Round;
  state.multipleFilter.League_Division.selected = state.multipleFilter
    .League_Division.isFilter
    ? state.multipleFilter.League_Division.selected
    : options.League_Division;
  state.multipleFilter.Spread_HDP_OU.selected = state.multipleFilter
    .Spread_HDP_OU.isFilter
    ? state.multipleFilter.Spread_HDP_OU.selected
    : options.Spread_HDP_OU;
  state.multipleFilter.MB_WK_HDP.selected = state.multipleFilter.MB_WK_HDP
    .isFilter
    ? state.multipleFilter.MB_WK_HDP.selected
    : options.MB_WK_HDP;
  state.multipleFilter.MB_WK_NO_HDP.selected = state.multipleFilter.MB_WK_NO_HDP
    .isFilter
    ? state.multipleFilter.MB_WK_NO_HDP.selected
    : options.MB_WK_NO_HDP;
  state.multipleFilter.MB_WD_HDP.selected = state.multipleFilter.MB_WD_HDP
    .isFilter
    ? state.multipleFilter.MB_WD_HDP.selected
    : options.MB_WD_HDP;
  state.multipleFilter.MB_WD_NO_HDP.selected = state.multipleFilter.MB_WD_NO_HDP
    .isFilter
    ? state.multipleFilter.MB_WD_NO_HDP.selected
    : options.MB_WD_NO_HDP;
  state.multipleFilter.Comment.selected = state.multipleFilter.Comment.isFilter
    ? state.multipleFilter.Comment.selected
    : options.Comment;
};

export {
  buildDefaultTableFilter,
  executeFilterTable,
  executeFilterTableExcludingColumn,
  buildTableMultipleFilterOptionsExceptColumn,
  buildTableMultipleFilterOptions,
  updateMultipleFilterOptionsAndSelected
};

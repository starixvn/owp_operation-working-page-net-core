import { filterDataTable } from '../../../../../Shared/Utils';
import {
  TCondition,
  TDataSource,
  TMultipleFilter,
  TColumnKey,
  TTableSlice,
  TColumnHasMultipleFilterKey
} from '../TableType';
import { Option } from '../../../../../Shared/MultipleSelection/type';
import helper from './calculateFilterOptions';

type TSavedMultipleFilter = {
  [key in TColumnHasMultipleFilterKey]: {
    isFilter: boolean;
    selected: Option[];
  };
};

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

const preserveAndRestoreMultipleFilter = (
  currentMultipleFilter: TMultipleFilter,
  newFilteredMatches: TDataSource[]
): TMultipleFilter => {
  // Step 1: Save current filter state (isFilter and selected)
  const savedMultipleFilter: TSavedMultipleFilter = {} as TSavedMultipleFilter;
  (Object.keys(currentMultipleFilter) as TColumnHasMultipleFilterKey[]).forEach(
    key => {
      savedMultipleFilter[key] = {
        isFilter: currentMultipleFilter[key].isFilter,
        selected: currentMultipleFilter[key].selected
      };
    }
  );

  // Step 2: Build new filter options from new data using existing buildDefaultTableFilter logic
  const newMultipleFilter = { ...currentMultipleFilter };
  buildDefaultTableFilter(newFilteredMatches, newMultipleFilter, true);

  // Step 3: Restore filter state
  (Object.keys(savedMultipleFilter) as TColumnHasMultipleFilterKey[]).forEach(
    key => {
      if (newMultipleFilter[key] && savedMultipleFilter[key]) {
        // Only restore if the filter was active and has matching selected options in new data
        const hasMatchingSelectedOptions =
          savedMultipleFilter[key].isFilter &&
          newMultipleFilter[key].options.some((option: Option) =>
            savedMultipleFilter[key].selected.some(
              (selected: Option) => selected.label === option.label
            )
          );

        newMultipleFilter[key] = {
          ...newMultipleFilter[key],
          isFilter: hasMatchingSelectedOptions
            ? savedMultipleFilter[key].isFilter
            : false,
          selected: hasMatchingSelectedOptions
            ? savedMultipleFilter[key].selected
            : newMultipleFilter[key].selected
        };
      }
    }
  );

  return newMultipleFilter;
};

const executeFilterTableExcludingColumn = (
  state: TTableSlice,
  excludeColumnKey: TColumnHasMultipleFilterKey
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

  // Apply text filters first
  let result = filterDataTable(
    entitiesOriginal,
    filterState || [],
    (matches: any) => matches
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

const updateFilterOptionsAfterFilter = (
  state: TTableSlice,
  filteredData: TDataSource[],
  excludeColumnKey?: TColumnHasMultipleFilterKey
) => {
  const filterConfigs = {
    League_Group: () => helper.League_Group(filteredData),
    Tv: () => helper.Tv(filteredData),
    Round: () => helper.Round(filteredData),
    League_Division: () => helper.League_Division(filteredData),
    Remark: () => helper.Remark(filteredData),
    Spread_HDP_OU: () => helper.Spread_HDP_OU(filteredData),
    MB_WD_HDP: () => helper.MB_WD_HDP(filteredData),
    MB_WD_NO_HDP: () => helper.MB_WD_NO_HDP(filteredData),
    MB_WK_HDP: () => helper.MB_WK_HDP(filteredData),
    MB_WK_NO_HDP: () => helper.MB_WK_NO_HDP(filteredData),
    LST_Note: () => helper.LST_Note(filteredData),
    Remark2: () => helper.Remark2(filteredData),
    Comment: () => helper.Comment(filteredData)
  };

  Object.entries(filterConfigs).forEach(([key, helperFn]) => {
    // Skip updating options for the column that was just filtered
    if (key === excludeColumnKey) {
      return;
    }

    const columnKey = key as TColumnHasMultipleFilterKey;
    const currentFilter = state.multipleFilter[columnKey];

    if (currentFilter) {
      const newOptions = helperFn();

      // Only update options, preserve isFilter and selected states
      state.multipleFilter[columnKey] = {
        ...currentFilter,
        options: newOptions,
        selected: newOptions
      };
    }
  });
};

export {
  buildDefaultTableFilter,
  executeFilterTable,
  executeFilterTableExcludingColumn,
  preserveAndRestoreMultipleFilter,
  updateFilterOptionsAfterFilter
};

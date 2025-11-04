import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import {
  TColumnKey,
  TTableSlice,
  FilterAllColumn,
  TFilterState,
  TColumnHasMultipleFilterKey
} from '../TableType';
import { getLeagueInfoThunk } from './leagueInfoThunk';
import { REQUEST_STATUS } from '../../../../../Shared/Constants';
import { Option } from '../../../../../Shared/MultipleSelection/type';
import {
  executeFilterTable,
  executeFilterTableExcludingColumn,
  buildDefaultTableFilter,
  preserveAndRestoreMultipleFilter,
  updateFilterOptionsAfterFilter
} from './reducerHelper';
import helper from './calculateFilterOptions';
import {
  IS_BLACKLIST_OPTIONS,
  IS_OFFERLIVE_OPTIONS
} from '../../filter/filterConst.json';
import {
  addOrReplaceFilterState,
  pushOrUpdateObject
} from '../../../../../Shared/Utils';
import filterByKeyword from '../filterByKeyword';

const applyFiltersToData = (state: TTableSlice) => {
  return executeFilterTable(state, filterByKeyword);
};

const resetFiltersForChangedFields = (
  state: TTableSlice,
  filterData: any[]
) => {
  if (filterData.length === 0 && state.valueFilterChange.length > 0) {
    state.valueFilterChange.forEach((filterChange: TFilterState) => {
      if (
        state.multipleFilter[
          filterChange.columnKey as TColumnHasMultipleFilterKey
        ]?.isFilter
      ) {
        state.multipleFilter[
          filterChange.columnKey as TColumnHasMultipleFilterKey
        ] = {
          ...state.multipleFilter[
            filterChange.columnKey as TColumnHasMultipleFilterKey
          ],
          isFilter: false,
          selected: []
        };
      }

      state.filterState = state.filterState.filter(
        filter => filter.columnKey !== filterChange.columnKey
      );

      state.valueFilterAllColumn = Object.fromEntries(
        Object.entries(state.valueFilterAllColumn).filter(
          ([key]) => key !== filterChange.columnKey
        )
      );
    });
    state.valueFilterChange = [];
    return executeFilterTable(state, filterByKeyword);
  }

  return filterData;
};

const buildMultipleFilterDefault = () => ({
  isFilter: false,
  selected: [],
  options: []
});

const LeagueInfoTableSlice = createSlice({
  name: 'leagueInfoTableSlice',
  initialState: {
    entities: [],
    entitiesOriginal: [],
    loading: REQUEST_STATUS.idle,
    leagueFilterHeader: {
      leagueName: '',
      leagueGroupId: null,
      BlackList: IS_BLACKLIST_OPTIONS.map(m => m.value),
      OfferLive: IS_OFFERLIVE_OPTIONS.map(m => m.value)
    },
    focusSettingId: 0,
    multipleFilter: {
      League_Group: buildMultipleFilterDefault(),
      Tv: buildMultipleFilterDefault(),
      Round: buildMultipleFilterDefault(),
      League_Division: buildMultipleFilterDefault(),
      Spread_HDP_OU: buildMultipleFilterDefault(),
      MB_WK_HDP: buildMultipleFilterDefault(),
      MB_WK_NO_HDP: buildMultipleFilterDefault(),
      MB_WD_HDP: buildMultipleFilterDefault(),
      MB_WD_NO_HDP: buildMultipleFilterDefault(),
      Comment: buildMultipleFilterDefault()
    },
    isOpenAddNewSetting: false,
    valueFilterAllColumn: {} as FilterAllColumn,
    columnKeyFilterCurrent: '',
    filterState: [] as TFilterState[],
    isSaveAction: false,
    valueFilterChange: [] as TFilterState[]
  } as TTableSlice,
  reducers: {
    setIsOpenAddNewSetting: state => {
      state.isOpenAddNewSetting = !state.isOpenAddNewSetting;
    },
    clearFilter: state => {
      const { entitiesOriginal } = state;
      state.filterState = [];
      state.valueFilterAllColumn = {};
      state.entities = entitiesOriginal;
      buildDefaultTableFilter(
        state.entitiesOriginal,
        state.multipleFilter,
        state.isSaveAction
      );
    },
    setLeagueHeaderFiler: (state, actions) => {
      state.leagueFilterHeader = actions.payload;
    },
    setFocusSettingId: (state, actions) => {
      state.focusSettingId = actions.payload;
    },
    filterBySearchKey: (state, action) => {
      const { filterState } = state;
      state.filterState = addOrReplaceFilterState(filterState, action.payload);
      state.entities = applyFiltersToData(state);

      // Update filter options for other columns based on filtered data
      updateFilterOptionsAfterFilter(state, state.entities);
    },
    setMultipleFilter: (
      state,
      action: PayloadAction<{
        columnKey: Required<TColumnKey>;
        selected: Option[];
        isFilter: boolean;
      }>
    ) => {
      const { columnKey, selected, isFilter } = action.payload;
      state.multipleFilter[columnKey as TColumnHasMultipleFilterKey] = {
        ...state.multipleFilter[columnKey as TColumnHasMultipleFilterKey],
        selected,
        isFilter
      };
      state.entities = applyFiltersToData(state);

      // Update filter options for other columns based on filtered data
      updateFilterOptionsAfterFilter(
        state,
        state.entities,
        columnKey as TColumnHasMultipleFilterKey
      );
    },
    removeBySettingId: (
      state,
      actions: PayloadAction<{ settingId: number }>
    ) => {
      state.entities = state.entities.filter(
        l => l.leagueSettingId !== actions.payload.settingId
      );
      state.entitiesOriginal = state.entitiesOriginal.filter(
        l => l.leagueSettingId !== actions.payload.settingId
      );
    },
    setColumnKeyValue: (state, action) => {
      state.columnKeyFilterCurrent = action.payload;
    },
    setValueFilter: (state, action) => {
      const { valueFilterAllColumn } = state;
      state.valueFilterAllColumn = pushOrUpdateObject(
        valueFilterAllColumn,
        action.payload
      );
    },
    setIsSaveAction: (state, action) => {
      state.isSaveAction = action.payload;
    },
    setFilterChange: (state, action: PayloadAction<TFilterState[]>) => {
      state.valueFilterChange = action.payload;
    },
    clearSingleColumnFilter: (
      state,
      action: PayloadAction<{
        columnKey: Required<TColumnKey>;
      }>
    ) => {
      const { columnKey } = action.payload;

      const { [columnKey]: _, ...rest } = state.valueFilterAllColumn;
      state.valueFilterAllColumn = rest;

      state.filterState = (state.filterState || []).filter(
        filter => filter.columnKey !== columnKey
      );

      if (columnKey in state.multipleFilter) {
        state.multipleFilter[columnKey as TColumnHasMultipleFilterKey] = {
          ...state.multipleFilter[columnKey as TColumnHasMultipleFilterKey],
          isFilter: false,
          selected: []
        };
      }

      state.entities = applyFiltersToData(state) || [];

      const allColumnKeys = Object.keys(
        state.multipleFilter
      ) as TColumnHasMultipleFilterKey[];
      const options: any = {};

      for (const colKey of allColumnKeys) {
        if (colKey === columnKey) {
          options[colKey] = helper[colKey](state.entities);
        } else {
          const dataExcludingThisColumn = executeFilterTableExcludingColumn(
            state,
            colKey
          );
          options[colKey] = helper[colKey](dataExcludingThisColumn);
        }
      }

      Object.keys(options).forEach(colKey => {
        const columnKey = colKey as TColumnHasMultipleFilterKey;
        const currentFilter = state.multipleFilter[columnKey];
        if (currentFilter) {
          state.multipleFilter[columnKey] = {
            ...currentFilter,
            options: options[colKey]
          };
        }
      });
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getLeagueInfoThunk.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(getLeagueInfoThunk.fulfilled, (state, { payload }) => {
        state.entitiesOriginal = payload;
        state.loading = REQUEST_STATUS.idle;

        if (!state.isSaveAction) {
          state.filterState = [];
          state.valueFilterAllColumn = {};
        }
        buildDefaultTableFilter(
          state.entitiesOriginal,
          state.multipleFilter,
          state.isSaveAction
        );
        let filterData = executeFilterTable(state, filterByKeyword);

        filterData = resetFiltersForChangedFields(state, filterData);

        state.entities = filterData;
        state.isSaveAction = false;
      });
  }
});
const { reducer, actions } = LeagueInfoTableSlice;

const {
  clearFilter,
  clearSingleColumnFilter,
  setMultipleFilter,
  setLeagueHeaderFiler,
  setFocusSettingId,
  setIsOpenAddNewSetting,
  removeBySettingId,
  setColumnKeyValue,
  setValueFilter,
  filterBySearchKey,
  setIsSaveAction,
  setFilterChange
} = actions;

export {
  clearFilter,
  clearSingleColumnFilter,
  setMultipleFilter,
  setLeagueHeaderFiler,
  setFocusSettingId,
  setIsOpenAddNewSetting,
  removeBySettingId,
  setColumnKeyValue,
  setValueFilter,
  filterBySearchKey,
  setIsSaveAction,
  setFilterChange
};

export default reducer;

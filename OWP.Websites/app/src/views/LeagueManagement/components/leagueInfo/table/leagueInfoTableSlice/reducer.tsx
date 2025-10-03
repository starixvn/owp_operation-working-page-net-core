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
import { executeFilterTable, buildDefaultTableFilter } from './reducerHelper';
import {
  IS_BLACKLIST_OPTIONS,
  IS_OFFERLIVE_OPTIONS
} from '../../filter/filterConst.json';
import {
  addOrReplaceFilterState,
  pushOrUpdateObject
} from '../../../../../Shared/Utils';
import filterByKeyword from '../filterByKeyword';

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
    isSaveAction: false
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
      buildDefaultTableFilter(state.entitiesOriginal, state.multipleFilter, state.isSaveAction);
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
      state.entities = executeFilterTable(state, filterByKeyword);
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
      state.entities = executeFilterTable(state, filterByKeyword);
    },
    removeBySettingId: (
      state,
      actions: PayloadAction<{ settingId: number }>
    ) => /*NOSONAR*/ {
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
        if(!state.isSaveAction) {
          state.filterState = [];
          state.valueFilterAllColumn = {};
        }
        buildDefaultTableFilter(state.entitiesOriginal, state.multipleFilter, state.isSaveAction);
        state.entities = executeFilterTable(state, filterByKeyword);
        state.isSaveAction = false;
      });
  }
});
const { reducer, actions } = LeagueInfoTableSlice;

const {
  clearFilter,
  setMultipleFilter,
  setLeagueHeaderFiler,
  setFocusSettingId,
  setIsOpenAddNewSetting,
  removeBySettingId,
  setColumnKeyValue,
  setValueFilter,
  filterBySearchKey,
  setIsSaveAction
} = actions;

export {
  clearFilter,
  setMultipleFilter,
  setLeagueHeaderFiler,
  setFocusSettingId,
  setIsOpenAddNewSetting,
  removeBySettingId,
  setColumnKeyValue,
  setValueFilter,
  filterBySearchKey,
  setIsSaveAction
};

export default reducer;

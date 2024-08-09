import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { editEventPickings, getEventPickings } from './eventSliceThunkNative';
import updateNoOfferMatchThunk from '../updateNoOfferMatchThunkNative';
import updateConfirmedThunk from '../updateConfirmedThunkNative';
import {
  updateNoOfferMatch,
  updateConfirmedMatch,
  buildMultipleFilterDefault,
  executeFilterTable
} from '../../helper/reducerHelper';
import TDataTable, {
  TTableSlice,
  TFilterState,
  FilterAllColumn,
  TColumnKey
} from '../../types/dataTableType';
import {
  addOrReplaceFilterState,
  filterDataTable,
  pushOrUpdateObject
} from '../../../Shared/Utils';
import filterByKeyword from '../filterByKeyword';
import type { PayloadAction } from '@reduxjs/toolkit';
import { Option } from '../../../Shared/MultipleSelection/type';
import { isDate } from 'lodash';

const EventPickingTableSlice = createSlice({
  name: 'EventPickingTableSlice',
  initialState: {
    matchSelected: [] as TDataTable[],
    openPopupState: {
      isOpenAddSpecialProduct: false,
      isOpenRemoveSpecialProduct: false
    },
    entitiesByDate: [],
    entities: [],
    entitiesOriginal: [],
    isNoOfferAll: false,
    isConfirmAll: false,
    isSelectedAll: false,
    valueFilterAllColumn: {} as FilterAllColumn,
    columnKeyFilterCurrent: '',
    filterState: [] as TFilterState[],
    loading: REQUEST_STATUS.idle,
    previousNoOfferIndex: null,
    multipleFilter: {
      KO_Time: {
        isFilter: false,
        selected: [],
        options: []
      },
      MB: {
        isFilter: false,
        selected: [],
        options: []
      }
    }
  } as TTableSlice,
  reducers: {
    setOpenPopupState: (state, action) => {
      const { key, value } = action.payload;
      state.openPopupState = { ...state.openPopupState, [key]: value };
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
    filterBySearchKey: (state, action) => {
      const { filterState } = state;
      state.filterState = addOrReplaceFilterState(filterState, action.payload);
      state.entities = executeFilterTable(state, filterByKeyword);
      state.isConfirmAll = state.entities?.every(x => x.isCheck);
      state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
    },
    clearFilter: state => {
      const { entitiesOriginal } = state;
      state.filterState = [];
      state.valueFilterAllColumn = {};
      state.entities = entitiesOriginal;
      state.isConfirmAll = state.entities?.every(x => x.isCheck);
      state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
      state.multipleFilter = buildMultipleFilterDefault(state.entitiesOriginal);
    },
    setMultipleFilter: (
      state,
      action: PayloadAction<{
        columnKey: Required<TColumnKey>;
        selected: Option[];
      }>
    ) => {
      const { columnKey, selected } = action.payload;
      state.multipleFilter[columnKey] = {
        ...state.multipleFilter[columnKey],
        isFilter:
          state.multipleFilter[columnKey].options.length !== selected.length,
        selected
      };
      state.entities = executeFilterTable(state, filterByKeyword);
      state.isConfirmAll = state.entities?.every(x => x.isCheck);
      state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
    },
    setPreviousNoOfferRow: (state, action) => {
      state.previousNoOfferIndex = action.payload;
    },
    setValueMaxbetSpread: (
      state,
      action: PayloadAction<{
        key: 'maxbetHdp' | 'spreadOu' | 'spreadHdp';
        infoSiteMatchId: number;
        value: number | undefined;
      }>
    ) => {
      const { key, value, infoSiteMatchId } = action.payload;
      const target = state.entities.find(
        s => s.infoSiteMatchId === infoSiteMatchId
      );
      if (target) {
        target.isModified = true;
        target[key] = value;
      }
    },
    setValueNoOfHdp: (
      state,
      action: PayloadAction<{
        infoSiteMatchId: number;
        value: string | undefined;
      }>
    ) => {
      const { value, infoSiteMatchId } = action.payload;
      const target = state.entities.find(
        s => s.infoSiteMatchId === infoSiteMatchId
      );
      if (target) {
        target.noOfHDP = value;
        target.isModified = true;
      }
    },
    setMatchSelected: (
      state,
      action: PayloadAction<{
        match: TDataTable;
        isAdded: boolean;
      }>
    ) => {
      const { match, isAdded } = action.payload;

      state.matchSelected = isAdded
        ? [...state.matchSelected, match]
        : state.matchSelected.filter(
            s => s.infoSiteMatchId !== match.infoSiteMatchId
          );
    },

    setListMatchSelected: (
      state,
      action: PayloadAction<{
        matches: TDataTable[];
        isAdded: boolean;
      }>
    ) => {
      const { matches, isAdded } = action.payload;
      state.matchSelected = isAdded
        ? [...state.matchSelected, ...matches]
        : state.matchSelected.filter(
            s => !matches.some(i => s.infoSiteMatchId === i.infoSiteMatchId)
          );
      state.isSelectedAll = false;
    },
    setAllMatchSelected: (
      state,
      action: PayloadAction<{
        isAdded: boolean;
      }>
    ) => {
      const { isAdded } = action.payload;
      state.matchSelected = isAdded ? state.entities : [];
      state.isSelectedAll = isAdded;
    }
  },
  extraReducers: builder => {
    builder
      .addCase(updateNoOfferMatchThunk.fulfilled, (state, { payload }) => {
        const noOfferMatch = payload;
        updateNoOfferMatch(state.entitiesOriginal, noOfferMatch);
        updateNoOfferMatch(state.entities, noOfferMatch);
        updateNoOfferMatch(state.entitiesByDate, noOfferMatch);
        state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
      })
      .addCase(updateConfirmedThunk.fulfilled, (state, { payload }) => {
        const confirmInfo = payload;
        updateConfirmedMatch(state.entitiesOriginal, confirmInfo);
        updateConfirmedMatch(state.entities, confirmInfo);
        state.isConfirmAll = state.entities?.every(x => x.isCheck);
      })
      .addCase(getEventPickings.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(getEventPickings.fulfilled, (state, { payload }) => {
        const { matchesByDate, filteredMatches } = payload;
        state.loading = REQUEST_STATUS.idle;
        state.entitiesByDate = matchesByDate;
        state.entitiesOriginal = filteredMatches;
        state.entities = filterDataTable(
          state.entitiesOriginal,
          state.filterState,
          filterByKeyword
        );

        state.multipleFilter = buildMultipleFilterDefault(state.entities);
        state.isConfirmAll = state.entities?.every(x => x.isCheck);
        state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
      })
      .addCase(getEventPickings.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      })
      .addCase(editEventPickings.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(editEventPickings.fulfilled, (state, { payload }) => {
        const { matchesByDate, filteredMatches } = payload;
        state.loading = REQUEST_STATUS.idle;
        state.entitiesByDate = matchesByDate;
        state.entitiesOriginal = filteredMatches;
        state.entities = filterDataTable(
          state.entitiesOriginal,
          state.filterState,
          filterByKeyword
        );

        state.multipleFilter = buildMultipleFilterDefault(state.entities);
        state.isConfirmAll = state.entities?.every(x => x.isCheck);
        state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
      })
      .addCase(editEventPickings.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      });
  }
});
const { reducer, actions } = EventPickingTableSlice;
const {
  filterBySearchKey,
  clearFilter,
  setMultipleFilter,
  setColumnKeyValue,
  setValueFilter,
  setPreviousNoOfferRow,
  setValueMaxbetSpread,
  setValueNoOfHdp,
  setOpenPopupState,
  setListMatchSelected,
  setAllMatchSelected
} = actions;

export {
  filterBySearchKey,
  clearFilter,
  setColumnKeyValue,
  setValueFilter,
  setPreviousNoOfferRow,
  setMultipleFilter,
  setValueMaxbetSpread,
  setValueNoOfHdp,
  setOpenPopupState,
  setListMatchSelected,
  setAllMatchSelected
};
export default reducer;

import { createSlice } from '@reduxjs/toolkit';
import { getConfirmProvider, getEvents } from './tableThunk';
import {
  TColumnKey,
  TDataSource,
  TMultipleFilter,
  TUpdateConfirmProvider,
  TUpdateRow
} from '../../type/table';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import type { PayloadAction } from '@reduxjs/toolkit';
import { Option } from '../../../Shared/MultipleSelection/type';
import { buildIsReadValue } from '../../../Shared/ticketEvent/helper/ticketHelper';
import { buildDefaultTableFilter, executeFilterTable } from './helper';

const buildMultipleFilterDefault = () => ({
  isFilter: false,
  selected: []
});

const tableSlice = createSlice({
  name: 'tableSlice',
  initialState: {
    loadingDataTable: REQUEST_STATUS.pending,
    loadingConfirmProviderOption: REQUEST_STATUS.pending,
    dataSource: [] as TDataSource[],
    dataSourceOriginal: [] as TDataSource[],
    confirmProviderOption: [] as Option[],
    multipleFilter: {
      MatchNo: buildMultipleFilterDefault(),
      KoTime: buildMultipleFilterDefault(),
      League: buildMultipleFilterDefault(),
      Match: buildMultipleFilterDefault(),
      LstBookProvider: buildMultipleFilterDefault(),
      Provider: buildMultipleFilterDefault(),
      ConfirmProvider: buildMultipleFilterDefault()
    } as TMultipleFilter
  },
  reducers: {
    setValue: (state, action: PayloadAction<TUpdateRow>) => {
      const { owpMatchId, key, value } = action.payload;
      const event = state.dataSource.find(d => d.OWPMatchId === owpMatchId);
      event && (event[key] = value);
      const eventOriginal = state.dataSourceOriginal.find(
        d => d.OWPMatchId === owpMatchId
      );
      eventOriginal && (eventOriginal[key] = value);
      state.multipleFilter[key] = {
        ...state.multipleFilter[key],
        selected: [
          ...state.multipleFilter[key].selected,
          { value: value ?? '', label: value ?? '' }
        ]
      };
    },
    setDataSignalR: (state, action) => {
      const { actorId, parentMatchId, userId } = action.payload;
      state.dataSource.forEach((item: TDataSource) => {
        if (item.OWPMatchId === parentMatchId) {
          item.IsRead = buildIsReadValue(actorId, userId, item.IsRead);
          item.HasTicket = true;
        }
      });
    },
    setConfirmProvider: (state, action: PayloadAction<TUpdateConfirmProvider>) => {
      const { owpMatchId, value } = action.payload;
      const event = state.dataSource.find(d => d.OWPMatchId === owpMatchId);
      event && (event.ConfirmProvider = value);
      const eventOriginal = state.dataSourceOriginal.find(
        d => d.OWPMatchId === owpMatchId
      );
      eventOriginal && (eventOriginal.ConfirmProvider = value);
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
      state.multipleFilter[columnKey] = {
        ...state.multipleFilter[columnKey],
        isFilter,
        selected
      };
      state.dataSource = executeFilterTable(
        state.dataSourceOriginal,
        state.multipleFilter
      );
    },
    setDataTable: (state, action) => {
      state.dataSource = action.payload;
    },

    handleClearFilter: state => {
      state.dataSource = state.dataSourceOriginal;
      buildDefaultTableFilter(
        state.dataSource,
        state.confirmProviderOption,
        state.multipleFilter
      );
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getEvents.pending, state => {
        state.loadingDataTable = REQUEST_STATUS.pending;
      })
      .addCase(
        getEvents.fulfilled,
        (state, action: PayloadAction<TDataSource[]>) => {
          state.loadingDataTable = REQUEST_STATUS.idle;
          state.dataSource = action.payload;
          state.dataSourceOriginal = action.payload;
          buildDefaultTableFilter(
            state.dataSourceOriginal,
            state.confirmProviderOption,
            state.multipleFilter
          );
        }
      )
      .addCase(getEvents.rejected, state => {
        state.loadingDataTable = REQUEST_STATUS.idle;
      })
      .addCase(getConfirmProvider.pending, state => {
        state.loadingConfirmProviderOption = REQUEST_STATUS.pending;
      })
      .addCase(getConfirmProvider.fulfilled, (state, { payload }) => {
        state.loadingConfirmProviderOption = REQUEST_STATUS.idle;
        state.confirmProviderOption = payload;
      })
      .addCase(getConfirmProvider.rejected, state => {
        state.loadingConfirmProviderOption = REQUEST_STATUS.idle;
      });
  }
});

const { actions, reducer } = tableSlice;
const {
  setValue,
  setDataSignalR,
  setConfirmProvider,
  setMultipleFilter,
  handleClearFilter,
  setDataTable
} = actions;
export {
  setValue,
  setDataSignalR,
  setConfirmProvider,
  setMultipleFilter,
  handleClearFilter,
  setDataTable
};
export default reducer;

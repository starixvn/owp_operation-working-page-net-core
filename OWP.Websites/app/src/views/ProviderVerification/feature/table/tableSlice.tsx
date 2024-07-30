import { createSlice } from '@reduxjs/toolkit';
import { getAgentConfirm, getEvents } from './tableThunk';
import {
  TColumnKey,
  TDataSource,
  TMultipleFilter,
  TUpdateAgentConfirm,
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
    loadingAgentConfirmOption: REQUEST_STATUS.pending,
    dataSource: [] as TDataSource[],
    dataSourceOriginal: [] as TDataSource[],
    agentConfirmOption: [] as Option[],
    multipleFilter: {
      MatchNo: buildMultipleFilterDefault(),
      MatchStatus: buildMultipleFilterDefault(),
      KoTime: buildMultipleFilterDefault(),
      League: buildMultipleFilterDefault(),
      Match: buildMultipleFilterDefault(),
      Mb: buildMultipleFilterDefault(),
      Provider: buildMultipleFilterDefault(),
      OtherProvider: buildMultipleFilterDefault(),
      AgentEst: buildMultipleFilterDefault(),
      RemarkEst: buildMultipleFilterDefault(),
      RemarkBy: buildMultipleFilterDefault(),
      AgentConfirm: buildMultipleFilterDefault(),
      EstNote: buildMultipleFilterDefault()
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
    setAgentConfirm: (state, action: PayloadAction<TUpdateAgentConfirm>) => {
      const { owpMatchId, value } = action.payload;
      const event = state.dataSource.find(d => d.OWPMatchId === owpMatchId);
      event && (event.AgentConfirm = value);
      const eventOriginal = state.dataSourceOriginal.find(
        d => d.OWPMatchId === owpMatchId
      );
      eventOriginal && (eventOriginal.AgentConfirm = value);
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
        state.agentConfirmOption,
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
            state.agentConfirmOption,
            state.multipleFilter
          );
        }
      )
      .addCase(getEvents.rejected, state => {
        state.loadingDataTable = REQUEST_STATUS.idle;
      })
      .addCase(getAgentConfirm.pending, state => {
        state.loadingAgentConfirmOption = REQUEST_STATUS.pending;
      })
      .addCase(getAgentConfirm.fulfilled, (state, { payload }) => {
        state.loadingAgentConfirmOption = REQUEST_STATUS.idle;
        state.agentConfirmOption = payload;
      })
      .addCase(getAgentConfirm.rejected, state => {
        state.loadingAgentConfirmOption = REQUEST_STATUS.idle;
      });
  }
});

const { actions, reducer } = tableSlice;
const {
  setValue,
  setDataSignalR,
  setAgentConfirm,
  setMultipleFilter,
  handleClearFilter,
  setDataTable
} = actions;
export {
  setValue,
  setDataSignalR,
  setAgentConfirm,
  setMultipleFilter,
  handleClearFilter,
  setDataTable
};
export default reducer;

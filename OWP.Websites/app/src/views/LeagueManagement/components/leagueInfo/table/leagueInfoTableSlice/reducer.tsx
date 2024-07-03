import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { TColumnKey, TTableSlice } from '../TableType';
import {
  getLeagueInfoThunk
} from './leagueInfoThunk';
import { REQUEST_STATUS } from '../../../../../Shared/Constants';
import { Option } from '../../../../../Shared/MultipleSelection/type';
import { executeFilterTable, buildDefaultTableFilter } from './reducerHelper';

const buildMultipleFilterDefault = () => ({
  isFilter: false,
  selected: []
});

const LeagueInfoTableSlice = createSlice({
  name: 'leagueInfoTableSlice',
  initialState: {
    entities: [],
    entitiesOriginal: [],
    loading: REQUEST_STATUS.idle,
    leagueFilterHeader: '',
    focusSettingId: 0,
    multipleFilter: {
      League_Group: buildMultipleFilterDefault(),
      League: buildMultipleFilterDefault(),
      Tv: buildMultipleFilterDefault(),
      Round: buildMultipleFilterDefault(),
      League_Division: buildMultipleFilterDefault(),
      Remark: buildMultipleFilterDefault(),
      Spread_HDP_OU: buildMultipleFilterDefault(),
      MB_WK_HDP: buildMultipleFilterDefault(),
      MB_WK_NO_HDP: buildMultipleFilterDefault(),
      MB_WD_HDP: buildMultipleFilterDefault(),
      MB_WD_NO_HDP: buildMultipleFilterDefault(),
      LST_Note: buildMultipleFilterDefault(),
      Remark2: buildMultipleFilterDefault(),
      Comment: buildMultipleFilterDefault()
    }
  } as TTableSlice,
  reducers: {
    clearFilter: state => {
      const { entitiesOriginal } = state;
      state.entities = entitiesOriginal;
      buildDefaultTableFilter(state.entitiesOriginal, state.multipleFilter);
    },
    setLeagueHeaderFiler: (state, actions) => { //NOSONAR
      const league = actions.payload;
      state.leagueFilterHeader = league;
    },
    setFocusSettingId: (state, actions) => { //NOSONAR
      state.focusSettingId = actions.payload;
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
        selected,
        isFilter
      };
      state.entities = executeFilterTable(
        state.entitiesOriginal,
        state.multipleFilter
      );
    },

  },
  extraReducers: builder => {
    builder
      .addCase(getLeagueInfoThunk.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(getLeagueInfoThunk.fulfilled, (state, { payload }) => {
        state.entities = payload;
        state.entitiesOriginal = payload;
        state.loading = REQUEST_STATUS.idle;
        buildDefaultTableFilter(state.entitiesOriginal, state.multipleFilter);
      });
  }
});
const { reducer, actions } = LeagueInfoTableSlice;

const { clearFilter, setMultipleFilter, setLeagueHeaderFiler, setFocusSettingId } = actions;

export { clearFilter, setMultipleFilter, setLeagueHeaderFiler, setFocusSettingId };

export default reducer;

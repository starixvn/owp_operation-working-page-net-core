import { createSlice } from '@reduxjs/toolkit';
import { Option } from '../../../Shared/MultipleSelection/type';
import { TFilter, TPayloadFilter, TPeriodFilter } from '../../types/filterType';
import {
  DATE,
  FILTER_MATCH,
  FILTER_MATCH_CATEGORY,
  FILTER_MATCH_CATEGORY_OPTIONS,
  FILTER_MATCH_OPTIONS,
  HAS_OTHER_PROVIDER,
  HAS_PROVIDER,
  HAS_PROVIDER_OPTIONS,
  SELECT_GAME,
  SELECT_GAME_OPTIONS,
  UPDATE_TYPE,
  NONE_OPTION,
  FROM_DATE,
  TO_DATE,
  PERIOD
} from '../filterConst.json';
import { SELECT_TURNOVER_OPTIONS } from '../const';
import { getUpdateTypes } from './filterThunkNative';
import Utils from '../../../Shared/Utils';

const {
  VALUE: { ONE_MONTH }
} = PERIOD;

const filterSlice = createSlice({
  name: 'filter/filterSlice',
  initialState: {
    leagues: [] as Option[],
    updateTypes: [] as Option[],
    lastSearchTime: null as Date | null,
    param: {
      [DATE]: Utils.formatDate(new Date()),
      [FILTER_MATCH]: FILTER_MATCH_OPTIONS.map(m => m.value),
      [SELECT_GAME]: SELECT_GAME_OPTIONS.map(m => m.value),
      [HAS_PROVIDER]: HAS_PROVIDER_OPTIONS.map(m => m.value),
      [HAS_OTHER_PROVIDER]: HAS_PROVIDER_OPTIONS.map(m => m.value),
      [FILTER_MATCH_CATEGORY]: FILTER_MATCH_CATEGORY_OPTIONS.map(m => m.value),
      [UPDATE_TYPE]: [],
      [PERIOD.KEY]: {
        [ONE_MONTH]: {
          [FROM_DATE]: SELECT_TURNOVER_OPTIONS[0].fromDate,
          [TO_DATE]: SELECT_TURNOVER_OPTIONS[0].toDate
        }
      }
    } as TFilter
  },
  reducers: {
    setFilter: (state, action: { payload: TPayloadFilter }) => {
      const { key, value } = action.payload;
      state.param[key] = value;
    },
    setPeriod: (state, action: { payload: TPeriodFilter }) => {
      state.param[PERIOD.KEY] = action.payload;
    },
    setLastSearchTime: (state, action: { payload: Date }) => {
      state.lastSearchTime = action.payload;
    }
  },
  extraReducers: builder => {
    builder.addCase(getUpdateTypes.fulfilled, (state, { payload }) => {
      state.updateTypes = payload.concat(NONE_OPTION);
      state.param[UPDATE_TYPE] = state.updateTypes.map(m => m.value);
    });
  }
});

const { reducer, actions } = filterSlice;
const { setFilter, setPeriod, setLastSearchTime } = actions;
export { setFilter, setPeriod, setLastSearchTime };
export default reducer;

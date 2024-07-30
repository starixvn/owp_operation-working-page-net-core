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
  LEAGUE,
  SELECT_GAME,
  SELECT_GAME_OPTIONS,
  UPDATE_TYPE,
  NONE_OPTION,
  FROM_DATE,
  TO_DATE,
  PERIOD
} from '../filterConst.json';
import { SELECT_TURNOVER_OPTIONS } from '../const';
import { getLeagues, getUpdateTypes } from './filterThunkNative';
import Utils from '../../../Shared/Utils';

const { VALUE: {
  ONE_MONTH,
  THREE_MONTHS, SIX_MONTHS, TWELVE_MONTH
} } = PERIOD;

const filterSlice = createSlice({
  name: 'filter/filterSlice',
  initialState: {
    leagues: [] as Option[],
    updateTypes: [] as Option[],
    param: {
      [DATE]: Utils.formatDate(new Date()),
      [FILTER_MATCH]: FILTER_MATCH_OPTIONS.map(m => m.value),
      [SELECT_GAME]: SELECT_GAME_OPTIONS.map(m => m.value),
      [HAS_PROVIDER]: HAS_PROVIDER_OPTIONS.map(m => m.value),
      [HAS_OTHER_PROVIDER]: HAS_PROVIDER_OPTIONS.map(m => m.value),
      [FILTER_MATCH_CATEGORY]: FILTER_MATCH_CATEGORY_OPTIONS.map(m => m.value),
      [LEAGUE]: [],
      [UPDATE_TYPE]: [],
      [PERIOD.KEY]: {
        [ONE_MONTH]: {
          [FROM_DATE]: SELECT_TURNOVER_OPTIONS[0].fromDate,
          [TO_DATE]: SELECT_TURNOVER_OPTIONS[0].toDate
        },
        [THREE_MONTHS]: {
          [FROM_DATE]: SELECT_TURNOVER_OPTIONS[1].fromDate,
          [TO_DATE]: SELECT_TURNOVER_OPTIONS[1].toDate
        },
        [SIX_MONTHS]: {
          [FROM_DATE]: SELECT_TURNOVER_OPTIONS[2].fromDate,
          [TO_DATE]: SELECT_TURNOVER_OPTIONS[2].toDate
        },
        [TWELVE_MONTH]: {
          [FROM_DATE]: SELECT_TURNOVER_OPTIONS[3].fromDate,
          [TO_DATE]: SELECT_TURNOVER_OPTIONS[3].toDate
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
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getLeagues.fulfilled, (state, { payload }) => {
        state.leagues = payload;
        state.param[LEAGUE] = payload.map(m => m.value);
      })
      .addCase(getUpdateTypes.fulfilled, (state, { payload }) => {
        const noneOption = NONE_OPTION as Option;
        state.updateTypes = payload.concat(noneOption);
        state.param[UPDATE_TYPE] = state.updateTypes.map(m => m.value);
      });
  }
});

const { reducer, actions } = filterSlice;
const { setFilter, setPeriod } = actions;
export { setFilter, setPeriod };
export default reducer;

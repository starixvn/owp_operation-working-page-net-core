import type { PayloadAction } from '@reduxjs/toolkit';
import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../../../Shared/Constants';
import { TCondition } from '../../table/TableType';
import { TOption, TAddSetting, TAddExpressionKey } from '../../component/Type';
import { getLeagueGroupOptionsThunk } from '../../component/commonThunk';
interface ConditionPayLoad {
  key: 'type' | 'value';
  property: keyof TAddExpressionKey;
  value: ('Contains' | 'Equals') | string;
  index: number;
}

const AddSettingSlice = createSlice({
  name: 'addSettingSlice',
  initialState: {
    leagueGroupOptions: [] as TOption[],
    loading: REQUEST_STATUS.idle,
    leagueSettingId: 0,
    league: '',
    owpLeagueId: 0,
    leagueGroupId: 0,
    leagueGroup: '',
    spread: '',
    maxbetWD_HDP: '',
    wD_NoOfHDP: '',
    maxbetWK_HDP: '',
    wK_NoOfHDP: '',
    lstNote: '',
    remark2: '',
    comment: '',
    hasTv: false,
    hasIpTv: false,
    remark: '',
    round: [{ value: '', type: 'Contains' }] as Array<TCondition>,
    division: [{ value: '', type: 'Contains' }] as Array<TCondition>
  },
  reducers: {
    updateObjectAddPopup: (
      state,
      action: PayloadAction<{
        property: keyof TAddSetting;
        value: string | number | boolean;
      }>
    ) => {
      const { property, value } = action.payload;
      return { ...state, [property]: value };
    },
    addRowExpressionsAddPopup: (
      state,
      action: { payload: keyof TAddExpressionKey }
    ) => {
      state[action.payload].push({ type: 'Contains', value: '' });
    },
    removeRowExpressionsAddPopup: (
      state,
      action: { payload: { index: number; property: keyof TAddExpressionKey } }
    ) => {
      const { property } = action.payload;

      state[property] = state[property].filter(
        (_, index) => index !== action.payload.index
      );
    },
    updateExpressionAddPopup: (
      state,
      action: PayloadAction<ConditionPayLoad>
    ) => {
      const { key, property, value, index } = action.payload;

      if (key === 'type' && (value === 'Contains' || value === 'Equals')) {
        state[property][index].type = value;
      }
      if (key === 'value') {
        state[property][index].value = value;
      }
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getLeagueGroupOptionsThunk.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(getLeagueGroupOptionsThunk.fulfilled, (state, { payload }) => {
        state.leagueGroupOptions = payload;
        state.loading = REQUEST_STATUS.idle;
      });
  }
});

const { reducer, actions } = AddSettingSlice;
const {
  updateObjectAddPopup,
  addRowExpressionsAddPopup,
  removeRowExpressionsAddPopup,
  updateExpressionAddPopup
} = actions;

export default reducer;
export {
  updateObjectAddPopup,
  addRowExpressionsAddPopup,
  removeRowExpressionsAddPopup,
  updateExpressionAddPopup
};

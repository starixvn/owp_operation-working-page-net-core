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
const addSetting = {
  leagueGroupOptions: [] as TOption[],
  loading: REQUEST_STATUS.idle,
  leagueSettingId: 0,
  league: '',
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
  division: [{ value: '', type: 'Contains' }] as Array<TCondition>,
  shortLeague: '',
  isBlackList: false,
  noOfferLive: false
};

const AddSettingSlice = createSlice({
  name: 'addSettingSlice',
  initialState: {
    ...addSetting
  },
  reducers: {
    setValueDefault: state => {
      const {
        leagueGroupOptions,
        loading,
        leagueSettingId,
        league,
        leagueGroupId,
        leagueGroup,
        spread,
        maxbetWD_HDP,
        wD_NoOfHDP,
        maxbetWK_HDP,
        wK_NoOfHDP,
        lstNote,
        remark2,
        comment,
        hasTv,
        hasIpTv,
        remark,
        round,
        division,
        shortLeague,
        isBlackList,
        noOfferLive
      } = addSetting;
      state.leagueGroupOptions = leagueGroupOptions;
      state.loading = loading;
      state.leagueSettingId = leagueSettingId;
      state.league = league;
      state.leagueGroupId = leagueGroupId;
      state.leagueGroup = leagueGroup;
      state.spread = spread;
      state.maxbetWD_HDP = maxbetWD_HDP;
      state.wD_NoOfHDP = wD_NoOfHDP;
      state.maxbetWK_HDP = maxbetWK_HDP;
      state.wK_NoOfHDP = wK_NoOfHDP;
      state.lstNote = lstNote;
      state.remark2 = remark2;
      state.comment = comment;
      state.hasTv = hasTv;
      state.hasIpTv = hasIpTv;
      state.remark = remark;
      state.round = round;
      state.division = division;
      state.shortLeague = shortLeague;
      state.isBlackList = isBlackList;
      state.noOfferLive = noOfferLive;
    },
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

      if (key === 'type' && (value === 'Contains' || value === 'Equals' || value === 'Pairwise Contains')) {
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
  updateExpressionAddPopup,
  setValueDefault
} = actions;

export default reducer;
export {
  updateObjectAddPopup,
  addRowExpressionsAddPopup,
  removeRowExpressionsAddPopup,
  updateExpressionAddPopup,
  setValueDefault
};

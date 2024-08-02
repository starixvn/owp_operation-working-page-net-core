import type { PayloadAction } from '@reduxjs/toolkit';
import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../../../Shared/Constants';
import { TCondition, TDataSource } from '../../table/TableType';
import { TOption } from '../../editPopup/EditType';
import { getLeagueGroupOptionsThunk } from '../../component/commonThunk';
const AddSettingSlice = createSlice({
  name: 'editSettingSlice',
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
  reducers: {},
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

export default reducer;

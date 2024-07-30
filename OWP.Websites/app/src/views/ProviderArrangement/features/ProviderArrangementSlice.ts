import { createSlice } from '@reduxjs/toolkit';
import {
  queryProviderArrangement,
  updateProviderArrangement,
  queryLogProviderArrangement
} from './ProviderArrangementThunkNative';
import { ProviderData, TQueryLogProviderArrangement } from './TypeNative';

const ProviderArrangementSlice = createSlice({
  name: 'providerArrangementSlice',
  initialState: {
    logData: [] as TQueryLogProviderArrangement[],
    isLoading: false,
    isEdit: false,
    originalProviders: [] as ProviderData[],
    providers: [] as ProviderData[]
  },
  reducers: {
    changeIsEdit: state => {
      state.isEdit = !state.isEdit;
    },
    setProviders: (state, { payload }) => {
      state.providers = payload;
    }
  },
  extraReducers: builder => {
    builder
      .addCase(queryProviderArrangement.pending, state => {
        state.isLoading = true;
      })
      .addCase(queryProviderArrangement.fulfilled, (state, { payload }) => {
        state.isLoading = false;
        state.originalProviders = payload;
        state.providers = payload;
      })
      .addCase(queryProviderArrangement.rejected, state => {
        state.isLoading = false;
      })
      .addCase(updateProviderArrangement.pending, state => {
        state.isLoading = true;
      })
      .addCase(updateProviderArrangement.fulfilled, state => {
        state.isLoading = false;
      })
      .addCase(updateProviderArrangement.rejected, state => {
        state.isLoading = false;
      })
      .addCase(queryLogProviderArrangement.fulfilled, (state, { payload }) => {
        state.logData = payload;
      });
  }
});

const { reducer, actions } = ProviderArrangementSlice;
const { changeIsEdit, setProviders } = actions;

export {
  queryProviderArrangement,
  changeIsEdit,
  updateProviderArrangement,
  setProviders,
  queryLogProviderArrangement
};
export default reducer;

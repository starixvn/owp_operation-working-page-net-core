import { createSlice } from '@reduxjs/toolkit';
import { getConfirmProvider } from '../table/tableThunk';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { Option } from '../../../Shared/MultipleSelection/type';

const commonSlice = createSlice({
  name: 'tableSlice',
  initialState: {
    confirmProvider: {
      loadingConfirmProviderOption: REQUEST_STATUS.pending,
      confirmProviderOption: [] as Option[]
    }
  },
  reducers: {},
  extraReducers: builder => {
    builder
      .addCase(getConfirmProvider.pending, state => {
        state.confirmProvider.loadingConfirmProviderOption =
          REQUEST_STATUS.pending;
      })
      .addCase(getConfirmProvider.fulfilled, (state, { payload }) => {
        state.confirmProvider.loadingConfirmProviderOption =
          REQUEST_STATUS.idle;
        state.confirmProvider.confirmProviderOption = payload;
      })
      .addCase(getConfirmProvider.rejected, state => {
        state.confirmProvider.loadingConfirmProviderOption =
          REQUEST_STATUS.idle;
      });
  }
});

const { reducer } = commonSlice;
export default reducer;

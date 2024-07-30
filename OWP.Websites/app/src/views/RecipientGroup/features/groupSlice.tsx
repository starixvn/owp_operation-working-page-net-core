import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { getRecipientGroup } from './groupThunkNative';
import { GroupDataSource } from './Type';

const groupSlice = createSlice({
  name: 'groupSlice',
  initialState: {
    loading: REQUEST_STATUS.pending,
    groups: [] as GroupDataSource[]
  },
  reducers: {},
  extraReducers: builder => {
    builder.addCase(getRecipientGroup.pending, state => {
      state.loading = REQUEST_STATUS.pending;
    });
    builder.addCase(getRecipientGroup.fulfilled, (state, { payload }) => {
      state.loading = REQUEST_STATUS.idle;
      state.groups = payload;
    });
    builder.addCase(getRecipientGroup.rejected, state => {
      state.loading = REQUEST_STATUS.idle;
    });
  }
});

const { reducer } = groupSlice;

export {
  getRecipientGroup
};

export default reducer;

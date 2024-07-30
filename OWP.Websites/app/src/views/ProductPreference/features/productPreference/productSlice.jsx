import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { getProductPreference } from './productThunkNative';

const productSlice = createSlice({
  name: 'productSlice',
  initialState: {
    entities: [],
    loading: REQUEST_STATUS.pending,
    dropdownOptionLoading: REQUEST_STATUS.pending
  },
  reducers: {
    updateUserUserInfo: (state, action) => {
      const { key, value, rowIndex } = action.payload;
      state.entities[rowIndex][key] = value;
    }
  },
  extraReducers: builder=>{
    builder.addCase(getProductPreference.pending, state => {
      state.loading = REQUEST_STATUS.pending;
    })
    .addCase(getProductPreference.fulfilled, (state, action) => {
      state.entities = action.payload;
      state.loading = REQUEST_STATUS.idle;
    })
    .addCase(getProductPreference.rejected, state => {
      state.loading = REQUEST_STATUS.idle;
    });
  }
});

const { reducer, actions } = productSlice;
const { updateUserUserInfo } = actions;
export { updateUserUserInfo };
export default reducer;

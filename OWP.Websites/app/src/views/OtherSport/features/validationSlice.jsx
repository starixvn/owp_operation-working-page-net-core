import { createSlice } from '@reduxjs/toolkit';
import validateFileThunk from './validationThunkNative';

const validationSlice = createSlice({
  name: 'validation',
  initialState: {
    validationResult: {}
  },
  extraReducers: builder => {
    builder
      .addCase(validateFileThunk.pending, state => {
        state.loading = true;
      })
      .addCase(validateFileThunk.fulfilled, (state, action) => {
        state.loading = false;
        state.validationResult = action.payload;
      });
  }
});

const { reducer, actions } = validationSlice;

export { actions };

export default reducer;

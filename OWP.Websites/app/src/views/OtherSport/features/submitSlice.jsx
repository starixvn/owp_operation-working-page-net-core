import { createSlice } from '@reduxjs/toolkit';
import submitThunk from './submitThunkNative';

const submitSlice = createSlice({
  name: 'submit',
  initialState: {
    submitResult: {}
  },
  reducers: {
    resetResponse: state => {
      state.submitResult = {};
    }
  },
  extraReducers: builder=>{
    builder.addCase(submitThunk.pending,state => {
      state.loading = true;
    })
    .addCase(submitThunk.fulfilled,(state, action) => {
      const {
        isSuccessful,
        message
      } = action.payload;
      state.loading = false;
      state.submitResult.isSuccessful = isSuccessful;
      state.submitResult.message = message;
    });
  }
});

const { reducer, actions } = submitSlice;
const { resetResponse } = actions;

export { resetResponse };
export default reducer;

import { createSlice } from '@reduxjs/toolkit';

const filterHeaderSlice = createSlice({
  name: 'filterHeader',
  initialState: {
    model: {}
  },
  reducers: {
    onSubmitFilterModel: (state, action) => {
      state.model = action.payload;
    }
  }
});

const { reducer, actions } = filterHeaderSlice;
const { onSubmitFilterModel } = actions;

export { onSubmitFilterModel };
export default reducer;

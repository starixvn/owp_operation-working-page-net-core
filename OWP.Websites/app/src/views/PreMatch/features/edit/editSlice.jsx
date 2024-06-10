import { createSlice } from '@reduxjs/toolkit';

const editSlice = createSlice({
  name: 'editor',
  initialState: {
    isEdit: false
  },
  reducers: {
    changeMode: {
      reducer: (state, action) => {
        const { isEdit } = action.payload;
        state.isEdit = isEdit;
      }
    }
  }
});

const { actions, reducer } = editSlice;
const { changeMode } = actions;
export { changeMode };
export default reducer;

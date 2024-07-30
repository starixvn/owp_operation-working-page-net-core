import { createSlice } from '@reduxjs/toolkit';
import { VIEW } from './mode.json';


const modeSlice = createSlice({
  name: 'mode/demoSlice',
  initialState: {
    mode: VIEW
  },
  reducers: {
    setMode: (state, action: { payload: string }) => {
      state.mode = action.payload;
    }
  }
});

const { reducer, actions } = modeSlice;
const { setMode } = actions;
export { setMode };
export default reducer;

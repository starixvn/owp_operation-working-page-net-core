import { createSlice } from '@reduxjs/toolkit';

const autoSlice = createSlice({
  name: 'auto',
  initialState: {
    isAuto: false,
    majorPairs:[],
    mainPairs:[],
    earlyPairs:[],
    majorPairsTakeOver:[]
  },
  reducers: {
    setAutoAssign: {
      reducer: (state, action) => {
        state.isAuto = action.payload;
      }
    }
  }
});

const { actions, reducer } = autoSlice;
const { setAutoAssign } = actions;
export { setAutoAssign };
export default reducer;

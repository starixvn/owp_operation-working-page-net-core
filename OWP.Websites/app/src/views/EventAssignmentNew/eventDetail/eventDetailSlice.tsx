import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';

const detailEventSlice = createSlice({
  name: 'detailEventSlice',
  initialState: {
    OwpMatchId: 0,
  },
  reducers: {
    setDetailEventInfo: (state, action: PayloadAction<number>) => {
      state.OwpMatchId = action.payload;
    }
  }
});
const { reducer, actions } = detailEventSlice;
const { setDetailEventInfo } = actions;
export { setDetailEventInfo };
export default reducer;

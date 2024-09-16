import { createSlice } from '@reduxjs/toolkit';
import { TMessageDetail } from '../../../type';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import { queryConsolidationThunk } from './consolidationThunk';

const consolidationSlice = createSlice({
  name: 'consolidationSlice',
  initialState: {
    loading: REQUEST_STATUS.pending,
    entities: [] as TMessageDetail[],
    isNewMessage: false
  },
  reducers: {
    setNewMessageConsolidation: (state, { payload: data }) => {
      state.entities.push(data);
      state.isNewMessage = true;
    },
    resetNewMessage: state => {
      state.isNewMessage = false;
    }
  },
  extraReducers(builder) {
    builder
      .addCase(queryConsolidationThunk.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(queryConsolidationThunk.fulfilled, (state, { payload }) => {
        state.loading = REQUEST_STATUS.idle;
        state.entities = payload;
      })
      .addCase(queryConsolidationThunk.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      });
  }
});
export { actions, setNewMessageConsolidation, resetNewMessage };

const { reducer, actions } = consolidationSlice;
const { setNewMessageConsolidation, resetNewMessage } = actions;

export default reducer;

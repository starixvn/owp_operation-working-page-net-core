import { createSlice } from '@reduxjs/toolkit';
import { TMessageDetail } from '../../../type';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import {queryConsolidationThunk} from './consolidationThunk';

const consolidationSlice = createSlice({
  name: 'consolidationSlice',
  initialState: {
    loading: REQUEST_STATUS.pending,
    entities: [] as TMessageDetail[]
  },
  reducers: {
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

const { reducer, actions } = consolidationSlice;

export default reducer;

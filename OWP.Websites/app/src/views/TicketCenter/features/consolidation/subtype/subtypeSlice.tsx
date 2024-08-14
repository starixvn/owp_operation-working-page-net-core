import { createSlice } from '@reduxjs/toolkit';
import { TNode } from '../../../../Shared/treeView/type';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import { querySubtypeThunk } from './subtypeThunk';

const subtypeSlice=createSlice({
  name: 'subtypeSlice',
  initialState: {
    loading: REQUEST_STATUS.pending,
    entities: [] as TNode[]
},
  reducers:{},
  extraReducers(builder) {
    builder.addCase(querySubtypeThunk.pending, state => {
      state.loading = REQUEST_STATUS.pending;
    })
    .addCase(querySubtypeThunk.fulfilled, (state, { payload }) => {
      state.loading = REQUEST_STATUS.idle;
      state.entities = payload;
    })
    .addCase(querySubtypeThunk.rejected, state => {
      state.loading = REQUEST_STATUS.pending;
    });
  },
});

const { reducer } = subtypeSlice;

export default reducer;
import { createSlice } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { deleteLeague } from './deleteLeagueThunkNative';

const deleteLeagueSlice = createSlice({
  name: 'deleteLeague',
  initialState: {
    leagueDeleted: [],
    loading: REQUEST_STATUS.idle,
    error: null
  },
  reducers: {
    refeshDeletedLeagueTable: {
      reducer: (state, action) => {
        const index = state.leagueDeleted.findIndex(x =>
          isEqual(x.PreMatchLeagueId, action.payload)
        );
        state.leagueDeleted.splice(index, 1);
      }
    }
  },
  extraReducers: builder=>{
    builder.addCase(deleteLeague.pending,state => {
      const { loading } = state;
      if (isEqual(loading, REQUEST_STATUS.idle)) {
        state.loading = REQUEST_STATUS.pending;
      }
    })
    .addCase(deleteLeague.fulfilled,(state, action) => {
      if (isEqual(state.loading, REQUEST_STATUS.pending)) {
        state.loading = REQUEST_STATUS.idle;
        state.leagueDeleted = action.payload;
      }
    })
    .addCase(deleteLeague.rejected, (state, action) => {
      if (isEqual(state.loading, REQUEST_STATUS.pending)) {
        state.loading = REQUEST_STATUS.idle;
        state.error = action.error;
      }
    });
  }
});

const { actions, reducer } = deleteLeagueSlice;
const { refeshDeletedLeagueTable } = actions;
export { actions, deleteLeague, refeshDeletedLeagueTable };

export default reducer;

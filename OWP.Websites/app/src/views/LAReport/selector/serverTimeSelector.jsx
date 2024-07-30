import { createSelector } from 'reselect';

const serverTimeSelector = createSelector(
  state => state.serverTimeReducer.pending,
  state => state.serverTimeReducer.serverTime,
  (pending, serverTime) => ({
    pending: pending ?? true,
    serverTime
  })
);

export { serverTimeSelector };

import { createSelector } from 'reselect';

const policySelector = createSelector(
  state => state.policyReducer.pending,
  state => state.policyReducer.laPolicy,
  (pending, laPolicy) => ({
    pending: pending ?? true,
    laPolicy
  })
);

export { policySelector };

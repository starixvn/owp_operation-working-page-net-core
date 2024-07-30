import { createSelector } from 'reselect';

const selectPolicy = createSelector(
  state => state.policyReducer.policyPending,
  policyPending => ({
    policyPending: policyPending ?? true
  })
);

export { selectPolicy };

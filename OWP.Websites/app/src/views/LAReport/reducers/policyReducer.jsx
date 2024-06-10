import { createReducer } from './reducerUtilities';
import { ACTION_QUERY_POLICY } from '../constants/action-type';

const defaultModel = {
  pending: true,
  laPolicy: {}
};

const handlePending = state => {
  return {
    ...state,
    pending: true
  };
};

const handleSuccess = (state, action) => {
  const { data } = action;
  return {
    ...state,
    pending: false,
    laPolicy: data.laPolicy
  };
};

const policyReducer = createReducer(defaultModel, {
  [ACTION_QUERY_POLICY.PENDING]: handlePending,
  [ACTION_QUERY_POLICY.SUCCESS]: handleSuccess
});

export default policyReducer;

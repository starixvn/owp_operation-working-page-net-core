import { createReducer } from './reducerUtilities';
import { ACTION_QUERY_POLICY } from '../constants/action-type';

const defaultModel = {
  policyPending: true,
  policyData: {}
};

const handlePending = state => {
  return {
    ...state,
    policyPending: true
  };
};

const handleSuccess = (state, action) => {
  const { data } = action;
  return {
    ...state,
    policyPending: false,
    policyData: {
      generalPolicy: data.generalPolicy,
      conditionPolicy: data.conditionPolicy,
      lstNotePolicy: data.lstNotePolicy,
      tvListPolicy: data.tvListPolicy,
      viewLogPolicy: data.viewLogPolicy
    }
  };
};

const policyReducer = createReducer(defaultModel, {
  [ACTION_QUERY_POLICY.PENDING]: handlePending,
  [ACTION_QUERY_POLICY.SUCCESS]: handleSuccess
});

export default policyReducer;

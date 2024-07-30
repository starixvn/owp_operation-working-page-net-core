import { createReducer } from './reducerUtilities';
import { ACTION_QUERY_SERVERTIME } from '../constants/action-type';

const defaultModel = {
  pending: true,
  serverTime: ''
};

const handlePending = state => {
  return {
    ...state,
    pending: true
  };
};

const handleSuccess = (state, action) => {
  const { serverTime } = action.data;
  return {
    ...state,
    pending: false,
    serverTime
  };
};

const serverTimeReducer = createReducer(defaultModel, {
  [ACTION_QUERY_SERVERTIME.PENDING]: handlePending,
  [ACTION_QUERY_SERVERTIME.SUCCESS]: handleSuccess
});

export default serverTimeReducer;

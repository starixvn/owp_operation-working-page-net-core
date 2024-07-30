import { createReducer } from './reducerUtilities';
import { QUERY_LOGHISTORY } from '../constants/action-type';

const defaultLogHistory = {
  Pending: true,
  Data: [],
  Error: []
};

const handlePending = state => {
  return {
    ...state,
    Pending: true
  };
};

const handleQuerySuccess = (state, action) => {
  return {
    ...state,
    Data: action.data,
    Pending: false
  };
};
const logHistoryReducer = createReducer(defaultLogHistory, {
  [QUERY_LOGHISTORY.PENDING]: handlePending,
  [QUERY_LOGHISTORY.SUCCESS]: handleQuerySuccess
});

export default logHistoryReducer;

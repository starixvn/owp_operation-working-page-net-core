import { createReducer } from './reducerUtilities';
import { ACTION_STATUS_FILTER } from '../constants/action-type';
import MaxbetConst from '../constants/MaxbetConst';

const defaultModel = {
  status: MaxbetConst.status[0]
};

const handleUpdateStatus = (state, action) => {
  return {
    ...state,
    status: action.payload.select
  };
};

const statusReducer = createReducer(defaultModel, {
  [ACTION_STATUS_FILTER.UPDATE]: handleUpdateStatus
});

export default statusReducer;

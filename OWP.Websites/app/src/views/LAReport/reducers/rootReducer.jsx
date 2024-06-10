import { combineReducers } from 'redux';
import reportReducer from './reportReducer';
import serverTimeReducer from './serverTimeReducer';
import policyReducer from './policyReducer';

export default combineReducers({
  reportReducer,
  serverTimeReducer,
  policyReducer
});

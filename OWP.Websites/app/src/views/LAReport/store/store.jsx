import { createStore, applyMiddleware, compose } from 'redux';
import rootReducer from '../reducers/rootReducer';
import {
  queryPolicyMiddleware,
  queryDataWithParamsMiddleware,
  queryServerTimeMiddleware
} from '../middlewares/apiMiddleware';

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

export default createStore(
  rootReducer,
  composeEnhancers(
    applyMiddleware(
      queryPolicyMiddleware,
      queryServerTimeMiddleware,
      queryDataWithParamsMiddleware
    )
  )
);

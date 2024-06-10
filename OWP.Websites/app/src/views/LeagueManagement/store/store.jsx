import { createStore, applyMiddleware, compose } from 'redux';
import rootReducer from '../reducers/rootReducer';
import {
  queryDataMiddleware,
  postDataMiddleware,
  queryPolicyMiddleware,
  queryDataWithParamsMiddleware,
  queryMatchOffersMiddleware
} from '../middlewares/apiMiddleware';

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

export default createStore(
  rootReducer,
  composeEnhancers(
    applyMiddleware(
      queryDataMiddleware,
      postDataMiddleware,
      queryPolicyMiddleware,
      queryDataWithParamsMiddleware,
      queryMatchOffersMiddleware
    )
  )
);

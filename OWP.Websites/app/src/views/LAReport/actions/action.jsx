import {
  QUERY_POLICY,
  ACTION_QUERY_POLICY,
  QUERY_SERVERTIME,
  ACTION_QUERY_SERVERTIME,
  QUERY_DATA_WITH_PARAMS,
  SEARCH,
  ORDER,
  CLEAR_ALLFILTERS
} from '../constants/action-type';

const queryServerTime = () => ({
  type: QUERY_SERVERTIME,
  payload: ACTION_QUERY_SERVERTIME
});

const queryPolicy = () => ({
  type: QUERY_POLICY,
  payload: ACTION_QUERY_POLICY
});

const queryDataWithParams = (url, model, actionPayload) => ({
  type: QUERY_DATA_WITH_PARAMS,
  payload: Object.assign({ url, model }, actionPayload)
});

const search = model => ({
  type: SEARCH,
  payload: model
});
const order = model => ({
  type: ORDER,
  payload: model
});

const clearAllFilters = isClearAll => ({
  type: CLEAR_ALLFILTERS,
  isClearAll
});
export {
  queryPolicy,
  queryServerTime,
  queryDataWithParams,
  search,
  clearAllFilters,
  order
};

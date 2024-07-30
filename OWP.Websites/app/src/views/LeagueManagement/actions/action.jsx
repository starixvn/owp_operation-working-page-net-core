import {
  QUERY_DATA,
  POST_DATA,
  UPDATE_STATUS_DISPLAY_POPUP_FILTER,
  EXECUTE_FILTER,
  CLEAR_FILTER,
  QUERY_POLICY,
  ACTION_QUERY_POLICY,
  TOGGLE_COLUMN,
  QUERY_DATA_WITH_PARAMS,
  EXECUTE_SPECIALPRODUCT_FILTER,
  QUERY_MATCH_OFFER,
  ACTION_QUERY_MATCH_OFFER
} from '../constants/action-type';

const crud = (type, model) => ({
  type,
  payload: model
});

const queryData = (url, actionPayload) => ({
  type: QUERY_DATA,
  payload: Object.assign({ url }, actionPayload)
});

const queryDataWithParams = (url, model, actionPayload) => ({
  type: QUERY_DATA_WITH_PARAMS,
  payload: Object.assign({ url, model }, actionPayload)
});

const postData = (url, model, actionPayload) => ({
  type: POST_DATA,
  payload: Object.assign({ url, model }, actionPayload)
});

const updateExpressionFilter = (type, model) => ({
  type,
  payload: model
});

const postDataWithExtraModel = (url, model, extraModel, actionPayload) => ({
  type: POST_DATA,
  payload: Object.assign({ url, model, extraModel }, actionPayload)
});

const updateStatusDisplayPopupFilter = model => ({
  type: UPDATE_STATUS_DISPLAY_POPUP_FILTER,
  payload: model
});

const executeFilter = model => ({
  type: EXECUTE_FILTER,
  payload: model
});

const executeSpecialProductFilter = model => ({
  type: EXECUTE_SPECIALPRODUCT_FILTER,
  payload: model
});

const clearFilter = () => ({
  type: CLEAR_FILTER,
  payload: []
});

const queryPolicy = () => ({
  type: QUERY_POLICY,
  payload: ACTION_QUERY_POLICY
});

const toggleColumn = columnKeys => ({
  type: TOGGLE_COLUMN,
  payload: {
    columnKeys
  }
});

const queryMatchOfferLists = () => ({
  type: QUERY_MATCH_OFFER,
  payload: ACTION_QUERY_MATCH_OFFER
});

export {
  updateStatusDisplayPopupFilter,
  executeFilter,
  clearFilter,
  postData,
  crud,
  queryData,
  queryDataWithParams,
  updateExpressionFilter,
  postDataWithExtraModel,
  queryPolicy,
  toggleColumn,
  executeSpecialProductFilter,
  queryMatchOfferLists
};

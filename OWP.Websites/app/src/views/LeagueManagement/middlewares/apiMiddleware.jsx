import { isEqual } from 'lodash';
import {
  QUERY_DATA,
  QUERY_DATA_WITH_PARAMS,
  POST_DATA,
  QUERY_POLICY,
  QUERY_MATCH_OFFER
} from '../constants/action-type';
import axios from 'axios';
import { FunctionKeys } from '../../Shared/Constants';
import AuthorizeService from '../../Authorize/Service';
import MaxbetConst from '../constants/MaxbetConst';

const queryDataWithParamsMiddleware = ({ dispatch }) => next => action => {
  if (!isEqual(action.type, QUERY_DATA_WITH_PARAMS)) {
    return next(action);
  }
  const { url, SUCCESS, PENDING, ERROR, model } = action.payload;
  dispatch({ type: PENDING });
  axios
    .get(url, { params: model })
    .then(res => dispatch({ type: SUCCESS, data: res.data }))
    .catch(error => dispatch({ type: ERROR, error }));
  return undefined;
};

const queryDataMiddleware = ({ dispatch }) => next => action => {
  if (!isEqual(action.type, QUERY_DATA)) {
    return next(action);
  }
  const { url, SUCCESS, PENDING, ERROR } = action.payload;
  dispatch({ type: PENDING });
  axios
    .get(url)
    .then(res => dispatch({ type: SUCCESS, data: res.data }))
    .catch(error => dispatch({ type: ERROR, error }));
  return undefined;
};

const postDataMiddleware = ({ dispatch }) => next => action => {
  if (!isEqual(action.type, POST_DATA)) {
    return next(action);
  }
  const { url, SUCCESS, PENDING, ERROR, model, extraModel } = action.payload;
  dispatch({ type: PENDING });
  axios
    .post(url, model)
    .then(res => {
      dispatch({
        type: SUCCESS,
        data: res.data,
        extraModel
      });
    })
    .catch(error =>
      dispatch({
        type: ERROR,
        error
      })
    );
  return undefined;
};

const queryPolicyMiddleware = ({ dispatch }) => next => async action => {
  if (!isEqual(action.type, QUERY_POLICY)) {
    return next(action);
  }
  const { SUCCESS, PENDING } = action.payload;
  dispatch({ type: PENDING });
  const [
    generalPolicy,
    conditionPolicy,
    lstNotePolicy,
    tvListPolicy,
    viewLogPolicy
  ] = await axios.all([
    AuthorizeService.authorize(FunctionKeys.LeagueManagement_General),
    AuthorizeService.authorize(FunctionKeys.LeagueManagement_EditConditions),
    AuthorizeService.authorize(FunctionKeys.LeagueManagement_LSTNote),
    AuthorizeService.authorize(FunctionKeys.LeagueManagement_TVList),
    AuthorizeService.authorize(FunctionKeys.LeagueManagement_ViewLog)
  ]);
  dispatch({
    type: SUCCESS,
    data: {
      generalPolicy,
      conditionPolicy,
      lstNotePolicy,
      tvListPolicy,
      viewLogPolicy
    }
  });
  return undefined;
};

const queryMatchOffersMiddleware = ({ dispatch }) => next => action => {
  if (!isEqual(action.type, QUERY_MATCH_OFFER)) {
    return next(action);
  }
  const { SUCCESS, PENDING, ERROR } = action.payload;
  dispatch({ type: PENDING });
  const url = MaxbetConst.getMatchOffers;
  axios
    .get(url)
    .then(res => dispatch({ type: SUCCESS, data: res.data }))
    .catch(error => dispatch({ type: ERROR, error }));
  return undefined;
};

export {
  queryDataMiddleware,
  postDataMiddleware,
  queryPolicyMiddleware,
  queryDataWithParamsMiddleware,
  queryMatchOffersMiddleware
};

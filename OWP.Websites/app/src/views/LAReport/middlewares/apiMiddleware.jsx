import axios from 'axios';
import {
  QUERY_DATA_WITH_PARAMS,
  QUERY_POLICY,
  QUERY_SERVERTIME
} from '../constants/action-type';
import { isEqual } from 'lodash';
import { FunctionKeys } from '../../Shared/Constants';
import AuthorizeService from '../../Authorize/Service';

const urlGetServerTime = `/api/report/get-server-time`;

const queryDataWithParamsMiddleware =
  ({ dispatch }) =>
  next =>
  async action => {
    if (!isEqual(action.type, QUERY_DATA_WITH_PARAMS)) {
      return next(action);
    }
    const { url, SUCCESS, PENDING, model } = action.payload;
    dispatch({ type: PENDING });
    const response = await axios.post(url, model);
    dispatch({ type: SUCCESS, data: response.data });
    return undefined;
  };

const queryPolicyMiddleware =
  ({ dispatch }) =>
  next =>
  async action => {
    if (!isEqual(action.type, QUERY_POLICY)) {
      return next(action);
    }
    const { SUCCESS, PENDING } = action.payload;
    dispatch({ type: PENDING });
    const laPolicy = await AuthorizeService.authorize(FunctionKeys.La_Report);
    dispatch({
      type: SUCCESS,
      data: {
        laPolicy
      }
    });
    return undefined;
  };

const queryServerTimeMiddleware =
  ({ dispatch }) =>
  next =>
  async action => {
    if (!isEqual(action.type, QUERY_SERVERTIME)) {
      return next(action);
    }
    const { SUCCESS, PENDING } = action.payload;
    dispatch({ type: PENDING });
    const res = await axios.get(urlGetServerTime);
    const serverTime = res.data;
    dispatch({
      type: SUCCESS,
      data: {
        serverTime
      }
    });
    return undefined;
  };

export {
  queryDataWithParamsMiddleware,
  queryPolicyMiddleware,
  queryServerTimeMiddleware
};

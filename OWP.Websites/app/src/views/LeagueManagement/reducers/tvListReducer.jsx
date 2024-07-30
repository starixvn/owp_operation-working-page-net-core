import { createReducer } from './reducerUtilities';
import { QUERY_TVLIST, ACTION_TVLIST } from '../constants/action-type';
import Converter from '../../Shared/Converter';
import { generateKeyTVList } from '../helper/commonHelper';

const defaultModel = {
  tvListPending: true,
  tvListExpression: [],
  tvListError: []
};

const handlePending = state => {
  return {
    ...state,
    tvListPending: true
  };
};

const handleQuerySuccess = (state, action) => {
  return {
    ...state,
    tvListExpression: Converter.convertResponseToTVList(action.data),
    tvListPending: false
  };
};

const handleError = (state, action) => {
  return {
    ...state,
    tvListPending: false,
    tvListError: action.error
  };
};

const handleUpdateTVList = (state, action) => {
  const { key, value } = action.payload;
  return {
    ...state,
    tvListExpression: state.tvListExpression.map((item, index) => {
      if (index !== action.payload.index) {
        return item;
      }
      return {
        ...item,
        [key]: value
      };
    })
  };
};

const handleAddTV = (state, action) => {
  const { tv } = action.payload;
  const newTvListExpression = state.tvListExpression.slice();
  newTvListExpression.push(tv);
  return {
    ...state,
    tvListExpression: generateKeyTVList(newTvListExpression)
  };
};

const handleDeleteTV = (state, action) => {
  const { index } = action.payload;
  const newTvListExpression = state.tvListExpression.slice();
  newTvListExpression.splice(index, 1);
  return {
    ...state,
    tvListExpression: generateKeyTVList(newTvListExpression)
  };
};

const tvListReducer = createReducer(defaultModel, {
  [QUERY_TVLIST.PENDING]: handlePending,
  [QUERY_TVLIST.SUCCESS]: handleQuerySuccess,
  [QUERY_TVLIST.ERROR]: handleError,
  [ACTION_TVLIST.UPDATE]: handleUpdateTVList,
  [ACTION_TVLIST.ADD]: handleAddTV,
  [ACTION_TVLIST.DELETE]: handleDeleteTV
});

export default tvListReducer;

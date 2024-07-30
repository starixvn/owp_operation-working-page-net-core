import { createReducer } from './reducerUtilities';
import {
  QUERY_LA,
  SEARCH,
  CLEAR_ALLFILTERS,
  ORDER
} from '../constants/action-type';
import { isEqual } from 'lodash';
import { executeFilter } from '../helper/filterHelper';
import { executeOrder } from '../helper/orderHelper';

const defaultModel = {
  pending: false,
  laInfo: [],
  laInfoOriginal: [],
  isDisplayTable: false,
  isClearAll: false,
  keyWordLAColumn: '',
  keyWordSectionColumn: '',
  orderType: '',
  columnId: ''
};

const handlePending = state => {
  return {
    ...state,
    pending: true
  };
};

const handleSuccess = (state, action) => {
  const currentKeyWordLAColumn = state.keyWordLAColumn;
  const currentKeyWordSectionColumn = state.keyWordSectionColumn;

  const newLAInfo = executeFilter(
    action.data,
    currentKeyWordLAColumn,
    currentKeyWordSectionColumn
  );
  return {
    ...state,
    laInfo: newLAInfo,
    laInfoOriginal: action.data,
    pending: false,
    isDisplayTable: true
  };
};
const handleSearch = (state, action) => {
  const { columnId, keyWord } = action.payload;
  let currentKeyWordLAColumn = state.keyWordLAColumn;
  let currentKeyWordSectionColumn = state.keyWordSectionColumn;

  if (isEqual(columnId, 'Section')) {
    currentKeyWordSectionColumn = keyWord;
  } else {
    currentKeyWordLAColumn = keyWord;
  }
  const newLAInfo = executeFilter(
    state.laInfoOriginal,
    currentKeyWordLAColumn,
    currentKeyWordSectionColumn
  );
  return {
    ...state,
    laInfo: newLAInfo,
    keyWordLAColumn: currentKeyWordLAColumn,
    keyWordSectionColumn: currentKeyWordSectionColumn
  };
};

const handleOrder = (state, action) => {
  const { columnId, orderType } = action.payload;
  const result = executeOrder(state.laInfo.slice(), columnId, orderType);
  return {
    ...state,
    laInfo: result,
    orderType,
    columnId
  };
};

const handleCLearFilter = (state, action) => {
  return {
    ...state,
    laInfo: state.laInfoOriginal.slice(),
    isClearAll: action.isClearAll,
    keyWordLAColumn: '',
    keyWordSectionColumn: ''
  };
};

const reportReducer = createReducer(defaultModel, {
  [QUERY_LA.PENDING]: handlePending,
  [QUERY_LA.SUCCESS]: handleSuccess,
  [SEARCH]: handleSearch,
  [CLEAR_ALLFILTERS]: handleCLearFilter,
  [ORDER]: handleOrder
});

export default reportReducer;

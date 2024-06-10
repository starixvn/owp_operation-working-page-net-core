import { createReducer } from './reducerUtilities';
import {
  QUERY_MAXBETGENARAL_INFO,
  MAXBETGENARAL_INFO_UPDATE_LEAGUEGROUP,
  MAXBETGENARAL_INFO_UPDATE_ACTIVE,
  MAXBETGENARAL_INFO_UPDATE_BLACK_LIST,
  EXECUTE_FILTER,
  CLEAR_FILTER
} from '../constants/action-type';
import {
  updateLeagueGroup,
  updateActive,
  updateBlackList,
  excludeOldLeague
} from '../helper/commonHelper';
import { nomalizeMaxbetSpreadResponse } from '../helper/nomalizeResponseHelper';
import { updateFilterKeyword, filterData } from '../helper/filterHelper.native';
import { v4 as uuidv4 } from 'uuid';

const defaultModel = {
  maxbetGeneralPending: true,
  maxbetGeneralData: [],
  maxbetGeneralError: [],
  filterInfo: []
};

const handlePending = state => {
  return {
    ...state,
    maxbetGeneralPending: true
  };
};

const handleSuccess = (state, action) => {
  const { displayOldLeague, displayMaxbetGeneralTable } = action.extraModel;
  let maxbetGeneralDataSource = nomalizeMaxbetSpreadResponse(action.data);
  if (!displayOldLeague) {
    maxbetGeneralDataSource = excludeOldLeague(maxbetGeneralDataSource);
  }
  let maxbetGeneralData = maxbetGeneralDataSource.slice();
  state.filterInfo.forEach(item => {
    maxbetGeneralData = filterData(maxbetGeneralData, item);
  });
  return {
    ...state,
    maxbetGeneralDataSource,
    displayMaxbetGeneralTable,
    tableKey: uuidv4(),
    maxbetGeneralData,
    maxbetGeneralPending: false
  };
};

const handleError = (state, action) => {
  return {
    ...state,
    maxbetGeneralPending: false,
    maxbetGeneralError: action.error
  };
};

const handleUpdateLeagueGroupSuccess = (state, action) => {
  return {
    ...state,
    maxbetGeneralData: updateLeagueGroup(
      state.maxbetGeneralData,
      action.extraModel
    ),
    maxbetGeneralPending: false,
    maxbetGeneralError: action.error
  };
};

const handleUpdateActiveSuccess = (state, action) => {
  return {
    ...state,
    maxbetGeneralData: updateActive(state.maxbetGeneralData, action.extraModel),
    maxbetGeneralPending: false,
    maxbetGeneralError: action.error
  };
};

const handleUpdateBlacklistSuccess = (state, action) => {
  return {
    ...state,
    maxbetGeneralData: updateBlackList(
      state.maxbetGeneralData,
      action.extraModel
    ),
    maxbetGeneralPending: false,
    maxbetGeneralError: action.error
  };
};

const handleFilter = (state, action) => {
  const { filterKey, keyword } = action.payload;
  let maxbetGeneralData = state.maxbetGeneralDataSource.slice();
  const filterInfo = updateFilterKeyword(
    state.filterInfo.slice(),
    filterKey,
    keyword
  );
  filterInfo.forEach(item => {
    maxbetGeneralData = filterData(maxbetGeneralData, item);
  });
  return {
    ...state,
    filterInfo,
    maxbetGeneralData
  };
};

const handleClearFilter = state => {
  return {
    ...state,
    filterInfo: [],
    maxbetGeneralData: state.maxbetGeneralDataSource
  };
};

const maxbetGeneralReducer = createReducer(defaultModel, {
  [QUERY_MAXBETGENARAL_INFO.PENDING]: handlePending,
  [QUERY_MAXBETGENARAL_INFO.SUCCESS]: handleSuccess,
  [QUERY_MAXBETGENARAL_INFO.ERROR]: handleError,
  [MAXBETGENARAL_INFO_UPDATE_LEAGUEGROUP.SUCCESS]: handleUpdateLeagueGroupSuccess,
  [MAXBETGENARAL_INFO_UPDATE_ACTIVE.SUCCESS]: handleUpdateActiveSuccess,
  [MAXBETGENARAL_INFO_UPDATE_BLACK_LIST.SUCCESS]: handleUpdateBlacklistSuccess,
  [EXECUTE_FILTER]: handleFilter,
  [CLEAR_FILTER]: handleClearFilter
});

export default maxbetGeneralReducer;

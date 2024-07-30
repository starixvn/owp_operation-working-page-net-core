import { createReducer } from './reducerUtilities';
import {
  QUERY_LEAGUE_DETAIL,
  LEAGUEDETAIL_UPDATEINFO,
  LEAGUEDETAIL_DELETE
} from '../constants/action-type';
import { updateLeagueDetail, removeLeagueDetail } from '../helper/commonHelper';
import { v4 as uuidv4 } from 'uuid';

const defaultModel = {
  leagueDetailPending: true,
  leagueDetailData: [],
  leagueDetailError: [],
  tableKey: uuidv4()
};

const handlePending = state => {
  return {
    ...state,
    leagueDetailPending: true
  };
};

const handleSuccess = (state, action) => {
  return {
    ...state,
    leagueDetailData: action.data,
    leagueDetailPending: false,
    tableKey: uuidv4()
  };
};

const handleError = (state, action) => {
  return {
    ...state,
    leagueDetailPending: false,
    leagueDetailError: action.error
  };
};

const handleUpdateLeagueSuccess = (state, action) => {
  return {
    ...state,
    leagueDetailPending: false,
    leagueDetailData: updateLeagueDetail(
      state.leagueDetailData,
      action.extraModel
    )
  };
};

const handleDeleteLeague = (state, action) => {
  return {
    ...state,
    leagueDetailPending: false,
    leagueDetailData: removeLeagueDetail(
      state.leagueDetailData,
      action.extraModel
    ),
    tableKey: uuidv4()
  };
};

const leagueDetailReducer = createReducer(defaultModel, {
  [QUERY_LEAGUE_DETAIL.PENDING]: handlePending,
  [QUERY_LEAGUE_DETAIL.SUCCESS]: handleSuccess,
  [QUERY_LEAGUE_DETAIL.ERROR]: handleError,
  [LEAGUEDETAIL_UPDATEINFO.SUCCESS]: handleUpdateLeagueSuccess,
  [LEAGUEDETAIL_DELETE.PENDING]: handlePending,
  [LEAGUEDETAIL_DELETE.SUCCESS]: handleDeleteLeague
});

export default leagueDetailReducer;

import { createReducer } from './reducerUtilities';
import {
  QUERY_LEAGUEGROUP,
  ACTION_LEAGUEGROUP_FILTER
} from '../constants/action-type';
import MaxbetConst from '../constants/MaxbetConst';
import Converter from '../../Shared/Converter';
import isEqual from 'lodash.isequal';

const defaultModel = {
  leagueGroupPending: true,
  leagueGroupOption: [],
  leagueGroupError: []
};

const handlePending = state => {
  return {
    ...state,
    leagueGroupPending: true
  };
};

const handleSuccess = (state, action) => {
  const leagueGroupOption = Converter.convertResourceToOptions(action.data);

  return {
    ...state,
    leagueGroup: MaxbetConst.leagueGroupAll,
    leagueGroupBackup: state.leagueGroupBackup ?? MaxbetConst.leagueGroupAll,
    leagueGroupOption,
    leagueGroupPending: false
  };
};

const handleError = (state, action) => {
  return {
    ...state,
    leagueGroupPending: false,
    leagueGroupError: action.error
  };
};

const handleUpdateLeagueGroup = (state, action) => {
  return {
    ...state,
    leagueGroup: action.payload.select,
    leagueGroupBackup: action.payload.select
  };
};

const leagueGroupReducer = createReducer(defaultModel, {
  [QUERY_LEAGUEGROUP.PENDING]: handlePending,
  [QUERY_LEAGUEGROUP.SUCCESS]: handleSuccess,
  [QUERY_LEAGUEGROUP.ERROR]: handleError,
  [ACTION_LEAGUEGROUP_FILTER.UPDATE]: handleUpdateLeagueGroup
});

export default leagueGroupReducer;

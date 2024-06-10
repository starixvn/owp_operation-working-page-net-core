import { createReducer } from './reducerUtilities';
import { ACTION_LEAGUE_FILTER } from '../constants/action-type';

const defaultModel = {
  league: ''
};

const handleUpdateLeague = (state, action) => {
  return {
    ...state,
    league: action.payload.value
  };
};

const leagueReducer = createReducer(defaultModel, {
  [ACTION_LEAGUE_FILTER.UPDATE]: handleUpdateLeague
});

export default leagueReducer;

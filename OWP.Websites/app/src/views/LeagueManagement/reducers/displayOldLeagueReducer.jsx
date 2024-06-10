import { createReducer } from './reducerUtilities';
import { ACTION_DISPLAY_OLDLEAGUE } from '../constants/action-type';

const defaultModel = {
  displayOldLeague: false
};

const handleUpdateDisplayOldLeague = (state, action) => {
  return {
    ...state,
    displayOldLeague: action.payload.value
  };
};

const displayOldLeagueReducer = createReducer(defaultModel, {
  [ACTION_DISPLAY_OLDLEAGUE.UPDATE]: handleUpdateDisplayOldLeague
});

export default displayOldLeagueReducer;

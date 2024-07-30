import { createReducer } from './reducerUtilities';
import { ACTION_CATEGORY_FILTER } from '../constants/action-type';
import MaxbetConst from '../constants/MaxbetConst';

const defaultModel = {
  leagueCategory: MaxbetConst.leagueCategoryOption[0]
};

const handleUpdateCategory = (state, action) => {
  return {
    ...state,
    leagueCategory: action.payload.select
  };
};

const leagueCategoryReducer = createReducer(defaultModel, {
  [ACTION_CATEGORY_FILTER.UPDATE]: handleUpdateCategory
});

export default leagueCategoryReducer;

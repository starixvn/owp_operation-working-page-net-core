import { combineReducers } from 'redux';
import leagueCategoryReducer from './leagueCategoryReducer';
import leagueGroupReducer from './leagueGroupReducer';
import tvListReducer from './tvListReducer';
import maxbetGeneralReducer from './maxbetGeneralReducer';
import leagueReducer from './leagueReducer';
import statusReducer from './statusReducer';
import displayOldLeagueReducer from './displayOldLeagueReducer';
import showHidePopupFilterReducer from './showHidePopupFilterReducer';
import leagueDetailReducer from './leagueDetailReducer';
import policyReducer from './policyReducer';
import matchOffersReducer from './matchOfferReducer';
import toggleColumnReducer from './toggleColumnReducer';
import conditionReducer from './conditionReducer';
import specialProductReducer from './specialProductReducer';
import logHistoryReducer from './logHistoryReducer';
import teamInfoReducer from './teamInfoReducer';

export default combineReducers({
  leagueReducer,
  leagueGroupReducer,
  leagueCategoryReducer,
  statusReducer,
  displayOldLeagueReducer,
  tvListReducer,
  maxbetGeneralReducer,
  showHidePopupFilterReducer,
  leagueDetailReducer,
  policyReducer,
  matchOffersReducer,
  toggleColumnReducer,
  conditionReducer,
  specialProductReducer,
  logHistoryReducer,
  teamInfoReducer
});

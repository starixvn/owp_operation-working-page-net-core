/* eslint-disable no-warning-comments */
import { createSelector } from 'reselect';
import MaxbetConst from '../constants/MaxbetConst';

const selectLeagueGroup = createSelector(
  state => state.leagueGroupReducer.leagueGroupPending,
  state => state.leagueGroupReducer.leagueGroupOption,
  state => state.leagueGroupReducer.leagueGroupBackup,
  state => state.leagueGroupReducer.leagueGroupError,
  (
    leagueGroupPending,
    leagueGroupOption,
    leagueGroupBackup,
    leagueGroupError
  ) => ({
    leagueGroupPending: leagueGroupPending ?? true,
    leagueGroupOption: [
      ...[MaxbetConst.leagueGroupAll],
      ...leagueGroupOption,
      ...[MaxbetConst.leagueGroupUnknown]
    ],
    leagueGroupBackup,
    leagueGroupError
  })
);

const selectTeamInfo = createSelector(
  state => state.teamInfoReducer.loading,
  state => state.teamInfoReducer.entity,
  (loading, entity) => ({
    loading: loading ?? true,
    entity,
    error: undefined // NOSONAR
  })
);

const selectTVList = createSelector(
  state => state.tvListReducer.tvListPending,
  state => state.tvListReducer.tvListExpression,
  (tvListPending, tvListExpression) => ({
    tvListPending: tvListPending ?? true,
    tvListExpression
  })
);

const selectMaxbetGeneralData = createSelector(
  state => state.maxbetGeneralReducer.maxbetGeneralPending,
  state => state.maxbetGeneralReducer.maxbetGeneralData,
  state => state.maxbetGeneralReducer.maxbetGeneralError,
  state => state.maxbetGeneralReducer.filterInfo,
  (
    maxbetGeneralPending,
    maxbetGeneralData,
    maxbetGeneralError,
    filterInfo
  ) => ({
    maxbetGeneralPending: maxbetGeneralPending ?? true,
    maxbetGeneralData,
    maxbetGeneralError,
    filterInfo
  })
);

const selectDisplayPopupFilter = createSelector(
  state => state.showHidePopupFilterReducer.filterKey,
  state => state.showHidePopupFilterReducer.isDisplay,
  (filterKey, isDisplay) => ({
    filterKey,
    isDisplay
  })
);

export {
  selectTeamInfo,
  selectLeagueGroup,
  selectTVList,
  selectMaxbetGeneralData,
  selectDisplayPopupFilter
};

import { createSelector } from 'reselect';

const selectLeagueDetail = createSelector(
  state => state.leagueDetailReducer.leagueDetailPending,
  state => state.leagueDetailReducer.leagueDetailData,
  state => state.leagueDetailReducer.tableKey,
  (leagueDetailPending, leagueDetailData, tableKey) => ({
    leagueDetailPending: leagueDetailPending ?? true,
    leagueDetailData: leagueDetailData ?? [],
    tableKey
  })
);

const selectToggleColum = createSelector(
  state => state.toggleColumnReducer.columnKeys,
  columnKeys => ({
    toggleColumnLoading: columnKeys === null,
    columnKeys
  })
);

const selectCondition = createSelector(
  state => state.conditionReducer.conditionPending,
  state => state.conditionReducer.leagueSettingId,
  state => state.conditionReducer.tv,
  state => state.conditionReducer.round,
  state => state.conditionReducer.division,
  (conditionPending, leagueSettingId, tv, round, division) => ({
    conditionPending: conditionPending ?? true,
    leagueSettingId,
    tv,
    round,
    division
  })
);

export { selectLeagueDetail, selectToggleColum, selectCondition };

import { createSelector } from 'reselect';

const selectDisplayAddLeagueButton = createSelector(
  state => state.leagueDetailReducer.leagueDetailData,
  leagueDetailData => ({
    canAddLeague: leagueDetailData.length < 20
  })
);

export { selectDisplayAddLeagueButton };

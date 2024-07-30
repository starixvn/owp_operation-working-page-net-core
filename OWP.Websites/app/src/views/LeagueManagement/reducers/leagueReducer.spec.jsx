import leagueReducer from './leagueReducer';
import { ACTION_LEAGUE_FILTER } from '../constants/action-type';

describe('leagueGroup reducer', () => {
  it('should return the initial state', () => {
    expect(leagueReducer(undefined, {})).toEqual({
      league: ''
    });
  });

  it('should handle ACTION_LEAGUE_FILTER.UPDATE', () => {
    expect(
      leagueReducer(undefined, {
        type: ACTION_LEAGUE_FILTER.UPDATE,
        payload: {
          value: 'Carlos'
        }
      })
    ).toEqual({
      league: 'Carlos'
    });
  });
});

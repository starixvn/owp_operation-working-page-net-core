import displayOldLeagueReducer from './displayOldLeagueReducer';
import { ACTION_DISPLAY_OLDLEAGUE } from '../constants/action-type';

describe('displayOldLeague reducer', () => {
  it('should return the initial state', () => {
    expect(displayOldLeagueReducer(undefined, {})).toEqual({
      displayOldLeague: false
    });
  });

  it('should handle ACTION_DISPLAY_OLDLEAGUE.UPDATE', () => {
    expect(
      displayOldLeagueReducer(
        {
          displayOldLeague: false
        },
        {
          type: ACTION_DISPLAY_OLDLEAGUE.UPDATE,
          payload: {
            value: true
          }
        }
      )
    ).toEqual({
      displayOldLeague: true
    });
  });
});

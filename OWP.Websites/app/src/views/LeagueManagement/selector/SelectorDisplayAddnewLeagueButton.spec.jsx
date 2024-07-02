import { selectDisplayAddLeagueButton } from './SelectorDisplayAddnewLeagueButton';

describe('test selector', () => {
  it('select SelectorDisplayAddnewLeagueButton ', () => {
    const state = {
      leagueDetailReducer: {
        leagueDetailData: ['123']
      }
    };
    expect(selectDisplayAddLeagueButton(state)).toEqual({ canAddLeague: true });
  });
});

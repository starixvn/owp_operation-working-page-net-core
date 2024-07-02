import { buildModelLeagueDetail } from './buildPostModelHelper';

describe('buildPostModelHelper', () => {
  it('Should build post model incorrect', () => {
    const model = {
      HasTV: true,
      Round: 'abc',
      Division: 'divi',
      TV: null,
      League: 'abc',
      LeagueGroup: 2
    };
    expect(buildModelLeagueDetail(model, 'League', null)).toEqual({
      League: null,
      LeagueGroup: 2,
      apiKey: 'league-management-update-league-setting'
    });
  });
});

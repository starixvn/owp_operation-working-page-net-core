import {
  LEAGUEDETAIL_DELETE,
  LEAGUEDETAIL_UPDATEINFO,
  QUERY_LEAGUE_DETAIL
} from '../constants/action-type';
import leagueDetailReducer from './leagueDetailReducer';

describe('leagueDetailReducer', () => {
  it('should return the initial state', () => {
    const actual = leagueDetailReducer(undefined, {});
    delete actual.tableKey;
    expect(leagueDetailReducer(undefined, {})).toEqual({
      leagueDetailPending: true,
      leagueDetailData: [],
      leagueDetailError: []
    });
  });

  it('should handle QUERY_LEAGUE_DETAIL.PENDING', () => {
    const actual = leagueDetailReducer(undefined, {
      type: QUERY_LEAGUE_DETAIL.PENDING
    });
    delete actual.tableKey;
    expect(actual).toEqual({
      leagueDetailPending: true,
      leagueDetailData: [],
      leagueDetailError: []
    });
  });

  it('should handle QUERY_LEAGUE_DETAIL.SUCCESS', () => {
    const actual = leagueDetailReducer(undefined, {
      type: QUERY_LEAGUE_DETAIL.SUCCESS,
      data: []
    });
    delete actual.tableKey;
    expect(actual).toEqual({
      leagueDetailPending: false,
      leagueDetailData: [],
      leagueDetailError: []
    });
  });

  it('should handle QUERY_LEAGUE_DETAIL.ERROR', () => {
    const actual = leagueDetailReducer(undefined, {
      type: QUERY_LEAGUE_DETAIL.ERROR,
      error: ['Fail to connect server']
    });
    expect(actual).toEqual({
      leagueDetailPending: false,
      leagueDetailData: [],
      leagueDetailError: ['Fail to connect server']
    });
  });

  it('should update league successfully', () => {
    const state = {
      leagueDetailData: [{ LeagueName: 'abc' }, { LeagueName: 'abc1' }]
    };
    const actual = leagueDetailReducer(state, {
      type: LEAGUEDETAIL_UPDATEINFO.SUCCESS,
      extraModel: {
        rowIndex: 1,
        key: 'LeagueName',
        value: 'Tylor'
      }
    });
    expect(actual).toEqual({
      leagueDetailData: [{ LeagueName: 'abc' }, { LeagueName: 'Tylor' }],
      leagueDetailPending: false
    });
  });

  it('should delete league successfully', () => {
    const state = {
      leagueDetailData: [{ LeagueName: 'abc' }, { LeagueName: 'abc1' }]
    };
    const actual = leagueDetailReducer(state, {
      type: LEAGUEDETAIL_DELETE.SUCCESS,
      extraModel: {
        rowIndex: 0
      }
    });
    delete actual.tableKey;
    expect(actual).toEqual({
      leagueDetailData: [{ LeagueName: 'abc1' }],
      leagueDetailPending: false
    });
  });
});

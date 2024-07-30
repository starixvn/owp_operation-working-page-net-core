import { QUERY_TEAM_INFO } from '../constants/action-type';
import teamInfoReducer from './teamInfoReducer';

describe('teamInfoReducer', () => {
  it('should return the initial state', () => {
    expect(
      teamInfoReducer(
        {
          loading: 'idle',
          entity: [],
          error: []
        },
        {}
      )
    ).toEqual({
      loading: 'idle',
      entity: [],
      error: []
    });
  });

  it('should handle QUERY_TEAM_INFO.PENDING', () => {
    expect(
      teamInfoReducer(undefined, {
        type: QUERY_TEAM_INFO.PENDING
      })
    ).toEqual({
      loading: 'pending',
      entity: [],
      error: undefined
    });
  });

  it('should handle QUERY_TEAM_INFO.SUCCESS', () => {
    const actual = teamInfoReducer(undefined, {
      type: QUERY_TEAM_INFO.SUCCESS,
      data: []
    });
    expect(actual.entity.length).toEqual(0);
  });

  it('should handle QUERY_TEAM_INFO.ERROR', () => {
    expect(
      teamInfoReducer(undefined, {
        type: QUERY_TEAM_INFO.ERROR,
        error: ['Fail to query data']
      })
    ).toEqual({
      loading: 'idle',
      entity: [],
      error: ['Fail to query data']
    });
  });
})
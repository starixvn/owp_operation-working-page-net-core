import {
  QUERY_LEAGUEGROUP,
  ACTION_LEAGUEGROUP_FILTER
} from '../constants/action-type';
import leagueGroupReducer from './leagueGroupReducer';

describe('leagueGroup reducer', () => {
  it('should return the initial state', () => {
    expect(leagueGroupReducer(undefined, {})).toEqual({
      leagueGroupPending: true,
      leagueGroupOption: [],
      leagueGroupError: []
    });
  });

  it('should handle QUERY_LEAGUEGROUP.PENDING', () => {
    expect(
      leagueGroupReducer(undefined, {
        type: QUERY_LEAGUEGROUP.PENDING
      })
    ).toEqual({
      leagueGroupPending: true,
      leagueGroupOption: [],
      leagueGroupError: []
    });
  });

  it('should handle QUERY_LEAGUEGROUP.SUCCESS', () => {
    expect(
      leagueGroupReducer(undefined, {
        type: QUERY_LEAGUEGROUP.SUCCESS,
        data: [
          {
            ResourceId: 1,
            Value: 'M1'
          }
        ]
      })
    ).toEqual({
      leagueGroup: { value: -1, label: 'All' },
      leagueGroupOption: [
        undefined,
        {
          value: 1,
          label: 'M1'
        }
      ],
      leagueGroupError: [],
      leagueGroupPending: false,
      leagueGroupBackup: {
        label: 'All',
        value: -1
      }
    });
  });

  it('should handle QUERY_LEAGUEGROUP.ERROR', () => {
    expect(
      leagueGroupReducer(undefined, {
        type: QUERY_LEAGUEGROUP.ERROR,
        error: ['Fail to query data']
      })
    ).toEqual({
      leagueGroupOption: [],
      leagueGroupPending: false,
      leagueGroupError: ['Fail to query data']
    });
  });

  it('should handle ACTION_LEAGUEGROUP_FILTER.UPDATE', () => {
    expect(
      leagueGroupReducer(undefined, {
        type: ACTION_LEAGUEGROUP_FILTER.UPDATE,
        payload: {
          select: { value: -1, label: 'All' }
        }
      })
    ).toEqual({
      leagueGroup: { value: -1, label: 'All' },
      leagueGroupError: [],
      leagueGroupOption: [],
      leagueGroupPending: true,
      leagueGroupBackup: {
        label: 'All',
        value: -1
      }
    });
  });
});

import { ACTION_QUERY_SERVERTIME } from '../constants/action-type';
import serverTimeReducer from './serverTimeReducer';

describe('serverTimeReducer under test', () => {
  it('should return the initial state', () => {
    expect(serverTimeReducer(undefined, {})).toEqual({
      pending: true,
      serverTime: ''
    });
  });
  it('should handle ACTION_QUERY_SERVERTIME.PENDING', () => {
    expect(
      serverTimeReducer(undefined, {
        type: ACTION_QUERY_SERVERTIME.PENDING
      })
    ).toEqual({
      pending: true,
      serverTime: ''
    });
  });
  it('should handle ACTION_QUERY_SERVERTIME.SUCCESS', () => {
    const data = {
      serverTime: '12/05/2020'
    };

    const { serverTime } = serverTimeReducer(undefined, {
      type: ACTION_QUERY_SERVERTIME.SUCCESS,
      data
    });
    expect(serverTime).toEqual('12/05/2020');
  });
});

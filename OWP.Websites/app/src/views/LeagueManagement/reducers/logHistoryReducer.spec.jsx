import logHistoryReducer from './logHistoryReducer';
import { QUERY_LOGHISTORY } from '../constants/action-type';

describe('logHistory reducer', () => {
  it('should return the initial state', () => {
    expect(logHistoryReducer(undefined, {})).toEqual({
      Data: [],
      Error: [],
      Pending: true
    });
  });

  it('should handle QUERY_LOGHISTORY.PENDING', () => {
    expect(
      logHistoryReducer(undefined, {
        type: QUERY_LOGHISTORY.PENDING,
        payload: {}
      })
    ).toEqual({
      Data: [],
      Error: [],
      Pending: true
    });
  });
  it('should handle QUERY_LOGHISTORY.SUCCESS', () => {
    expect(
      logHistoryReducer(undefined, {
        type: QUERY_LOGHISTORY.SUCCESS,
        payload: {}
      })
    ).toEqual({
      Data: undefined,
      Error: [],
      Pending: false
    });
  });
});

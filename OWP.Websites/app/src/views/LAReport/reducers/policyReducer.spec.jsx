import { ACTION_QUERY_POLICY } from '../constants/action-type';
import policyReducer from './policyReducer';

describe('policyReducer under test', () => {
  it('should return the initial state', () => {
    expect(policyReducer(undefined, {})).toEqual({
      pending: true,
      laPolicy: {}
    });
  });
  it('should handle ACTION_QUERY_POLICY.PENDING', () => {
    expect(
      policyReducer(undefined, {
        type: ACTION_QUERY_POLICY.PENDING
      })
    ).toEqual({
      pending: true,
      laPolicy: {}
    });
  });
  it('should handle ACTION_QUERY_POLICY.SUCCESS', () => {
    const data = {
      laPolicy: 123
    };

    const { laPolicy } = policyReducer(undefined, {
      type: ACTION_QUERY_POLICY.SUCCESS,
      data
    });
    expect(laPolicy).toEqual(123);
  });
});

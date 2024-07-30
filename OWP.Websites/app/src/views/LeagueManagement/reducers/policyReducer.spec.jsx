import policyReducer from './policyReducer';
import { ACTION_QUERY_POLICY } from '../constants/action-type';

describe('policyReducer', () => {
  it('Should return status pending', () => {
    const actual = policyReducer(undefined, {
      type: ACTION_QUERY_POLICY.PENDING
    });
    expect(actual).toEqual({ policyPending: true, policyData: {} });
  });

  it('Should handle [ACTION_QUERY_POLICY.SUCCESS', () => {
    const action = {
      type: ACTION_QUERY_POLICY.SUCCESS,
      data: {
        generalPolicy: {
          hasWrite: true
        },
        conditionPolicy: {
          hasWrite: true
        },
        lstNotePolicy: {
          hasWrite: true
        },
        tvListPolicy: {
          hasWrite: true
        }
      }
    };
    const actual = policyReducer(undefined, action);
    expect(actual).toEqual({
      policyPending: false,
      policyData: {
        generalPolicy: { hasWrite: true },
        conditionPolicy: { hasWrite: true },
        lstNotePolicy: { hasWrite: true },
        tvListPolicy: { hasWrite: true }
      }
    });
  });
});

import { ACTION_STATUS_FILTER } from '../constants/action-type';
import statusReducer from './statusReducer';

describe('status reducer', () => {
  it('should return the initial state', () => {
    expect(statusReducer({ value: 1, label: 'Active' }, {})).toEqual({
      value: 1,
      label: 'Active'
    });
  });

  it('should handle ACTION_STATUS_FILTER.UPDATE', () => {
    expect(
      statusReducer(undefined, {
        type: ACTION_STATUS_FILTER.UPDATE,
        payload: {
          select: { value: 1, label: 'Active' }
        }
      })
    ).toEqual({
      status: { value: 1, label: 'Active' }
    });
  });
});

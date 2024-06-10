import reducer, { LastUpdateState } from './lastUpdateSlice';
import getLastUpdateThunkNative from './getLastUpdateThunkNative';

describe('lastUpdateSlice under unit test', () => {
  it('return new state when action getLastUpdateThunkNative is pending ', () => {
    const action = {
      type: getLastUpdateThunkNative.pending
    };
    const state = {
      entity: {},
      loading: 'idle'
    };
    const newState = {
      entity: {},
      loading: 'pending'
    };
    const actual = reducer(state as LastUpdateState, action);

    expect(newState).toEqual(actual);
  });
  it('return new state when action getLastUpdateThunkNative is fulfilled ', () => {
    const action = {
      type: getLastUpdateThunkNative.fulfilled,
      payload: {
        a: 1
      }
    };
    const state = {
      entity: {},
      loading: 'idle'
    };
    const newState = {
      entity: {
        a: 1
      },
      loading: 'idle'
    };
    const actual = reducer(state as LastUpdateState, action);

    expect(newState).toEqual(actual);
  });
});

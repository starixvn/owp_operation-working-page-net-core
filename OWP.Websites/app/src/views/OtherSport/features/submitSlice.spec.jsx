import reducer, { resetResponse } from './submitSlice';
import submitThunk from './submitThunkNative';

describe('submitSlice under test', () => {
  it('should return the initial state', () => {
    expect(reducer(undefined, {})).toEqual({
      submitResult: {}
    });
  });

  it('should handle resetResponse', () => {
    const action = {
      type: resetResponse.type
    };

    const actual = reducer(undefined, action);
    expect(actual).toEqual({ submitResult: {} });
  });

  it('should handle submit.pending', () => {
    const action = {
      type: submitThunk.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      submitResult: {},
      loading: true
    });
  });

  it('should handle submit.fulfilled', () => {
    const action = {
      type: submitThunk.fulfilled.type,
      payload: {
        isSuccessful: true,
        message: 'Os upload successfully'
      }
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      submitResult: {
        isSuccessful: true,
        message: 'Os upload successfully'
      },
      loading: false
    });
  });
});

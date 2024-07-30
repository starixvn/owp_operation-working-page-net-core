import reducer from './validationSlice';
import validateFileThunk from './validationThunkNative';

describe('submitSlice under test', () => {
  it('should return the initial state', () => {
    expect(reducer(undefined, {})).toEqual({ validationResult: {} });
  });

  it('should handle validateFileThunk.pending', () => {
    const action = {
      type: validateFileThunk.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: true,
      validationResult: {}
    });
  });

  it('should handle validateFileThunk.fulfilled', () => {
    const action = {
      type: validateFileThunk.fulfilled.type,
      payload: {
        hasError: false,
        message: ''
      }
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      validationResult: {
        hasError: false,
        message: ''
      },
      loading: false
    });
  });
});

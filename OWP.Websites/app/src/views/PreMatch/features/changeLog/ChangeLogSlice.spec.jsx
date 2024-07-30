import { REQUEST_STATUS } from '../../../Shared/Constants';
import ChangeLogSlice, { changeLog } from './ChangeLogSlice';

jest.mock('../../app/store');
describe('ChangeLogSlice under test', () => {
  it('should return the initial state', () => {
    expect(ChangeLogSlice(undefined, {})).toEqual({
      dataChangeLog: {
        Logs: null,
        IsLastPage: undefined
      },
      loading: REQUEST_STATUS.idle,
      error: null
    });
  });
  it('initialState loading is idle, should handle changeLog.pending', () => {
    const initialState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.idle,
      error: null
    };

    const expectState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.pending,
      error: null
    };

    const action = { type: changeLog.pending.type };

    const state = ChangeLogSlice(initialState, action);

    expect(state).toEqual(expectState);
  });
  it('initialState loading is pending, should handle changeLog.pending', () => {
    const initialState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.pending,
      error: null
    };

    const expectState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.pending,
      error: null
    };

    const action = { type: changeLog.pending.type };

    const state = ChangeLogSlice(initialState, action);

    expect(state).toEqual(expectState);
  });
  it('initialState loading is pending, should handle changeLog.fulfilled', () => {
    const initialState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.pending,
      error: null
    };

    const expectState = {
      dataChangeLog: {
        id: 1,
        test: [2, 3]
      },
      loading: REQUEST_STATUS.idle,
      error: null
    };

    const action = {
      type: changeLog.fulfilled.type,
      payload: { id: 1, test: [2, 3] }
    };

    const state = ChangeLogSlice(initialState, action);

    expect(state).toEqual(expectState);
  });
  it('initialState loading is idle, should handle changeLog.fulfilled', () => {
    const initialState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.idle,
      error: null
    };

    const action = {
      type: changeLog.fulfilled.type,
      payload: { id: 1, test: [2, 3] }
    };

    const state = ChangeLogSlice(initialState, action);

    expect(state).toEqual(initialState);
  });
  it('initialState loading is pending, should handle changeLog.rejected', () => {
    const initialState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.pending,
      error: null
    };

    const expectState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.idle,
      error: 'test'
    };

    const action = {
      type: changeLog.rejected.type,
      payload: { id: 1, test: [2, 3] },
      error: 'test'
    };

    const state = ChangeLogSlice(initialState, action);

    expect(state).toEqual(expectState);
  });
  it('initialState loading is idle, should handle changeLog.rejected', () => {
    const initialState = {
      dataChangeLog: [123],
      loading: REQUEST_STATUS.idle,
      error: null
    };

    const action = {
      type: changeLog.rejected.type,
      payload: { id: 1, test: [2, 3] },
      error: 'test'
    };

    const state = ChangeLogSlice(initialState, action);

    expect(state).toEqual(initialState);
  });
});

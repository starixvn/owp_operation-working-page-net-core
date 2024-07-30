/* eslint-disable no-warning-comments */
import createReducer, { queryPolicy } from './multiplePolicySlice';
import { REQUEST_STATUS } from '../Constants';

describe('multiplePolicySlice under test', () => {
  it('should return the initial state', () => {
    expect(createReducer()(undefined, {})).toEqual({
      permission: {"ActorId": 0,
         "HasRead": false,
        "HasWrite": false,
      "Username": null,},
      loading: REQUEST_STATUS.idle,
      error: null
    });
  });

  describe('queryPolicy under test', () => {
    it(' should handle queryPolicy.pending', () => {
      // Arrange
      const initialState = {
        loading: REQUEST_STATUS.idle
      };

      const expectState = {
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: queryPolicy().pending.type
      };

      // Action
      const state = createReducer()(initialState, action);
      // Assert
      expect(state).toEqual(expectState);
    });

    it(' should handle queryPolicy.fulfilled', () => {
      const initialState = {
        loading: REQUEST_STATUS.pending,
        permission: {}
      };

      const expectState = {
        loading: REQUEST_STATUS.idle,
        permission: {
          HasWrite: true,
          HasRead: true,
          Username: 'sa'
        }
      };

      const action = {
        type: queryPolicy().fulfilled.type,
        payload: {
          HasWrite: true,
          HasRead: true,
          Username: 'sa'
        }
      };

      const state = createReducer()(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('should handle queryPolicy.rejected', () => {
      const initialState = {
        entities: [],
        loading: REQUEST_STATUS.pending,
        error: null
      };

      const expectState = {
        entities: [],
        loading: REQUEST_STATUS.idle,
        error: 'test'
      };

      const action = {
        type: queryPolicy().rejected.type,
        error: 'test'
      };

      const state = createReducer()(initialState, action);

      expect(state).toEqual(expectState);
    });
  });
});

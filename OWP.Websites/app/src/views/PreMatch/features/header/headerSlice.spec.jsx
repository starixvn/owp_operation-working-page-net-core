/* eslint-disable no-warning-comments */
import { REQUEST_STATUS } from '../../../Shared/Constants';
import headerSlice, {
  updateStartTimeFilter,
  updateStartTimeOriginal,
  getLastUpdate
} from './headerSlice';

jest.mock('../../app/store');

describe('headerSlice under test', () => {
  it('should return the initial state', () => {
    expect(headerSlice(undefined, {})).toEqual({
      info: {
        UpdatedTime: undefined,
        Username: undefined
      },
      loading: REQUEST_STATUS.idle,
      error: null,
      startTimeOriginal: undefined,
      startTimeFilter: undefined
    });
  });
  describe('updateStartTimeFilter under test', () => {
    it('should handle updateStartTimeFilter reducer', () => {
      // Arrange
      const initialState = {
        startTimeFilter: undefined
      };

      const expectState = {
        startTimeFilter: new Date()
      };

      const action = {
        type: updateStartTimeFilter.type,
        payload: new Date()
      };
      // Action
      const state = headerSlice(initialState, action);
      // Assert
      expect(state).toEqual(expectState);
    });
  });

  describe('updateStartTimeOriginal under test', () => {
    it('should handle updateStartTimeOriginal reducer', () => {
      // Arrange
      const initialState = {
        startTimeFilter: undefined,
        startTimeOriginal: undefined
      };

      const expectState = {
        startTimeFilter: new Date(),
        startTimeOriginal: new Date()
      };

      const action = {
        type: updateStartTimeOriginal.type,
        payload: new Date()
      };
      // Action
      const state = headerSlice(initialState, action);
      // Assert
      expect(state).toEqual(expectState);
    });
  });

  describe('getLastUpdate under test', () => {
    it('initialState loading is idle, should handle getLastUpdate.pending', () => {
      // Arrange
      const initialState = {
        loading: REQUEST_STATUS.idle
      };

      const expectState = {
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: getLastUpdate.pending.type
      };

      // Action
      const state = headerSlice(initialState, action);
      // Assert
      expect(state).toEqual(expectState);
    });

    it('initialState loading is pending, should handle getLastUpdate.pending', () => {
      // Arrange
      const initialState = {
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: getLastUpdate.pending.type
      };

      // Action
      const state = headerSlice(initialState, action);
      // Assert
      expect(state).toEqual(initialState);
    });

    it('initialState loading is pending, has data, should handle getLastUpdate.fulfilled', () => {
      const initialState = {
        loading: REQUEST_STATUS.pending,
        info: {
          UpdatedTime: undefined,
          Username: undefined
        }
      };

      const expectState = {
        loading: REQUEST_STATUS.idle,
        info: {
          UpdatedTime: '2020-08-25T11:44:54.38+07:00',
          Username: 'sa'
        }
      };

      const action = {
        type: getLastUpdate.fulfilled.type,
        payload: [
          {
            UpdatedTime: '2020-08-25T11:44:54.38+07:00',
            Username: 'sa'
          }
        ]
      };

      const state = headerSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is pending, has not data, should handle getLastUpdate.fulfilled', () => {
      const initialState = {
        loading: REQUEST_STATUS.pending,
        info: {
          UpdatedTime: undefined,
          Username: undefined
        }
      };

      const expectState = {
        loading: REQUEST_STATUS.idle,
        info: {
          UpdatedTime: undefined,
          Username: undefined
        }
      };

      const action = {
        type: getLastUpdate.fulfilled.type,
        payload: []
      };

      const state = headerSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is pending, should handle getLastUpdate.rejected', () => {
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
        type: getLastUpdate.rejected.type,
        error: 'test'
      };

      const state = headerSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is idle, should handle getLastUpdate.fulfilled', () => {
      const initialState = {
        loading: REQUEST_STATUS.idle,
        info: {
          UpdatedTime: undefined,
          Username: undefined
        }
      };

      const action = {
        type: getLastUpdate.fulfilled.type,
        payload: []
      };

      const state = headerSlice(initialState, action);

      expect(state).toEqual(initialState);
    });

    it('initialState loading is idle, should handle getLastUpdate.rejected', () => {
      const initialState = {
        entities: [],
        loading: REQUEST_STATUS.idle,
        error: null
      };

      const action = {
        type: getLastUpdate.rejected.type,
        error: 'test'
      };

      const state = headerSlice(initialState, action);

      expect(state).toEqual(initialState);
    });
  });
});

import { REQUEST_STATUS } from '../../../Shared/Constants';
import signalRSlice, {
  backupFilterModel,
  backupGetIssueModel
} from './signalRSlice';

describe('signalRSlice under test', () => {
  const initialState = {
    backupFilterModel: null,
    backupGetIssueModel: null,
    loading: REQUEST_STATUS.idle,
    currentRequestId: undefined,
    error: null
  };
  it('should return the initial state', () => {
    expect(signalRSlice(undefined, {})).toEqual({
      ...initialState
    });
  });
  describe('backupFilterModel under test', () => {
    it('should handle backupFilterModel reducer', () => {
      const expectState = {
        backupFilterModel: { EventDate: '10/19/2020', Employee: 'sa' },
        backupGetIssueModel: null,
        currentRequestId: undefined,
        error: null,
        loading: 'idle'
      };

      const action = {
        type: backupFilterModel.type,
        payload: { EventDate: '10/19/2020', Employee: 'sa' }
      };

      const state = signalRSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('backupGetIssueModel under test', () => {
    it('should handle backupGetIssueModel reducer', () => {
      const expectState = {
        backupFilterModel: null,
        backupGetIssueModel: { EventDate: '10/19/2020', Employee: 'sa' },
        currentRequestId: undefined,
        error: null,
        loading: 'idle'
      };

      const action = {
        type: backupGetIssueModel.type,
        payload: { EventDate: '10/19/2020', Employee: 'sa' }
      };

      const state = signalRSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });
});

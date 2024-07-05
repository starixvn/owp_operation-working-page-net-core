import { REQUEST_STATUS } from '../../../Shared/Constants';
import filterSlice, {
  updateFilterDate,
  updateFilterEmployer,
  updateFilterMatchOffers
} from './filterSlice';
import { getMMDDYYYYStringFromDateTime } from '../../../Shared/DateTime/dateTimeHelper';

describe('filterSlice under test', () => {
  it('should return the initial state', () => {
    expect(filterSlice(undefined, {})).toEqual({
      backupFilterModel: null,
      employer: undefined,
      'filterDate': getMMDDYYYYStringFromDateTime(new Date()),
      loading: REQUEST_STATUS.idle,
      currentRequestId: undefined,
      'matchOffers': ['Opened', 'HDP (?)', 'OU (?)', ''],
      error: null,
      "isReloadEdit": false,
      "earlyOffered":  [
         1,
         0,
       ]
    });
  });
  describe('updateFilterDate under test', () => {
    it('should handle updateFilterDate reducer', () => {
      const initialState = {
        filterDate: undefined,
        loading: REQUEST_STATUS.idle,
        currentRequestId: undefined,
        error: null
      };

      const expectState = {
        filterDate: new Date(2023, 22, 10),
        loading: REQUEST_STATUS.idle,
        currentRequestId: undefined,
        error: null
      };

      const action = {
        type: updateFilterDate.type,
        payload: { selectedDate: new Date(2023, 22, 10) }
      };

      const state = filterSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('updateFilterEmployer under test', () => {
    it('should handle updateFilterEmployer reducer', () => {
      const initialState = {
        employer: undefined,
        loading: REQUEST_STATUS.idle,
        currentRequestId: undefined,
        error: null
      };

      const expectState = {
        employer: 'esther',
        loading: REQUEST_STATUS.idle,
        currentRequestId: undefined,
        error: null
      };

      const action = {
        type: updateFilterEmployer.type,
        payload: { employer: 'esther' }
      };

      const state = filterSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('updateFilterMatchOffers under test', () => {
    it('should handle updateFilterMatchOffers reducer', () => {
      const initialState = {
        matchOffers: undefined,
        loading: REQUEST_STATUS.idle,
        currentRequestId: undefined,
        error: null
      };

      const expectState = {
        matchOffers: { match: 'match A' },
        loading: REQUEST_STATUS.idle,
        currentRequestId: undefined,
        error: null
      };

      const action = {
        type: updateFilterMatchOffers.type,
        payload: { match: 'match A' }
      };

      const state = filterSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });
});

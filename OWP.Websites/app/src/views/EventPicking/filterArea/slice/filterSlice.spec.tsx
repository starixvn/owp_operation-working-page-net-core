import reducer, {
  setFilter,
  setPeriod,
  setLastSearchTime
} from './filterSlice';
import { Option } from '../../../Shared/MultipleSelection/type';
import {
  DATE,
  FILTER_MATCH,
  FILTER_MATCH_CATEGORY,
  FILTER_MATCH_CATEGORY_OPTIONS,
  FILTER_MATCH_OPTIONS,
  HAS_OTHER_PROVIDER,
  HAS_PROVIDER,
  HAS_PROVIDER_OPTIONS,
  SELECT_GAME,
  SELECT_GAME_OPTIONS,
  UPDATE_TYPE,
  FROM_DATE,
  TO_DATE,
  PERIOD
} from '../filterConst.json';
import { SELECT_TURNOVER_OPTIONS } from '../const';
import { TFilter } from '../../types/filterType';
import Utils from '../../../Shared/Utils';
import { getUpdateTypes } from './filterThunkNative';

const {
  VALUE: { ONE_MONTH, THREE_MONTHS }
} = PERIOD;

describe('filterSlice under test', () => {
  it('should return the initial state', () => {
    const initState = {
      leagues: [] as Option[],
      updateTypes: [] as Option[],
      lastSearchTime: 0,
      param: {
        [DATE]: Utils.formatDate(new Date()),
        [FILTER_MATCH]: FILTER_MATCH_OPTIONS.map(m => m.value),
        [SELECT_GAME]: SELECT_GAME_OPTIONS.map(m => m.value),
        [HAS_PROVIDER]: HAS_PROVIDER_OPTIONS.map(m => m.value),
        [HAS_OTHER_PROVIDER]: HAS_PROVIDER_OPTIONS.map(m => m.value),
        [FILTER_MATCH_CATEGORY]: FILTER_MATCH_CATEGORY_OPTIONS.map(
          m => m.value
        ),
        [UPDATE_TYPE]: [],
        [PERIOD.KEY]: {
          [ONE_MONTH]: {
            [FROM_DATE]: SELECT_TURNOVER_OPTIONS[0].fromDate,
            [TO_DATE]: SELECT_TURNOVER_OPTIONS[0].toDate
          }
        }
      } as TFilter
    };
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initState);
  });

  describe('setFilter', () => {
    it('should update date filter', () => {
      const action = {
        type: setFilter.type,
        payload: {
          key: 'date',
          value: 'Nov 11, 2023'
        }
      };
      const state = reducer(undefined, action);
      expect(state.param.date).toEqual('Nov 11, 2023');
    });

    it('should update filterMatch filter', () => {
      const action = {
        type: setFilter.type,
        payload: {
          key: FILTER_MATCH,
          value: ['Beige', 'Gray']
        }
      };
      const state = reducer(undefined, action);
      expect(state.param.filterMatch).toEqual(['Beige', 'Gray']);
    });

    it('should update selectGame filter', () => {
      const action = {
        type: setFilter.type,
        payload: {
          key: SELECT_GAME,
          value: [1]
        }
      };
      const state = reducer(undefined, action);
      expect(state.param.selectGame).toEqual([1]);
    });

    it('should update hasProvider filter', () => {
      const action = {
        type: setFilter.type,
        payload: {
          key: HAS_PROVIDER,
          value: ['yes']
        }
      };
      const state = reducer(undefined, action);
      expect(state.param.hasProvider).toEqual(['yes']);
    });

    it('should update updateType filter', () => {
      const action = {
        type: setFilter.type,
        payload: {
          key: UPDATE_TYPE,
          value: [1, 2, null]
        }
      };
      const state = reducer(undefined, action);
      expect(state.param.updateType).toEqual([1, 2, null]);
    });
  });

  describe('setPeriod', () => {
    it('should update period filter with ONE_MONTH', () => {
      const periodData = {
        [ONE_MONTH]: {
          [FROM_DATE]: '2023-10-01',
          [TO_DATE]: '2023-11-01'
        }
      };
      const action = {
        type: setPeriod.type,
        payload: periodData
      };
      const state = reducer(undefined, action);
      expect(state.param.period).toEqual(periodData);
    });

    it('should update period filter with THREE_MONTHS', () => {
      const periodData = {
        [THREE_MONTHS]: {
          [FROM_DATE]: '2023-08-01',
          [TO_DATE]: '2023-11-01'
        }
      };
      const action = {
        type: setPeriod.type,
        payload: periodData
      };
      const state = reducer(undefined, action);
      expect(state.param.period).toEqual(periodData);
    });

    it('should replace existing period filter', () => {
      const initialPeriod = {
        [ONE_MONTH]: {
          [FROM_DATE]: '2023-10-01',
          [TO_DATE]: '2023-11-01'
        }
      };
      const newPeriod = {
        [THREE_MONTHS]: {
          [FROM_DATE]: '2023-08-01',
          [TO_DATE]: '2023-11-01'
        }
      };

      const firstAction = {
        type: setPeriod.type,
        payload: initialPeriod
      };
      const firstState = reducer(undefined, firstAction);

      const secondAction = {
        type: setPeriod.type,
        payload: newPeriod
      };
      const finalState = reducer(firstState, secondAction);

      expect(finalState.param.period).toEqual(newPeriod);
    });
  });

  describe('setLastSearchTime', () => {
    it('should update lastSearchTime', () => {
      const timestamp = 1234567890;
      const action = {
        type: setLastSearchTime.type,
        payload: timestamp
      };
      const state = reducer(undefined, action);
      expect(state.lastSearchTime).toEqual(timestamp);
    });

    it('should update lastSearchTime from 0 to new value', () => {
      const timestamp = Date.now();
      const action = {
        type: setLastSearchTime.type,
        payload: timestamp
      };
      const state = reducer(undefined, action);
      expect(state.lastSearchTime).toEqual(timestamp);
      expect(state.lastSearchTime).not.toEqual(0);
    });

    it('should overwrite existing lastSearchTime', () => {
      const firstTimestamp = 1000000000;
      const firstAction = {
        type: setLastSearchTime.type,
        payload: firstTimestamp
      };
      const firstState = reducer(undefined, firstAction);
      expect(firstState.lastSearchTime).toEqual(firstTimestamp);

      const secondTimestamp = 2000000000;
      const secondAction = {
        type: setLastSearchTime.type,
        payload: secondTimestamp
      };
      const finalState = reducer(firstState, secondAction);
      expect(finalState.lastSearchTime).toEqual(secondTimestamp);
    });
  });

  describe('getUpdateTypes.fulfilled', () => {
    it('should handle getUpdateTypes.fulfilled with single item', () => {
      const data: Option[] = [{ value: 1, label: 'sms' }];
      const expectedData: Option[] = [
        { value: 1, label: 'sms' },
        { value: null, label: '-- None --' }
      ];
      const action = {
        type: getUpdateTypes.fulfilled.type,
        payload: data
      };
      const actual = reducer(undefined, action);
      expect(actual.updateTypes).toEqual(expectedData);
      expect(actual.param.updateType).toEqual([1, null]);
    });

    it('should handle getUpdateTypes.fulfilled with multiple items', () => {
      const data: Option[] = [
        { value: 1, label: 'sms' },
        { value: 2, label: 'email' },
        { value: 3, label: 'push' }
      ];
      const expectedData: Option[] = [
        { value: 1, label: 'sms' },
        { value: 2, label: 'email' },
        { value: 3, label: 'push' },
        { value: null, label: '-- None --' }
      ];
      const action = {
        type: getUpdateTypes.fulfilled.type,
        payload: data
      };
      const actual = reducer(undefined, action);
      expect(actual.updateTypes).toEqual(expectedData);
      expect(actual.param.updateType).toEqual([1, 2, 3, null]);
    });

    it('should handle getUpdateTypes.fulfilled with empty array', () => {
      const data: Option[] = [];
      const expectedData: Option[] = [{ value: null, label: '-- None --' }];
      const action = {
        type: getUpdateTypes.fulfilled.type,
        payload: data
      };
      const actual = reducer(undefined, action);
      expect(actual.updateTypes).toEqual(expectedData);
      expect(actual.param.updateType).toEqual([null]);
    });

    it('should replace existing updateTypes', () => {
      const firstData: Option[] = [{ value: 1, label: 'sms' }];
      const firstAction = {
        type: getUpdateTypes.fulfilled.type,
        payload: firstData
      };
      const firstState = reducer(undefined, firstAction);

      const secondData: Option[] = [
        { value: 2, label: 'email' },
        { value: 3, label: 'push' }
      ];
      const secondAction = {
        type: getUpdateTypes.fulfilled.type,
        payload: secondData
      };
      const finalState = reducer(firstState, secondAction);

      const expectedData: Option[] = [
        { value: 2, label: 'email' },
        { value: 3, label: 'push' },
        { value: null, label: '-- None --' }
      ];
      expect(finalState.updateTypes).toEqual(expectedData);
      expect(finalState.param.updateType).toEqual([2, 3, null]);
    });
  });
});

import reducer, { setFilter } from './filterSlice';
import { REQUEST_STATUS } from '../../../Shared/Constants';
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
  LEAGUE,
  SELECT_GAME,
  SELECT_GAME_OPTIONS,
  UPDATE_TYPE,
  NONE_OPTION
} from '../filterConst.json';
import { TFilter } from '../../types/filterType';
import Utils from '../../../Shared/Utils';
import { getLeagues, getUpdateTypes } from './filterThunkNative';

describe('filterSlice under test', () => {
  it('should return the initial state', () => {
    const initState = {
      leagues: [] as Option[],
      updateTypes: [] as Option[],
      param: {
        [DATE]: Utils.formatDate(new Date()),
        [FILTER_MATCH]: FILTER_MATCH_OPTIONS.map(m => m.value),
        [SELECT_GAME]: SELECT_GAME_OPTIONS.map(m => m.value),
        [HAS_PROVIDER]: HAS_PROVIDER_OPTIONS.map(m => m.value),
        [HAS_OTHER_PROVIDER]: HAS_PROVIDER_OPTIONS.map(m => m.value),
        [FILTER_MATCH_CATEGORY]: FILTER_MATCH_CATEGORY_OPTIONS.map(m => m.value),
        [LEAGUE]: [],
        [UPDATE_TYPE]: []
      } as TFilter
    };
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initState);
  });

  it('should handle getLeagues.fulfilled', () => {
    const data: Option[] = [
      { value: 1, label: 'sms' }
    ];
    const action = {
      type: getLeagues.fulfilled.type,
      payload: data
    };
    const actual = reducer(undefined, action);
    expect(actual.leagues).toEqual(data);
    expect(actual.param.league).toEqual([1]);
  });

  it('should handle getUpdateTypes.fulfilled', () => {
    const data: Option[] = [
      { value: 1, label: 'sms' }
    ];
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

  it('setFilter', () => {
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
});

import {} from '../constants/reportConst';
import {
  QUERY_POLICY,
  ACTION_QUERY_POLICY,
  QUERY_SERVERTIME,
  ACTION_QUERY_SERVERTIME,
  QUERY_DATA_WITH_PARAMS,
  SEARCH,
  ORDER,
  CLEAR_ALLFILTERS
} from '../constants/action-type';
import {
  queryPolicy,
  queryServerTime,
  queryDataWithParams,
  search,
  clearAllFilters,
  order
} from './action';

describe('LA Action under test', () => {
  it('queryPolicy should create a action to query Policy', () => {
    const expectedAction = {
      type: QUERY_POLICY,
      payload: ACTION_QUERY_POLICY
    };
    expect(queryPolicy()).toEqual(expectedAction);
  });
  it('queryServerTime should create a action to query ServerTime', () => {
    const expectedAction = {
      type: QUERY_SERVERTIME,
      payload: ACTION_QUERY_SERVERTIME
    };
    expect(queryServerTime()).toEqual(expectedAction);
  });
  it('search should create a action to search', () => {
    const model = {
      LAKeyWord: 'Laa',
      SectionKeyWord: 'Saa'
    };
    const expectedAction = {
      type: SEARCH,
      payload: model
    };
    expect(search(model)).toEqual(expectedAction);
  });
  it('order should create a action to order', () => {
    const model = {
      ColumnId: 'LinkX3',
      TypeSort: 'Ord_Des'
    };
    const expectedAction = {
      type: ORDER,
      payload: model
    };
    expect(order(model)).toEqual(expectedAction);
  });
  it('clearAllFilters should create a action to set isClearAll = true', () => {
    const expectedAction = {
      type: CLEAR_ALLFILTERS,
      isClearAll: true
    };
    expect(clearAllFilters(true)).toEqual(expectedAction);
  });
  it('clearAllFilters should create a action to set isClearAll = false', () => {
    const expectedAction = {
      type: CLEAR_ALLFILTERS,
      isClearAll: false
    };
    expect(clearAllFilters(false)).toEqual(expectedAction);
  });
  it('queryDataWithParams should create a action to query Data', () => {
    const url = 'MockURl';
    const model = 'MockModel';
    const actionPayload = 'MockactionPayload';

    const expectedAction = {
      type: QUERY_DATA_WITH_PARAMS,
      payload: Object.assign({ url, model }, actionPayload)
    };
    expect(queryDataWithParams(url, model, actionPayload)).toEqual(
      expectedAction
    );
  });
});

import MaxbetConst from '../constants/MaxbetConst';
import {
  ACTION_TVLIST,
  QUERY_DATA,
  ACTION_LEAGUE_FILTER,
  POST_DATA,
  UPDATE_STATUS_DISPLAY_POPUP_FILTER,
  EXECUTE_FILTER,
  CLEAR_FILTER,
  QUERY_DATA_WITH_PARAMS,
  QUERY_POLICY,
  TOGGLE_COLUMN,
  EXECUTE_SPECIALPRODUCT_FILTER
} from '../constants/action-type';
import {
  crud,
  queryData,
  updateExpressionFilter,
  postDataWithExtraModel,
  updateStatusDisplayPopupFilter,
  executeFilter,
  clearFilter,
  queryDataWithParams,
  postData,
  queryPolicy,
  toggleColumn,
  executeSpecialProductFilter
} from './action';

describe('actions', () => {
  it('Should create a action to add TVList', () => {
    const model = {
      tv: {
        expression: MaxbetConst.expressionOption[0],
        value: ''
      }
    };
    const expectedAction = {
      type: ACTION_TVLIST.ADD,
      payload: model
    };
    expect(crud(ACTION_TVLIST.ADD, model)).toEqual(expectedAction);
  });

  it('Should create a action to update TVList', () => {
    const model = {
      key: 'expression',
      value: 'abc',
      index: 1
    };
    const expectedAction = {
      type: ACTION_TVLIST.UPDATE,
      payload: model
    };
    expect(crud(ACTION_TVLIST.UPDATE, model)).toEqual(expectedAction);
  });

  it('Should create a action to delete TVList', () => {
    const model = {
      index: 1
    };
    const expectedAction = {
      type: ACTION_TVLIST.DELETE,
      payload: model
    };
    expect(crud(ACTION_TVLIST.DELETE, model)).toEqual(expectedAction);
  });

  it('Should create a action to query data', () => {
    const url = '#';
    const actionPayload = 'SUCCESS';
    const expectedAction = {
      type: QUERY_DATA,
      payload: {
        0: 'S',
        1: 'U',
        2: 'C',
        3: 'C',
        4: 'E',
        5: 'S',
        6: 'S',
        url: '#'
      }
    };
    expect(queryData(url, actionPayload)).toEqual(expectedAction);
  });

  it('Should create a action to update expression filter', () => {
    const value = 'abc';
    const expectedAction = {
      type: ACTION_LEAGUE_FILTER.UPDATE,
      payload: {
        value
      }
    };
    expect(
      updateExpressionFilter(ACTION_LEAGUE_FILTER.UPDATE, { value })
    ).toEqual(expectedAction);
  });

  it('Should create a action to post data with extra model', () => {
    const actionPayload = 'SUCCESS';
    const model = {
      IsBlackList: true,
      OWPLeagueId: 113,
      apiKey: MaxbetConst.maxbetUpdateLeagueIsBlacklistAPIKey
    };
    const extraModel = {
      rowIndex: 123,
      IsBlackList: true
    };
    const url = '#';
    const expectedAction = {
      type: POST_DATA,
      payload: {
        url,
        model,
        extraModel,
        0: 'S',
        1: 'U',
        2: 'C',
        3: 'C',
        4: 'E',
        5: 'S',
        6: 'S'
      }
    };
    expect(
      postDataWithExtraModel(url, model, extraModel, actionPayload)
    ).toEqual(expectedAction);
  });

  it('Should create a action to update status display popup filter', () => {
    const model = {
      filterKey: 'league',
      isDisplay: true
    };
    const expectedAction = {
      type: UPDATE_STATUS_DISPLAY_POPUP_FILTER,
      payload: model
    };
    expect(updateStatusDisplayPopupFilter(model)).toEqual(expectedAction);
  });

  it('Should create a action to execute filter', () => {
    const model = {
      filterKey: 'league',
      keyword: 'abx'
    };
    const expectedAction = {
      type: EXECUTE_FILTER,
      payload: model
    };
    expect(executeFilter(model)).toEqual(expectedAction);
  });

  it('Should create a action to clear filter', () => {
    const expectedAction = {
      type: CLEAR_FILTER,
      payload: []
    };
    expect(clearFilter()).toEqual(expectedAction);
  });

  it('Should create action queryDataWithParams', () => {
    const expectedAction = {
      type: QUERY_DATA_WITH_PARAMS,
      payload: { url: 'abc', model: { name: 'a' }, match: 'ABC' }
    };

    const actual = queryDataWithParams('abc', { name: 'a' }, { match: 'ABC' });

    expect(actual).toEqual(expectedAction);
  });

  it('Should create action POST_DATA', () => {
    const expectedAction = {
      type: POST_DATA,
      payload: { url: 'abc', model: { name: 'a' }, match: 'ABC' }
    };

    const actual = postData('abc', { name: 'a' }, { match: 'ABC' });

    expect(actual).toEqual(expectedAction);
  });

  it('Should create action queryPolicy ', () => {
    const expectedAction = {
      type: QUERY_POLICY,
      payload: {
        'ERROR': 'ACTION_QUERY_POLICY_ERROR',
        'PENDING': 'ACTION_QUERY_POLICY_PENDING',
        'SUCCESS': 'ACTION_QUERY_POLICY_SUCCESS'
      }
    };

    const actual = queryPolicy();

    expect(actual).toEqual(expectedAction);
  });

  it('Should create action TOGGLE_COLUMN ', () => {
    const expectedAction = {
      type: TOGGLE_COLUMN,
      payload: {
        columnKeys: 'Trader'
      }
    };

    const actual = toggleColumn('Trader');

    expect(actual).toEqual(expectedAction);
  });

  it('Should create action executeSpecialProductFilter  ', () => {
    const expectedAction = {
      type: EXECUTE_SPECIALPRODUCT_FILTER,
      payload: {}
    };

    const actual = executeSpecialProductFilter({});

    expect(actual).toEqual(expectedAction);
  });
});

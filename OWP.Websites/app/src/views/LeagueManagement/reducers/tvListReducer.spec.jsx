import { QUERY_TVLIST, ACTION_TVLIST } from '../constants/action-type';
import tvListReducer from './tvListReducer';

describe('tvList reducer', () => {
  it('should return the initial state', () => {
    expect(
      tvListReducer(
        {
          tvListPending: true,
          tvListExpression: [],
          tvListError: []
        },
        {}
      )
    ).toEqual({
      tvListPending: true,
      tvListExpression: [],
      tvListError: []
    });
  });

  it('should handle QUERY_TVLIST.PENDING', () => {
    expect(
      tvListReducer(undefined, {
        type: QUERY_TVLIST.PENDING
      })
    ).toEqual({
      tvListPending: true,
      tvListExpression: [],
      tvListError: []
    });
  });

  it('should handle QUERY_TVLIST.SUCCESS', () => {
    const actual = tvListReducer(undefined, {
      type: QUERY_TVLIST.SUCCESS,
      data: []
    });
    expect(actual.tvListExpression[0].value).toEqual('');
    expect(actual.tvListExpression[0].expression).toEqual({
      value: 1,
      label: 'Contains'
    });
  });

  it('should handle QUERY_TVLIST.ERROR', () => {
    expect(
      tvListReducer(undefined, {
        type: QUERY_TVLIST.ERROR,
        error: ['Fail to query data']
      })
    ).toEqual({
      tvListExpression: [],
      tvListPending: false,
      tvListError: ['Fail to query data']
    });
  });

  it('should handle ACTION_TVLIST.UPDATE', () => {
    expect(
      tvListReducer(
        {
          tvListExpression: [
            {
              expression: { value: 1, label: 'Contains' }
            }
          ]
        },
        {
          type: ACTION_TVLIST.UPDATE,
          payload: {
            index: 0,
            key: 'expression',
            value: { value: 0, label: 'Equals' }
          }
        }
      )
    ).toEqual({
      tvListExpression: [
        {
          expression: { value: 0, label: 'Equals' }
        }
      ]
    });
  });

  it('should handle ACTION_TVLIST.DELETE', () => {
    expect(
      tvListReducer(
        {
          tvListExpression: [
            {
              expression: { value: 0, label: 'Equals' },
              value: 'abc'
            },
            {
              expression: { value: 1, label: 'Contains' },
              value: 'abcx'
            }
          ]
        },
        {
          type: ACTION_TVLIST.DELETE,
          payload: {
            index: 1
          }
        }
      ).tvListExpression[0].expression
    ).toEqual({ value: 0, label: 'Equals' });
  });

  it('should handle ACTION_TVLIST.ADD', () => {
    expect(
      tvListReducer(
        {
          tvListExpression: []
        },
        {
          type: ACTION_TVLIST.ADD,
          payload: {
            tv: {
              expression: { value: 0, label: 'Equals' },
              value: ''
            }
          }
        }
      ).tvListExpression[0].expression
    ).toEqual({ value: 0, label: 'Equals' });
  });
});

import {
  QUERY_CONDITION,
  ACTION_TV,
  ACTION_ROUND,
  ACTION_DIVISION
} from '../constants/action-type';
import conditionReducer from './conditionReducer';

describe('condition reducer', () => {
  it('should return the initial state', () => {
    expect(conditionReducer(undefined, {})).toEqual({
      conditionPending: true,
      tv: {},
      round: [],
      division: []
    });
  });

  it('should handle QUERY_CONDITION.PENDING', () => {
    expect(
      conditionReducer(undefined, {
        type: QUERY_CONDITION.PENDING
      })
    ).toEqual({
      conditionPending: true,
      tv: {},
      round: [],
      division: []
    });
  });

  it('should handle QUERY_CONDITION.SUCCESS', () => {
    const data = {
      LeagueSettingId: 123,
      HasTV: true,
      TV: 'TV',
      Rounds: [],
      Divisions: []
    };
    let {
      conditionPending,
      tv,
      round,
      division,
      leagueSettingId
    } = conditionReducer(undefined, {
      type: QUERY_CONDITION.SUCCESS,
      data
    });
    round = round.map(item => ({
      expression: item.expression,
      value: item.value
    }));
    division = division.map(item => ({
      expression: item.expression,
      value: item.value
    }));
    expect(conditionPending).toEqual(false);
    expect(tv).toEqual({ hasTV: true, tv: 'TV' });
    expect(leagueSettingId).toEqual(123);
    expect(round).toEqual([
      { expression: { value: 1, label: 'Contains' }, value: '' }
    ]);
    expect(division).toEqual([
      { expression: { value: 1, label: 'Contains' }, value: '' }
    ]);
  });

  it('should update TV successfully', () => {
    const { tv } = conditionReducer(undefined, {
      type: ACTION_TV.UPDATE,
      payload: {
        key: 'value',
        value: 'carlos'
      }
    });
    expect(tv).toEqual({ value: 'carlos' });
  });

  it('should update Round successfully', () => {
    const state = {
      round: [{ value: 'abc' }]
    };
    const { round } = conditionReducer(state, {
      type: ACTION_ROUND.UPDATE,
      payload: {
        index: 0,
        key: 'value',
        value: 'carlos'
      }
    });
    expect(round).toEqual([{ value: 'carlos' }]);
  });

  it('should add Round successfully', () => {
    const state = {
      round: [{ value: 'abc' }]
    };
    let { round } = conditionReducer(state, {
      type: ACTION_ROUND.ADD,
      payload: {
        round: {
          value: 'carlos'
        }
      }
    });
    round = round.map(item => ({ value: item.value }));
    expect(round).toEqual([{ value: 'abc' }, { value: 'carlos' }]);
  });

  it('should delete Round successfully', () => {
    const state = {
      round: [{ value: 'carlos' }, { value: 'abc' }]
    };
    let { round } = conditionReducer(state, {
      type: ACTION_ROUND.DELETE,
      payload: {
        index: 1
      }
    });
    round = round.map(item => ({ value: item.value }));
    expect(round).toEqual([{ value: 'carlos' }]);
  });

  it('should update division successfully', () => {
    const state = {
      division: [{ value: 'abc' }]
    };
    const { division } = conditionReducer(state, {
      type: ACTION_DIVISION.UPDATE,
      payload: {
        index: 0,
        key: 'value',
        value: 'carlos'
      }
    });
    expect(division).toEqual([{ value: 'carlos' }]);
  });

  it('should add division successfully', () => {
    const state = {
      division: [{ value: 'abc' }]
    };
    let { division } = conditionReducer(state, {
      type: ACTION_DIVISION.ADD,
      payload: {
        division: {
          value: 'carlos'
        }
      }
    });
    division = division.map(item => ({ value: item.value }));
    expect(division).toEqual([{ value: 'abc' }, { value: 'carlos' }]);
  });

  it('should delete division successfully', () => {
    const state = {
      division: [{ value: 'carlos' }, { value: 'abc' }]
    };
    let { division } = conditionReducer(state, {
      type: ACTION_DIVISION.DELETE,
      payload: {
        index: 1
      }
    });
    division = division.map(item => ({ value: item.value }));
    expect(division).toEqual([{ value: 'carlos' }]);
  });
});

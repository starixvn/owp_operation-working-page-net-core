import reducer, {
  updateObject,
  updateExpression,
  setInitSetting,
  setOldData,
  addRowExpressions,
  removeRowExpressions
} from './editSettingSlice';
import { getLeagueGroupOptionsThunk } from '../../component/commonThunk';
import { REQUEST_STATUS } from '../../../../../Shared/Constants';

describe('editSettingSlice', () => {
  const initialState = {
    leagueGroupOptions: [],
    loading: REQUEST_STATUS.idle,
    leagueSettingId: 0,
    league: '',
    owpLeagueId: 0,
    leagueGroupId: 0,
    leagueGroup: '',
    shortLeague: '',
    isBlackList: false,
    noOfferLive: 0,
    spread: '',
    maxbetWD_HDP: '',
    wD_NoOfHDP: '',
    maxbetWK_HDP: '',
    wK_NoOfHDP: '',
    lstNote: '',
    remark2: '',
    comment: '',
    hasTv: false,
    hasIpTv: false,
    remark: '',
    oldLeagueGroupId: 0,
    round: [],
    division: [],
    oldData: null
  };

  describe('Initial State', () => {
    it('should return initial state', () => {
      expect(reducer(undefined, { type: 'unknown' })).toEqual(initialState);
    });
  });

  describe('setInitSetting', () => {
    it('should set initial setting from data source', () => {
      const mockDataSource = {
        leagueSettingId: 123,
        league: 'Test League',
        owpLeagueId: 456,
        leagueGroupId: 789,
        leagueGroup: 'Test Group',
        spreadShow: '1.5',
        maxbetWD_HDP_Show: '1000',
        wD_NoOfHDP_Show: '5',
        maxbetWK_HDP_Show: '2000',
        wK_NoOfHDP_Show: '10',
        lstNote: 'Note',
        remark2: 'Remark',
        comment: 'Comment',
        condition: {
          hasTv: true,
          hasIpTv: false,
          round: [{ type: 'Contains', value: 'Round 1' }],
          division: [{ type: 'Equals', value: 'Div 1' }],
          remark: 'Test remark'
        },
        shortLeague: 'TL',
        isBlackList: true,
        noOfferLive: 1
      };

      jest.mock('../../helper/helper', () => ({
        mapEditSettingFromDataSource: jest.fn((data) => ({
          leagueSettingId: data.leagueSettingId,
          league: data.league,
          owpLeagueId: data.owpLeagueId,
          leagueGroupId: data.leagueGroupId,
          leagueGroup: data.leagueGroup,
          spread: data.spreadShow,
          maxbetWD_HDP: data.maxbetWD_HDP_Show,
          wD_NoOfHDP: data.wD_NoOfHDP_Show,
          maxbetWK_HDP: data.maxbetWK_HDP_Show,
          wK_NoOfHDP: data.wK_NoOfHDP_Show,
          lstNote: data.lstNote,
          remark2: data.remark2,
          comment: data.comment,
          hasTv: data.condition.hasTv,
          hasIpTv: data.condition.hasIpTv,
          remark: data.condition.remark,
          round: data.condition.round,
          division: data.condition.division,
          shortLeague: data.shortLeague,
          isBlackList: data.isBlackList,
          noOfferLive: data.noOfferLive
        }))
      }));

      const action = setInitSetting(mockDataSource as any);
      const state = reducer(initialState, action);

      expect(state.leagueSettingId).toBe(123);
      expect(state.oldData).toEqual(mockDataSource);
      expect(state.oldLeagueGroupId).toBe(789);
    });
  });

  describe('setOldData', () => {
    it('should set old data', () => {
      const mockData = { leagueSettingId: 123, league: 'Test' } as any;
      const action = setOldData(mockData);
      const state = reducer(initialState, action);

      expect(state.oldData).toEqual(mockData);
    });
  });

  describe('updateObject', () => {
    it('should update string property', () => {
      const action = updateObject({ property: 'league', value: 'New League' });
      const state = reducer(initialState, action);

      expect(state.league).toBe('New League');
    });

    it('should update number property', () => {
      const action = updateObject({ property: 'leagueSettingId', value: 999 });
      const state = reducer(initialState, action);

      expect(state.leagueSettingId).toBe(999);
    });

    it('should update boolean property', () => {
      const action = updateObject({ property: 'hasTv', value: true });
      const state = reducer(initialState, action);

      expect(state.hasTv).toBe(true);
    });

    it('should update multiple properties independently', () => {
      let state = reducer(initialState, updateObject({ property: 'league', value: 'League 1' }));
      state = reducer(state, updateObject({ property: 'spread', value: '1.5' }));

      expect(state.league).toBe('League 1');
      expect(state.spread).toBe('1.5');
    });
  });

  describe('addRowExpressions', () => {
    it('should add row to round array', () => {
      const action = addRowExpressions('round');
      const state = reducer(initialState, action);

      expect(state.round).toHaveLength(1);
      expect(state.round[0]).toEqual({ type: 'Contains', value: '' });
    });

    it('should add row to division array', () => {
      const action = addRowExpressions('division');
      const state = reducer(initialState, action);

      expect(state.division).toHaveLength(1);
      expect(state.division[0]).toEqual({ type: 'Contains', value: '' });
    });

    it('should add multiple rows', () => {
      let state = reducer(initialState, addRowExpressions('round'));
      state = reducer(state, addRowExpressions('round'));

      expect(state.round).toHaveLength(2);
    });
  });

  describe('removeRowExpressions', () => {
    it('should remove row from round array', () => {
      const stateWithRows = reducer(initialState, addRowExpressions('round'));
      const stateWithTwoRows = reducer(stateWithRows, addRowExpressions('round'));

      const action = removeRowExpressions({ index: 0, property: 'round' });
      const state = reducer(stateWithTwoRows, action);

      expect(state.round).toHaveLength(1);
    });

    it('should remove row from division array', () => {
      const stateWithRows = reducer(initialState, addRowExpressions('division'));
      const stateWithTwoRows = reducer(stateWithRows, addRowExpressions('division'));

      const action = removeRowExpressions({ index: 1, property: 'division' });
      const state = reducer(stateWithTwoRows, action);

      expect(state.division).toHaveLength(1);
    });

    it('should not remove if index is out of bounds', () => {
      const stateWithRows = reducer(initialState, addRowExpressions('round'));

      const action = removeRowExpressions({ index: 10, property: 'round' });
      const state = reducer(stateWithRows, action);

      expect(state.round).toHaveLength(1);
    });
  });

  describe('updateExpression', () => {
    it('should update expression type', () => {
      const stateWithRow = reducer(initialState, addRowExpressions('round'));

      const action = updateExpression({
        property: 'round',
        key: 'type',
        value: 'Equals',
        index: 0
      });
      const state = reducer(stateWithRow, action);

      expect(state.round[0].type).toBe('Equals');
    });

    it('should update expression value', () => {
      const stateWithRow = reducer(initialState, addRowExpressions('round'));

      const action = updateExpression({
        property: 'round',
        key: 'value',
        value: 'Round 1',
        index: 0
      });
      const state = reducer(stateWithRow, action);

      expect(state.round[0].value).toBe('Round 1');
    });

    it('should update expression type to Pairwise Contains', () => {
      const stateWithRow = reducer(initialState, addRowExpressions('division'));

      const action = updateExpression({
        property: 'division',
        key: 'type',
        value: 'Pairwise Contains',
        index: 0
      });
      const state = reducer(stateWithRow, action);

      expect(state.division[0].type).toBe('Pairwise Contains');
    });

    it('should not update type if value is invalid', () => {
      const stateWithRow = reducer(initialState, addRowExpressions('round'));

      const action = updateExpression({
        property: 'round',
        key: 'type',
        value: 'InvalidType',
        index: 0
      });
      const state = reducer(stateWithRow, action);

      expect(state.round[0].type).toBe('Contains');
    });
  });

  describe('extraReducers - getLeagueGroupOptionsThunk', () => {
    it('should set loading to pending when thunk is pending', () => {
      const action = { type: getLeagueGroupOptionsThunk.pending.type };
      const state = reducer(initialState, action);

      expect(state.loading).toBe(REQUEST_STATUS.pending);
    });

    it('should set leagueGroupOptions and loading to idle when thunk is fulfilled', () => {
      const mockOptions = [
        { value: 1, label: 'Group 1' },
        { value: 2, label: 'Group 2' }
      ];

      const action = {
        type: getLeagueGroupOptionsThunk.fulfilled.type,
        payload: mockOptions
      };
      const state = reducer(initialState, action);

      expect(state.leagueGroupOptions).toEqual(mockOptions);
      expect(state.loading).toBe(REQUEST_STATUS.idle);
    });
  });
});


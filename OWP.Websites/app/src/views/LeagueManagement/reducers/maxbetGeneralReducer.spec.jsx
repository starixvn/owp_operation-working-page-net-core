import maxbetGeneralReducer from './maxbetGeneralReducer';
import {
  QUERY_MAXBETGENARAL_INFO,
  MAXBETGENARAL_INFO_UPDATE_LEAGUEGROUP,
  MAXBETGENARAL_INFO_UPDATE_ACTIVE,
  MAXBETGENARAL_INFO_UPDATE_BLACK_LIST,
  CLEAR_FILTER
} from '../constants/action-type';

describe('maxbetGeneralReducer', () => {
  it('Should return status pending', () => {
    const actual = maxbetGeneralReducer(undefined, {
      type: QUERY_MAXBETGENARAL_INFO.PENDING,
      maxbetGeneralPending: false
    });
    expect(actual).toEqual({
      maxbetGeneralPending: true,
      maxbetGeneralData: [],
      maxbetGeneralError: [],
      filterInfo: []
    });
  });

  it('Should return error', () => {
    const actual = maxbetGeneralReducer(undefined, {
      type: QUERY_MAXBETGENARAL_INFO.ERROR,
      error: ['Faile to connect server']
    });
    expect(actual).toEqual({
      maxbetGeneralPending: false,
      maxbetGeneralData: [],
      maxbetGeneralError: ['Faile to connect server'],
      filterInfo: []
    });
  });

  it('Should update league group correct', () => {
    const state = {
      maxbetGeneralData: [
        { GroupValue: 1, GroupLabel: 'Carlos' },
        { GroupValue: 2, GroupLabel: 'Tylor' }
      ]
    };
    const action = {
      type: MAXBETGENARAL_INFO_UPDATE_LEAGUEGROUP.SUCCESS,
      extraModel: {
        rowIndex: 1,
        GroupValue: 3,
        GroupLabel: 'xxx'
      }
    };
    const actual = maxbetGeneralReducer(state, action);
    expect(actual).toEqual({
      maxbetGeneralData: [
        { GroupValue: 1, GroupLabel: 'Carlos' },
        { GroupValue: 3, GroupLabel: 'xxx' }
      ],
      maxbetGeneralPending: false,
      maxbetGeneralError: undefined
    });
  });

  it('Should update active correct', () => {
    const state = {
      maxbetGeneralData: [{ IsActive: true }, { IsActive: false }]
    };
    const action = {
      type: MAXBETGENARAL_INFO_UPDATE_ACTIVE.SUCCESS,
      extraModel: {
        rowIndex: 1,
        IsActive: true
      }
    };
    const actual = maxbetGeneralReducer(state, action);
    expect(actual).toEqual({
      maxbetGeneralData: [{ IsActive: true }, { IsActive: true }],
      maxbetGeneralPending: false,
      maxbetGeneralError: undefined
    });
  });

  it('Should update blackList correct', () => {
    const state = {
      maxbetGeneralData: [{ IsBlackList: true }, { IsBlackList: false }]
    };
    const action = {
      type: MAXBETGENARAL_INFO_UPDATE_BLACK_LIST.SUCCESS,
      extraModel: {
        rowIndex: 1,
        IsBlackList: true
      }
    };
    const actual = maxbetGeneralReducer(state, action);
    expect(actual).toEqual({
      maxbetGeneralData: [{ IsBlackList: true }, { IsBlackList: true }],
      maxbetGeneralPending: false,
      maxbetGeneralError: undefined
    });
  });

  it('Should update blackList correct', () => {
    const state = {
      maxbetGeneralDataSource: []
    };
    const action = {
      type: CLEAR_FILTER
    };
    const actual = maxbetGeneralReducer(state, action);
    expect(actual).toEqual({
      maxbetGeneralDataSource: [],
      filterInfo: [],
      maxbetGeneralData: []
    });
  });
});

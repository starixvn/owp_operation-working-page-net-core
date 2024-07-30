import shiftSlice, {
  addNoteIdToLeague,
  addNoteIdToMatch,
  changeRowHightLevelWhenPrepareTyping,
  changeStatePopupLeagueNote,
  changeStatePopupMatchNote,
  clearEntities,
  clearStateDayShift,
  clearStateNightShift,
  collapseAllLeague,
  collapseOneLeague,
  deleteRow,
  expandAllLeague,
  expandOneLeague,
  getShiftByDateAndEmployee,
  restore,
  showUnreadMessageOnly,
  sortByColumnKey,
  sortByDefault,
  unShowUnreadMessageOnly,
  updatedIconIssueLeagueRow,
  updatedIconIssueMatchRow,
  updateIconNoteToPencil,
  updatePrematch
} from './shiftSlice';
import {
  addIssueIdToLeague,
  addIssueIdToMatch,
  pasteCopiedValue
} from '../../../ImportPrematchHelperNative';
import { REQUEST_STATUS } from '../../../../Shared/Constants';

jest.mock('../../../app/store');
let initialState = null;

afterEach(() => {
  initialState = null;
});

describe('shiftSlice under test', () => {
  describe('getShiftByDateAndEmployee under test', () => {
    it('initialState loading is idle, should handle pending', () => {
      initialState = {
        currentRequestId: 1,
        loading: REQUEST_STATUS.idle
      };

      const expectState = {
        currentRequestId: 1,
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: getShiftByDateAndEmployee.pending.type,
        meta: { requestId: 1 }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
    it('initialState loading is pending, should handle pending', () => {
      initialState = {
        currentRequestId: 1,
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: getShiftByDateAndEmployee.pending.type,
        meta: { requestId: 1 }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(initialState);
    });

    it('should handle getShiftByDateAndEmployee.fulfilled case "requestId === currentRequestId"', () => {
      initialState = {
        entities: [],
        currentRequestId: 2,
        loading: REQUEST_STATUS.pending,
        error: null
      };

      const expectState = {
        entities: [
          { PreMatchLeagueId: 1, isExpand: undefined },
          { PreMatchLeagues: 2, isExpand: undefined },
          { PreMatchLeagues: 3, isExpand: undefined }
        ],
        loading: REQUEST_STATUS.idle,
        currentRequestId: null,
        error: null,
        matches: [
          { InfoSiteMatchId: 1 },
          { InfoSiteMatchId: 2 },
          { InfoSiteMatchId: 3 }
        ],
        leagues: [
          { PreMatchLeagueId: 1, isExpand: undefined },
          { PreMatchLeagues: 2, isExpand: undefined },
          { PreMatchLeagues: 3, isExpand: undefined }
        ]
      };

      const action = {
        type: getShiftByDateAndEmployee.fulfilled.type,
        meta: { requestId: 2 },
        payload: {
          PreMatchLeagues: [
            { PreMatchLeagueId: 1 },
            { PreMatchLeagues: 2 },
            { PreMatchLeagues: 3 }
          ],
          InfoSiteMatches: [
            { InfoSiteMatchId: 1 },
            { InfoSiteMatchId: 2 },
            { InfoSiteMatchId: 3 }
          ]
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('should handle getShiftByDateAndEmployee.fulfilled case "requestId !== currentRequestId"', () => {
      initialState = {
        entities: [],
        currentRequestId: 3,
        loading: REQUEST_STATUS.pending,
        error: null
      };

      const expectState = {
        entities: [],
        currentRequestId: 3,
        loading: REQUEST_STATUS.pending,
        error: null
      };

      const action = {
        type: getShiftByDateAndEmployee.fulfilled.type,
        meta: { requestId: 2 },
        payload: [1, 3, 4]
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is pending, should handle getShiftByDateAndEmployee.rejected', () => {
      initialState = {
        entities: [],
        currentRequestId: 2,
        loading: REQUEST_STATUS.pending,
        error: null
      };

      const expectState = {
        entities: [],
        loading: REQUEST_STATUS.idle,
        error: 'test',
        currentRequestId: null
      };

      const action = {
        type: getShiftByDateAndEmployee.rejected.type,
        meta: { requestId: 2 },
        payload: { id: 1, test: [2, 3] },
        error: 'test'
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
    it('initialState loading is idle, should handle getShiftByDateAndEmployee.rejected', () => {
      initialState = {
        entities: [],
        currentRequestId: 2,
        loading: REQUEST_STATUS.idle,
        error: null
      };

      const action = {
        type: getShiftByDateAndEmployee.rejected.type,
        meta: { requestId: 2 },
        payload: { id: 1, test: [2, 3] },
        error: 'test'
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(initialState);
    });
  });

  describe('updatePrematch under test', () => {
    it(' should handle updatePrematch', () => {
      initialState = {
        entities: [
          { PreMatchLeagueId: 4, LeagueName: 'test', HDP: '5KR3OCF3N0' }
        ],
        originDataSource: [],
        prematchIdUpdated: [4],
        loading: REQUEST_STATUS.idle,
        leagues: [{ PreMatchLeagueId: 4, HDP: 12 }]
      };

      const expectState = {
        entities: [
          { PreMatchLeagueId: 4, LeagueName: 'test', HDP: 'fakeName' }
        ],
        loading: REQUEST_STATUS.idle,
        prematchIdUpdated: [4],
        originDataSource: [],
        leagues: [{ PreMatchLeagueId: 4, HDP: 'fakeName' }]
      };

      const action = {
        type: updatePrematch.type,
        payload: {
          rowIndex: '0',
          newValue: 'fakeName',
          key: 'HDP'
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('sortByColumnKey under test', () => {
    it(' should handle sort ORDER_DESC', () => {
      initialState = {
        entities: [
          { PreMatchLeagueId: 1, LeagueName: 'test1' },
          { PreMatchLeagueId: 2, LeagueName: 'test2' },
          { PreMatchLeagueId: 3, LeagueName: 'test3' },
          { PreMatchLeagueId: 4, LeagueName: 'test0' }
        ],
        leagues: [
          { PreMatchLeagueId: 1, LeagueName: 'test1' },
          { PreMatchLeagueId: 2, LeagueName: 'test2' },
          { PreMatchLeagueId: 3, LeagueName: 'test3' },
          { PreMatchLeagueId: 4, LeagueName: 'test0' }
        ],
        matches: [],
        sortState: {},
        loading: REQUEST_STATUS.idle
      };

      const expectState = {
        entities: [
          { PreMatchLeagueId: 4, LeagueName: 'test0' },
          { PreMatchLeagueId: 3, LeagueName: 'test3' },
          { PreMatchLeagueId: 2, LeagueName: 'test2' },
          { PreMatchLeagueId: 1, LeagueName: 'test1' }
        ],
        leagues: [
          { PreMatchLeagueId: 4, LeagueName: 'test0' },
          { PreMatchLeagueId: 3, LeagueName: 'test3' },
          { PreMatchLeagueId: 2, LeagueName: 'test2' },
          { PreMatchLeagueId: 1, LeagueName: 'test1' }
        ],
        loading: REQUEST_STATUS.idle,
        sortState: { columnKey: 'PreMatchLeagueId', orderType: 'ORDER_DESC' },
        matches: []
      };

      const action = {
        type: sortByColumnKey.type,
        payload: { columnKey: 'PreMatchLeagueId', orderType: 'ORDER_DESC' }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('sortByDefault under test', () => {
    it(' should handle sort', () => {
      initialState = {
        leagues: [
          {
            PreMatchLeagueId: 98,
            HDP: 'sa',
            OU: 'starix1',
            HDPTakeOver: null,
            OUTakeOver: null
          },
          {
            PreMatchLeagueId: 99,
            HDP: null,
            OU: null,
            HDPTakeOver: null,
            OUTakeOver: null
          },
          {
            PreMatchLeagueId: 100,
            HDP: 'sa',
            OU: 'starix1',
            HDPTakeOver: 'ha',
            OUTakeOver: null
          }
        ],
        matches: []
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 99,
            HDP: null,
            OU: null,
            HDPTakeOver: null,
            OUTakeOver: null
          },
          {
            PreMatchLeagueId: 100,
            HDP: 'sa',
            OU: 'starix1',
            HDPTakeOver: 'ha',
            OUTakeOver: null
          },
          {
            PreMatchLeagueId: 98,
            HDP: 'sa',
            OU: 'starix1',
            HDPTakeOver: null,
            OUTakeOver: null
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 99,
            HDP: null,
            OU: null,
            HDPTakeOver: null,
            OUTakeOver: null
          },
          {
            PreMatchLeagueId: 100,
            HDP: 'sa',
            OU: 'starix1',
            HDPTakeOver: 'ha',
            OUTakeOver: null
          },
          {
            PreMatchLeagueId: 98,
            HDP: 'sa',
            OU: 'starix1',
            HDPTakeOver: null,
            OUTakeOver: null
          }
        ],
        matches: []
      };

      const action = {
        type: sortByDefault.type
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('deleteRow under test', () => {
    it(' should handle deleteRow', () => {
      initialState = {
        entities: [],
        matches: [],
        prematchIdDeleted: [4],
        loading: REQUEST_STATUS.idle,
        leagues: []
      };

      const expectState = {
        entities: [],
        loading: REQUEST_STATUS.idle,
        prematchIdDeleted: [4],
        leagues: [],
        matches: []
      };

      const action = {
        type: deleteRow.type,
        payload: {
          PreMatchLeagueId: 4
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('pasteCopiedValue under test', () => {
    it('isTakeOver = false, should handle pasteCopiedValue', () => {
      initialState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDP: null,
            OU: null,
            OtherHDP: null,
            OtherOU: null
          }
        ],
        prematchIdUpdated: [4],
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDP: null,
            OU: null,
            OtherHDP: null,
            OtherOU: null
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDP: 'Esther',
            OU: 'Jack',
            OtherHDP: 'Mira',
            OtherOU: 'Carlos'
          }
        ],
        loading: REQUEST_STATUS.idle,
        prematchIdUpdated: [4],
        leagues: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDP: 'Esther',
            OU: 'Jack',
            OtherHDP: 'Mira',
            OtherOU: 'Carlos'
          }
        ]
      };

      const action = {
        type: pasteCopiedValue.type,
        payload: {
          rowIndex: 0,
          isTakeOver: false,
          copiedValue: [
            {
              value: {
                MainAssignee1: 'Esther',
                MainAssignee2: 'Jack',
                OtherAssignee1: 'Mira',
                OtherAssignee2: 'Carlos'
              }
            }
          ]
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('isExistsIdInPrematchIdUpdated  = null, should handle pasteCopiedValue', () => {
      initialState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDP: null,
            OU: null,
            OtherHDP: null,
            OtherOU: null
          }
        ],
        prematchIdUpdated: [4],
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDP: null,
            OU: null,
            OtherHDP: null,
            OtherOU: null
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDP: 'Esther',
            OU: 'Jack',
            OtherHDP: 'Mira',
            OtherOU: 'Carlos'
          }
        ],
        loading: REQUEST_STATUS.idle,
        prematchIdUpdated: [4],
        leagues: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDP: 'Esther',
            OU: 'Jack',
            OtherHDP: 'Mira',
            OtherOU: 'Carlos'
          }
        ]
      };

      const action = {
        type: pasteCopiedValue.type,
        payload: {
          rowIndex: 0,
          isTakeOver: false,
          copiedValue: [
            {
              value: {
                MainAssignee1: 'Esther',
                MainAssignee2: 'Jack',
                OtherAssignee1: 'Mira',
                OtherAssignee2: 'Carlos'
              }
            }
          ]
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('isTakeOver = true, should handle pasteCopiedValue', () => {
      initialState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDPTakeOver: '',
            OUTakeOver: '',
            OtherHDPTakeOver: '',
            OtherOUTakeOver: ''
          }
        ],
        prematchIdUpdated: [4],
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDPTakeOver: '',
            OUTakeOver: '',
            OtherHDPTakeOver: '',
            OtherOUTakeOver: ''
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDPTakeOver: 'Esther',
            OUTakeOver: 'Jack',
            OtherHDPTakeOver: 'Mira',
            OtherOUTakeOver: 'Carlos'
          }
        ],
        loading: REQUEST_STATUS.idle,
        prematchIdUpdated: [4],
        leagues: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague',
            HDPTakeOver: 'Esther',
            OUTakeOver: 'Jack',
            OtherHDPTakeOver: 'Mira',
            OtherOUTakeOver: 'Carlos'
          }
        ]
      };

      const action = {
        type: pasteCopiedValue.type,
        payload: {
          rowIndex: 0,
          isTakeOver: true,
          copiedValue: [
            {
              value: {
                MainAssignee1: 'Esther',
                MainAssignee2: 'Jack',
                OtherAssignee1: 'Mira',
                OtherAssignee2: 'Carlos'
              }
            }
          ]
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('clearStateDayShift under test', () => {
    it(' should handle clearStateDayShift', () => {
      initialState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague'
          }
        ],
        prematchIdUpdated: [4],
        prematchIdDeleted: [3],
        loading: REQUEST_STATUS.idle,
        leagues: [{ PreMatchLeagueId: 4 }]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague'
          }
        ],
        loading: REQUEST_STATUS.idle,
        prematchIdUpdated: [],
        prematchIdDeleted: [],
        leagues: [
          {
            PreMatchLeagueId: 4
          }
        ],
        sortState: {}
      };

      const action = {
        type: clearStateDayShift.type
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('clearStateNightShift under test', () => {
    it(' should handle clearStateNightShift', () => {
      initialState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague'
          }
        ],
        prematchIdUpdated: [4],
        prematchIdDeleted: [3],
        loading: REQUEST_STATUS.idle,
        leagues: [{ PreMatchLeagueId: 4 }],
        matches: []
      };

      const expectState = {
        entities: [
          {
            LeagueName: 'PremierLeague',
            PreMatchLeagueId: 4
          }
        ],
        loading: REQUEST_STATUS.idle,
        prematchIdUpdated: [],
        prematchIdDeleted: [],
        leagues: [
          {
            PreMatchLeagueId: 4
          }
        ],
        matches: [],
        sortState: {}
      };

      const action = {
        type: clearStateNightShift.type
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });
  describe('restore under test', () => {
    it(' should handle restore', () => {
      initialState = {
        entities: [],
        loading: REQUEST_STATUS.idle
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague'
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: restore.type,
        payload: {
          PreMatchLeagueId: 4,
          LeagueName: 'PremierLeague'
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('clearEntities under test', () => {
    it(' should handle clearEntities', () => {
      initialState = {
        entities: [
          {
            PreMatchLeagueId: 4,
            LeagueName: 'PremierLeague'
          }
        ],
        sortState: {
          LeagueName: 'PremierLeague',
          PreMatchLeagueId: 4
        },
        loading: REQUEST_STATUS.idle
      };

      const expectState = {
        entities: [],
        sortState: {
          LeagueName: 'PremierLeague',
          PreMatchLeagueId: 4,
          length: 0
        },
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: clearEntities.type
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('changeStatePopupLeagueNote under test', () => {
    it(' should handle changeStatePopupLeagueNote', () => {
      initialState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            LeagueName: 'PremierLeague',
            MatchCount: 1
          }
        ],
        loading: REQUEST_STATUS.idle,
        leagues: [{ PreMatchLeagueId: 33, MatchCount: 1 }],
        matches: [{ PreMatchLeagueId: 33, InfoSiteMatchId: 1 }]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            isOpenNote: true
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            isOpenNote: true
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            isOpenNote: false
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: changeStatePopupLeagueNote.type,
        payload: {
          PreMatchLeagueId: 33,
          openState: true
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('changeStatePopupMatchNote under test', () => {
    it(' should handle changeStatePopupMatchNote', () => {
      initialState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            LeagueName: 'PremierLeague',
            MatchCount: 1
          }
        ],
        loading: REQUEST_STATUS.idle,
        leagues: [{ PreMatchLeagueId: 33, MatchCount: 1 }],
        matches: [{ PreMatchLeagueId: 33, InfoSiteMatchId: 1 }]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            isOpenNote: false
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            isOpenNote: false
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            isOpenNote: true
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: changeStatePopupMatchNote.type,
        payload: {
          InfoSiteMatchId: 1,
          openState: true
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('collapseOneLeague under test', () => {
    it(' should handle collapseOneLeague', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: false
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: false
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: collapseOneLeague.type,
        payload: 33
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('expandOneLeague under test', () => {
    it(' should handle expandOneLeague', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: true
          },
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: true
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: expandOneLeague.type,
        payload: 33
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('collapseAllLeague under test', () => {
    it(' should handle collapseAllLeague', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: false
          }
        ],
        isExpandAll: false,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: false
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: collapseAllLeague.type
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('expandAllLeague under test', () => {
    it(' should handle expandAllLeague', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: true
          },
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        isExpandAll: true,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: true
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: expandAllLeague.type
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('showUnreadMessageOnly under test', () => {
    it(' should handle showUnreadMessageOnly', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: true
          }
        ],
        isExpandAll: true,
        isShowUnreadMessage: true,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: true
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: showUnreadMessageOnly.type
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('unShowUnreadMessageOnly under test', () => {
    it(' should handle unShowUnreadMessageOnly', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: false
          }
        ],
        isExpandAll: false,
        isShowUnreadMessage: false,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isExpand: false
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: unShowUnreadMessageOnly.type
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('updatedIconIssueLeagueRow under test', () => {
    it(' should handle updatedIconIssueLeagueRow', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            PreMatchIssueId: 50,
            TicketList: [{ TicketId: 111, IsRead: false }]
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: true,
            MatchCount: 1,
            PreMatchIssueId: 50,
            TicketList: [{ TicketId: 111, IsRead: true }]
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: true,
            MatchCount: 1,
            PreMatchIssueId: 50,
            TicketList: [{ TicketId: 111, IsRead: true }]
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: updatedIconIssueLeagueRow.type,
        payload: { PreMatchLeagueId: 33, TicketId: 111 }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('updatedIconIssueMatchRow under test', () => {
    it(' should handle updatedIconIssueMatchRow', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            PreMatchIssueId: 51,
            IsRead: false,
            TicketList: [{ TicketId: 111, IsRead: false }]
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            PreMatchIssueId: 51,
            IsRead: true,
            TicketList: [{ TicketId: 111, IsRead: true }]
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: updatedIconIssueMatchRow.type,
        payload: { InfoSiteMatchId: 1, TicketId: 111 }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('addIssueIdToLeague under test', () => {
    it(' should handle addIssueIdToLeague', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: false,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: true,
            MatchCount: 1,
            PreMatchIssueId: undefined
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: true,
            MatchCount: 1,
            PreMatchIssueId: undefined
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: addIssueIdToLeague.type,
        payload: {
          newCreateIssueId: true,
          PreMatchLeagueId: 33
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('addIssueIdToMatch under test', () => {
    it(' should handle addIssueIdToMatch', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            IsRead: false
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            HasMessages: true,
            PreMatchIssueId: undefined,
            IsRead: true
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: addIssueIdToMatch.type,
        payload: {
          newCreateIssueId: true,
          InfoSiteMatchId: 1
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('changeRowHightLevelWhenPrepareTyping under test', () => {
    it(' should handle changeRowHightLevelWhenPrepareTyping', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            PreMatchIssueId: 50
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            PreMatchIssueId: 51,
            IsRead: false
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            isInputTyping: true,
            PreMatchIssueId: 50
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            HasMessages: true,
            IsRead: false,
            MatchCount: 1,
            PreMatchIssueId: 50,
            isInputTyping: true
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            PreMatchIssueId: 51,
            IsRead: false
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: changeRowHightLevelWhenPrepareTyping.type,
        payload: {
          stateButtonPlus: true,
          PreMatchLeagueId: 33
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('updateIconNoteToPencil under test', () => {
    it(' should handle updateIconNoteToPencil with league', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: false,
            LeagueId: 40
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: false,
            PreMatchNoteId: null,
            LeagueId: 40
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: false,
            PreMatchNoteId: null,
            LeagueId: 40
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: updateIconNoteToPencil.type,
        payload: {
          key: 'LEAGUE',
          value: 40
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it(' should handle updateIconNoteToPencil with matches', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: false
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: false
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: false
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            PreMatchNoteId: null,
            HasNotes: false,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: updateIconNoteToPencil.type,
        payload: {
          key: 'MATCH',
          value: 1
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('addNoteIdToLeague under test', () => {
    it(' should handle addNoteIdToLeague', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: false,
            LeagueId: 40
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ]
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: true,
            PreMatchNoteId: 234,
            LeagueId: 40
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1,
            HasNotes: true,
            PreMatchNoteId: 234,
            LeagueId: 40
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: addNoteIdToLeague.type,
        payload: {
          newCreatedNoteId: 234,
          LeagueId: 40
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('addNoteIdToMatch under test', () => {
    it(' should handle addNoteIdToMatch', () => {
      initialState = {
        loading: REQUEST_STATUS.idle,
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1
          }
        ],
        isShowUnreadMessage: false
      };

      const expectState = {
        entities: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1
          }
        ],
        leagues: [
          {
            PreMatchLeagueId: 33,
            MatchCount: 1
          }
        ],
        matches: [
          {
            PreMatchLeagueId: 33,
            InfoSiteMatchId: 1,
            PreMatchNoteId: 123,
            HasNotes: true
          }
        ],
        isShowUnreadMessage: false,
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: addNoteIdToMatch.type,
        payload: {
          newCreatedNoteId: 123,
          InfoSiteMatchId: 1
        }
      };

      const state = shiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });
});

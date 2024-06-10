import { REQUEST_STATUS } from '../../../Shared/Constants';
import deleteLeagueSlice, {
  refeshDeletedLeagueTable,
  deleteLeague
} from './deleteLeagueSlice';

jest.mock('../../app/store');

describe('deleteLeagueSlice under test', () => {
  it('should return the initial state', () => {
    expect(deleteLeagueSlice(undefined, {})).toEqual({
      leagueDeleted: [],
      loading: REQUEST_STATUS.idle,
      error: null
    });
  });

  describe('deleteLeague under test', () => {
    it('initialState loading is idle,  should handle deleteLeague.pending', () => {
      const initialState = {
        loading: REQUEST_STATUS.idle
      };

      const expectState = {
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: deleteLeague.pending.type
      };

      const state = deleteLeagueSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is pending,  should handle deleteLeague.pending', () => {
      const initialState = {
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: deleteLeague.pending.type
      };

      const state = deleteLeagueSlice(initialState, action);

      expect(state).toEqual(initialState);
    });

    it('initialState loading is pending,  should handle deleteLeague.fulfilled', () => {
      const initialState = {
        loading: REQUEST_STATUS.pending
      };

      const expectState = {
        loading: REQUEST_STATUS.idle,
        leagueDeleted: [
          {
            PreMatchLeagueId: 1,
            LeagueName: 'C9AN07406YD5SH4AR78YYBTIULXD9DQTZVQ5L08JMBQNU7EHSR'
          },
          {
            PreMatchLeagueId: 2,
            LeagueName: 'K002IUF718Z18YMWUFYXGPVRANOA3PQT7U8WLYEA1HQT2LEPNE'
          },
          {
            PreMatchLeagueId: 3,
            LeagueName: 'FFDLSF3OZZL6BX689BZYJ9WZAGP2MDRC6J5WEEBWSZZX3TIPIJ'
          },
          {
            PreMatchLeagueId: 4,
            LeagueName: '0ACWIJ912J34ACGTK86R79UCHOS4XZE02Y6SXDPUTRW9PBCR6C'
          }
        ]
      };

      const action = {
        type: deleteLeague.fulfilled.type,
        payload: [
          {
            PreMatchLeagueId: 1,
            LeagueName: 'C9AN07406YD5SH4AR78YYBTIULXD9DQTZVQ5L08JMBQNU7EHSR'
          },
          {
            PreMatchLeagueId: 2,
            LeagueName: 'K002IUF718Z18YMWUFYXGPVRANOA3PQT7U8WLYEA1HQT2LEPNE'
          },
          {
            PreMatchLeagueId: 3,
            LeagueName: 'FFDLSF3OZZL6BX689BZYJ9WZAGP2MDRC6J5WEEBWSZZX3TIPIJ'
          },
          {
            PreMatchLeagueId: 4,
            LeagueName: '0ACWIJ912J34ACGTK86R79UCHOS4XZE02Y6SXDPUTRW9PBCR6C'
          }
        ]
      };

      const state = deleteLeagueSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is idle,  should handle deleteLeague.fulfilled', () => {
      const initialState = {
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: deleteLeague.fulfilled.type,
        payload: [
          {
            PreMatchLeagueId: 1,
            LeagueName: 'C9AN07406YD5SH4AR78YYBTIULXD9DQTZVQ5L08JMBQNU7EHSR'
          }
        ]
      };

      const state = deleteLeagueSlice(initialState, action);

      expect(state).toEqual(initialState);
    });

    it('initialState loading is idle, should handle deleteLeague.rejected', () => {
      const initialState = {
        entities: [],
        loading: REQUEST_STATUS.idle,
        error: null
      };

      const action = {
        type: deleteLeague.rejected.type,
        error: 'test'
      };

      const state = deleteLeagueSlice(initialState, action);

      expect(state).toEqual(initialState);
    });

    it('initialState loading is pending, should handle deleteLeague.rejected', () => {
      const initialState = {
        entities: [],
        loading: REQUEST_STATUS.pending,
        error: null
      };

      const expectState = {
        entities: [],
        loading: REQUEST_STATUS.idle,
        error: 'test'
      };

      const action = {
        type: deleteLeague.rejected.type,
        error: 'test'
      };

      const state = deleteLeagueSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });

  describe('refeshDeletedLeagueTable under test', () => {
    it('should handle refeshDeletedLeagueTable reducer (delete by PreMatchLeagueId)', () => {
      const initialState = {
        leagueDeleted: [
          {
            PreMatchLeagueId: 1,
            LeagueName: 'C9AN07406YD5SH4AR78YYBTIULXD9DQTZVQ5L08JMBQNU7EHSR'
          },
          {
            PreMatchLeagueId: 2,
            LeagueName: 'K002IUF718Z18YMWUFYXGPVRANOA3PQT7U8WLYEA1HQT2LEPNE'
          },
          {
            PreMatchLeagueId: 3,
            LeagueName: 'FFDLSF3OZZL6BX689BZYJ9WZAGP2MDRC6J5WEEBWSZZX3TIPIJ'
          },
          {
            PreMatchLeagueId: 4,
            LeagueName: '0ACWIJ912J34ACGTK86R79UCHOS4XZE02Y6SXDPUTRW9PBCR6C'
          }
        ],
        loading: REQUEST_STATUS.idle,
        error: null
      };

      const expectState = {
        leagueDeleted: [
          {
            PreMatchLeagueId: 1,
            LeagueName: 'C9AN07406YD5SH4AR78YYBTIULXD9DQTZVQ5L08JMBQNU7EHSR'
          },
          {
            PreMatchLeagueId: 2,
            LeagueName: 'K002IUF718Z18YMWUFYXGPVRANOA3PQT7U8WLYEA1HQT2LEPNE'
          },
          {
            PreMatchLeagueId: 4,
            LeagueName: '0ACWIJ912J34ACGTK86R79UCHOS4XZE02Y6SXDPUTRW9PBCR6C'
          }
        ],
        loading: REQUEST_STATUS.idle,
        error: null
      };

      const action = {
        type: refeshDeletedLeagueTable.type,
        payload: 3
      };

      const state = deleteLeagueSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });
});

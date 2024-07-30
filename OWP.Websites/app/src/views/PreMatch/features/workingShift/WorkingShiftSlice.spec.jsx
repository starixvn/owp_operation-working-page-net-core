import { REQUEST_STATUS } from '../../../Shared/Constants';
import WorkingShiftSlice, { workingShift } from './WorkingShiftSlice';
jest.mock('../../app/store');
describe('WorkingShiftSlice under test', () => {
  it('should return the initial state', () => {
    expect(WorkingShiftSlice(undefined, {})).toEqual({
      workingShiftData: null,
      loading: REQUEST_STATUS.idle,
      error: null
    });
  });
  describe('workingShift under test', () => {
    it('initialState loading is idle, should handle deleteLeague.pending', () => {
      const initialState = {
        loading: REQUEST_STATUS.idle
      };

      const expectState = {
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: workingShift.pending.type
      };

      const state = WorkingShiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is pending, should handle deleteLeague.pending', () => {
      const initialState = {
        loading: REQUEST_STATUS.pending
      };

      const action = {
        type: workingShift.pending.type
      };

      const state = WorkingShiftSlice(initialState, action);

      expect(state).toEqual(initialState);
    });

    it('initialState loading is pending, should handle workingShift.fulfilled', () => {
      const initialState = {
        loading: REQUEST_STATUS.pending
      };

      const expectState = {
        loading: REQUEST_STATUS.idle,
        workingShiftData: [
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
        type: workingShift.fulfilled.type,
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

      const state = WorkingShiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is pending, should handle workingShift.fulfilled', () => {
      const initialState = {
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: workingShift.fulfilled.type,
        payload: [
          {
            PreMatchLeagueId: 1,
            LeagueName: 'C9AN07406YD5SH4AR78YYBTIULXD9DQTZVQ5L08JMBQNU7EHSR'
          }
        ]
      };

      const state = WorkingShiftSlice(initialState, action);

      expect(state).toEqual(initialState);
    });

    it('initialState loading is pending, should handle workingShift.rejected', () => {
      const initialState = {
        workingShiftData: [],
        loading: REQUEST_STATUS.pending,
        error: null
      };

      const expectState = {
        workingShiftData: [],
        loading: REQUEST_STATUS.idle,
        error: 'test'
      };

      const action = {
        type: workingShift.rejected.type,
        error: 'test'
      };

      const state = WorkingShiftSlice(initialState, action);

      expect(state).toEqual(expectState);
    });

    it('initialState loading is idle, should handle workingShift.rejected', () => {
      const initialState = {
        workingShiftData: [],
        loading: REQUEST_STATUS.idle,
        error: null
      };

      const action = {
        type: workingShift.rejected.type,
        error: 'test'
      };

      const state = WorkingShiftSlice(initialState, action);

      expect(state).toEqual(initialState);
    });
  });
});

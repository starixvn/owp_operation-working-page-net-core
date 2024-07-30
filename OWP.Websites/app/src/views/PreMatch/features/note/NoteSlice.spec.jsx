import { REQUEST_STATUS } from '../../../Shared/Constants';
import noteSlice, {
  clearNewCreatedNoteId,
  clearNote,
  deleteNote,
  getNote,
  saveNote,
  updateNote
} from './NoteSlice';

jest.mock('../../app/store');

describe('noteSlice under test', () => {
  it('should return the initial state', () => {
    expect(noteSlice(undefined, {})).toEqual({
      notes: [],
      newCreatedNoteId: 0,
      loading: REQUEST_STATUS.idle,
      error: null
    });
  });
  describe('getNote under test', () => {
    it('should handle getNote.fulfilled', () => {
      const expectedState = {
        notes: [45678],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: getNote.fulfilled.type,
        payload: [45678]
      };
      const state = noteSlice({}, action);

      expect(state).toEqual(expectedState);
    });
    it('should handle getNote.rejected', () => {
      const initialState = {
        notes: [45678],
        loading: REQUEST_STATUS.idle
      };
      const expectedState = {
        notes: [45678],
        loading: REQUEST_STATUS.idle,
        error: 'test'
      };

      const action = {
        type: getNote.rejected.type,
        payload: [123456],
        error: 'test'
      };
      const state = noteSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
  });

  describe('saveNote under test', () => {
    it('should handle saveNote.fulfilled', () => {
      const initialState = {
        notes: [
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 176,
            Message: 'gtgwwwwwwwwwwwwwwwwwwwwwwwww',
            CreatedDate: '2020-10-08T15:45:32.82+07:00',
            Username: 'starix15'
          }
        ],
        loading: REQUEST_STATUS.pending
      };
      const expectedState = {
        notes: [
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 178,
            Message: 'f3ew4f',
            CreatedDate: '2020-10-08T15:46:43.6+07:00',
            Username: 'sa_nickname'
          },
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 176,
            Message: 'gtgwwwwwwwwwwwwwwwwwwwwwwwww',
            CreatedDate: '2020-10-08T15:45:32.82+07:00',
            Username: 'starix15'
          }
        ],
        newCreatedNoteId: 96,
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: saveNote.fulfilled.type,
        payload: {
          PreMatchNoteId: 96,
          PreMatchNoteMessageId: 178,
          Message: 'f3ew4f',
          CreatedDate: '2020-10-08T15:46:43.6+07:00',
          Username: 'sa_nickname'
        }
      };
      const state = noteSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
    it('should handle saveNote.rejected', () => {
      const initialState = {
        notes: [45678],
        loading: REQUEST_STATUS.idle
      };
      const expectedState = {
        notes: [45678],
        loading: REQUEST_STATUS.idle,
        error: 'test'
      };

      const action = {
        type: saveNote.rejected.type,
        payload: [123456],
        error: 'test'
      };
      const state = noteSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
  });

  describe('deleteNote under test', () => {
    it('should handle deleteNote.fulfilled', () => {
      const initialState = {
        notes: [
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 178,
            Message: 'f3ew4f'
          },
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 176,
            Message: 'gtgwwwwwwwwwwwwwwwwwwwwwwwww'
          }
        ],
        loading: REQUEST_STATUS.pending
      };
      const expectedState = {
        notes: [
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 176,
            Message: 'gtgwwwwwwwwwwwwwwwwwwwwwwwww'
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: deleteNote.fulfilled.type,
        payload: {
          PreMatchNoteId: 96,
          PreMatchNoteMessageId: 178,
          Message: 'f3ew4f'
        }
      };
      const state = noteSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
  });

  describe('updateNote under test', () => {
    it('should handle updateNote.fulfilled', () => {
      const initialState = {
        notes: [
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 178,
            Message: 'f3ew4f'
          },
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 176,
            Message: 'gtgwwwwwwwwwwwwwwwwwwwwwwwww'
          }
        ],
        loading: REQUEST_STATUS.pending
      };
      const expectedState = {
        notes: [
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 178,
            Message: 'f3ew4f'
          },
          {
            PreMatchNoteId: 96,
            PreMatchNoteMessageId: 176,
            Message: 'update message ne'
          }
        ],
        loading: REQUEST_STATUS.idle
      };

      const action = {
        type: updateNote.fulfilled.type,
        payload: {
          PreMatchNoteId: 96,
          PreMatchNoteMessageId: 176,
          Message: 'update message ne'
        }
      };
      const state = noteSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });

    it('should handle updateNote.rejected', () => {
      const initialState = {
        notes: [45678],
        loading: REQUEST_STATUS.idle
      };
      const expectedState = {
        notes: [45678],
        loading: REQUEST_STATUS.idle,
        error: 'test'
      };

      const action = {
        type: updateNote.rejected.type,
        payload: [123456],
        error: 'test'
      };
      const state = noteSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
  });

  describe('reducer under test', () => {
    it('should handle clearNewCreatedNoteId', () => {
      const initialState = {
        newCreatedNoteId: 123
      };

      const expectedState = {
        newCreatedNoteId: 0
      };

      const action = {
        type: clearNewCreatedNoteId.type
      };
      const state = noteSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });

    it('should handle clearNote', () => {
      const initialState = {
        notes: [1111222]
      };

      const expectedState = {
        notes: []
      };

      const action = {
        type: clearNote.type
      };
      const state = noteSlice(initialState, action);

      expect(state).toEqual(expectedState);
    });
  });
});

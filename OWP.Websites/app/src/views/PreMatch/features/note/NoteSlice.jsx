import { createSlice } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { getNote, saveNote, deleteNote, updateNote } from './NoteThunkNative';

const noteSlice = createSlice({
  name: 'note',
  initialState: {
    notes: [],
    newCreatedNoteId: 0,
    loading: REQUEST_STATUS.idle,
    error: null
  },
  reducers: {
    clearNewCreatedNoteId: {
      reducer: state => {
        state.newCreatedNoteId = 0;
      }
    },
    clearNote: {
      reducer: state => {
        state.notes = [];
      }
    }
  },
  extraReducers: builder=>{
    builder.addCase(getNote.fulfilled,(state, action) => {
      state.loading = REQUEST_STATUS.idle;
      state.notes = action.payload;
    })
    .addCase(getNote.rejected,(state, action) => {
      state.loading = REQUEST_STATUS.idle;
      state.error = action.error;
    })
    .addCase(saveNote.fulfilled, (state, action) => {
      state.loading = REQUEST_STATUS.idle;
      state.notes = [action.payload, ...state.notes];
      state.newCreatedNoteId = action.payload.PreMatchNoteId;
    })
    .addCase(saveNote.rejected, (state, action) => {
      state.loading = REQUEST_STATUS.idle;
      state.error = action.error;
    })
    .addCase(deleteNote.fulfilled,(state, action) => {
      state.loading = REQUEST_STATUS.idle;
      const { PreMatchNoteMessageId } = action.payload;
      state.notes = state.notes.filter(
        n => !isEqual(n.PreMatchNoteMessageId, PreMatchNoteMessageId)
      );
    })
    .addCase(updateNote.fulfilled, (state, action) => {
      state.loading = REQUEST_STATUS.idle;
      const {
        PreMatchNoteMessageId,
        Message,
        Username,
        CreatedDate
      } = action.payload;
      const editRow = state.notes.find(n =>
        isEqual(n.PreMatchNoteMessageId, PreMatchNoteMessageId)
      );
      editRow.Message = Message;
      editRow.CreatedDate = CreatedDate;
      editRow.Username = Username;
    })
    .addCase(updateNote.rejected, (state, action) => {
      state.loading = REQUEST_STATUS.idle;
      state.error = action.error;
    });
  }
});

const { actions, reducer } = noteSlice;
const { clearNewCreatedNoteId, clearNote } = actions;

export {
  actions,
  getNote,
  saveNote,
  deleteNote,
  updateNote,
  clearNewCreatedNoteId,
  clearNote
};

export default reducer;

import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../ImportPrematchHelperNative';

const handleNote = name =>
  createAsyncThunk(name, async model => {
    const response = await post(model);
    return response.data;
  });

const getNote = createAsyncThunk('note/getNote', async model => {
  const { PreMatchNoteId } = model;
  if (!PreMatchNoteId) {
    return [];
  }
  const response = await post(model);
  return response.data;
});

const saveNote = handleNote('createNote');

const updateNote = handleNote('updateNote');

const deleteNote = handleNote('deleteNote');

export { getNote, saveNote, updateNote, deleteNote };

import { createAsyncThunk } from '@reduxjs/toolkit';
import { get, post } from '../../../../Shared/Service/Service';
import { TQuickMessage } from '../../../Type';
import { urlDeleteQuickMessage, urlGetQuickMessage, urlSaveQuickMessage } from '../../../const/const';

const queryQuickMessageThunk = createAsyncThunk<TQuickMessage[]>(
  'quickMessage/query',
  async () => {
    const response = await get({ url: urlGetQuickMessage });
    return response.data;
  }
);

const saveQuickMessageThunk = createAsyncThunk<number, TQuickMessage>(
  'quickMessage/save',
  async model => {
    const response = await post({ url: urlSaveQuickMessage, ...model });
    return response.status;
  }
);

const deleteQuickMessageThunk = createAsyncThunk<number, { id: number }>(
  'quickMessage/delete',
  async model => {
    const response = await post({ url: urlDeleteQuickMessage, ...model });
    return response.status;
  }
);

export { queryQuickMessageThunk, saveQuickMessageThunk, deleteQuickMessageThunk };

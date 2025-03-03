import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';

const API_BASE_URL = 'api/ProviderVerification/upload-agent';

const uploadProviderThunk = createAsyncThunk(
  'upload',
  async (model: File) => {
    const formData = new FormData();
    formData.append('uploadFile', model, model.name);
    const response = await axios.post(`${API_BASE_URL}/validate`, formData);

    return response.data;
  }
);

const submitProviderThunk = createAsyncThunk(
  'submit',
  async ({ date, sessionId }: { sessionId: object, date: Date; }) => {
    const response = await axios.post(`${API_BASE_URL}/submit`, { ...sessionId, date });

    return response.data.value;
  });

export { uploadProviderThunk, submitProviderThunk };

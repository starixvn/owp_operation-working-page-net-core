import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { UploadRequest } from '../../type/typeNative';

const uploadProviderThunk = createAsyncThunk(
  'upload',
  async (model: UploadRequest) => {
    const file  = model;
    const formData = new FormData();
    formData.append('uploadFile', file, file.name);

    const url = `/api/provider-uploading/validate`;
    const response = await axios.post(url, formData);

    return response.data;
  }
);

const submitProviderThunk = createAsyncThunk('submit', async (model: object) => {
  const url = `/api/provider-uploading/submit`;
  const response = await axios.post(url, model);

  return response.data;
});

export { uploadProviderThunk, submitProviderThunk };

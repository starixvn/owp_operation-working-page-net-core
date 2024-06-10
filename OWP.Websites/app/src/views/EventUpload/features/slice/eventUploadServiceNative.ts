import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { UploadRequest } from '../../typeNative';
import { buildRequestURL } from '../helper/buildHelper';

const uploadEventThunk = createAsyncThunk(
  'upload',
  async (model: UploadRequest) => {
    const { eventDate, isUploadFMAndAIO, file } = model;
    const formData = new FormData();
    formData.append('uploadFile', file, file.name);

    const url = buildRequestURL(`validate/${eventDate}/${isUploadFMAndAIO}`);
    const response = await axios.post(url, formData);

    return response.data;
  }
);

const submitEventThunk = createAsyncThunk('submit', async (model: object) => {
  const url = buildRequestURL('submit');
  const response = await axios.post(url, model);

  return response.data;
});

export { uploadEventThunk, submitEventThunk };

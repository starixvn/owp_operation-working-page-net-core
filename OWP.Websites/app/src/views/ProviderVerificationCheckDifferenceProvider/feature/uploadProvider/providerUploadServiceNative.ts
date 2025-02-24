import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { UploadRequest } from '../../type/typeNative';
import Utils from '../../../Shared/Utils';
import { get, post } from '../../../Shared/Service/Service';

const uploadProviderThunk = createAsyncThunk(
  'upload',
  async (model: UploadRequest) => {
    const file  = model;
    const formData = new FormData();
    formData.append('uploadFile', file, file.name);

    const url = `api/ProviderVerification/upload-agent/validate`;
    const response = await axios.post(url, formData);

    return response.data;
  }
);

const submitProviderThunk = createAsyncThunk('submit', async (model: object) => {
  const url = `api/ProviderVerification/upload-agent/submit`;
  const response = await axios.post(url, model);

  return response.data;
});

const getUrl = (methodRoute: string) =>
  `/api/ProviderVerification/${methodRoute}`;

const getEvents = createAsyncThunk(
  'provider-verification/get-event',
  async ({
    eventDate,
    onlyShowIsMismatchEvent
  }: {
    eventDate: Date;
    onlyShowIsMismatchEvent: boolean;
  }) => {
    const url = getUrl('get-provider-verification');
    const response = await get({
      url,
      eventDate: Utils.formatDate(eventDate),
      onlyShowIsMismatchEvent
    });
    return response.data;
  }
);

export { uploadProviderThunk, submitProviderThunk, getEvents };

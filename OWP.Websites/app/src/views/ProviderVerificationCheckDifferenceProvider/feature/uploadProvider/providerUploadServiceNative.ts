import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import Utils from '../../../Shared/Utils';
import { get } from '../../../Shared/Service/Service';

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
  async (model: object) => {
    const response = await axios.post(`${API_BASE_URL}/submit`, model);

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

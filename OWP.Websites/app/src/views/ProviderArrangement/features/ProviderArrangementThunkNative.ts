import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { get } from '../../Shared/Service/Service';
import {
  GET_LOG,
  GET_PROVIDER,
  UPDATE_PROVIDER_ARRANGEMENT
} from '../const/ProviderArrangement.json';
import { TQueryLogProviderArrangement } from './TypeNative';
import { baseApiUrl } from '../../Shared/Constants';

const buildRequestURL = (key: string) =>
  `${baseApiUrl}/api/ProviderArrangement/${key}`;

const queryProviderArrangement = createAsyncThunk('queryProvider', async () => {
  const url = buildRequestURL(GET_PROVIDER);
  const response = await axios.get(url);
  return response.data;
});

const updateProviderArrangement = createAsyncThunk(
  'updateProvider',
  async (model: object) => {
    const url = buildRequestURL(UPDATE_PROVIDER_ARRANGEMENT);
    const response = await axios.post(url, model);
    return response.data;
  }
);

const queryLogProviderArrangement = createAsyncThunk('queryLog', async () => {
  const url = buildRequestURL(GET_LOG);
  const response = await get({ url });
  return response.data as TQueryLogProviderArrangement[];
});

export {
  queryProviderArrangement,
  updateProviderArrangement,
  queryLogProviderArrangement
};

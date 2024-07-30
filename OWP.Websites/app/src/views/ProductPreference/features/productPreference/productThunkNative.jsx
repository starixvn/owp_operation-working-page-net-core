import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import {
  buildQueryProductUrl,
} from '../../utils';

const getProductPreference = createAsyncThunk(
  'getProductPreference',
  async (_, { getState }) => {
    const { filter } = getState();
    const { filterInfo } = filter;
    const url = buildQueryProductUrl(filterInfo);
    const response = await axios.get(url);
    return response.data;
  }
);
export { getProductPreference };

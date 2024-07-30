import { createAsyncThunk } from '@reduxjs/toolkit';

import axios from 'axios';
import {
  buildQueryDropdownOptionUrl,
  buildDropdownOptionFromResponse,
  buildLastUpdateUrl
} from '../../utils';

const getDropdownOption = createAsyncThunk('getDropdownOption', async () => {
  const multipleDropdownOptionUrl = buildQueryDropdownOptionUrl();
  const response = await axios.get(multipleDropdownOptionUrl);

  return buildDropdownOptionFromResponse(response);
});

const getLastUpdate = createAsyncThunk('getLastUpdate', async () => {
  const url = buildLastUpdateUrl();
  const response = await axios.get(url);
  return response.data;
});

export { getDropdownOption, getLastUpdate };

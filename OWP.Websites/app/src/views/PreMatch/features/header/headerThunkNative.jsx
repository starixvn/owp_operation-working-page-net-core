import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../ImportPrematchHelperNative';

const getLastUpdate = createAsyncThunk('get/getLastUpdate', async model => {
  const response = await post(model);
  return response.data;
});

export { getLastUpdate };

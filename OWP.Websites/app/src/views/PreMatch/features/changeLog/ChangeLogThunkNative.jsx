import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../ImportPrematchHelperNative';

const changeLog = createAsyncThunk('changeLog/queryChangeLog', async model => {
  const response = await get(model);
  return response.data;
});

export { changeLog };

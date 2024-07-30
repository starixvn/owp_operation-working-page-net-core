import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../ImportPrematchHelperNative';

const workingShiftThunkNative = createAsyncThunk(
  'workingShift',
  async filterWorkingShiftModel => {
    const response = await get(filterWorkingShiftModel);
    return response.data;
  }
);

export default workingShiftThunkNative;

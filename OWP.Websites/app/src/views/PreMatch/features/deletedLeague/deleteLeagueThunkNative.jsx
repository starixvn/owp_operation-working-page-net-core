import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../ImportPrematchHelperNative';

const deleteLeague = createAsyncThunk(
  'delete/deleteLeague',
  async filterDeleteModel => {
    const response = await post(filterDeleteModel);
    return response.data;
  }
);

export { deleteLeague };

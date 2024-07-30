import { createAsyncThunk } from '@reduxjs/toolkit';
import getPermission from './getPermission';

const queryPolicy = (sliceName = 'policy') =>
  createAsyncThunk(`${sliceName}/queryPolicy`, async model => {
    const { functionKey } = model;
    const policy = await getPermission(functionKey);
    return policy;
  });

export { queryPolicy };

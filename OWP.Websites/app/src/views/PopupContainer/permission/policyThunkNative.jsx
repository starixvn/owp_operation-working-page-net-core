import { createAsyncThunk } from '@reduxjs/toolkit';
import getPermission from './getPermission';

const queryPolicy = (sliceName = 'policy', url) =>
  createAsyncThunk(`${sliceName}/queryPolicy`, async model => {
    const { functionKey } = model;
    const policy = await getPermission(functionKey, url);
    return policy;
  });

export { queryPolicy };

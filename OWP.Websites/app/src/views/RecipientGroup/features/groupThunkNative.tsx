import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { buildGetRecipientGroupsURL } from './helper/groupHelper';
import { GroupDataSource } from './Type';

const getRecipientGroup = createAsyncThunk<GroupDataSource[]>(
  'group/GetRecipientGroup',
  async () => {
    const url = buildGetRecipientGroupsURL();
    const response = await axios.get(url);
    return response.data;
  }
);

export { getRecipientGroup };

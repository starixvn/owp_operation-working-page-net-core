import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { rootUrl, lastUpdate } from '../../otherSport.json';
import { baseApiUrl } from '../../../Shared/Constants';

type LastUpdateInfo = {
  date?: string;
  username?: string;
};

const getLastUpdateThunkNative = createAsyncThunk<LastUpdateInfo>(
  'osAssignment/getLastUpdate',
  async () => {
    const url = `${baseApiUrl}/api/OtherSport${lastUpdate}`;

    const response = await axios.get<LastUpdateInfo>(url);
    return response.data;
  }
);

export default getLastUpdateThunkNative;

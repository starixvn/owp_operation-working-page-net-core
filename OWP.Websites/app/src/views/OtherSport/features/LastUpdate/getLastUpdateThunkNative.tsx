import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { rootUrl, lastUpdate } from '../../otherSport.json';

type LastUpdateInfo = {
  date?: string,
  username?: string
};

const netCoreRootUrl = window.netCoreRootUrl;

const getLastUpdateThunkNative = createAsyncThunk<LastUpdateInfo>(
  'osAssignment/getLastUpdate',
  async () => {
    const url = process.env.NODE_ENV === 'development'
    ? `${rootUrl}${lastUpdate}`
    : `${netCoreRootUrl}/api/OtherSport${lastUpdate}`;

    const response = await axios.get<LastUpdateInfo>(url);
    return response.data;
  }
);

export default getLastUpdateThunkNative;

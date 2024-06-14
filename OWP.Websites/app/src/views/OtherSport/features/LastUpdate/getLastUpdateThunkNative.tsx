import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { lastUpdate } from '../../otherSport.json';
import { netCoreURLOtherSport } from '../../const/Constants';

type LastUpdateInfo = {
  date?: string;
  username?: string;
};

const getLastUpdateThunkNative = createAsyncThunk<LastUpdateInfo>(
  'osAssignment/getLastUpdate',
  async () => {
    const url = `${netCoreURLOtherSport}${lastUpdate}`;

    const response = await axios.get<LastUpdateInfo>(url);
    return response.data;
  }
);

export default getLastUpdateThunkNative;

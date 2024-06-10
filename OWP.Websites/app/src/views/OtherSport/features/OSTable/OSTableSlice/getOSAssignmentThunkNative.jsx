import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import OtherSport from '../../../otherSport.json';

const getOSAssignmentThunkNative = createAsyncThunk(
  'OsAssignmentSubmit',
  async model => {
    const url =
      process.env.NODE_ENV === 'development'
        ? `${OtherSport.rootUrl}${OtherSport.OsAssignmentSubmit}`
        : `${window.netCoreRootUrl}/api/OtherSport${OtherSport.OsAssignmentSubmit}`;
    const result = await axios.post(url, model);
    return result.data;
  }
);

export default getOSAssignmentThunkNative;

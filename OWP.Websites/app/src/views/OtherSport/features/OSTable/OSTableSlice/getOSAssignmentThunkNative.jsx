import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import OtherSport from '../../../otherSport.json';
import { baseApiUrl } from '../../../../Shared/Constants';

const getOSAssignmentThunkNative = createAsyncThunk(
  'OsAssignmentSubmit',
  async model => {
    const url = `${baseApiUrl}/api/OtherSport${OtherSport.OsAssignmentSubmit}`;
    const result = await axios.post(url, model);
    return result.data;
  }
);

export default getOSAssignmentThunkNative;

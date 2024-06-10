import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import OtherSport from '../otherSport.json';

const submitThunk = createAsyncThunk('submit', async props => {
  const { ...rest } = props;
  const url =
    process.env.NODE_ENV === 'development'
      ? `${OtherSport.rootUrl}${OtherSport.submit}`
      : `${window.netCoreRootUrl}/api/OtherSport${OtherSport.submit}`;
  const result = await axios.post(url, rest);
  return result.data;
});

export default submitThunk;

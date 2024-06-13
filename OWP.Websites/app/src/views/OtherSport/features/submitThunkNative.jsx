import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import OtherSport from '../otherSport.json';
import { baseApiUrl } from '../../Shared/Constants';

const submitThunk = createAsyncThunk('submit', async props => {
  const { ...rest } = props;
  const url = `${baseApiUrl}/api/OtherSport${OtherSport.submit}`;
  const result = await axios.post(url, rest);
  return result.data;
});

export default submitThunk;

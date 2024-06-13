import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import OtherSport from '../otherSport.json';
import { baseApiUrl } from '../../Shared/Constants';

const validateFileThunk = createAsyncThunk('validation', async props => {
  const { file, fromDate, toDate } = props;
  const formData = new FormData();
  formData.append('uploadFile', file, file.name);
  formData.append('fromDate', fromDate);
  formData.append('toDate', toDate);
  const url = `${baseApiUrl}/api/OtherSport${OtherSport.validation}`;
  const result = await axios.post(url, formData);
  return result.data;
});

export default validateFileThunk;

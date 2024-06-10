import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { getReportByMonthRoute } from '../../../Const.json';
import { buildRequestURL } from '../../helpers/helpers';
import { MonthOvertimeData } from './TypeNative';

const getReportByMonth = createAsyncThunk<MonthOvertimeData, object>(
  'group/getReportByMonth',
  async (model: object) => {
    const url = buildRequestURL(getReportByMonthRoute);
    const response = await axios.post(url, model);
    return response.data;
  }
);

export {
  getReportByMonth
};


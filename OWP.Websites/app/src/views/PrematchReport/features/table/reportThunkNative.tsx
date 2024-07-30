import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { getPrematchReportRoute } from '../../Const.json';
import { FilterRequest } from '../../models/filter';
import { PreMatchReportData } from '../../models/report-data';
import { buildRequestURL } from '../helpers/helpers';

const getReportData = createAsyncThunk(
  'prematch/get-prematch-report',
  async (model: FilterRequest) => {
    const url = buildRequestURL(getPrematchReportRoute);
    const response = await axios.post<PreMatchReportData[]>(url, model);
    return response.data;
  }
);

export { getReportData };

import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { queryReport } from '../../Const.json';
import { FilterRequest } from '../../models/filter';
import { ReportData } from '../../models/report-data';
import { buildRequestURL } from '../../helper/helper';

const getReportData = createAsyncThunk(
  'MatchCountLiveReport/query',
  async (model: FilterRequest) => {
    const url = buildRequestURL(queryReport);
    const response = await axios.get<ReportData[]>(url, {
      params: model
    });
    return response.data;
  }
);

export { getReportData };

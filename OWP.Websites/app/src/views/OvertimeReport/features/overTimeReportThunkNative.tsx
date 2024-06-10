import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import {
  getLocationRoute,
  getReportByYearRoute, locationOptionAll,
  locationOptionNone
} from '../Const.json';
import { buildRequestURL } from './helpers/helpers';
import { OverTimeDataByYear } from './typeNative';

const getLocationOptions = createAsyncThunk(
  'filter/getLocationOptions',
  async () => {
    const url = buildRequestURL(getLocationRoute);
    const response = await axios.get(url);

    return [locationOptionAll, ...response.data, locationOptionNone];
  }
);

const getReportByYear = createAsyncThunk(
  'yearTab/getReportByYear',
  async (model: object) => {
    const url = buildRequestURL(getReportByYearRoute);
    const response = await axios.post(url, model);

    return response.data as OverTimeDataByYear[];
  }
);

export { getLocationOptions, getReportByYear };


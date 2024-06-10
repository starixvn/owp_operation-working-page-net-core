import axios from 'axios';
import { netCoreURL, netFwURL } from './const/const.json';

const exportExcelUrl =
  process.env.NODE_ENV === 'development'
    ? `api-netcore/EventPicking/export-excel`
    : `${window.netCoreRootUrl}/api/EventPicking/export-excel`;

const queryDataExcel = async (url, queryInfo) =>
  await axios.post(url, queryInfo, { responseType: 'blob' });

const exportExcelLSTUrl =
  process.env.NODE_ENV === 'development'
    ? `api-netcore/EventPicking/export-excel-lst-format`
    : `${window.netCoreRootUrl}/api/EventPicking/export-excel-lst-format`;

const buildUrl = methodRoute =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${methodRoute}`
    : `${window.netCoreRootUrl}/${netFwURL}/${methodRoute}`;

export { exportExcelUrl, exportExcelLSTUrl, queryDataExcel, buildUrl };

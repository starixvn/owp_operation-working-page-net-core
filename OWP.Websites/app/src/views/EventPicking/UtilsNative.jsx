import axios from 'axios';
import { netCoreURL } from './const/apiUrl';

const exportExcelUrl = `${netCoreURL}/export-excel`;

const queryDataExcel = async (url, queryInfo) =>
  await axios.post(url, queryInfo, { responseType: 'blob' });

const exportExcelLSTUrl = `${baseApiUrl}/api/EventPicking/export-excel-lst-format`;

const buildUrl = methodRoute => `${netCoreURL}/${methodRoute}`;

export { exportExcelUrl, exportExcelLSTUrl, queryDataExcel, buildUrl };

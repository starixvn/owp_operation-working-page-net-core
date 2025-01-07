import axios from 'axios';
import { netCoreURLEventPicking } from './const/apiUrl';

const exportExcelUrl = `${netCoreURLEventPicking}/export-excel`;

const queryDataExcel = async (url, queryInfo) =>
  await axios.post(url, queryInfo, { responseType: 'blob' });

const buildUrl = methodRoute => `${netCoreURLEventPicking}/${methodRoute}`;

export { exportExcelUrl, queryDataExcel, buildUrl };

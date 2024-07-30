import axios from 'axios';
import { netCoreURLEventPicking } from './const/apiUrl';

const exportExcelUrl = `${netCoreURLEventPicking}/export-excel`;

const queryDataExcel = async (url, queryInfo) =>
  await axios.post(url, queryInfo, { responseType: 'blob' });

const exportExcelLSTUrl = `${netCoreURLEventPicking}/export-excel-lst-format`;

const buildUrl = methodRoute => `${netCoreURLEventPicking}/${methodRoute}`;

export { exportExcelUrl, exportExcelLSTUrl, queryDataExcel, buildUrl };

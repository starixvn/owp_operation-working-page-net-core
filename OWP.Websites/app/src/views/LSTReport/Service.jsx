import { queryServerTime, queryData } from '../Shared/Service/ServiceCommon';

const urlGetServerTime =
  process.env.NODE_ENV === 'development'
    ? `api-netcore/report/get-server-time`
    : `${window.netCoreRootUrl}/api/report/get-server-time`;

const urlGetLstData =
  process.env.NODE_ENV === 'development'
    ? `api-netcore/report/lst`
    : `${window.netCoreRootUrl}/api/report/lst`;

const urlExportLst =
  process.env.NODE_ENV === 'development'
    ? `api-netcore/report/lst/export-excel`
    : `${window.netCoreRootUrl}/api/report/lst/export-excel`;


const query = async model => {
  const data = await queryData(model, urlGetLstData);
  return data;
};
const getServerTime = async () => {
  const time = await queryServerTime(urlGetServerTime);
  return time;
};

const exportUrl = (fromDate, toDate) => {
  const url = `${urlExportLst}?fromDate=${fromDate}&toDate=${toDate}`;
  return url;
};

const service = {
  query,
  getServerTime,
  exportUrl
};
export default service;

import { queryServerTime, queryData } from '../Shared/Service/ServiceCommon';

const urlGetServerTime = `api/report/get-server-time`;

const urlGetLstData = `api/report/lst`;

const urlExportLst = `api/report/lst/export-excel`;

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

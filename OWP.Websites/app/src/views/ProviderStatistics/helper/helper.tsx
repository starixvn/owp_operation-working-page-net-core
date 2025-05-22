
import { dateRangeDefault, dateRangeValid, time } from '../Const.json';

const curr = new Date(Date.now());
const toDateDefault = new Date(curr.setDate(curr.getDate() - 1));
const fromDateDefault = new Date(
  curr.setDate(curr.getDate() - dateRangeDefault)
);

const validDateRange = (fromDate: Date, toDate: Date) => {
  const dateRange = (toDate.getTime() - fromDate.getTime()) / time;
  return dateRange > dateRangeValid;
};

const getKeyValue = <T, K extends keyof T>(obj: T, key: K): T[K] => obj[key];

const buildRequestURL = (key: string) => `/api/ProviderStatistics/${key}`;

export {
  getKeyValue,
  buildRequestURL,
  validDateRange,
  toDateDefault,
  fromDateDefault,
};

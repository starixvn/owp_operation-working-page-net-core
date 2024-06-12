import { netCoreURL, dateRangeDefault, dateRangeValid } from '../../Const.json';

const buildRequestURL = (key: string) => `${netCoreURL}/${key}`;

const curr = new Date(Date.now());
const toDateDefault = new Date(curr.setDate(curr.getDate() - 1));
const fromDateDefault = new Date(
  curr.setDate(curr.getDate() - dateRangeDefault)
);

const isValidTimeRange = (fromDate: Date, toDate: Date): boolean => {
  const timeDifference = toDate.getTime() - fromDate.getTime();
  const oneYearInMilliseconds = dateRangeValid * 24 * 60 * 60 * 1000;

  return timeDifference <= oneYearInMilliseconds;
};

const getKeyValue = <T, K extends keyof T>(obj: T, key: K): T[K] => obj[key];

export {
  buildRequestURL,
  isValidTimeRange,
  toDateDefault,
  fromDateDefault,
  getKeyValue
};

import { ReportData } from '../models/report-data';
import { ColumnOrder } from '../typeNative';
import { orderBy } from 'lodash';
import {
  netCoreURL,
  dateRangeDefault,
  dateRangeValid,
  time
} from '../Const.json';

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

const buildRequestURL = (key: string) => `${netCoreURL}/${key}`;

const sortDataReport = (
  currentData: ReportData[],
  columnOrder: ColumnOrder
) => {
  const emptyData = currentData.filter(
    item => !getKeyValue(item, columnOrder.subColumnId as keyof ReportData)
  );
  const hasData = currentData.filter(
    item => !!getKeyValue(item, columnOrder.subColumnId as keyof ReportData)
  );
  const sortedData = orderBy(
    hasData,
    item => getKeyValue(item, columnOrder.subColumnId as keyof ReportData),
    [columnOrder.orderType]
  );
  return [...sortedData, ...emptyData];
};

type formatType = number | string | null | undefined;

const formatNumber = (source: formatType) =>
  typeof source === 'number' ? new Intl.NumberFormat().format(source) : source;

export {
  getKeyValue,
  sortDataReport,
  buildRequestURL,
  validDateRange,
  toDateDefault,
  fromDateDefault,
  formatNumber
};

import { isEqual, isEmpty } from 'lodash';
import {
  maxbetSettingTable,
  netCoreURL,
  netFwURL,
  matchOfferParam
} from '../Const.json';

const { secondAcceptValue } = maxbetSettingTable;
const SPECIAL_MAXBET = {
  Others: -1000,
  FollowFT: -1
};
const MAXBET = {
  Min: 0,
  Max: 999999,
  Thousand: 1000
};

const isSecondAcceptValue = (columnId: string) =>
  columnId.startsWith('secondAccept');

const isColumnKey = (columnId: string) =>
  isEqual(columnId, maxbetSettingTable.key.id);

const getGroupColumnId = (columnId: string) => {
  if (isSecondAcceptValue(columnId)) {
    return secondAcceptValue.id;
  }
  return columnId;
};

const formatMaxbetValue = (value: number | null) => {
  switch (value) {
    case null:
      return null;
    case SPECIAL_MAXBET.Others:
      return 'Others';
    case SPECIAL_MAXBET.FollowFT:
      return 'Follow FT';
    default:
      return `${(value / MAXBET.Thousand).toLocaleString()}K`;
  }
};

const formatMaxbetValueForLog = (value: string) => {
  if (parseFloat(value) === SPECIAL_MAXBET.FollowFT) {
    return `${value}`;
  } else {
    if (isEmpty(value)) {
      return null;
    } else {
      return `${(parseFloat(value) / MAXBET.Thousand).toLocaleString()}K`;
    }
  }
};

const covertToNumber = (value: number | null) =>
  value === null || value < 0 ? value : Number(value) * MAXBET.Thousand;

const validateMaxbet = (value: number | null) =>
  value === null ||
  (value >= MAXBET.Min && value <= MAXBET.Max) ||
  value === -1;

const buildURL = (urlName: string) =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${urlName}`
    : `${window.netCoreRootUrl}/${netFwURL}/${urlName}`;

const buildMatchOfferURL = () =>
  process.env.NODE_ENV === 'development'
    ? `api-netcore/resource-assignment/${matchOfferParam}`
    : `${window.netCoreRootUrl}/api/resource-assignment/${matchOfferParam}`;

export {
  isSecondAcceptValue,
  isColumnKey,
  getGroupColumnId,
  formatMaxbetValue,
  formatMaxbetValueForLog,
  validateMaxbet,
  covertToNumber,
  buildURL,
  buildMatchOfferURL
};

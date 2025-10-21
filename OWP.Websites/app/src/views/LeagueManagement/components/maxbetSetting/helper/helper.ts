import { isEqual, isEmpty } from 'lodash';
import { maxbetSettingTable, matchOfferParam } from '../Const.json';
import {
  netCoreURLMaxbetSetting,
  netCoreURLResourceAssignment
} from '../../../constants/Const';

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

const SPREAD = {
  Min: 1,
  Max: 100,
  Percentage: 100
};

const isSecondAcceptValue = (columnId: string) =>
  columnId.startsWith('secondAccept');

const isSpreadValue = (columnId: string) => columnId.includes('Spread');

const isFullTimeMaxbetReferColumn = (columnId: string) =>
  isEqual(columnId, maxbetSettingTable.fullTimeMaxbetRefer.id);

const getGroupColumnId = (columnId: string) => {
  if (isSecondAcceptValue(columnId)) {
    return secondAcceptValue.id;
  }

  if (isSpreadValue(columnId)) {
    return 'spread';
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

const formatSpreadValue = (value: number | string | null): string | null => {
  if (value === null || value === '') {
    return null;
  }
  return `${(Number(value) * 100).toLocaleString()}%`;
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

const validateSpread = (value: number | null) =>
  value === null || (value >= SPREAD.Min && value <= SPREAD.Max);

const buildURL = (urlName: string) => `${netCoreURLMaxbetSetting}/${urlName}`;

const buildMatchOfferURL = () =>
  `${netCoreURLResourceAssignment}/${matchOfferParam}`;

export {
  isSecondAcceptValue,
  isFullTimeMaxbetReferColumn,
  getGroupColumnId,
  formatMaxbetValue,
  formatSpreadValue,
  formatMaxbetValueForLog,
  validateMaxbet,
  validateSpread,
  covertToNumber,
  buildURL,
  buildMatchOfferURL,
  isSpreadValue
};

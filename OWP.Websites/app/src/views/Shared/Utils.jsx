import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';
import axios from 'axios';
import { MatchStatuses } from './Constants';
import { post } from './Service/Service';
import isSameOrAfterDayjs from 'dayjs/plugin/isSameOrAfter';
dayjs.extend(utc);
dayjs.extend(isSameOrAfterDayjs);

const getRootUrl = () => {
  let rootUrl = '';
  const baseElement = document.getElementsByTagName('base')[0];
  if (baseElement) {
    rootUrl = baseElement.href;
    if (rootUrl.endsWith('/')) {
      rootUrl = rootUrl.substring(0, rootUrl.length - 1);
    }
  }

  return rootUrl;
};

const resolveUrl = url => `${getRootUrl()}${url}`;

const formatDate = date => dayjs(date).format('MM/DD/YYYY');

const formatDateTime = date => dayjs(date).format('MM/DD/YYYY hh:ss A');

const formatDateTimeString = date => dayjs(date).format('MM/DD/YYYY hh:mm A');

const formatDateTimeDisplay = date => dayjs(date).format('MMM D, YYYY h:mm A');

const formatDateTimeCreateDate = date =>
  dayjs(date).add(12, 'h').format('MM/DD/YYYY hh:mm A');

const formatDateTimeGMT8 = date => {
  const d = dayjs.utc(date).add(8, 'h');
  return d.format('MM/DD/YYYY hh:mm A');
};

const isSameOrAfter = (selectedDate, currentDate) => {
  const selectedDateInDayjs = dayjs(selectedDate);
  const currentDateInDayjs = dayjs(currentDate);
  return selectedDateInDayjs.isSameOrAfter(currentDateInDayjs, 'D');
};

const isYesterdayOrAfterForAssignment = date => {
  const eventDate = dayjs(date).startOf('d');
  const yesterday = dayjs().startOf('d').subtract(1, 'days');
  return eventDate.isSameOrAfter(yesterday);
};

const isYesterdayOrAfterForLiveDetail = (date, time) => {
  let eventDate = dayjs(date).startOf('d');
  const kickOffTime = dayjs(time, 'hh:mm A');
  if (kickOffTime.hour() >= 0 && kickOffTime.hour() < 12) {
    eventDate = eventDate.subtract(1, 'days');
  }

  const yesterday = dayjs().startOf('d').subtract(1, 'days');
  return eventDate.isSameOrAfter(yesterday);
};

const convertJsonToObject = (json, defaultObject = {}) => {
  try {
    return JSON.parse(json);
  } catch (error) {
    return defaultObject;
  }
};

const toLowerCaseForFilter = value => {
  if (value) {
    return value.toString().trim().toLowerCase();
  }
  return '';
};

const convertToThousand = number => {
  if (number) {
    if (number < 1) {
      return number;
    }

    return number / 1000;
  }
  return undefined;
};

const convertThousandToK = (number, needSymbolK) => {
  const convertedNumber = convertToThousand(number);
  if (!convertedNumber) {
    return '';
  }

  return needSymbolK ? `${convertedNumber}K` : convertedNumber;
};

const convertToPercent = number => {
  if (number) {
    const percentNumber = 100;
    const parseFloatToFixed = Number.parseFloat(number * percentNumber).toFixed(
      2
    );
    return Number.parseFloat(parseFloatToFixed);
  }
  return undefined;
};

const convertSpreadToPercent = (number, needSymbolPercent) => {
  const convertedNumber = convertToPercent(number);
  if (!convertedNumber) {
    return '';
  }

  return needSymbolPercent
    ? `${convertedNumber}%`
    : convertedNumber.toLocaleString();
};

const requestOrGetSessionStorage = async (key, url) => {
  const response = await axios.get(url);
  sessionStorage.setItem(key, JSON.stringify(response.data));
  return response.data;
};

const requestOrGetLocalStorage = async props => {
  const { localStoreKey, ...params } = props;

  let data = JSON.parse(localStorage.getItem(localStoreKey));
  if (!data) {
    const response = await post(params);
    data = response.data;
    localStorage.setItem(localStoreKey, JSON.stringify(data));
  }

  return data;
};

const getDataFromLocalStore = localStoreKey =>
  localStorage.getItem(localStoreKey);

const buildMatchStatusOption = matchStatuses =>
  matchStatuses.map(x => ({
    label: x.Value,
    value: x.Id
  }));

const buildSelectedMatchStatus = matchStatusOptions =>
  matchStatusOptions.filter(
    x => x.label === MatchStatuses.Live || x.label === MatchStatuses.InPlay
  );

const buildOffVROption = offVRs => {
  const offVROptions = offVRs.map(x => ({
    label: x.Value,
    value: x.Id
  }));

  return offVROptions.concat({ label: 'None', value: null });
};

const handleOnDateChange = (matchStatusOptions, date) => {
  let selectedMatchStatus = [];
  const today = new Date();
  if (isSameOrAfter(date, today)) {
    selectedMatchStatus = matchStatusOptions.filter(
      x => x.label === MatchStatuses.Live || x.label === MatchStatuses.InPlay
    );
  } else {
    selectedMatchStatus = matchStatusOptions;
  }
  return selectedMatchStatus;
};

const getValueRef = ref => ref?.current?.value.trim();
const getDisableRef = ref => ref?.current?.disabled;
const getValueHrefByReversedIndex = pathIndex => {
  const paths = window.location.href.split('/');
  if (pathIndex < 1 || pathIndex > paths.length) {
    return null;
  }
  const selectedPath = paths[paths.length - pathIndex];
  return selectedPath;
};

const pushOrUpdateObject = (currentState, newFilter) => {
  const { columnKey, keyword } = newFilter;
  currentState[columnKey] = keyword;
  return currentState;
};

const addOrReplaceFilterState = (currentState, newFilter) => {
  const index = currentState.findIndex(
    x => x.columnKey === newFilter.columnKey
  );
  return index > -1
    ? [
        ...currentState.slice(0, index),
        newFilter,
        ...currentState.slice(index + 1)
      ]
    : [...currentState, newFilter];
};

const filterDataTable = (data, filterState, callback) =>
  filterState.reduce((matches, filter) => callback(matches, filter), data);

const formatNumberWithCommas = source =>
  source?.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');

const openNewChattingTab = (actorId, ticketId, eventType) => {
  const top = window.screenY + (window.innerHeight - 700) / 2;
  const left = window.screenX + (window.innerWidth - 500) / 2;
  window.open(
    `SmallChatting?actorId=${actorId}&eventType=${eventType}&ticketId=${ticketId}`,
    `${ticketId}`,
    `toolbar=yes,scrollbars=yes,resizable=yes,top=${top},left=${left},width=515,height=700`
  );
};

const openCurrentChattingTab = (actorId, ticketId, eventType) => {
  window.location.href = `SmallChatting?actorId=${actorId}&eventType=${eventType}&ticketId=${ticketId}`;
};

const openNewSmallTicket = () => {
  const top = window.screenY + (window.innerHeight - 700) / 2;
  const left = window.screenX + (window.innerWidth - 500) / 2;
  window.open(
    `SmallTicket`,
    `_blank`,
    `toolbar=yes,scrollbars=yes,resizable=yes,top=${top},left=${left},width=500,height=700`
  );
};

const backToSmallTicket = () => {
  window.location.href = `SmallTicket`;
};

const formatTime = date => dayjs(date).format('hh:ss A');

const Utils = {
  rootUrl: getRootUrl(),
  resolveUrl,
  formatDate,
  formatDateTime,
  formatDateTimeString,
  formatDateTimeDisplay,
  formatDateTimeCreateDate,
  formatDateTimeGMT8,
  formatTime,
  isSameOrAfter,
  toLowerCaseForFilter,
  convertJsonToObject,
  convertToThousand,
  convertThousandToK,
  convertToPercent,
  convertSpreadToPercent,
  getDataFromLocalStore,
  isYesterdayOrAfterForAssignment,
  requestOrGetSessionStorage,
  isYesterdayOrAfterForLiveDetail,
  buildMatchStatusOption,
  buildOffVROption,
  buildSelectedMatchStatus,
  handleOnDateChange,
  requestOrGetLocalStorage,
  formatNumberWithCommas,
  openNewChattingTab
};
export {
  getValueRef,
  getValueHrefByReversedIndex,
  getDisableRef,
  addOrReplaceFilterState,
  pushOrUpdateObject,
  filterDataTable,
  openNewChattingTab,
  openNewSmallTicket,
  backToSmallTicket,
  openCurrentChattingTab
};
export default Utils;

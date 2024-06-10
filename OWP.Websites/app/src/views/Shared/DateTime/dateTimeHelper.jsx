import dayjs from 'dayjs';
import localizedFormat from 'dayjs/plugin/localizedFormat';
import utc from 'dayjs/plugin/utc';
import timezone from 'dayjs/plugin/timezone';
import customParseFormat from 'dayjs/plugin/customParseFormat';

dayjs.extend(utc);
dayjs.extend(localizedFormat);
dayjs.extend(timezone);
dayjs.extend(customParseFormat);

const buildTimeRangeForFromDate = fromDate => {
  const maxFromDate = new Date(fromDate);
  const minFromDate = new Date(fromDate);
  minFromDate.setDate(maxFromDate.getDate() - 365);
  return {
    minFromDate,
    maxFromDate
  };
};

const buildTimeRangeForToDate = (fromDate, serverTime) => {
  const minToDate = new Date(fromDate);
  const currentDate = new Date(serverTime);
  const maxToDate = new Date(currentDate);
  maxToDate.setDate(currentDate.getDate());
  return {
    minToDate,
    maxToDate
  };
};

const buildFromDateToDateDefaultWithServerTime = serverTime => {
  const currentDate = new Date(serverTime);
  const fromDate = new Date(currentDate);
  const toDate = new Date(currentDate);
  fromDate.setDate(currentDate.getDate() - 30);
  toDate.setDate(currentDate.getDate());
  return {
    fromDate,
    toDate
  };
};

const buildTimeRangeLast30Day = time => {
  const currentDate = new Date(time);
  const fromDate = new Date(currentDate);
  fromDate.setDate(currentDate.getDate() - 30);
  return fromDate;
};

const convertFrom24hTo12h = (time, isDisplaySecond = null) => {
  time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [
    time
  ];

  if (time.length > 1) {
    time = time.slice(1);
    time[5] = +time[0] < 12 ? ' AM' : ' PM';
    const hour = +time[0] % 12;
    time[0] = hour < 10 ? `0${hour}` : hour || 12;
  }
  isDisplaySecond ?? time.splice(3, 1, '');
  return time.join('');
};

const getCurrentTimeGmtPlus7 = () => {
  const dateTimeGmtPlus7String = new Date().toLocaleString('en-US', {
    timeZone: 'Asia/Ho_Chi_Minh'
  });
  const dateTimeGmtPlus7 = new Date(dateTimeGmtPlus7String);
  const time = {
    hour: dateTimeGmtPlus7.getHours(),
    minute: dateTimeGmtPlus7.getMinutes()
  };
  return time;
};

const getCurrentDateGmtPlus8 = () => {
  const dateTimeGmtPlus8String = new Date().toLocaleString('en-US', {
    timeZone: 'Asia/Manila'
  });
  const dateTimeGmtPlus8 = new Date(dateTimeGmtPlus8String);

  return dateTimeGmtPlus8;
};

const buildFilterDate = () => {
  const currentTimeGmtPlus7 = getCurrentTimeGmtPlus7();
  const filterDate = new Date();
  if (currentTimeGmtPlus7.hour < 8) {
    filterDate.setDate(filterDate.getDate() - 1);
  }
  return filterDate;
};

const getHourAndMinutes = time => (time ? time.substring(0, 5) : undefined);

const formatTime = (value, startOf, formatOutput) => {
  if (!value) {
    return null;
  }
  return dayjs(value).startOf(startOf).format(formatOutput);
};

const formatTimeForTicketLGAPopup = (value, startOf, formatOutput) => {
  if (!value) {
    return null;
  }

  return dayjs(value).startOf(startOf).format(formatOutput);
};

const formatTimeOsLastUpdate = dateStr =>
  dayjs.utc(dateStr).format('MMMM DD, YYYY hh:mm A');

const addDays = (date, days) => {
  const result = new Date(date);
  result.setDate(result.getDate() + days);
  return result;
};

const buildDateString = date => {
  const year = date.getFullYear();
  const month = date.getMonth() + 1;
  const day = date.getDate();
  return `${month}-${day}-${year}`;
};

const convertTZ = (date, tzString) =>
  new Date(
    (typeof date === 'string' ? new Date(date) : date).toLocaleString('en-US', {
      timeZone: tzString
    })
  );

const buildDefaultFilterDateProviderVerification = () => {
  const currentDate = getCurrentDateGmtPlus8();
  return currentDate.getHours() < 11
    ? dayjs(currentDate).add(-2, 'days').toDate()
    : dayjs(currentDate).add(-1, 'days').toDate();
};

const buildDefaultFilterDate = () => {
  const currentDate = new Date();
  const gmt8 = convertTZ(currentDate, 'Asia/Hong_Kong');
  currentDate.setDate(currentDate.getDate() - 1);
  return gmt8.getHours() < 11 ? currentDate : new Date();
};

const buildDefaultLGLFilterDate = () => {
  const currentDate = getCurrentDateGmtPlus8();
  return currentDate.getHours() < 11
    ? dayjs(currentDate).add(-1, 'days').toDate()
    : currentDate;
};

const getMMDDYYYYStringFromDateTime = source => dayjs(source).format('L');

const getLocaleDateTime = (source, format = 'L') =>
  dayjs(source).format(format);

const getDayjsFromTime = time => {
  const hourAndMinutes = getHourAndMinutes(time).split(':');
  const { hour, minute } = {
    hour: hourAndMinutes[0],
    minute: hourAndMinutes[1]
  };
  return dayjs(new Date()).hour(hour).minute(minute);
};

export {
  buildTimeRangeForFromDate,
  buildTimeRangeForToDate,
  getCurrentTimeGmtPlus7,
  buildFromDateToDateDefaultWithServerTime,
  buildTimeRangeLast30Day,
  convertFrom24hTo12h,
  getHourAndMinutes,
  buildFilterDate,
  formatTime,
  getCurrentDateGmtPlus8,
  addDays,
  buildDateString,
  buildDefaultFilterDate,
  buildDefaultLGLFilterDate,
  getMMDDYYYYStringFromDateTime,
  getLocaleDateTime,
  getDayjsFromTime,
  formatTimeOsLastUpdate,
  formatTimeForTicketLGAPopup,
  buildDefaultFilterDateProviderVerification
};

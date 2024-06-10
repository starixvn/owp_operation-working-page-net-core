import dayjs from 'dayjs';
import 'dayjs/locale/de';
import localizedFormat from 'dayjs/plugin/localizedFormat';
dayjs.extend(localizedFormat);


const buildFromDateToDateDefault = () => {
  const currentDate = new Date();
  const fromDate = new Date(currentDate);
  const toDate = new Date(currentDate);
  fromDate.setDate(currentDate.getDate() - 31);
  toDate.setDate(currentDate.getDate() - 1);
  return {
    fromDate,
    toDate
  };
};

const getMMDDYYYYStringFromDateTime = source => dayjs(source).format('L');

const DateTimeHelper = {
  buildFromDateToDateDefault,
  getMMDDYYYYStringFromDateTime
};

export default DateTimeHelper;

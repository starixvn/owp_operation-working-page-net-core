/* eslint-disable no-warning-comments */
const initDefaultFilterFromDate = () => {
  const fromYear = new Date().getFullYear() - 1; // NOSONAR
  const fromDate = new Date(fromYear, 0, 1); // NOSONAR

  return fromDate;
};

const initDefaultFilterToDate = () => {
  const currentYear = new Date().getFullYear();
  return new Date(currentYear, 11, 30); // NOSONAR
};

const initDefaultFilterDate = () => ({
  defaultFromDate: initDefaultFilterFromDate(),
  defaultToDate: initDefaultFilterToDate()
});

const getDaysInMonth = (date: Date) =>
  new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();

const getMonthDifference = (startDate: Date, endDate: Date) => {
  const MONTH_NUMBER = 12;
  return (
    endDate.getMonth() -
    startDate.getMonth() +
    MONTH_NUMBER * (endDate.getFullYear() - startDate.getFullYear()) +
    1
  );
};
const isValidTimeRange = (startDate: Date, endDate: Date) => {
  const TWO_YEARS = 24;
  return getMonthDifference(startDate, endDate) > TWO_YEARS;
};

export {
  initDefaultFilterDate,
  getMonthDifference,
  getDaysInMonth,
  isValidTimeRange
};

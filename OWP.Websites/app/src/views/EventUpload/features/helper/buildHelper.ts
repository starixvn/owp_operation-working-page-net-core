import dayjs from 'dayjs';

const netCoreURL = 'api-netcore/event-assignment-uploading';
const netFwURL = 'api/event-assignment-uploading';

const buildRequestURL = (key: string) =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${key}`
    : `${window.netCoreRootUrl}/${netFwURL}/${key}`;

const buildDateString = (date: Date) => dayjs(date).format('MMM DD, YYYY');

export { buildRequestURL, buildDateString };

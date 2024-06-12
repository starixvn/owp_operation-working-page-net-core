import dayjs from 'dayjs';

const netCoreURL = 'api/event-assignment-uploading';

const buildRequestURL = (key: string) => `${netCoreURL}/${key}`;

const buildDateString = (date: Date) => dayjs(date).format('MMM DD, YYYY');

export { buildRequestURL, buildDateString };

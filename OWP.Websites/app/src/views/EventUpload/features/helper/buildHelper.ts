import dayjs from 'dayjs';

const netCoreURLAssignmentUpload = `/api/event-assignment-uploading`;

const buildRequestURL = (key: string) => `${netCoreURLAssignmentUpload}/${key}`;

const buildDateString = (date: Date) => dayjs(date).format('MM/DD/YYYY');

export { buildRequestURL, buildDateString };

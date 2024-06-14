import dayjs from 'dayjs';
import { baseApiUrl } from '../../../Shared/Constants';

const netCoreURLAssignmentUpload = `${baseApiUrl}/api/event-assignment-uploading`;

const buildRequestURL = (key: string) => `${netCoreURLAssignmentUpload}/${key}`;

const buildDateString = (date: Date) => dayjs(date).format('MMM DD, YYYY');

export { buildRequestURL, buildDateString };

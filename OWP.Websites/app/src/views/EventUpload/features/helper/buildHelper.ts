import dayjs from 'dayjs';
import { baseApiUrl } from '../../../Shared/Constants';

const netCoreURL = `${baseApiUrl}/api/event-assignment-uploading`;

const buildRequestURL = (key: string) => `${netCoreURL}/${key}`;

const buildDateString = (date: Date) => dayjs(date).format('MMM DD, YYYY');

export { buildRequestURL, buildDateString };

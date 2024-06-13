import { baseApiUrl } from '../Shared/Constants';

const netCoreURL = `${baseApiUrl}/api/live-detail`;
const buildRequestURL = keyword => `${netCoreURL}/${keyword}`;

export { buildRequestURL };

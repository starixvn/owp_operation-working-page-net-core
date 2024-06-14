import { baseApiUrl } from '../Shared/Constants';

const netCoreURLLiveDetail = `${baseApiUrl}/api/live-detail`;
const buildRequestURL = keyword => `${netCoreURLLiveDetail}/${keyword}`;

export { buildRequestURL };

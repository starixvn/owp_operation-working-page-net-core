const netCoreURLLiveDetail = `/api/live-detail`;
const buildRequestURL = keyword => `${netCoreURLLiveDetail}/${keyword}`;

export { buildRequestURL };

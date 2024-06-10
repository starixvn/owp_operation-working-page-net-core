const netCoreURL = 'api-netcore/live-detail';
const netFwURL = 'api/live-detail';
const buildRequestURL = keyword =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${keyword}`
    : `${window.netCoreRootUrl}/${netFwURL}/${keyword}`;

export { buildRequestURL };

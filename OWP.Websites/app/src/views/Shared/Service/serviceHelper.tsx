const netCoreURL = 'api-netcore';
const netFwURL = 'api';

const buildRequestURL = (baseUrl: string, key: string) =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${baseUrl}/${key}`
    : `${netFwURL}/${baseUrl}/${key}`;

const buildLeagueGroupURL = () =>
  process.env.NODE_ENV === 'development'
    ? 'api-netcore/resource-assignment/?refName=NewLeagueCategory'
    : `api/resource-assignment/?refName=NewLeagueCategory`;

export { buildRequestURL, buildLeagueGroupURL };

const netCoreURL = 'api';

const buildRequestURL = (baseUrl: string, key: string) =>
  `${netCoreURL}/${baseUrl}/${key}`;

const buildLeagueGroupURL = () =>
  `api/resource-assignment/?refName=NewLeagueCategory`;

export { buildRequestURL, buildLeagueGroupURL };

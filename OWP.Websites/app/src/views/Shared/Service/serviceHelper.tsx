import { baseApiUrl } from '../Constants';

const netCoreURL = `${baseApiUrl}/api`;

const buildRequestURL = (baseUrl: string, key: string) =>
  `${netCoreURL}/${baseUrl}/${key}`;

const buildLeagueGroupURL = () =>
  `${baseApiUrl}/api/resource-assignment/?refName=NewLeagueCategory`;

export { buildRequestURL, buildLeagueGroupURL };

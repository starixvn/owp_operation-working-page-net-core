import { netCoreURL, netFwURL } from './const/Const.json';
const buildRequestURL = (key: string) =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${key}`
    : `${netFwURL}/${key}`;

export { buildRequestURL };

import { netCoreURL } from './const/Const.json';
const buildRequestURL = (key: string) => `${netCoreURL}/${key}`;

export { buildRequestURL };

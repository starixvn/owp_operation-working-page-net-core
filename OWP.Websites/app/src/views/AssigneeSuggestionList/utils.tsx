import { baseApiUrl } from '../Shared/Constants';

const netCoreURL = `${baseApiUrl}/api/AssigneeSuggestion`;
const buildRequestURL = (key: string) => `${netCoreURL}/${key}`;

export { buildRequestURL };

import { baseApiUrl } from '../Shared/Constants';

const netCoreURLAssigneeSuggestion = `${baseApiUrl}/api/AssigneeSuggestion`;
const buildRequestURL = (key: string) =>
  `${netCoreURLAssigneeSuggestion}/${key}`;

export { buildRequestURL };

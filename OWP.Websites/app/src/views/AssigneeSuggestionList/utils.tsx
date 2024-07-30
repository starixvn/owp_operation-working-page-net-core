const netCoreURLAssigneeSuggestion = `/api/AssigneeSuggestion`;
const buildRequestURL = (key: string) =>
  `${netCoreURLAssigneeSuggestion}/${key}`;

export { buildRequestURL };

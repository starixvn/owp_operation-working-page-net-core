const netCoreURLMaxbetSetting = `/api/MaxbetSetting`;
const netCoreURLResourceAssignment = `/api/resource-assignment`;
const netCoreURLLeagueManagement = `/api/league-management`;

const matchOfferParam = '?refName=MatchOffer';

const buildRequestURL = key => `${netCoreURLLeagueManagement}/${key}`;

const buildMatchOfferURL = () =>
  `${netCoreURLResourceAssignment}/${matchOfferParam}`;

const message = {
  changeLeagueGroup:
    'Attention! Your change could impact a Sync</br>rule between Adminsite and OWP. Do you</br>want to continue?'
};

export {
  buildRequestURL,
  buildMatchOfferURL,
  message,
  netCoreURLMaxbetSetting,
  netCoreURLResourceAssignment,
  netCoreURLLeagueManagement
};

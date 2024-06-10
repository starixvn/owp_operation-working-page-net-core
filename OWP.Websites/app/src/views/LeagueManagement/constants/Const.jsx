const netCoreURL = 'api-netcore/league-management';
const netFwURL = 'api/league-management';
const matchOfferParam = '?refName=MatchOffer';

const buildRequestURL = key =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${key}`
    : `${netFwURL}/${key}`;

const buildMatchOfferURL = () =>
  process.env.NODE_ENV === 'development'
    ? `api-netcore/resource-assignment/${matchOfferParam}`
    : `api/resource-assignment/${matchOfferParam}`;

const message = {
  changeLeagueGroup:
    'Attention! Your change could impact a Sync</br>rule between Adminsite and OWP. Do you</br>want to continue?'
};

export { buildRequestURL, buildMatchOfferURL, message };

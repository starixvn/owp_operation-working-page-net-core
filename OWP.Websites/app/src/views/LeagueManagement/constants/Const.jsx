import { baseApiUrl } from '../../Shared/Constants';
const netCoreURL = 'api/league-management';
const matchOfferParam = '?refName=MatchOffer';

const buildRequestURL = key => `${netCoreURL}/${key}`;

const buildMatchOfferURL = () =>
  `${baseApiUrl}/api/resource-assignment/${matchOfferParam}`;

const message = {
  changeLeagueGroup:
    'Attention! Your change could impact a Sync</br>rule between Adminsite and OWP. Do you</br>want to continue?'
};

export { buildRequestURL, buildMatchOfferURL, message };

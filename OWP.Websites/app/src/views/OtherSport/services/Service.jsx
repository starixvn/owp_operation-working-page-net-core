import { post, get } from '../../Shared/Service/Service';
import {
  rootUrl,
  updateOfferLive,
  getAdminSiteLeagueId
} from '../otherSport.json';
const saveEditOfferLive = async rest => {
  const url = `${rootUrl}/${updateOfferLive}`;
  const model = {
    ...rest,
    url
  };
  const res = await post(model);
  return res.status;
};

const getLeagueIdFromAdminSite = async adminSiteMatchId => {
  const url = `${rootUrl}/${getAdminSiteLeagueId}`;
  const res = await get({ url, adminSiteMatchId });
  return res.data;
};

export { saveEditOfferLive, getLeagueIdFromAdminSite };

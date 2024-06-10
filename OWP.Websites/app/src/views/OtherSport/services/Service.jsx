import { post, get } from '../../Shared/Service/Service';
import {
  rootUrl,
  updateOfferLive,
  netFw,
  getAdminSiteLeagueId
} from '../otherSport.json';
const saveEditOfferLive = async rest => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `${rootUrl}/${updateOfferLive}`
      : `${window.netCoreRootUrl}/${netFw}/${updateOfferLive}`;
  const model = {
    ...rest,
    url
  };
  const res = await post(model);
  return res.status;
};

const getLeagueIdFromAdminSite = async adminSiteMatchId => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `${rootUrl}/${getAdminSiteLeagueId}`
      : `${window.netCoreRootUrl}/${netFw}/${getAdminSiteLeagueId}`;
  const res = await get({ url, adminSiteMatchId });
  return res.data;
};

export { saveEditOfferLive, getLeagueIdFromAdminSite };

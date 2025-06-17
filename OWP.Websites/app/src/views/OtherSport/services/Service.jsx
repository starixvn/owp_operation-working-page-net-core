import { post, get } from '../../Shared/Service/Service';
import { updateOfferLive, getAdminSiteLeagueId } from '../otherSport.json';
import {
  netCoreURLOtherSport,
  netCoreURLConfiguration
} from '../const/Constants';

const saveEditOfferLive = async rest => {
  const url = `${netCoreURLOtherSport}/${updateOfferLive}`;
  const model = {
    ...rest,
    url
  };
  const res = await post(model);
  return res.status;
};

const getLeagueIdFromAdminSite = async adminSiteMatchId => {
  const url = `${netCoreURLOtherSport}/${getAdminSiteLeagueId}`;
  const res = await get({ url, adminSiteMatchId });
  return res.data;
};

const getTradingLinkInfo = async () => {
  const url = `${netCoreURLConfiguration}/os/get`;
  const res = await get({ url });
  return res.data;
};
export { saveEditOfferLive, getLeagueIdFromAdminSite, getTradingLinkInfo };

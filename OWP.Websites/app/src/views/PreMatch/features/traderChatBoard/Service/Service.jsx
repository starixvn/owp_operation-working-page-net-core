import { URL_QUERY_INFO_MATCH, URL_QUERY_ASSIGNEE } from '../../../const/const';
import { post } from '../../../../Shared/Service/Service';
const GetRecipientPreMatch = async PreMatchLeagueId => {
  const model = {
    id: PreMatchLeagueId,
    url: URL_QUERY_ASSIGNEE
  };
  return await post(model);
};
const GetInfoSiteMatch = async InfoSiteMatchId => {
  const model = {
    id: InfoSiteMatchId,
    url: URL_QUERY_INFO_MATCH
  };
  return await post(model);
};

export default {
  GetRecipientPreMatch,
  GetInfoSiteMatch
};

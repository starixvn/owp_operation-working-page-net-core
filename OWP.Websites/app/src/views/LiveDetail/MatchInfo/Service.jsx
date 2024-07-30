import axios from 'axios';
import { buildRequestURL } from '../buildUrlHelper';
import { buildRequestURLForLGA } from '../../EventAssignmentNew/helper/apiHelper';
const query = async owpMatchId => {
  const response = await axios.get(buildRequestURL('match-info'), {
    params: {
      owpMatchId
    }
  });
  return response.data;
};

const updateMatchInfo = async model => {
  const response = await axios.put(buildRequestURL('match-info'), model);
  return response.data;
};

const updateProviderInfo = async model => {
  const response = await axios.put(
    buildRequestURL('match-info/provider-info'),
    model
  );
  return response.data;
};

const updateTradingInfo = async model => {
  const response = await axios.put(
    buildRequestURL('match-info/trading-info'),
    model
  );
  return response.data;
};

const editEvent = async formModel => {
  const url = buildRequestURLForLGA('edit-event');
  const response = await axios.post(url, formModel);
  return response.data;
};

const validateToEditEvent = async model => {
  const url = buildRequestURLForLGA('validate-to-edit-event');
  const response = await axios.post(url, model);
  return response.data;
};

const validateInfoSiteMatchId = async (matchId, infoSiteMatchId) => {
  const response = await axios.get(
    buildRequestURL('match-info/validate-infositeId'),
    {
      params: {
        matchId,
        infoSiteMatchId: infoSiteMatchId ?? 0
      }
    }
  );
  return response.data;
};

const service = {
  query,
  editEvent,
  updateMatchInfo,
  updateProviderInfo,
  updateTradingInfo,
  validateToEditEvent,
  validateInfoSiteMatchId
};
export default service;

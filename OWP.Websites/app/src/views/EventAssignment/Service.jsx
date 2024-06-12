import axios from 'axios';
import { NET_CORE_QUERY_SIMPLE_API } from '../Shared/Constants';
import { buildRequestURL } from '../LiveDetail/buildUrlHelper';
const queryDataExcel = async url => {
  const response = await axios.get(url, { responseType: 'blob' });
  return response.data;
};

const query = async eventModel => {
  const response = await axios.post('api/event-assignment', eventModel);
  return response.data;
};

const addNew = async formModel => {
  const response = await axios.post(
    'api/event-assignment/add-event',
    formModel
  );
  return response.data;
};

const editEvent = async formModel => {
  const response = await axios.post(
    'api/event-assignment/edit-event',
    formModel
  );
  return response.data;
};

const getBettypeForTraderInfo = async () => {
  const postModel = {
    apiKey: 'live-game-assignment-get-bettype-for-trading-info'
  };
  const response = await axios.post(NET_CORE_QUERY_SIMPLE_API, postModel);
  return response.data;
};

const getInfoSiteMatch = async postModel => {
  const response = await axios.post(
    buildRequestURL('infosite-match/get'),
    postModel
  );
  return response.data;
};

const getBettypeForMatchInfo = async parentMatchId => {
  const postModel = {
    ApiKey: 'live-game-assignment-get-bettype-for-special-event',
    ParentMatchId: parentMatchId
  };
  const response = await axios.post(NET_CORE_QUERY_SIMPLE_API, postModel);
  return response.data;
};

const getSpecialEventTemplate = async (parentMatchId, minorBettypeId) => {
  const postModel = {
    ApiKey: 'live-game-assignment-get-special-event-template',
    ParentMatchId: parentMatchId,
    MinorBettypeId: minorBettypeId
  };
  const response = await axios.post(NET_CORE_QUERY_SIMPLE_API, postModel);
  return response.data[0];
};

const getLastUpdated = async eventDate => {
  const postModel = {
    ApiKey: 'get-last-updated',
    EventDate: eventDate
  };
  const response = await axios.post(NET_CORE_QUERY_SIMPLE_API, postModel);

  return response.data.length === 0 ? null : response.data[0].UploadDateTime;
};

const syncOffVRs = async eventModel => {
  const response = await axios.post(
    'api/event-assignment/sync-off-vr',
    eventModel
  );
  return response.data;
};

const getSyncOffVRLastUpdated = async eventDate => {
  const response = await axios.get(
    `api/event-assignment/get-sync-off-vr-last-updated?date=${eventDate}`
  );

  return response.data;
};

const Service = {
  queryDataExcel,
  query,
  addNew,
  editEvent,
  getBettypeForTraderInfo,
  getInfoSiteMatch,
  getBettypeForMatchInfo,
  getSpecialEventTemplate,
  getLastUpdated,
  syncOffVRs,
  getSyncOffVRLastUpdated
};

export default Service;

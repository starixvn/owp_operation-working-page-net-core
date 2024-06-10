import Utils from '../../Shared/Utils';
import { netCoreURL, netFwURL } from '../const.json';
import axios from 'axios';
import Converter from '../../Shared/Converter';
import { buildQuerySimpleApi } from '../../Shared/Constants';
import { buildRequestURL } from '../../LiveDetail/buildUrlHelper';

const buildRequestURLForLGA = (key: string) =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}${key}`
    : `${netFwURL}${key}`;

const queryMatchInfo = async (owpMatchId: number) => {
  const response = await axios.get(buildRequestURL('match-info'), {
    params: {
      owpMatchId
    }
  });
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
  const response = await axios.post(buildQuerySimpleApi(), postModel);
  return response.data;
};

const getInfoSiteMatch = async postModel => {
  const response = await axios.post(
    buildRequestURLForLGA('infosite-match/get'),
    postModel
  );
  return response.data;
};

const getBettypeForMatchInfo = async (parentMatchId: number) => {
  const postModel = {
    ApiKey: 'live-game-assignment-get-bettype-for-special-event',
    ParentMatchId: parentMatchId
  };
  const response = await axios.post(buildQuerySimpleApi(), postModel);
  return response.data;
};

const getSpecialEventTemplate = async (
  parentMatchId: number,
  minorBettypeId: number
) => {
  const postModel = {
    ApiKey: 'live-game-assignment-get-special-event-template',
    ParentMatchId: parentMatchId,
    MinorBettypeId: minorBettypeId
  };
  const response = await axios.post(buildQuerySimpleApi(), postModel);
  return response.data[0];
};

const getSyncOffVRLastUpdated = async (eventDate: Date) => {
  const response = await axios.get(
    `api/event-assignment/get-sync-off-vr-last-updated?date=${eventDate}`
  );

  return response.data;
};

const getByRefName = (refName: string) => {
  const url =
    process.env.NODE_ENV === 'development'
      ? `api-netcore/resource-assignment`
      : `api/resource-assignment`;

  const key = `all-ref-${refName}`;
  return Utils.requestOrGetSessionStorage(key, `${url}/?refName=${refName}`);
};

const getSuggestionAgentsOptions = async (keyword: string) => {
  if (keyword.length < 2) {
    return null;
  }

  const postModel = {
    ApiKey: 'get-agent-suggestion',
    Keyword: keyword
  };
  const response = await axios.post(buildQuerySimpleApi(), postModel);
  const data = response.data;
  return Converter.convertAgentsToOptions(data);
};

const getSuggestionTVsOptions = async (keyword: string) => {
  if (keyword.length < 2) {
    return null;
  }

  const postModel = {
    ApiKey: 'get-television-suggestion',
    Keyword: keyword
  };
  const response = await axios.post(buildQuerySimpleApi(), postModel);
  const data = response.data;
  return Converter.convertTVsToOptions(data);
};

export {
  buildRequestURLForLGA,
  queryMatchInfo,
  getInfoSiteMatch,
  addNew,
  editEvent,
  getBettypeForTraderInfo,
  getBettypeForMatchInfo,
  getSpecialEventTemplate,
  getSyncOffVRLastUpdated,
  getByRefName,
  getSuggestionAgentsOptions,
  getSuggestionTVsOptions
};

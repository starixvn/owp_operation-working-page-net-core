import axios from 'axios';
import { buildQuerySimpleApi } from '../../Shared/Constants';
import { buildRequestURL } from '../buildUrlHelper';

const crudUrl = buildRequestURL('match-setting');
const getMatchsettingByMatchId = async matchId => {
  const postModel = {
    apiKey: 'live-detail-get-matchsetting-by-id',
    MatchId: matchId
  };
  const response = await axios.post(buildQuerySimpleApi(), postModel);
  return response.data;
};

const createMatchSetting = async createModel => {
  const response = await axios.post(crudUrl, createModel);
  return response.data;
};

const updateMatchSetting = async updateModel => {
  const response = await axios.put(crudUrl, updateModel);
  return response.data;
};

const deleteMatchSetting = async deleteModel => {
  const response = await axios.delete(crudUrl, {
    data: deleteModel
  });
  return response.data;
};

const query = async model => {
  const response = await axios.get(crudUrl, {
    params: model
  });
  return response.data;
};

const service = {
  query,
  getMatchsettingByMatchId,
  createMatchSetting,
  updateMatchSetting,
  deleteMatchSetting
};

export default service;

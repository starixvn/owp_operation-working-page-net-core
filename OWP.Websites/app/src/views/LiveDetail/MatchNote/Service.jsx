import axios from 'axios';
import { buildRequestURL } from '../buildUrlHelper';

const url = buildRequestURL('match-note');

const query = async model => {
  const response = await axios.get(url, {
    params: model
  });
  return response.data;
};

const create = async model => {
  const response = await axios.post(url, model);
  return response.data;
};

const update = async model => {
  const response = await axios.put(url, model);
  return response.data;
};

const _delete = async model => {
  const response = await axios.delete(url, {
    data: model
  });
  return response.data;
};

const service = {
  query,
  create,
  update,
  delete: _delete
};

export default service;

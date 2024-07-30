import axios from 'axios';
import { buildRequestURL } from '../../LiveDetail/buildUrlHelper';
const url = buildRequestURL('assignment');
const update = async model => {
  const response = await axios.put(url, model);
  return response.data;
};

const _delete = async model => {
  const response = await axios.delete(url, { params: model });
  return response.data;
};

const create = async model => {
  const response = await axios.post(url, model);
  return response.data;
};

const validate = async model => {
  const response = await axios.post(
    buildRequestURL('assignment/validate'),
    model
  );
  return response.data;
};

const service = {
  update,
  create,
  delete: _delete,
  validate
};

export default service;

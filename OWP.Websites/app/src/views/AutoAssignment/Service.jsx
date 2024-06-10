import axios from 'axios';

const url =
  process.env.NODE_ENV === 'development'
    ? `api-netcore/auto-assignment`
    : `api/auto-assignment`;

const getRequests = async () => {
  const response = await axios.get(`${url}/get-request`);
  return response.data;
};

const createRequest = async createModel => {
  const response = await axios.post(`${url}/create-request`, createModel);
  return response.data;
};

const checkRequest = async requestId => {
  const response = await axios.post(`${url}/check-request`, { requestId });
  return response.data;
};

const saveRequest = async (requestId, status, data) => {
  const response = await axios.post(`${url}`, { requestId, status, data });
  return response.data;
};

const downloadRequest = async requestId => {
  const response = await axios.post(`${url}/export-excel`, { requestId });
  return response.data;
};

const removeRequest = async requestId => {
  const response = await axios.post(`${url}/remove-request`, { requestId });
  return response.data;
};

const service = {
  url,
  getRequests,
  createRequest,
  checkRequest,
  saveRequest,
  downloadRequest,
  removeRequest
};
export default service;

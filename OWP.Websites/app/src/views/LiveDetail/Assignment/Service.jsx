import axios from 'axios';
import { buildRequestURL } from '../buildUrlHelper';

const query = async model => {
  const response = await axios.get(buildRequestURL('assignment'), {
    params: model
  });
  return response.data;
};
const service = {
  query
};

export default service;

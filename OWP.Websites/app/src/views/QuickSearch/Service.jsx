import axios from 'axios';

const query = async model => {
  const response = await axios.post(`/api/application/quicksearch`, model);
  return response.data;
};
const service = {
  query
};
export default service;

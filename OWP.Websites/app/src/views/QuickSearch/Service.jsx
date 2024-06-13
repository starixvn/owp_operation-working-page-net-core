import axios from 'axios';
import { baseApiUrl } from '../Shared/Constants';

const query = async model => {
  const response = await axios.post(
    `${baseApiUrl}/api/application/quicksearch`,
    model
  );
  return response.data;
};
const service = {
  query
};
export default service;

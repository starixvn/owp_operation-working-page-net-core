import axios from 'axios';
import { baseApiUrl } from '../Shared/Constants';

const url = `${baseApiUrl}/api/notification-setting`;

const getAll = async () => {
  const response = await axios.get(url + '/all');
  return response.data;
};

const getByCurrentUser = async () => {
  const response = await axios.get(url + '/me');
  return response.data;
};

const update = async model => {
  const response = await axios.put(url + '/update', model);
  return response.data;
};
const Service = {
  getAll,
  getByCurrentUser,
  update
};
export default Service;

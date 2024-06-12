import axios from 'axios';
import Utils from '../Shared/Utils';
import { isEqual } from 'lodash';

const url = `api/resource-assignment`;

const getResourceOptions = () => {
  return [
    { value: 'MatchOffer', label: 'Match Offer' },
    { value: 'NoOfHDP', label: 'No Of HDP' }
  ];
};

const getMaxLength = refName => (isEqual(refName, 'NoOfHDP') ? 20 : 200);

const getByRefName = refName => {
  const key = `all-ref-${refName}`;
  return Utils.requestOrGetSessionStorage(key, `${url}/?refName=${refName}`);
};

const create = async model => {
  const response = await axios.post(url, model);
  return response.data;
};

const update = async model => {
  const response = await axios.put(url, model);
  return response.data;
};
const Service = {
  getResourceOptions,
  getMaxLength,
  getByRefName,
  create,
  update
};
export default Service;

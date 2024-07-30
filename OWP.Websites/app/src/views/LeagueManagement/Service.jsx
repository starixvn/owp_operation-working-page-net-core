import axios from 'axios';
import { buildUpdateSimpleApi } from '../Shared/Constants';
import MaxbetConst from './constants/MaxbetConst';

const updateTVList = async data => {
  const response = await axios.post(MaxbetConst.updateTVListUrl, data);
  return response;
};

const addNewLeague = async model => {
  const response = await axios.post(buildUpdateSimpleApi(), model);
  return response;
};

const updateCondition = async model => {
  const response = await axios.post(MaxbetConst.updateConditionUrl, model);
  return response;
};

const Service = {
  updateTVList,
  addNewLeague,
  updateCondition
};
export default Service;

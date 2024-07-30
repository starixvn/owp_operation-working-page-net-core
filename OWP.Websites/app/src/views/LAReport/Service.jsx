import { queryData } from '../Shared/Service/ServiceCommon';

const query = async model => {
  const data = await queryData(model, `/api/lst-report`);
  return data;
};

const service = {
  query
};

export default service;

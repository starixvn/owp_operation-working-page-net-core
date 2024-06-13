import { baseApiUrl } from '../Shared/Constants';
import { queryData } from '../Shared/Service/ServiceCommon';

const query = async model => {
  const data = await queryData(model, `${baseApiUrl}/api/lst-report`);
  return data;
};

const service = {
  query
};

export default service;

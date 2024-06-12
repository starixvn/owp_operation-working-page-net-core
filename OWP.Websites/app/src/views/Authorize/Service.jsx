import axios from 'axios';

const buildUrl = () => 'api/application/authorize';

const authorize = async functionKey => {
  const authorizeModel = {
    FunctionKey: functionKey
  };
  const response = await axios.post(buildUrl(), authorizeModel);
  return response.data;
};
const Service = {
  authorize
};

export default Service;

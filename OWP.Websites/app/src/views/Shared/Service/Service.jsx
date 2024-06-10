import axios from 'axios';

const getWithoutParams = async promise => await axios.all(promise);

const get = async props => {
  const { url, ...rest } = props;
  const response = await axios.get(url, {
    params: {
      ...rest
    }
  });
  return response;
};

const post = async props => {
  const { url, ...rest } = props;
  const response = await axios.post(url, rest);
  return response;
};

export { get, post, getWithoutParams };

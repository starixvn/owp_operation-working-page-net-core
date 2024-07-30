import axios from 'axios';
const getByRefName = async refName => {
  const key = `all-ref-${refName}`;
  const url = `/api/resource-assignment`;

  let data = JSON.parse(sessionStorage.getItem(key));
  if (!data) {
    const response = await axios.get(`${url}/?refName=${refName}`);
    data = response.data;
    sessionStorage.setItem(key, JSON.stringify(data));
  }
  return data;
};

const authorize = async functionKey => {
  const response = await axios.post(`/api/application/authorize`, {
    functionKey
  });
  return response.data;
};

export { getByRefName, authorize };

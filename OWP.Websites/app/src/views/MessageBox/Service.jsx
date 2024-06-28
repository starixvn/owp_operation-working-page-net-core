import axios from 'axios';

const url = `/api/message_management`;
const searchAll = async model => {
  const response = await axios.post(url + '/search', model);

  return response.data;
};

const updateStatus = async messageId => {
  const response = await axios.put(url + '/update_read_status', {
    MessageId: messageId
  });

  return response.data;
};
const service = {
  searchAll,
  updateStatus
};
export default service;

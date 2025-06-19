import { get } from '../Service/Service';

const getServerInfo = async () => {
  const response = await get({
    url: '/api/owp/chat/get-chat-server-info'
  });
  return response.data;
};

const getServerUrl = async () => {
  const serverInfo = await getServerInfo();
  const url = serverInfo.url;

  return url;
};

export { getServerInfo, getServerUrl };

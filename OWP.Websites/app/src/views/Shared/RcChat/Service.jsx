import { get } from '../Service/Service';

const getServerInfo = async () => {
  const response = await get({
    url: '/api/owp/chat/get-chat-server-info'
  });
  return response.data;
};

const getServerUrl = async () => {
  const cachedUrl = localStorage.getItem('chatServerUrl');

  if (cachedUrl) {
    return cachedUrl;
  }

  const serverInfo = await getServerInfo();
  const url = serverInfo.url;

  localStorage.setItem('chatServerUrl', url);

  return url;
};

export { getServerInfo, getServerUrl };

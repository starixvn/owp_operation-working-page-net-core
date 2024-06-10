import axios from 'axios';
const getPermission = async (functionKey, url) => {
  const { data } = await axios.post(url, {
    functionKey
  });

  const policy = {
    Username: data.Username,
    HasRead: data.HasRead,
    HasWrite: data.HasWrite,
    ActorId: data.UserId,
    SiteId: data.SiteId
  };

  return policy;
};

export default getPermission;

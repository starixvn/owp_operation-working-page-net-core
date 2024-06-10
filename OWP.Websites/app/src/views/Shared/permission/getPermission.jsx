import AuthorizeService from '../../Authorize/Service';

const getPermission = async functionKey => {
  const permission = await AuthorizeService.authorize(functionKey);
  const policy = {
    Username: permission.username,
    HasRead: permission.hasRead,
    HasWrite: permission.hasWrite,
    ActorId: permission.userId,
    SiteId: permission.siteId
  };

  return policy;
};

export default getPermission;

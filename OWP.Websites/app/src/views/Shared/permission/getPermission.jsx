import AuthorizeService from '../../Authorize/Service';

const getPermission = async functionKey => {
  const permission = await AuthorizeService.authorize(functionKey);
  const policy = {
    Username: permission.Username,
    HasRead: permission.HasRead,
    HasWrite: permission.HasWrite,
    ActorId: permission.UserId,
    SiteId: permission.SiteId
  };

  return policy;
};

export default getPermission;

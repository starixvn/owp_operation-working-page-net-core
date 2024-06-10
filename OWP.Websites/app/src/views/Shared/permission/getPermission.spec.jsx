import AuthorizeService from '../../Authorize/Service';
import getPermission from './getPermission';

jest.mock('../../Authorize/Service');

describe('get permission', () => {
  const policy = {
    Username: 'carlos',
    HasRead: true,
    HasWrite: true
  };
  beforeEach(() => {
    AuthorizeService.authorize.mockResolvedValue(policy);
  });

  it('should get permission successfully', () => {
    (async () => {
      const permission = await getPermission('Event-checking');
      expect(permission).toEqual(policy);
    })();
  });
});

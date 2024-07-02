import axios from 'axios';
import NotificationService from './Service';

jest.mock('axios');

describe('Notification Serivce test', () => {
  const responseData = { data: 'ahihi' };

  beforeEach(() => {
    axios.get.mockReset();
    axios.put.mockReset();
  });

  it('return data when getAll is called', async () => {
    axios.get.mockResolvedValue(responseData);

    expect.assertions(3);
    const data = await NotificationService.getAll();

    expect(data).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toBeCalledWith('api/notification-setting/all');
  });

  it('return data when getByCurrentUser is called', async () => {
    axios.get.mockResolvedValue(responseData);

    expect.assertions(3);
    const data = await NotificationService.getByCurrentUser();

    expect(data).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toBeCalledWith('api/notification-setting/me');
  });

  it('return data when update is called', async () => {
    axios.put.mockResolvedValue(responseData);
    const model = { id: 99 };

    expect.assertions(3);
    const data = await NotificationService.update(model);

    expect(data).toEqual(responseData.data);
    expect(axios.put).toBeCalledTimes(1);
    expect(axios.put).toBeCalledWith('api/notification-setting/update', model);
  });
});

import axios from 'axios';
import Service from './Service';
import { buildRequestURL } from '../../LiveDetail/buildUrlHelper';
jest.mock('axios');

describe('Assigment Service Tests', () => {
  const responseData = { data: 'data' };
  beforeEach(() => {
    axios.get.mockReset();
    axios.get.mockResolvedValue(responseData);
  });

  it('should return data whene create called', async () => {
    axios.post.mockReset();
    axios.post.mockResolvedValue(responseData);
    const model = {
      UserId: 8
    };
    const data = await Service.create(model);
    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledWith(buildRequestURL('assignment'), model);
  });

  it('should return data whene update called', async () => {
    axios.put.mockReset();
    axios.put.mockResolvedValue(responseData);
    const model = {
      UserId: 8
    };
    const data = await Service.update(model);
    expect(data).toEqual(responseData.data);
    expect(axios.put).toBeCalledWith(buildRequestURL('assignment'), model);
  });

  it('should return data whene delete called', async () => {
    axios.delete.mockReset();
    axios.delete.mockResolvedValue(responseData);
    const model = 123;
    const data = await Service.delete(model);
    expect(data).toEqual(responseData.data);
    expect(axios.delete).toBeCalledWith(buildRequestURL('assignment'), {
      params: model
    });
  });
});

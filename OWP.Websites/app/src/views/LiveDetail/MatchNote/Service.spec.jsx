import axios from 'axios';
import Service from './Service';
import { buildRequestURL } from '../buildUrlHelper';
jest.mock('axios');

describe('Service', () => {
  beforeEach(() => {
    axios.get.mockReset();
    axios.get.mockReset();
  });

  it('should return data when query is called', async () => {
    const responseData = { data: 'ahihi' };
    axios.get.mockReset();
    axios.get.mockResolvedValue(responseData);
    const model = 123;
    const data = await Service.query(model);
    expect(data).toEqual(responseData.data);
    expect(axios.get).toBeCalledWith(buildRequestURL('match-note'), {
      params: 123
    });
  });

  it('should return data when create is called', async () => {
    const responseData = { data: 'ahihi' };
    axios.post.mockClear();
    axios.post.mockResolvedValue(responseData);
    const model = 123;
    const data = await Service.create(model);
    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledWith(buildRequestURL('match-note'), 123);
  });

  it('should return data when update is called', async () => {
    const responseData = { data: 'ahihi' };
    axios.put.mockClear();
    axios.put.mockResolvedValue(responseData);
    const model = 123;
    const data = await Service.update(model);
    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledWith(buildRequestURL('match-note'), 123);
  });

  it('should return data when delete is called', async () => {
    const responseData = { data: 'ahihi' };
    axios.delete.mockClear();
    axios.delete.mockResolvedValue(responseData);
    const model = 123;
    const data = await Service.delete(model);
    expect(data).toEqual(responseData.data);
    expect(axios.delete).toBeCalledWith(buildRequestURL('match-note'), {
      data: 123
    });
  });
});

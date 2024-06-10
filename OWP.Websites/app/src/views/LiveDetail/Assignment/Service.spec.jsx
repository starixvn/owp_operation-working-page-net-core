import axios from 'axios';
import Service from './Service';
import { buildRequestURL } from '../buildUrlHelper';

jest.mock('axios');

describe('Service', () => {
  const responseData = { data: 'ahihi' };
  const model = { matchId: 1 };

  it('return data when query is called', async () => {
    axios.get.mockReset();
    axios.get.mockResolvedValue(responseData);
    const actualData = await Service.query(model);
    expect(actualData).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toHaveBeenCalledWith(buildRequestURL('assignment'), {
      params: model
    });
  });
});

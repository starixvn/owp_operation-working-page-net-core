import axios from 'axios';
import AgentService from './Service';
import { buildRequestURL } from '../buildUrlHelper';
jest.mock('axios');

describe('Agent service test', () => {
  const responseData = { data: 'ahihi' };

  beforeEach(() => {
    axios.get.mockReset();
  });

  it('return data when getAll is called', async () => {
    axios.get.mockResolvedValue(responseData);

    const actualData = await AgentService.getAll();

    expect(actualData).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toHaveBeenCalledWith(buildRequestURL('agent/all'));
  });
});

import axios from 'axios';
import { get, post } from './Service';

jest.mock('axios');

describe('Service', () => {
  const responseData = { data: 'carlos' };

  beforeEach(() => {
    axios.post.mockReset();
  });

  it('return data when get is called', async () => {
    axios.get.mockResolvedValue(responseData);
    const url = 'api/event-picking';
    const props = {
      url,
      date: new Date()
    };

    const response = await get(props);

    expect(responseData).toEqual(response);
    expect(axios.get).toBeCalledTimes(1);
  });

  it('return data when post is called', async () => {
    axios.post.mockResolvedValue(responseData);
    const url = 'api/event-picking';
    const props = {
      url,
      date: new Date()
    };

    const response = await post(props);

    expect(responseData).toEqual(response);
    expect(axios.post).toBeCalledTimes(1);
  });
});

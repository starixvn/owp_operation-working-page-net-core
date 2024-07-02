import axios from 'axios';
import Service from './Service';

jest.mock('axios');

describe('Service', () => {
  const responseData = { data: 'ahihi' };

  beforeEach(() => {
    axios.post.mockReset();
  });

  it('return data when getAll is called', async () => {
    axios.post.mockResolvedValue(responseData);
    const postModel = {
      EventDate: '2018-12-05',
      Username: 'sa',
      MatchStatusIds: [1, 2]
    };

    const data = await Service.query(postModel);

    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('api/event-assignment', postModel);
  });
});

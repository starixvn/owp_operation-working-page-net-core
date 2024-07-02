import axios from 'axios';
import Service from './Service';

jest.mock('axios');

const responseData = { data: 'ahihi' };
const model = { Id: 1, Value: 'value', RefName: 'MatchOffer' };

describe('ResourceManagement service test', () => {
  beforeEach(() => {
    axios.get.mockReset();
    axios.post.mockReset();
    axios.put.mockReset();
  });

  it('return response data when getByRefName is called', async () => {
    const refName = 'MatchOffer';
    axios.get.mockResolvedValue(responseData);

    const actual = await Service.getByRefName(refName);

    expect(actual).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
  });

  it('return response data when create is called', async () => {
    axios.post.mockResolvedValue(responseData);

    const actual = await Service.create(model);
    expect(actual).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
  });

  it('return response data when update is called', async () => {
    axios.put.mockResolvedValue(responseData);

    const actual = await Service.update(model);

    expect(actual).toEqual(responseData.data);
    expect(axios.put).toBeCalledTimes(1);
  });
});

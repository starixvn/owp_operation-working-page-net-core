import axios from 'axios';
import { getByRefName, authorize } from './Service';

jest.mock('axios');
describe('Service', () => {
  const responseData = { data: 'hi' };

  beforeEach(() => {
    axios.post.mockReset();
  });

  it('return response data when getByRefName is called', async () => {
    const refName = 'MatchOffer';
    axios.get.mockResolvedValue(responseData);

    const actual = await getByRefName(refName);

    expect(actual).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
  });

  it('return response data when authorize is called', async () => {
    const functionKey = 'FullTimeView';
    axios.post.mockResolvedValue(responseData);

    const actual = await authorize(functionKey);

    expect(actual).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
  });
});

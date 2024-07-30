import axios from 'axios';
import AuthorizeService from './Service';

jest.mock('axios');
describe('Authorize service test', () => {
  it('authorize return response.data when called', async () => {
    const FunctionKey = '_functionKey';
    const response = {
      data: { a: 'ahihi' }
    };
    axios.post.mockResolvedValue(response);
    const actual = await AuthorizeService.authorize(FunctionKey);

    expect(actual).toEqual(response.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('/api/application/authorize', {
      FunctionKey
    });
  });
});

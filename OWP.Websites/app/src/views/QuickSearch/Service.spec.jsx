import axios from 'axios';
import QuickSearchService from './Service';

jest.mock('axios');

describe('QuickSearch service test', () => {
  const responseData = { data: 'ahihi' };

  beforeEach(() => {
    axios.post.mockReset();
  });

  it('return data when query is called', async () => {
    const model = { id: 33, text: 'abc' };
    axios.post.mockResolvedValue(responseData);

    expect.assertions(3);
    const actualData = await QuickSearchService.query(model);

    expect(actualData).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toHaveBeenCalledWith(
      '/api/application/quicksearch',
      model
    );
  });
});

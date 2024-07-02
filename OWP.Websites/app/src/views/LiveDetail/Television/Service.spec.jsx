import axios from 'axios';
import TelevisionService from './Service';
import { buildRequestURL } from '../buildUrlHelper';

jest.mock('axios');

describe('Television Service test', () => {
  const responseData = { data: 'ahihi' };

  beforeEach(() => {
    axios.get.mockReset();
  });

  it('return data when getAll is called', async () => {
    axios.get.mockResolvedValue(responseData);

    expect.assertions(3);
    const data = await TelevisionService.getAll();

    expect(data).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toBeCalledWith(buildRequestURL('television/all'));
  });
});

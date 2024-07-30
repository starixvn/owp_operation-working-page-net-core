import axios from 'axios';
import { buildRequestURL } from '../buildUrlHelper';
import OtherProviderService from './Service';

jest.mock('axios');

describe('OtherProvider Service test', () => {
  const responseData = { data: 'ahihi' };

  beforeEach(() => {
    axios.get.mockReset();
  });

  it('return data when getAll is called', async () => {
    axios.get.mockResolvedValue(responseData);

    expect.assertions(3);
    const data = await OtherProviderService.getAll();

    expect(data).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toBeCalledWith(buildRequestURL('other-provider/all'));
  });
});

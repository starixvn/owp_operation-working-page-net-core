import axios from 'axios';
import MatchSettingService from './Service';
import { buildRequestURL } from '../buildUrlHelper';

jest.mock('axios');

describe('MatchSetting service test', () => {
  const responseData = { data: 'ahihi' };

  beforeEach(() => {
    axios.get.mockReset();
  });

  it('return data when query is called', async () => {
    const model = { id: 99, spread: 0.22 };
    axios.get.mockResolvedValue(responseData);

    expect.assertions(3);
    const actualData = await MatchSettingService.query(model);

    expect(actualData).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toHaveBeenCalledWith(buildRequestURL('match-setting'), {
      params: model
    });
  });
});

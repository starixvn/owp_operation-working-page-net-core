import axios from 'axios';
import Service from './Service';
import MaxbetConst from './constants/MaxbetConst';
import { buildUpdateSimpleApi } from '../Shared/Constants';

jest.mock('axios');

describe('league-management service test', () => {
  beforeEach(() => {
    axios.post.mockReset();
  });
  it('updateTVList return response when called', async () => {
    const data = {};
    const response = {};
    axios.post.mockResolvedValue(response);

    const actual = await Service.updateTVList(data);

    expect(actual).toEqual(response);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith(MaxbetConst.updateTVListUrl, data);
  });
  it('addNewLeague return response when called', async () => {
    const model = {};
    const response = {};
    axios.post.mockResolvedValue(response);

    const actual = await Service.addNewLeague(model);

    expect(actual).toEqual(response);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith(buildUpdateSimpleApi(), model);
  });
  it('updateCondition return response when called', async () => {
    const model = {};
    const response = {};
    axios.post.mockResolvedValue(response);

    const actual = await Service.updateCondition(model);

    expect(actual).toEqual(response);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith(MaxbetConst.updateConditionUrl, model);
  });
});

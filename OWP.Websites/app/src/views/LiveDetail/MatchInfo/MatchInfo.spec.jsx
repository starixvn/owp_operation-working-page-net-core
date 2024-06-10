import axios from 'axios';
import MatchInfoService from './Service';
import { buildRequestURL } from '../buildUrlHelper';

jest.mock('axios');

const responseData = { data: 'ahihi' };
const model = { matchId: 11, name: 'ahehe' };

describe('Match Info service test', () => {
  beforeEach(() => {
    axios.get.mockReset();
    axios.put.mockReset();
    axios.post.mockReset();
  });

  it('return response data when query is called', async () => {
    const matchId = 89;
    axios.get.mockResolvedValue(responseData);

    expect.assertions(3);
    const actual = await MatchInfoService.query(matchId);

    expect(actual).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toBeCalledWith(buildRequestURL('match-info'), {
      params: { owpMatchId: matchId }
    });
  });

  it('return response data when updateMatchInfo is called', async () => {
    axios.put.mockResolvedValue(responseData);

    expect.assertions(3);
    const actual = await MatchInfoService.updateMatchInfo(model);

    expect(actual).toEqual(responseData.data);
    expect(axios.put).toBeCalledTimes(1);
    expect(axios.put).toBeCalledWith(buildRequestURL('match-info'), model);
  });

  it('return response data when updateProviderInfo is called', async () => {
    axios.put.mockResolvedValue(responseData);

    expect.assertions(3);
    const actual = await MatchInfoService.updateProviderInfo(model);

    expect(actual).toEqual(responseData.data);
    expect(axios.put).toBeCalledTimes(1);
    expect(axios.put).toBeCalledWith(
      buildRequestURL('match-info/provider-info'),
      model
    );
  });

  it('return response data when updateTradingInfo is called', async () => {
    axios.put.mockResolvedValue(responseData);

    expect.assertions(3);
    const actual = await MatchInfoService.updateTradingInfo(model);

    expect(actual).toEqual(responseData.data);
    expect(axios.put).toBeCalledTimes(1);
    expect(axios.put).toBeCalledWith(
      buildRequestURL('match-info/trading-info'),
      model
    );
  });

  it('return response data when editEvent is called', async () => {
    axios.post.mockResolvedValue(responseData);

    expect.assertions(3);
    const actual = await MatchInfoService.editEvent(model);

    expect(actual).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('api/event-assignment/edit-event', model);
  });

  it('return response data when validateToEditEvent  is called', async () => {
    axios.post.mockResolvedValue(responseData);

    expect.assertions(3);
    const actual = await MatchInfoService.validateToEditEvent(model);

    expect(actual).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith(
      'api/event-assignment/validate-to-edit-event',
      model
    );
  });

  it('return response data when validateInfoSiteMatchId is called', async () => {
    axios.get.mockResolvedValue(responseData);
    const actual = await MatchInfoService.validateInfoSiteMatchId(123, 456);

    expect(actual).toEqual(responseData.data);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toBeCalledWith(
      buildRequestURL('match-info/validate-infositeId'),
      {
        params: {
          matchId: 123,
          infoSiteMatchId: 456
        }
      }
    );
  });
});

import axios from 'axios';
import Service from './Service';
import { MajorBetTypes } from '../../Shared/Constants';
import { buildRequestURL } from '../buildUrlHelper';

jest.mock('axios');

describe('Service', () => {
  it('should return data when query is called', async () => {
    axios.get.mockClear();
    const responseData = {
      data: [
        {
          Id: 1,
          Bettype: MajorBetTypes.Handicap,
          HDP: 1,
          TeamChoice: 1,
          Choice: 1,
          Odds: 1
        },
        {
          Id: 1,
          Bettype: MajorBetTypes.Handicap,
          HDP: 1,
          TeamChoice: 1,
          Choice: 2,
          Odds: 2
        },
        {
          Id: 1,
          Bettype: MajorBetTypes.Handicap,
          HDP: 1,
          TeamChoice: 1,
          Choice: 3,
          Odds: 3
        },
        {
          Id: 2,
          Bettype: MajorBetTypes.Handicap,
          HDP: 2,
          TeamChoice: 2,
          Choice: 1,
          Odds: 1
        },
        {
          Id: 3,
          Bettype: MajorBetTypes.OverUnder,
          HDP: 3,
          TeamChoice: 2,
          Choice: 2,
          Odds: 2
        }
      ]
    };

    axios.get.mockResolvedValue(responseData);

    const model = 123;
    const actualResult = await Service.query(model);
    expect(axios.get).toBeCalledWith(buildRequestURL('match-odd'), {
      params: 123
    });
    const expectedResult = [
      {
        Id: 1,
        BetType: MajorBetTypes.Handicap,
        HDP: 1,
        TeamChoice: 1,
        HomeOdd: 1,
        AwayOdd: 2,
        DrawOdd: 3
      },
      {
        Id: 2,
        BetType: MajorBetTypes.Handicap,
        HDP: 2,
        TeamChoice: 2,
        HomeOdd: 1,
        AwayOdd: undefined,
        DrawOdd: undefined
      },
      {
        Id: 3,
        BetType: MajorBetTypes.OverUnder,
        HDP: 3,
        TeamChoice: 2,
        HomeOdd: undefined,
        AwayOdd: 2,
        DrawOdd: undefined
      }
    ];
    expect(actualResult).toEqual(expectedResult);
  });

  it('should return data when create is called', async () => {
    const responseData = { data: 'ahihi' };
    axios.post.mockClear();
    axios.post.mockResolvedValue(responseData);
    const model = 123;
    const data = await Service.create(model);
    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledWith(buildRequestURL('match-odd'), 123);
  });

  it('should return data when update is called', async () => {
    const responseData = { data: 'ahihi' };
    axios.put.mockClear();
    axios.put.mockResolvedValue(responseData);
    const model = 123;
    const data = await Service.update(model);
    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledWith(buildRequestURL('match-odd'), 123);
  });

  it('should return data when delete is called', async () => {
    const responseData = { data: 'ahihi' };
    axios.delete.mockClear();
    axios.delete.mockResolvedValue(responseData);
    const model = 123;
    const data = await Service.delete(model);
    expect(data).toEqual(responseData.data);
    expect(axios.delete).toBeCalledWith(buildRequestURL('match-odd'), {
      data: 123
    });
  });
});

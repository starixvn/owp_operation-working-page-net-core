import axios from 'axios';
import Service from './Service';

jest.mock('axios');

describe('Service', () => {
  const responseData = { data: 'ahihi' };

  beforeEach(() => {
    axios.post.mockReset();
  });

  it('return data when getAll is called', async () => {
    axios.post.mockResolvedValue(responseData);
    const postModel = {
      EventDate: '2018-12-05',
      Username: 'sa',
      MatchStatusIds: [1, 2]
    };

    const data = await Service.query(postModel);

    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('/api/odds-compiler/query', postModel);
  });
  it('return data when insertOdds is called', async () => {
    axios.post.mockResolvedValue(responseData);
    await Service.insertOdds({});

    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('/api/odds-compiler/insert-odds', {});
  });

  it('return data when deleteOdds is called', async () => {
    axios.post.mockResolvedValue(responseData);
    const data = await Service.deleteOdds({});

    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('/api/odds-compiler/delete-odds', {});
  });

  it('return data when updateOdds is called', async () => {
    axios.post.mockResolvedValue(responseData);
    const data = await Service.updateOdds({});

    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('/api/odds-compiler/update-odds', {});
  });

  it('return data when updateMarket is called', async () => {
    axios.post.mockResolvedValue(responseData);
    const data = await Service.updateMarket({});

    expect(data).toEqual(responseData.data);
    expect(axios.post).toBeCalledTimes(1);
    expect(axios.post).toBeCalledWith('/api/odds-compiler/save-market', {});
  });

  it('return data when exportExcel is called', async () => {
    const mockUrl =
      '/api/odds-compiler/export-excel?date=20-2&league=hello&matchStatuses=Live&isExcludeSpecialProducts=true';

    const Url = Service.exportExcel({
      date: '20-2',
      league: 'hello',
      matchStatuses: 'Live',
      isExcludeSpecialProducts: true
    });

    expect(Url).toEqual(mockUrl);
  });
});

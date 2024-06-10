import axios from 'axios';
import Service from './Service';

jest.mock('axios');
const responseData = { data: 'hello' };

describe('QuickSearch service test', () => {
  afterEach(() => {
    jest.clearAllMocks();
    axios.post.mockReset();
  });

  it('return data when query is called', async () => {
    const model = { id: 33, text: 'abc' };
    axios.post.mockResolvedValue(responseData);
    const actualData = await Service.query(model);
    expect(actualData).toEqual(responseData.data);
  });

  it('return data when getServerTime is called', async () => {
    axios.get.mockResolvedValue(responseData);
    const actualData = await Service.getServerTime();
    expect(actualData).toEqual(responseData.data);
  });

  it('return data when exportUrl  is called', async () => {
    const actualData = await Service.exportUrl('2022-06-17', '2022-06-17');
    const urlExportLst =
      process.env.NODE_ENV === 'development'
        ? `api-netcore/report/lst/export-excel`
        : `api/report/lst/export-excel`;
    const expectUrl = `${urlExportLst}?fromDate=2022-06-17&toDate=2022-06-17`;
    expect(actualData).toEqual(expectUrl);
  });
});

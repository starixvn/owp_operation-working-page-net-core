import axios from 'axios';
import {
  getTicketTypeOption,
  getStatusHistory,
  markAllTicketsAsRead
} from './service';

jest.mock('axios');
const mockResponseData = {
  data: [
    { id: 1, name: 'others' },
    { id: 2, name: 'eventChecking' }
  ]
};

describe('service test', () => {
  beforeEach(() => {
    axios.get.mockReset();
  });
  it('should return response data when query is called', async () => {
    axios.get.mockResolvedValue(mockResponseData);

    const actual = await getTicketTypeOption();

    expect(actual).toEqual(mockResponseData.data);
  });

  it('getStatusHistory, should return response data when query is called', async () => {
    axios.get.mockResolvedValue(mockResponseData);

    const actual = await getStatusHistory(3);

    expect(actual).toEqual(mockResponseData.data);
  });

  it('markAllTicketsAsRead, should return response data when query is called', async () => {
    axios.get.mockResolvedValue(mockResponseData);

    const actual = await markAllTicketsAsRead(8);

    expect(actual).toEqual(mockResponseData);
  });
});

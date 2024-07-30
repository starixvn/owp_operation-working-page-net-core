import {
  filterResult,
  getSubTypeByType,
  saveFileToBackEnd
} from './HelperCreateTicket';
import Service from '../Service';
import Axios from 'axios';

jest.mock('../Service');
jest.mock('axios');
const recipient = {
  data: [
    {
      ticketTypeRecipientId: 1,
      recipientId: 1,
      recipientType: 1,
      recipientName: 'sa',
      recipientNickname: 'sa_nick'
    }
  ]
};

const responseDataSubType = {
  data: [
    {
      subTypeId: 11,
      subTypeName: 'Verify League M UPDATE11'
    }
  ]
};
describe('Helper under test', () => {
  it('getSubTypeByType', async () => {
    Service.getSubType.mockResolvedValue(responseDataSubType);
    const result = await getSubTypeByType(1);
    expect(result).toEqual(responseDataSubType.data);
  });
  it('saveFileToBackEnd', async () => {
    const file = new File([new ArrayBuffer(6000000)], 'file.abc');
    Axios.post.mockResolvedValue({ data: 1 });
    const result = await saveFileToBackEnd(file);
    expect(result).toEqual(1);
  });
  it('should return sorted result by league name and event datetime', () => {
    const value = [
      {
        eventInfoViewModel: { leagueName: 'B', eventDateTimeGmt8: '2023-01-01' }
      },
      {
        eventInfoViewModel: { leagueName: 'C', eventDateTimeGmt8: '2022-01-01' }
      },
      {
        eventInfoViewModel: { leagueName: 'A', eventDateTimeGmt8: '2022-12-31' }
      }
    ];

    const expectedResult = [
      { leagueName: 'B', eventDateTimeGmt8: '2023-01-01' },
      { leagueName: 'C', eventDateTimeGmt8: '2022-01-01' },
      { leagueName: 'A', eventDateTimeGmt8: '2022-12-31' }
    ];

    const result = filterResult(value);

    expect(result).toEqual(expectedResult);
  });

  it('should return empty array when input value is empty', () => {
    const value = [];

    const expectedResult = [];

    const result = filterResult(value);

    expect(result).toEqual(expectedResult);
  });
});

import axios from 'axios';
import Service from './Service';

jest.mock('axios');

const responseRecipientByType = {
  data: [
    {
      recipientId: 25334034,
      groupName: 'Group',
      userName: 'CPHSAN',
      nickName: 'CPHSan',
      recipientType: 1
    }
  ]
};
const responseDataEvent = {
  owpLeagueId: 32459,
  leagueName: '*UEFA EUROPA LEAGUE',
  homeId: 0,
  awayId: 0,
  homeName: null,
  awayName: null,
  eventDateTimeGmt8: '2021-10-01T00:45:00',
  isLeague: true
};
const responseDataSubType = {
  subTypeId: 11,
  subTypeName: 'Verify League M UPDATE11'
};
describe('service test', () => {
  beforeEach(() => {
    axios.get.mockReset();
    axios.post.mockReset();
  });

  it('return response data when query recipient is called', async () => {
    axios.post.mockResolvedValue(responseRecipientByType);
    const actual = await Service.recipientBySubType(1);
    expect(actual).toEqual(responseRecipientByType);
  });
  it('return response data when query event is called', async () => {
    axios.get.mockResolvedValue(responseDataEvent);
    const actual = await Service.event('eu');
    expect(actual).toEqual(responseDataEvent);
  });
  it('return response data when query getSubType is called', async () => {
    axios.post.mockResolvedValue(responseDataSubType);
    const actual = await Service.getSubType(1);
    expect(actual).toEqual(responseDataSubType);
  });
});

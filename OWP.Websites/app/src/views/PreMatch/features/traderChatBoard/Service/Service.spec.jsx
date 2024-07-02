import Service from './Service';
import { post } from '../../../../Shared/Service/Service';
jest.mock('../../../../Shared/Service/Service');

const responseGetRecipientPreMatch = {
  recipientId: 25334034,
  groupName: null,
  userName: 'CPHSAN',
  nickName: 'CPHSan',
  recipientType: 1
};
const responseGetInfoSiteMatch = {
  owpLeagueId: 32459,
  leagueName: '*UEFA EUROPA LEAGUE',
  homeId: 0,
  awayId: 0,
  eventDateTimeGmt8: '2021-10-01T00:45:00',
  isLeague: true
};
describe('service test', () => {
  beforeEach(() => {
    post.mockReset();
  });

  it('return response recipient when query is called', async () => {
    post.mockResolvedValue(responseGetRecipientPreMatch);
    const actual = await Service.GetRecipientPreMatch(1);
    expect(actual).toEqual(responseGetRecipientPreMatch);
  });
  it('return response info site match when query is called', async () => {
    post.mockResolvedValue(responseGetInfoSiteMatch);
    const actual = await Service.GetInfoSiteMatch(1);
    expect(actual).toEqual(responseGetInfoSiteMatch);
  });
});

import { executeOrder } from './orderHelper';

describe('orderHelper under test', () => {
  it('should sort column Fantasy ASC', () => {
    const data = [
      {
        UserId: 1,
        Username: 'Sa',
        Nickname: 'Sa',
        Section: 'LA-Section2',
        TotalMatches: 1003,
        LinkLowerThan3: 12,
        LinkX3: 1,
        LinkX6: 50,
        LinkX9Plus: 131,
        ProviderTv: 13,
        Provider: 113,
        TV: 581,
        Reporter: 471,
        CornerPlus: 913,
        TBookingPlus: 741,
        Fantasy: 292221,
        GoalKick: 11,
        OtherProducts: 91,
        FTCorner: 51,
        ESoccer5Mins: 414,
        ESoccer6Mins: 100,
        ESoccer15Mins: 12,
        ESoccer16Mins: 17,
        ESoccer20Mins: 161,
        Others: 31
      },
      {
        UserId: 2,
        Username: 'aa',
        Nickname: 'gg',
        Section: 'LA-Section2',
        TotalMatches: 10,
        LinkLowerThan3: 12,
        LinkX3: 1,
        LinkX6: 50,
        LinkX9Plus: 131,
        ProviderTv: 13,
        Provider: 113,
        TV: 581,
        Reporter: 471,
        CornerPlus: 913,
        TBookingPlus: 741,
        Fantasy: 291,
        GoalKick: 11,
        OtherProducts: 91,
        FTCorner: 51,
        ESoccer5Mins: 414,
        ESoccer6Mins: 100,
        ESoccer15Mins: 12,
        ESoccer16Mins: 17,
        ESoccer20Mins: 161,
        Others: 31
      }
    ];
    const result = executeOrder(data.slice(), 'Fantasy', 'ORDER_ASC');
    expect(result[0]).toEqual(data[1]);
    expect(result[1]).toEqual(data[0]);
  });
  it('should sort column Fantasy Desc', () => {
    const data = [
      {
        UserId: 1,
        Username: 'Sa',
        Nickname: 'Sa',
        Section: 'LA-Section2',
        TotalMatches: 1003,
        LinkLowerThan3: 12,
        LinkX3: 1,
        LinkX6: 50,
        LinkX9Plus: 131,
        ProviderTv: 13,
        Provider: 113,
        TV: 581,
        Reporter: 471,
        CornerPlus: 913,
        TBookingPlus: 741,
        Fantasy: 292221,
        GoalKick: 11,
        OtherProducts: 91,
        FTCorner: 51,
        ESoccer5Mins: 414,
        ESoccer6Mins: 100,
        ESoccer15Mins: 12,
        ESoccer16Mins: 17,
        ESoccer20Mins: 161,
        Others: 31
      },
      {
        UserId: 2,
        Username: 'aa',
        Nickname: 'gg',
        Section: 'LA-Section2',
        TotalMatches: 10,
        LinkLowerThan3: 12,
        LinkX3: 1,
        LinkX6: 50,
        LinkX9Plus: 131,
        ProviderTv: 13,
        Provider: 113,
        TV: 581,
        Reporter: 471,
        CornerPlus: 913,
        TBookingPlus: 741,
        Fantasy: 291,
        GoalKick: 11,
        OtherProducts: 91,
        FTCorner: 51,
        ESoccer5Mins: 414,
        ESoccer6Mins: 100,
        ESoccer15Mins: 12,
        ESoccer16Mins: 17,
        ESoccer20Mins: 161,
        Others: 31
      }
    ];
    const result = executeOrder(data.slice(), 'Fantasy', 'kkkk');
    expect(result[1]).toEqual(data[1]);
    expect(result[0]).toEqual(data[0]);
  });
});

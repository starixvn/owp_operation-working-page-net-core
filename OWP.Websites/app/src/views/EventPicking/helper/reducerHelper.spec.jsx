import { updateNoOfferMatch } from './reducerHelper';

describe('reducerHelper under test', () => {
  it('updateNoOfferMatch update isNoOffer', () => {
    const actual = [
      {
        infoSiteMatchId: 123,
        adminSiteMatchId: 456,
        isNoOffer: false
      }
    ];

    const expectedState = [
      {
        infoSiteMatchId: 123,
        adminSiteMatchId: 456,
        isNoOffer: true
      }
    ];

    const noOfferMatch = [
      {
        infoSiteMatchId: 123,
        adminSiteMatchId: 456,
        isNoOffer: true
      }
    ];

    updateNoOfferMatch(actual, noOfferMatch);

    expect(actual).toEqual(expectedState);
  });
});

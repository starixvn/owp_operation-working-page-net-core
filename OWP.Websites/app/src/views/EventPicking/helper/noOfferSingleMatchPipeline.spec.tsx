import {
  TNoOfferMainMatch,
  TNoOfferChildMatch
} from '../types/noOfferMatchType';
import {
  buildNoOfferChildMatchInfo,
  buildNoOfferMainMatchInfo
} from './noOfferSingleMatchPipeline';

describe('noOfferSingleMatchPipeline', () => {
  it('buildNoOfferMainMatchInfo', () => {
    const props: TNoOfferMainMatch = {
      dataSource: [
        {
          infoSiteMatchId: 123,
          childMatchId: 111
        }
      ],
      source: {
        infoSiteMatchId: 123,
        childMatchId: 111,
        isNoOffer: false
      },
      matchNoOffer: []
    };

    const result = buildNoOfferMainMatchInfo(props);

    const expected = [
      { childMatchId: 111, infoSiteMatchId: 123, isNoOffer: false }
    ];
    expect(result).toEqual(expected);
  });

  it('buildNoOfferChildMatchInfo', () => {
    const props: TNoOfferChildMatch = {
      source: {
        infoSiteMatchId: 123,
        childMatchId: 111,
        isNoOffer: false
      },
      matchNoOffer: []
    };

    const result = buildNoOfferChildMatchInfo(props);
    const expected = [
      { childMatchId: null, infoSiteMatchId: 123, isNoOffer: false },
      { childMatchId: 111, infoSiteMatchId: 123, isNoOffer: false }
    ];
    expect(result).toEqual(expected);
  });
});

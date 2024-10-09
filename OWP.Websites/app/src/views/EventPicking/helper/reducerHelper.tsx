import { TConfirmed } from '../types/confirmed';
import TDataTable, { TMatchOfferInfo } from '../types/dataTableType';
import { TNoOfferMatch } from '../types/noOfferMatchType';

const updateNoOfferMatch = (
  entities: TMatchOfferInfo[],
  noOfferMatch: TNoOfferMatch[]
) => {
  entities.forEach(element => {
    const noOfferMatchTarget = noOfferMatch.find(
      x => x.infoSiteMatchId === element.infoSiteMatchId
    );

    if (noOfferMatchTarget) {
      element.isNoOffer = noOfferMatchTarget.isNoOffer;
    }
  });
};

const updateConfirmedMatch = (
  entities: TDataTable[],
  confirmInfo: TConfirmed
) => {
  entities.forEach(element => {
    if (
      confirmInfo.infoSiteMatchIds.some(id => id === element.infoSiteMatchId)
    ) {
      element.isCheck = confirmInfo.confirmed;
    }
  });
};

export { updateConfirmedMatch, updateNoOfferMatch };

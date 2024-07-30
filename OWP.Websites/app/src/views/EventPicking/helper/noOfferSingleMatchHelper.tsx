import { TNoOfferChildMatch, TNoOfferMainMatch } from "../types/noOfferMatchType";

const buildNoOfferMainMatchInfoStepOne = (props: TNoOfferMainMatch) => {
  const { source, matchNoOffer } = props;
  const {
    infoSiteMatchId,
    childMatchId,
    isNoOffer
  } = source;

  !isNoOffer && matchNoOffer.push({
    infoSiteMatchId,
    isNoOffer: false,
    childMatchId
  });
  return props;
};

const buildNoOfferMainMatchInfoStepTwo = (
  props: TNoOfferMainMatch) => {
  const { dataSource, source, matchNoOffer } = props;
  const {
    infoSiteMatchId,
    isNoOffer
  } = source;

  isNoOffer && dataSource
    .filter(element => element.infoSiteMatchId === infoSiteMatchId)
    .forEach(element => {
      matchNoOffer.push({
        isNoOffer: true,
        infoSiteMatchId: element.infoSiteMatchId,
        childMatchId: element.childMatchId
      });
    });
  return props;
};

const buildNoOfferChildMatchInfoStepOne = (props: TNoOfferChildMatch) => {
  const { source, matchNoOffer } = props;
  const {
    infoSiteMatchId,
    childMatchId,
    isNoOffer
  } = source;

  if (!isNoOffer) {
    matchNoOffer.push({
      infoSiteMatchId,
      isNoOffer: false,
      childMatchId: null
    });
    matchNoOffer.push({
      infoSiteMatchId,
      isNoOffer: false,
      childMatchId
    });
  }
  return props;
};


const buildNoOfferChildMatchInfoStepTwo = (props: TNoOfferChildMatch) => {
  const { source, matchNoOffer } = props;
  const {
    infoSiteMatchId,
    childMatchId,
    isNoOffer
  } = source;

  isNoOffer &&
    matchNoOffer.push({
      infoSiteMatchId,
      isNoOffer: true,
      childMatchId
    });
  return props;
};

export {
  buildNoOfferMainMatchInfoStepOne,
  buildNoOfferMainMatchInfoStepTwo,
  buildNoOfferChildMatchInfoStepOne,
  buildNoOfferChildMatchInfoStepTwo
};



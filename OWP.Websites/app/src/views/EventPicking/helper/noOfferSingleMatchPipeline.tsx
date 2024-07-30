import { TNoOfferChildMatch, TNoOfferMainMatch } from '../types/noOfferMatchType';
import {
  buildNoOfferMainMatchInfoStepOne,
  buildNoOfferMainMatchInfoStepTwo,
  buildNoOfferChildMatchInfoStepOne,
  buildNoOfferChildMatchInfoStepTwo
} from './noOfferSingleMatchHelper';

const pipelineBuildNoOfferMatch = (...fns: Array<Function>) => (props: TNoOfferChildMatch | TNoOfferMainMatch) => fns.reduce((y, func) => func(y), props);

const buildNoOfferMainMatchInfoPipe = pipelineBuildNoOfferMatch(
  buildNoOfferMainMatchInfoStepOne,
  buildNoOfferMainMatchInfoStepTwo
);

const buildNoOfferChildMatchInfoPipe = pipelineBuildNoOfferMatch(
  buildNoOfferChildMatchInfoStepOne,
  buildNoOfferChildMatchInfoStepTwo
);

const buildNoOfferMainMatchInfo = (props: TNoOfferMainMatch) => {
  const { matchNoOffer } = buildNoOfferMainMatchInfoPipe(props);
  return matchNoOffer;
};

const buildNoOfferChildMatchInfo = (props: TNoOfferChildMatch) => {
  const { matchNoOffer } = buildNoOfferChildMatchInfoPipe(props);
  return matchNoOffer;
};

export {
  buildNoOfferChildMatchInfo,
  buildNoOfferMainMatchInfo
};
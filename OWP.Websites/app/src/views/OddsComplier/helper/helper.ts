import { MajorBetTypes } from '../../Shared/Constants';
import ToastrWrapper from '../../components/ToastrWrapper';

const STEP = 0.25;
const MIN_SPREAD_HDP = 0.08;
const MAX_SPREAD_HDP = 0.3;
const NUMBER_TWO = 2;
const ONE_HUNDRED = 100;

const roundNumber = (num: number) =>
  Math.round((num + Number.EPSILON) * ONE_HUNDRED) / ONE_HUNDRED;

const calculateOdds = (
  matchSettings: { Bettype?: number; Spread?: number }[],
  oddHome: number,
  spread: number
) => {
  const setting = matchSettings.find(x => x.Bettype === spread);
  const spreadValue = setting?.Spread ?? 0;

  if (spreadValue < MIN_SPREAD_HDP || spreadValue > MAX_SPREAD_HDP) {
    return null;
  }
  if (oddHome === null) {
    return null;
  }

  return roundNumber(calculateStep2(oddHome, spreadValue));
};

const calculateStep2 = (oddHome: number, spread: number) => {
  const tmpOdds = (oddHome + spread) * -1;
  if (tmpOdds > 1) {
    return (NUMBER_TWO - tmpOdds) * -1;
  } else if (tmpOdds < -1) {
    return NUMBER_TWO + tmpOdds;
  } else if (tmpOdds === -1) {
    return 1;
  } else {
    return tmpOdds;
  }
};

const calculateAwayOdds = (matchSettings: [], oddHome: number) => {
  return calculateOdds(matchSettings, oddHome, 1);
};

const calculateOverOdds = (matchSettings: [], oddUnder: number) => {
  return calculateOdds(matchSettings, oddUnder, MajorBetTypes.OverUnder);
};

const validateGoal = (condition: boolean, errorMessage: string) => {
  if (!condition) {
    ToastrWrapper.showError(errorMessage);
    return false;
  }
  return true;
};

const validateHDPGoal = (value: number) => {
  const isValidHDP = !value || (value <= 0 && value % STEP === 0);

  return validateGoal(
    isValidHDP,
    'Please input numbers below 0 that are multiples of 0.25.<br>E.g. 0, -0.25, -0.5, -1, ...'
  );
};

const validateOUGoal = (value: number) => {
  const isValidOU = !value || (value >= 0 && value % STEP === 0);

  return validateGoal(
    isValidOU,
    'Please input numbers above 0 that are multiples of 0.25.<br>E.g. 0, 0.25, 0.5, 1, ...'
  );
};

export {
  validateHDPGoal,
  validateOUGoal,
  calculateAwayOdds,
  calculateOverOdds
};

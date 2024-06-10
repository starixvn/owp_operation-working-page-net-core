import {
  OFFER_LIVES,
  LINKED_MATCHES,
  MATCH_STATUSES,
  SPORTS
} from './FilterHeaderConst.json';

const getDefaultValueSelected = localStorageKey => {
  const userInfo = JSON.parse(localStorage.getItem(localStorageKey));
  const defaultOfferLives = userInfo
    ? userInfo.selectedOfferLives
    : OFFER_LIVES;

  const defaultMatchStatuses = userInfo
    ? userInfo.selectedMatchStatuses
    : MATCH_STATUSES;

  const defaultLinkedMatches = LINKED_MATCHES;
  const defaultSports = SPORTS;

  return {
    defaultOfferLives,
    defaultLinkedMatches,
    defaultMatchStatuses,
    defaultSports
  };
};

export { getDefaultValueSelected };

import { createReducer } from './reducerUtilities';
import { ACTION_QUERY_MATCH_OFFER } from '../constants/action-type';

const defaultModel = {
  matchOfferPending: true,
  matchOfferData: {}
};

const handlePending = state => {
  return {
    ...state,
    matchOfferPending: true
  };
};

const handleSuccess = (state, action) => {
  const { data } = action;
  return {
    ...state,
    matchOfferPending: false,
    matchOffers: data
  };
};

const matchOfferReducer = createReducer(defaultModel, {
  [ACTION_QUERY_MATCH_OFFER.PENDING]: handlePending,
  [ACTION_QUERY_MATCH_OFFER.SUCCESS]: handleSuccess
});

export default matchOfferReducer;

import { createAsyncThunk } from '@reduxjs/toolkit';
import { netCoreURL, UPDATE_NO_OFFER_MATCH } from '../const/const.json';
import { post } from '../../Shared/Service/Service';
import { TNoOfferMatch } from '../types/noOfferMatchType';

const buildRequestURL = () =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${UPDATE_NO_OFFER_MATCH}`
    : `${window.netCoreRootUrl}/api/EventPicking/${UPDATE_NO_OFFER_MATCH}`;

const updateNoOfferMatchThunk = createAsyncThunk<TNoOfferMatch[], TNoOfferMatch[]>(
  'noOfferMatch/update',
  async noOfferMatch => {
    const url = buildRequestURL();
    await post({
      url,
      noOfferMatch
    });

    return noOfferMatch;
  }
);

export default updateNoOfferMatchThunk;

import { createAsyncThunk } from '@reduxjs/toolkit';
import { UPDATE_NO_OFFER_MATCH } from '../const/const.json';
import { post } from '../../Shared/Service/Service';
import { TNoOfferMatch } from '../types/noOfferMatchType';
import { netCoreURL } from '../const/apiUrl';

const buildRequestURL = () => `${netCoreURL}/${UPDATE_NO_OFFER_MATCH}`;

const updateNoOfferMatchThunk = createAsyncThunk<
  TNoOfferMatch[],
  TNoOfferMatch[]
>('noOfferMatch/update', async noOfferMatch => {
  const url = buildRequestURL();
  await post({
    url,
    noOfferMatch
  });

  return noOfferMatch;
});

export default updateNoOfferMatchThunk;

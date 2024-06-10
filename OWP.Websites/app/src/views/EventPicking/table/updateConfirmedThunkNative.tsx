import { createAsyncThunk } from '@reduxjs/toolkit';
import { netCoreURL, UPDATE_CONFIRMED } from '../const/const.json';
import { post } from '../../Shared/Service/Service';
import { TConfirmed } from '../types/confirmed';

const buildRequestURL = () =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${UPDATE_CONFIRMED}`
    : `${window.netCoreRootUrl}/api/EventPicking/${UPDATE_CONFIRMED}`;

const updateConfirmedThunk = createAsyncThunk<TConfirmed, TConfirmed>(
  'confirmed/update',
  async confirmInfo => {
    const url = buildRequestURL();
    const infoSiteMatchIds = confirmInfo.infoSiteMatchIds.join(',');
    await post({
      url,
      ...confirmInfo,
      infoSiteMatchIds
    });

    return confirmInfo;
  }
);

export default updateConfirmedThunk;

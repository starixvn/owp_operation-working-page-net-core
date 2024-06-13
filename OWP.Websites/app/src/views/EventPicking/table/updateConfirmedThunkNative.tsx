import { createAsyncThunk } from '@reduxjs/toolkit';
import { UPDATE_CONFIRMED } from '../const/const.json';
import { post } from '../../Shared/Service/Service';
import { TConfirmed } from '../types/confirmed';
import { netCoreURL } from '../const/apiUrl';

const buildRequestURL = () => `${netCoreURL}/${UPDATE_CONFIRMED}`;

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

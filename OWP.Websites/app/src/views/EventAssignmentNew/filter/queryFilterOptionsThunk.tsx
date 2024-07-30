import { createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import ResourceManagementService from '../../ResourceManagement/Service';
import { TFilterQueryOptions, TMatchOffer } from '../type/type';

const queryFilterOptionsThunk = createAsyncThunk<TFilterQueryOptions>(
  'filter/query',
  async () => {
    const [matchStatus, matchOffers] = await axios.all([
      ResourceManagementService.getByRefName('MatchStatus'),
      ResourceManagementService.getByRefName('MatchOffer')
    ]);
    return {
      MatchStatus: matchStatus,
      OffVR: matchOffers.filter((x: TMatchOffer) => x.Value.includes('OFF VR'))
    };
  }
);
export default queryFilterOptionsThunk;

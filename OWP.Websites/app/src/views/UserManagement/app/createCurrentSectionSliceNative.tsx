import { StateCreator } from 'zustand';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { TSectionName } from '../type';
import axios from 'axios';
import update from 'immutability-helper';

const createCurrentSectionSliceNative: StateCreator<
  TSectionName,
  [],
  [],
  TSectionName
> = set => ({
  querySectionNameStatus: REQUEST_STATUS.pending,
  currentSection: null,
  async querySectionName() {
    const url =
      process.env.NODE_ENV === 'development'
        ? `api-netcore/UserManagement/query-current-section`
        : `api/UserManagement/query-current-section`;
    const response = await axios.get(url);
    set(state =>
      update(state, {
        querySectionNameStatus: { $set: REQUEST_STATUS.idle },
        currentSection: { $set: response.data }
      })
    );
  }
});

export default createCurrentSectionSliceNative;

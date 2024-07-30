import { StateCreator } from 'zustand';
import { TQueryColorExplainInfo, TColorExplain } from '../features/TypeNative';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { buildRequestURL } from '../utils';
import axios from 'axios';
import update from 'immutability-helper';

const createColorExplainSlice: StateCreator<
  TQueryColorExplainInfo,
  [],
  [],
  TQueryColorExplainInfo
> = set => ({
  loadingColorExplain: REQUEST_STATUS.idle,
  colorExplainInfo: [] as TColorExplain[],
  queryColorExplain: async () => {
    set({ loadingColorExplain: REQUEST_STATUS.pending });
    const url = buildRequestURL('get-color-explain');
    const response = await axios.get(url);
    set(state =>
      update(state, {
        loadingColorExplain: { $set: REQUEST_STATUS.idle },
        colorExplainInfo: { $set: response.data }
      })
    );
  }
});

export default createColorExplainSlice;

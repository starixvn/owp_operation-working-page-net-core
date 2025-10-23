import axios from 'axios';
import { create } from 'zustand';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import getPermission from '../../../../Shared/permission/getPermission';
import {
  functionKey,
  getLog,
  query,
  update,
  maxbetSettingTable
} from '../Const.json';
import { buildMatchOfferURL, buildURL, orderMaxbetSettingByFtHdp } from '../helper/helper';
import {
  EditModel,
  EditPopupModel,
  LogData,
  MaxBetSettingModel,
  MaxbetSettingState,
  Policy,
  SecondAccept,
  SelectOption
} from '../typeNative';

const useStore = create<MaxbetSettingState>((set, _) => ({
  isOpenLogPopup: false,
  maxBetValue: 0,
  logData: [] as LogData[],
  loading: REQUEST_STATUS.idle,
  editModel: {} as EditModel,
  isOpenEditPopup: false,
  popupType: null,
  policy: {} as Policy,
  orderInfo: {
    columnId: maxbetSettingTable.fullTimeMaxbetRefer.id,
    orderType: 'NONE'
  },
  maxbetSettings: [] as MaxBetSettingModel[],
  secondAcceptValues: [] as SelectOption[],
  getMaxbetSettings: async () => {
    set({ loading: REQUEST_STATUS.pending });

    const url = buildURL(query);
    const response = await axios.get(url);
    set({ loading: REQUEST_STATUS.idle });
    set(state => ({
      maxbetSettings: orderMaxbetSettingByFtHdp(response.data, state.orderInfo.orderType)
    }));
  },
  queryPolicy: async () => {
    const policy = await getPermission(functionKey);
    set({ policy });
  },
  setOpenEditPopup: () => set({ isOpenEditPopup: true }),
  setCloseEditPopup: () => set({ isOpenEditPopup: false, popupType: null }),
  setPopupType: (type: number) => set({ popupType: type }),
  setEditModel: (value: EditModel) => {
    set({ editModel: value });
  },
  updateMaxbetSetting: async (object: EditPopupModel) => {
    const url = buildURL(update);
    await axios.post(url, object);
  },
  getSecondAcceptValues: async () => {
    const response = await axios.get(buildMatchOfferURL());
    const options = response.data.map((item: SecondAccept) => {
      return {
        value: item.Id,
        label: item.Value
      } as SelectOption;
    });
    options.unshift({ value: null, label: '-- None --' });
    set({ secondAcceptValues: options });
  },
  setLogData: async (maxBetValue: number) => {
    const url = buildURL(getLog);
    const result = await axios.get(url, {
      params: { FT_HDP: maxBetValue }
    });
    if (result.data) {
      set({ logData: result.data });
    }
    set({ maxBetValue });
  },
  setOpenLogPopup: () => set({ isOpenLogPopup: true }),
  setCloseLogPopup: () => set({ isOpenLogPopup: false }),
  sortMaxbetSettings: async (columnId: string, orderType: 'ORDER_DESC' | 'ORDER_ASC' | 'NONE') => {
    set({ orderInfo: { columnId, orderType } });
    set(state => ({
      maxbetSettings: orderMaxbetSettingByFtHdp([...state.maxbetSettings], orderType)
    }));
  }
}));

export type RootState = ReturnType<typeof useStore.getState>;

export default useStore;

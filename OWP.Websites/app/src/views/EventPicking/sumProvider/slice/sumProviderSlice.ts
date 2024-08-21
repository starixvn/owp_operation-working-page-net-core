import { createSlice } from '@reduxjs/toolkit';
import TDataTable, { TMatchOfferInfo } from '../../types/dataTableType';
import { groupAndCountByHour } from '../../helper/sumProviderHelper';
import { ProviderSumType } from '../../types/providerSumType';

const sumProviderSlice = createSlice({
  name: 'filter/filterSlice',
  initialState: {
    sumTable: [] as Array<ProviderSumType>
  },
  reducers: {
    sumProvider: (
      state,
      action: { payload: { data: TMatchOfferInfo[]; date: string } }
    ) => {
      const { data, date } = action.payload;
      state.sumTable = groupAndCountByHour(data, date);
    }
  },
  extraReducers: {}
});

const { reducer, actions } = sumProviderSlice;
const { sumProvider } = actions;
export { sumProvider };
export default reducer;

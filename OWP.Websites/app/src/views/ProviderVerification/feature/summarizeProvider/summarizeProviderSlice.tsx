import { createSlice } from '@reduxjs/toolkit';
import { summarize } from './helper';
import { TProviderCount } from '../../type/provider';
import { TDataSource } from '../../type/table';
import { Option } from '../../../Shared/MultipleSelection/type';

const sumProviderSlice = createSlice({
  name: 'filter/filterSlice',
  initialState: {
    sumTable: [] as Array<TProviderCount>
  },
  reducers: {
    sumProvider: (
      state,
      action: { payload: { data: TDataSource[], agentOptions: Option[] } }
    ) => {
      const { data, agentOptions } = action.payload;
      state.sumTable = summarize(data, agentOptions);
    }
  },
  extraReducers: {}
});

const { reducer, actions } = sumProviderSlice;
const { sumProvider } = actions;
export { sumProvider };
export default reducer;

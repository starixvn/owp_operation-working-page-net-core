import { createSlice } from '@reduxjs/toolkit';
import update from 'immutability-helper';
import { buildDefaultFilterDateProviderVerification } from '../../../Shared/DateTime/dateTimeHelper';
import { getConfirmProvider } from '../table/tableThunk';
import { Option } from '../../../Shared/MultipleSelection/type';

const filterSlice = createSlice({
  name: 'tableSlice',
  initialState: {
    filter: {
      eventDate: buildDefaultFilterDateProviderVerification(),
      onlyShowIsMismatchEvent: false,
      confirmProvider: [] as Option[]
    }
  },
  reducers: {
    updateFilter: (state, action) => {
      const { key, value } = action.payload;
      const { filter } = state;
      const newFilter = update(filter, {
        [key]: {
          $set: value
        }
      });
      state.filter = newFilter;
    }
  },
  extraReducers: builder => {
    builder.addCase(getConfirmProvider.fulfilled, (state, { payload }) => {
      state.filter.confirmProvider = payload;
    });
  }
});

const { actions, reducer } = filterSlice;
const { updateFilter } = actions;
export { updateFilter };
export default reducer;

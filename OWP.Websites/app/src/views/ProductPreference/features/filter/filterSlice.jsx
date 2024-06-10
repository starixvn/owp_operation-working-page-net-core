/* eslint-disable no-warning-comments */
import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { FILTER_KEY } from '../../Const.json';
import { getDropdownOption } from './filterThunk';

const { USER, ROLE_ID, SECTION_ID, LOCATION_ID } = FILTER_KEY;

const filterSlice = createSlice({
  name: 'filterSlice',
  initialState: {
    filterInfo: {
      [USER]: undefined, // NOSONAR
      [ROLE_ID]: [],
      [SECTION_ID]: [],
      [LOCATION_ID]: []
    },
    dropdownOptions: {},
    loading: REQUEST_STATUS.pending
  },
  reducers: {
    setFilter: (state, action) => {
      const { key, value } = action.payload;
      state.filterInfo[key] = value;
    }
  },
  extraReducers: builder => {
    builder
      .addCase(getDropdownOption.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(getDropdownOption.fulfilled, (state, action) => {
        const { locationOption, sectionOption, roleOption } = action.payload;
        const filterDefault = {
          [ROLE_ID]: roleOption.map(l => l.value),
          [SECTION_ID]: sectionOption.map(l => l.value),
          [LOCATION_ID]: locationOption.map(l => l.label)
        };
        state.filterInfo = filterDefault;
        state.dropdownOptions = action.payload;
        state.loading = REQUEST_STATUS.idle;
      })
      .addCase(getDropdownOption.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      });
  }
});

const { reducer, actions } = filterSlice;
const { setFilter } = actions;
export { setFilter };
export default reducer;

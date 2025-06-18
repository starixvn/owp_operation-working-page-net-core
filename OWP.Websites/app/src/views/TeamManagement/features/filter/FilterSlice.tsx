import { createSlice } from '@reduxjs/toolkit';
import update from 'immutability-helper';
import { Option } from '../../../Shared/MultipleSelection/type';

const filterSlice = createSlice({
  name: 'tableSlice',
  initialState: {
    filter: {
      leagueGroup: [] as Option[]
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
  }
});

const { actions, reducer } = filterSlice;
const { updateFilter } = actions;
export { updateFilter };
export default reducer;

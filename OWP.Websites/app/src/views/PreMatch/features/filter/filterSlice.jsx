/* eslint-disable no-warning-comments */
import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import {
  formatTime,
  getMMDDYYYYStringFromDateTime
} from '../../../Shared/DateTime/dateTimeHelper';
import { EARLY_OFFERED, MATCH_OFFER } from '../../const/const';

const filterSlice = createSlice({
  name: 'filter',
  initialState: {
    filterDate: getMMDDYYYYStringFromDateTime(new Date()),
    employer: undefined, // NOSONAR
    loading: REQUEST_STATUS.idle,
    currentRequestId: undefined, // NOSONAR
    error: null,
    backupFilterModel: null,
    matchOffers: [...MATCH_OFFER.map(m => m.value)],
    earlyOffered: [...EARLY_OFFERED.map(m => m.value)],
    isReloadEdit: false
  },
  reducers: {
    reloadDataAfterEdit: state => {
      state.isReloadEdit = !state.isReloadEdit;
    },
    updateFilterDate: {
      reducer: (state, action) => {
        const { selectedDate } = action.payload;
        state.filterDate = selectedDate;
      },
      prepare: selectedDate => {
        return {
          payload: {
            selectedDate: selectedDate
              ? formatTime(selectedDate, 'minute', 'LLL')
              : null
          }
        };
      }
    },
    updateFilterMatchOffers: {
      reducer: (state, action) => {
        state.matchOffers = action.payload;
      }
    },
    updateFilterEarlyOffered: {
      reducer: (state, action) => {
        state.earlyOffered = action.payload;
      }
    },
    updateFilterEmployer: {
      reducer: (state, action) => {
        const { employer } = action.payload;
        state.employer = employer;
      }
    }
  }
});

const { actions, reducer } = filterSlice;
const {
  reloadDataAfterEdit,
  updateFilterDate,
  updateFilterEmployer,
  updateFilterMatchOffers,
  updateFilterEarlyOffered
} = actions;
export {
  actions,
  reloadDataAfterEdit,
  updateFilterDate,
  updateFilterEmployer,
  updateFilterMatchOffers,
  updateFilterEarlyOffered
};

export default reducer;

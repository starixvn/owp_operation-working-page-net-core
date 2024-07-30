import { StateCreator } from 'zustand';
import {
  TAssigneeSuggestion,
  TQueryWorkScheduleInfo,
  UserWorkingSchedule,
  TMatchInfo,
  OVER_TIME_OPTIONS_VALUE
} from '../features/TypeNative';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { buildRequestURL } from '../utils';
import axios from 'axios';
import { executeFilter } from '../features/helper/AssigneeSuggestionHelper';


const createTableSlice: StateCreator<
  TQueryWorkScheduleInfo,
  [],
  [],
  TQueryWorkScheduleInfo
> = (set, get) => ({
  loadingDataTable: REQUEST_STATUS.pending,
  searchText: '',
  filterOTValue: OVER_TIME_OPTIONS_VALUE.ALL,
  workScheduleInfoOrigin: {
    matchInfo: {} as TMatchInfo,
    fullyAvailable: [] as UserWorkingSchedule[],
    partiallyAvailable: [] as UserWorkingSchedule[],
    busy: [] as UserWorkingSchedule[]
  } as TAssigneeSuggestion,
  workScheduleInfo: {
    matchInfo: {} as TMatchInfo,
    fullyAvailable: [] as UserWorkingSchedule[],
    partiallyAvailable: [] as UserWorkingSchedule[],
    busy: [] as UserWorkingSchedule[]
  } as TAssigneeSuggestion,
  queryDataTable: async ({ matchId, betTypeId, filterDate, otherTime }) => {
    set({ loadingDataTable: REQUEST_STATUS.pending });
    const url = buildRequestURL('get-trader-suggestion');
    const response = await axios.get(url, {
      params: {
        owpMatchId: matchId,
        betTypeId,
        filterDate,
        otherTime
      }
    });
    set({
      workScheduleInfo: response.data,
      workScheduleInfoOrigin: response.data,
      loadingDataTable: REQUEST_STATUS.idle
    });
  },
  filterByAssignee(keyword) {
    const { workScheduleInfoOrigin, filterOTValue } = get();
    set({
      searchText: keyword,
      workScheduleInfo: executeFilter(workScheduleInfoOrigin, keyword, filterOTValue)
    });
  },
  filterByOtValue(value) {
    const { workScheduleInfoOrigin, searchText } = get();
    set({
      filterOTValue: value,
      workScheduleInfo: executeFilter(workScheduleInfoOrigin, searchText, value)
    });
  }
});

export default createTableSlice;

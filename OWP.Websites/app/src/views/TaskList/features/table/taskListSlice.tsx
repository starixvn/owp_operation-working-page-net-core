import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { DataSource, PopupState } from '../../type/TTaskListTable';
import { taskListThunk, updateReadyStatusThunk } from './tableThunk';
import { buildIsReadValue } from '../../../Shared/ticketEvent/helper/ticketHelper';
import { filterMatchByCriteria } from '../../helper/tasklistHelper';
import { RefreshModel } from '../../type/Header';

const defaultEntities: Readonly<Array<DataSource>> = [];
const taskListSlice = createSlice({
  name: 'taskList/taskListSlice',
  initialState: {
    queryTaskListLoading: REQUEST_STATUS.idle,
    matchesSelected: defaultEntities,
    matchesOriginal: defaultEntities,
    matches: defaultEntities,
    userId: 0 as number,
    hasDetailInformationPermission: false,
    isHideCompletedMatches: false,
    isShowUnreadMsg: false,
    popupState: {
      matchId: undefined,
      isOpen: false
    } as PopupState,
    refreshModel: {
      filterDate: undefined,
      isToday: true
    } as RefreshModel
  },
  reducers: {
    setUserId: (state, action) => {
      state.userId = action.payload;
    },
    setDataTable: (state, action) => {
      state.matches = action.payload;
    },
    setMatchesSelected: (state, action) => {
      const { match, isAdded } = action.payload;
      state.matchesSelected = isAdded
        ? [...state.matchesSelected, match]
        : state.matchesSelected.filter(s => s.OWPMatchId !== match.OWPMatchId);
    },
    resetMatchesSelected: state => {
      state.matchesSelected = [];
    },
    setDataSignalR: (state, action) => {
      const { actorId, parentMatchId } = action.payload;
      state.matches.forEach(item => {
        if (item.ParentMatchId === parentMatchId) {
          item.IsRead = buildIsReadValue(actorId, state.userId, item.IsRead);
          item.HasTicket = true;
        }
      });
    },
    handleHideCompleteMatches: state => {
      const { isHideCompletedMatches, isShowUnreadMsg } = state;
      const currentValue = !isHideCompletedMatches;
      state.isHideCompletedMatches = currentValue;
      state.matches = filterMatchByCriteria(state.matchesOriginal)(
        isShowUnreadMsg
      )(currentValue);
    },
    handleShowUnreadMessage: state => {
      const { isShowUnreadMsg, isHideCompletedMatches } = state;
      const currentValue = !isShowUnreadMsg;
      state.isShowUnreadMsg = currentValue;
      state.matches = filterMatchByCriteria(state.matchesOriginal)(
        currentValue
      )(isHideCompletedMatches);
    },
    openPopupDetail: (state, action) => {
      state.popupState = {
        matchId: action.payload,
        isOpen: true
      };
    },
    closePopupDetail: state => {
      state.popupState = {
        matchId: undefined,
        isOpen: false
      };
    },
    setRefreshModel: (state, action) => {
      state.refreshModel = action.payload;
    },
    filterByKeyword: (state, action) => {
      const keyword = action.payload.toLowerCase();
      if (!keyword) {
        state.matches = filterMatchByCriteria(state.matchesOriginal)(state.isShowUnreadMsg)(state.isHideCompletedMatches);
        return;
      }
      const filteredMatches = state.matchesOriginal.filter(match =>
        match.MatchNo?.toLowerCase().includes(keyword) ||
        match.Home?.toLowerCase().includes(keyword) ||
        match.Away?.toLowerCase().includes(keyword) ||
        match.League?.toLowerCase().includes(keyword) ||
        match.Round?.toLowerCase().includes(keyword) ||
        match.Division?.toLowerCase().includes(keyword) ||
        match.IPTV?.toLowerCase().includes(keyword) ||
        match.Provider?.toLowerCase().includes(keyword) ||
        match.Remark?.toLowerCase().includes(keyword) ||
        match.Channel?.toLowerCase().includes(keyword) ||
        match.TeamRound?.toLowerCase().includes(keyword) ||
        match.MatchCode?.toLowerCase().includes(keyword) ||
        match.WatchBet?.toLowerCase().includes(keyword) ||
        match.OtherProvider?.toLowerCase().includes(keyword) ||
        match.NoOfHDP?.toLowerCase().includes(keyword) ||
        match.KickOffTime?.toLowerCase().includes(keyword)
      );
      state.matches = filterMatchByCriteria(filteredMatches)(state.isShowUnreadMsg)(state.isHideCompletedMatches);
    }
  },
  extraReducers: builder => {
    builder
      .addCase(taskListThunk.pending, state => {
        state.queryTaskListLoading = REQUEST_STATUS.pending;
      })
      .addCase(taskListThunk.fulfilled, (state, { payload }) => {
        const { isShowUnreadMsg, isHideCompletedMatches } = state;
        state.matchesOriginal = payload.Matches;
        state.matches = filterMatchByCriteria(payload.Matches)(isShowUnreadMsg)(
          isHideCompletedMatches
        );
        state.hasDetailInformationPermission =
          payload.HasDetailInformationPermission;
        state.queryTaskListLoading = REQUEST_STATUS.idle;
      })
      .addCase(updateReadyStatusThunk.fulfilled, (state, { payload }) => {
        const { rowIndex, readyTime } = payload;
        const matchTarget = state.matches[rowIndex];
        const matchOrigin = state.matchesOriginal.find(
          x => x.OWPMatchId === matchTarget.OWPMatchId
        );
        matchTarget.IsReady = true;
        matchTarget.ReadyDateTimeDisplay = readyTime;
        if (matchOrigin) {
          matchOrigin.IsReady = true;
          matchOrigin.ReadyDateTimeDisplay = readyTime;
        }
      });
  }
});

const { actions, reducer } = taskListSlice;
const {
  setUserId,
  setDataTable,
  setDataSignalR,
  handleShowUnreadMessage,
  handleHideCompleteMatches,
  openPopupDetail,
  closePopupDetail,
  setRefreshModel,
  setMatchesSelected,
  resetMatchesSelected,
  filterByKeyword
} = actions;
export {
  setUserId,
  setDataTable,
  setDataSignalR,
  handleHideCompleteMatches,
  handleShowUnreadMessage,
  openPopupDetail,
  closePopupDetail,
  setRefreshModel,
  setMatchesSelected,
  resetMatchesSelected,
  filterByKeyword
};

export default reducer;

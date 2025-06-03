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
      const keywordUpper = action.payload.toUpperCase();
      if (!keyword) {
        state.matches = filterMatchByCriteria(state.matchesOriginal)(state.isShowUnreadMsg)(state.isHideCompletedMatches);
        return;
      }
      const filteredMatches = state.matchesOriginal.filter(match =>
        String(match.MatchNo || '').toLowerCase().includes(keyword) ||
        String(match.KickOffTimeDisplay || '').toLowerCase().includes(keyword) ||
        String(match.InfoSiteTimeDisplay || '').toLowerCase().includes(keyword) ||
        String(match.OtherTimeDisplay || '').toLowerCase().includes(keyword) ||
        String(match.EventDateDisplay || '').toLowerCase().includes(keyword) ||
        String(match.League || '').toLowerCase().includes(keyword) ||
        String(match.LeagueGroup || '').toLowerCase().includes(keyword) ||
        match.MatchOffers?.some(offer => String(offer.MatchOfferValue || '').toLowerCase().includes(keyword)) ||
        String(match.NoOfHDP || '').trim().replace(' ', '').toLowerCase().includes(keyword) ||
        String(match.Home || '').toLowerCase().includes(keyword) ||
        String(match.Away || '').toLowerCase().includes(keyword) ||
        String(match.Remark || '').toLowerCase().includes(keyword) ||
        String(match.Round || '').toLowerCase().includes(keyword) ||
        String(match.TeamRound || '').toLowerCase().includes(keyword) ||
        String(match.Division || '').toLowerCase().includes(keyword) ||
        String(match.IPTV || '').toLowerCase().includes(keyword) ||
        String(match.IPTVCopy || '').toLowerCase().includes(keyword) ||
        String(match.Provider || '').toLowerCase().includes(keyword) ||
        String(match.WatchBet || '').toLowerCase().includes(keyword) ||
        String(match.OtherProvider || '').toLowerCase().includes(keyword) ||
        String(match.MatchSetting?.Handicap?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        String(match.MatchSetting?.Handicap1st?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        String(match.MatchSetting?.OverUnder?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        String(match.MatchSetting?.OverUnder1st?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        String(match.MatchSetting?.OneXTwo?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        String(match.MatchSetting?.OneXTwo1st?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        String(match.MatchSetting?.Specific15Mins?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        String(match.MatchSetting?.Specific10Mins?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        String(match.MatchSetting?.FastMarket?.MaxbetSpreadToDisplay || '').toUpperCase().includes(keywordUpper) ||
        match.Assignment?.Handicap?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.Handicap1st?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.OverUnder?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.OverUnder1st?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.OneXTwo?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.OneXTwo1st?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.Specific15Mins?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.Specific10Mins?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.FastMarket?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.LA?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Assignment?.LAOthers?.some(trader => String(trader.Username || '').toLowerCase().includes(keyword)) ||
        match.Televisions?.some(tv =>
          String(tv.TVCode || '').toLowerCase().includes(keyword)
        )
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


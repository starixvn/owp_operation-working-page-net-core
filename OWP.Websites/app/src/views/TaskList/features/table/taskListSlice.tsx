import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { DataSource, PopupState } from '../../type/TTaskListTable';
import { taskListThunk, updateReadyStatusThunk } from './tableThunk';
import { buildIsReadValue } from '../../../Shared/ticketEvent/helper/ticketHelper';
import { filterMatchByCriteria } from '../../helper/tasklistHelper';
import { RefreshModel } from '../../type/Header';

const defaultEntities: Readonly<Array<DataSource>> = [];

const checkBasicMatchFields = (match: DataSource, keyword: string): boolean => {
  return [
    match.MatchNo,
    match.KickOffTimeDisplay,
    match.InfoSiteTimeDisplay,
    match.OtherTimeDisplay,
    match.EventDateDisplay,
    match.League,
    match.LeagueGroup,
    match.NoOfHDP,
    match.Home,
    match.Away,
    match.Remark,
    match.Round,
    match.TeamRound,
    match.Division,
    match.IPTV,
    match.IPTVCopy,
    match.Provider,
    match.WatchBet,
    match.OtherProvider
  ].some(field => String(field || '').toLowerCase().includes(keyword));
};

const checkMatchOffers = (match: DataSource, keyword: string): boolean => {
  return match.MatchOffers?.some(offer =>
    String(offer.MatchOfferValue || '').toLowerCase().includes(keyword)
  ) || false;
};

const checkMatchSettings = (match: DataSource, keywordUpper: string): boolean => {
  const settings = match.MatchSetting;
  if (!settings) {
    return false;
  }

  const maxbetFields = [
    settings.Handicap?.MaxbetSpreadToDisplay,
    settings.Handicap1st?.MaxbetSpreadToDisplay,
    settings.OverUnder?.MaxbetSpreadToDisplay,
    settings.OverUnder1st?.MaxbetSpreadToDisplay,
    settings.OneXTwo?.MaxbetSpreadToDisplay,
    settings.OneXTwo1st?.MaxbetSpreadToDisplay,
    settings.Specific15Mins?.MaxbetSpreadToDisplay,
    settings.Specific10Mins?.MaxbetSpreadToDisplay,
    settings.FastMarket?.MaxbetSpreadToDisplay
  ];

  return maxbetFields.some(field =>
    String(field || '').toUpperCase().includes(keywordUpper)
  );
};

const checkAssignments = (match: DataSource, keyword: string): boolean => {
  const assignment = match.Assignment;
  if (!assignment) {
    return false;
  }

  const assignmentTypes = [
    assignment.Handicap,
    assignment.Handicap1st,
    assignment.OverUnder,
    assignment.OverUnder1st,
    assignment.OneXTwo,
    assignment.OneXTwo1st,
    assignment.Specific15Mins,
    assignment.Specific10Mins,
    assignment.FastMarket,
    assignment.LA,
    assignment.LAOthers
  ];

  return assignmentTypes.some(traders =>
    traders?.some(trader =>
      String(trader.Username || '').toLowerCase().includes(keyword)
    )
  );
};

const checkTelevisions = (match: DataSource, keyword: string): boolean => {
  return match.Televisions?.some(tv =>
    String(tv.TVCode || '').toLowerCase().includes(keyword)
  ) || false;
};

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

      const filteredMatches = state.matchesOriginal.filter(match => {
        if (checkBasicMatchFields(match, keyword)) {
          return true;
        }

        if (checkMatchOffers(match, keyword)) {
          return true;
        }

        if (checkMatchSettings(match, keywordUpper)) {
          return true;
        }

        if (checkAssignments(match, keyword)) {
          return true;
        }

        if (checkTelevisions(match, keyword)) {
          return true;
        }

        return false;
      });
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


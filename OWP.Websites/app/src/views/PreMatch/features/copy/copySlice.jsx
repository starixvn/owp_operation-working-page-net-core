import { createSlice } from '@reduxjs/toolkit';
import { compareValues } from './copyHelper';

const copySlice = createSlice({
  name: 'copy',
  initialState: {
    isCopying: false,
    copiedValue: [],
    rowStart: null,
    rowEnd: null,
    highlightedCell: {
      PreMatchLeagueId: [],
      isTakeOver: false,
      isTakeOverBefore: false
    }
  },
  reducers: {
    copyElement: {
      reducer: (state, action) => {
        const { copiedValue, isCopying, leagueIndex, rowIndex, isTakeOver } =
          action.payload;
        if (isTakeOver !== state.highlightedCell.isTakeOverBefore) {
          state.rowStart = null;
          state.rowEnd = null;
          state.copiedValue = [];
          state.highlightedCell.isTakeOverBefore = isTakeOver;
        }
        state.isCopying = isCopying;
        const copyInfo = state.copiedValue.find(c => c.rowIndex === rowIndex);
        if (!copyInfo) {
          state.copiedValue.push({
            value: copiedValue,
            rowIndex
          });
          state.copiedValue = [...state.copiedValue].sort(compareValues);
        } else {
          copyInfo.value = copiedValue;
        }
        const { rowStart, rowEnd } = state;
        if (!rowStart) {
          state.rowStart = leagueIndex;
          state.rowEnd = leagueIndex;
        }
        if (leagueIndex < rowStart) {
          state.rowStart = leagueIndex;
        }
        if (leagueIndex > rowEnd) {
          state.rowEnd = leagueIndex;
        }
      }
    },
    highlightElement: {
      reducer: (state, action) => {
        const { PreMatchLeagueId, isTakeOver } = action.payload;
        state.highlightedCell.PreMatchLeagueId.push(PreMatchLeagueId);
        state.isCopying = false;
        if (isTakeOver !== state.highlightedCell.isTakeOver) {
          state.highlightedCell.PreMatchLeagueId.length = 0;
          state.highlightedCell.PreMatchLeagueId.push(PreMatchLeagueId);
          state.highlightedCell.isTakeOverBefore =
            state.highlightedCell.isTakeOver;
          state.highlightedCell.isTakeOver = isTakeOver;
        }
      }
    },
    resetCopyInfo: {
      reducer: state => {
        state.highlightedCell.PreMatchLeagueId.length = 0;
        state.copiedValue.length = 0;
        state.highlightedCell.isTakeOver = false;
        state.highlightedCell.isTakeOverBefore = false;
        state.rowStart = null;
        state.rowEnd = null;
        state.isCopying = false;
      }
    }
  }
});

const { actions, reducer } = copySlice;
const { copyElement, highlightElement, resetCopyInfo } = actions;
export { copyElement, highlightElement, resetCopyInfo };
export default reducer;

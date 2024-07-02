import copySlice, {
  copyElement,
  highlightElement,
  resetCopyInfo
} from './copySlice';

describe('copySlice under test', () => {
  it('should return the initial state', () => {
    expect(copySlice(undefined, {})).toEqual({
      copiedValue: [],
      highlightedCell: {
        PreMatchLeagueId: [],
        isTakeOver: false,
        isTakeOverBefore: false
      },
      isCopying: false,
      rowEnd: null,
      rowStart: null
    });
  });
  describe('copyElement under test', () => {
    it('should handle update state isCopying, copiedValue', () => {
      const initialState = {
        isCopying: false,
        copiedValue: [
          {
            HDP: '',
            OU: ''
          }
        ],
        highlightedCell: {
          rowId: null,
          isTakeOver: false
        }
      };

      const expectState = {
        isCopying: true,
        copiedValue: [
          {
            HDP: '',
            OU: '',
            value: [
              {
                HDP: 'starix14',
                OU: 'starix16'
              }
            ]
          }
        ],
        highlightedCell: {
          rowId: null,
          isTakeOver: false
        },
        rowStart: undefined,
        rowEnd: undefined
      };

      const action = {
        type: copyElement.type,
        payload: {
          copiedValue: [{ HDP: 'starix14', OU: 'starix16' }],
          isCopying: true
        }
      };

      const state = copySlice(initialState, action);
      expect(state).toEqual(expectState);
    });
  });

  describe('highlightElement under test', () => {
    it('should handle highlightElement reducer', () => {
      const initialState = {
        isCopying: false,
        copiedValue: [
          {
            HDP: 'starix14',
            OU: 'starix16'
          }
        ],
        highlightedCell: {
          PreMatchLeagueId: [],
          rowId: null,
          isTakeOver: false
        }
      };

      const expectState = {
        isCopying: false,
        copiedValue: [
          {
            HDP: 'starix14',
            OU: 'starix16'
          }
        ],
        highlightedCell: {
          PreMatchLeagueId: [undefined],
          rowId: 1,
          rowId: null,
          isTakeOver: undefined,
          isTakeOverBefore: false
        }
      };

      const action = {
        type: highlightElement.type,
        payload: {
          highlightedCell: {
            rowId: 1,
            isTakeOver: true
          }
        }
      };

      const state = copySlice(initialState, action);
      expect(state).toEqual(expectState);
    });
  });

  describe('reset under test', () => {
    it('should handle reset reducer', () => {
      const initialState = {
        isCopying: false,
        copiedValue: [
          {
            HDP: '',
            OU: ''
          }
        ],
        highlightedCell: {
          isTakeOver: false,
          PreMatchLeagueId: [1]
        }
      };

      const expectState = {
        isCopying: false,
        copiedValue: [],
        highlightedCell: {
          isTakeOver: false,
          PreMatchLeagueId: [],
          isTakeOverBefore: false
        },
        rowEnd: null,
        rowStart: null
      };

      const action = {
        type: resetCopyInfo.type
      };

      const state = copySlice(initialState, action);
      expect(state).toEqual(expectState);
    });
  });
});

import reducer, {
  filterBySearchKey,
  clearFilter,
  collapseAllRow,
  expandAllRow,
  order,
  collapseRow,
  expandRow
} from './reducer';
import getOSAssignment from './getOSAssignmentThunkNative';
import { REQUEST_STATUS } from '../../../../Shared/Constants';

describe('Slice under test', () => {
  it('should return the initial state', () => {
    expect(reducer(undefined, {})).toEqual({
      "editOfferLiveMatches":  [],
      "isEdit": false,
      collapseExpandAllRowState: {
        columnKey: 'Trader',
        mode: 'expand'
      },
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entitiesOriginal: [],
      entities: [],
      loading: REQUEST_STATUS.idle,
      sortState: {
        columnKey: undefined,
        orderType: undefined
      },
      filterState: []
    });
  });

  it('should handle getOSAssignment.pending', () => {
    const action = {
      type: getOSAssignment.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      "isEdit": false,
      "editOfferLiveMatches": [],
      collapseExpandAllRowState: {
        columnKey: 'Trader',
        mode: 'expand'
      },
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entitiesOriginal: [],
      entities: [],
      loading: 'pending',
      sortState: {
        columnKey: undefined,
        orderType: undefined
      },
      filterState: []
    });
  });

  it('should handle getOSAssignment.fulfilled', () => {
    const action = {
      type: getOSAssignment.fulfilled.type,
      payload: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-04T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ]
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      "editOfferLiveMatches": [],
      "isEdit": false,
      collapseExpandAllRowState: {
        columnKey: 'Trader',
        mode: 'expand'
      },
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entitiesOriginal: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-04T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ],
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-04T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ],
      loading: 'idle',
      sortState: {
        columnKey: undefined,
        orderType: undefined
      },
      filterState: []
    });
  });

  it('should return new state isCollapsed   getOSAssignment.fulfilled', () => {
    const action = {
      type: getOSAssignment.fulfilled.type,
      payload: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-04T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ]
    };
    const initState = {
      collapseExpandRowState: {
        rowHasModeCollapse: [10018]
      },
      entities: [
        {
          osMatchId: 10018
        }
      ],
      sortState: {
        columnKey: '',
        orderType: ''
      },
      filterState: []
    };
    const expected = {
      'collapseExpandRowState': {
        'rowHasModeCollapse': [10018]
      },
      'entities': [
        {
          'autoLink': 'YES',
          'away': 'LA Laker',
          'category': '',
          'dateTime': '2021-01-04T01:00:00',
          'home': 'Portal',
          'iptv': 'Euro40',
          'league': 'EstherMiraaaaa23',
          'matchOffer': '15s Accept',
          'matchStatus': 'Live',
          'noHDP': '3HDP',
          'osMatchId': 10018,
          'otherProvider': 'BGtv/RB2tv/BETtv/',
          'provider': 'TV/BGTV/Glive53',
          'remark': '',
          'service': 'InPlay',
          'sport': 'Basketball',
          'traderMaxbetModels': [
            {
              'betTypeName': 'FT (HDP)',
              'hiden': false,
              'maxbet': {
                'betTypeName': 'FT (HDP)',
                'maxBet': 1000,
                'osMatchId': 10018
              },
              'traderAssignment': {
                'assignmentNote': '#sa noted',
                'betTypeName': 'FT (HDP)',
                'mainTrader': 'sa',
                'osMatchId': 10018
              }
            }
          ],
          'wb': 'IMG-B/Watch & Bet-160939/Glive-65'
        }
      ],
      'entitiesOriginal': [
        {
          'autoLink': 'YES',
          'away': 'LA Laker',
          'category': '',
          'dateTime': '2021-01-04T01:00:00',
          'home': 'Portal',
          'iptv': 'Euro40',
          'league': 'EstherMiraaaaa23',
          'matchOffer': '15s Accept',
          'matchStatus': 'Live',
          'noHDP': '3HDP',
          'osMatchId': 10018,
          'otherProvider': 'BGtv/RB2tv/BETtv/',
          'provider': 'TV/BGTV/Glive53',
          'remark': '',
          'service': 'InPlay',
          'sport': 'Basketball',
          'traderMaxbetModels': [
            {
              'betTypeName': 'FT (HDP)',
              'hiden': false,
              'maxbet': {
                'betTypeName': 'FT (HDP)',
                'maxBet': 1000,
                'osMatchId': 10018
              },
              'traderAssignment': {
                'assignmentNote': '#sa noted',
                'betTypeName': 'FT (HDP)',
                'mainTrader': 'sa',
                'osMatchId': 10018
              }
            }
          ],
          'wb': 'IMG-B/Watch & Bet-160939/Glive-65'
        }
      ],
      'filterState': [],
      'loading': 'idle',
      'sortState': {
        'columnKey': '',
        'orderType': ''
      }
    };
    const actual = reducer(initState, action);

    expect(actual).toEqual(expected);
  });

  it('should handle getOSAssignment.rejected', () => {
    const action = {
      type: getOSAssignment.rejected.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      collapseExpandAllRowState: {
        columnKey: 'Trader',
        mode: 'expand'
      },
      "editOfferLiveMatches": [],
      "isEdit": false,
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entitiesOriginal: [],
      entities: [],
      loading: 'idle',
      sortState: {
        columnKey: undefined,
        orderType: undefined
      },
      filterState: []
    });
  });

  it('should oder ASC when dispatch order ASC', () => {
    const action = {
      type: order.type,
      payload: {
        columnKey: 'KO',
        orderType: 'ORDER_ASC'
      }
    };
    const initialState = {
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ],
      entitiesOriginal: [],
      sortState: {
        columnKey: undefined,
        orderType: undefined
      },
      loading: REQUEST_STATUS.idle
    };
    const expected = {
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ],
      entitiesOriginal: [],
      sortState: { columnKey: 'KO', orderType: 'ORDER_ASC' },
      loading: 'idle'
    };
    const state = reducer(initialState, action);
    expect(expected).toEqual(state);
  });

  it('should oder DESC when dispatch order DESC', () => {
    const action = {
      type: order.type,
      payload: {
        columnKey: 'KO',
        orderType: 'ORDER_DESC'
      }
    };
    const initialState = {
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ],
      entitiesOriginal: [],
      sortState: {
        columnKey: undefined,
        orderType: undefined
      },
      loading: REQUEST_STATUS.idle
    };
    const expected = {
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ],
      entitiesOriginal: [],
      sortState: { columnKey: 'KO', orderType: 'ORDER_DESC' },
      loading: 'idle'
    };
    const state = reducer(initialState, action);
    expect(expected).toEqual(state);
  });

  it('should filter when dispatch filter key', () => {
    const action = {
      type: filterBySearchKey.type,
      payload: {
        columnKey: 'League',
        keyword: 'EstherMiraaaaa23'
      }
    };
    const initialState = {
      entitiesOriginal: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal1',
          homeName: 'Portal1 (N)',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          homeName: 'Portal (N)',
          away: 'LA Laker',
          league: 'GGGG',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ],
      sortState: {
        columnKey: undefined,
        orderType: undefined
      },
      filterState: [],
      loading: REQUEST_STATUS.idle
    };
    const expected = {
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal1',
          homeName: 'Portal1 (N)',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ]
    };
    const state = reducer(initialState, action);
    expect(expected.entities).toEqual(state.entities);
  });

  it('should restore entitiesOriginal when clear filter', () => {
    const action = {
      type: clearFilter.type,
      payload: 'Event'
    };
    const initialState = {
      entitiesOriginal: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal1',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ],
      filterState: {
        columnKey: 'Event'
      },
      sortState: {
        columnKey: undefined,
        orderType: undefined
      },
      loading: REQUEST_STATUS.idle
    };
    const expected = {
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal1',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ]
    };
    const state = reducer(initialState, action);
    expect(expected.entities).toEqual(state.entities);
  });

  it('should collapseAllRow', () => {
    const action = {
      type: collapseAllRow.type,
      payload: {
        osMatchId: 10019,
        columnKey: 'Trader',
        mode: 'collapse'
      }
    };
    const initialState = {
      collapseExpandAllRowState: {
        columnKey: 'Trader',
        mode: 'expand'
      },
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal1',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10019,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10019,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10019,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ]
    };
    const expected = {
      collapseExpandAllRowState: {
        osMatchId: 10019,
        columnKey: 'Trader',
        mode: 'collapse'
      },
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: [10018, 10019]
      },
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal1',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              hiden: false,
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10019,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              hiden: false,
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10019,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10019,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ]
    };
    const state = reducer(initialState, action);
    expect(expected).toEqual(state);
  });

  it('should expandAllRow', () => {
    const action = {
      type: expandAllRow.type,
      payload: {
        osMatchId: 10019,
        columnKey: 'Trader',
        mode: 'collapse'
      }
    };
    const initialState = {
      collapseExpandAllRowState: {
        columnKey: 'Trader',
        mode: 'expand'
      },
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal1',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10019,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10019,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10019,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ]
    };
    const expected = {
      collapseExpandAllRowState: {
        osMatchId: 10019,
        columnKey: 'Trader',
        mode: 'collapse'
      },
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entities: [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-06T01:00:00',
          home: 'Portal1',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              hiden: false,
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        },
        {
          osMatchId: 10019,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-05T01:00:00',
          home: 'Portal',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              hiden: false,
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10019,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10019,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ]
    };
    const state = reducer(initialState, action);
    //console.debug(state);
    expect(expected).toEqual(state);
  });

  it('should return state collapseRow', () => {
    const action = {
      type: collapseRow.type,
      payload: {
        osMatchId: 10018,
        columnKey: 'Trader',
        mode: 'collapse'
      }
    };
    const initialState = {
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entities: []
    };

    const newState = {
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'collapse',
        rowHasModeCollapse: [10018]
      },
      entities: []
    };

    const actual = reducer(initialState, action);

    expect(actual).toEqual(newState);
  });

  it('should return state expandRow', () => {
    const action = {
      type: expandRow.type,
      payload: {
        osMatchId: 10018,
        columnKey: 'Trader',
        mode: 'expand'
      }
    };
    const initialState = {
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'collapse',
        rowHasModeCollapse: []
      },
      entities: []
    };

    const newState = {
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: []
      },
      entities: []
    };

    const actual = reducer(initialState, action);

    expect(actual).toEqual(newState);
  });

  it('should return state not osmatchid expandRow', () => {
    const action = {
      type: expandRow.type,
      payload: {
        osMatchId: 10018,
        columnKey: 'Trader',
        mode: 'expand'
      }
    };
    const initialState = {
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'collapse',
        rowHasModeCollapse: [10018, 10019]
      },
      entities: []
    };

    const newState = {
      collapseExpandRowState: {
        columnKey: 'Trader',
        mode: 'expand',
        rowHasModeCollapse: [10019]
      },
      entities: []
    };

    const actual = reducer(initialState, action);

    expect(actual).toEqual(newState);
  });
});

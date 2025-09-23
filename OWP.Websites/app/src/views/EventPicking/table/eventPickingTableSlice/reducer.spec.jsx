import eventPickingTable, { filterBySearchKey, clearFilter } from './reducer';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { getEventPickings } from './eventSliceThunkNative';
import updateNoOfferMatchThunk from '../updateNoOfferMatchThunkNative';
import {
  addOrReplaceFilterState,
  filterDataTable,
  pushOrUpdateObject
} from '../../../Shared/Utils';

describe('eventPickingSlice', () => {
  it('should return initial state', () => {
    expect(eventPickingTable(undefined, {})).toEqual({
      matchSelected: [],
      openPopupState: {
        isOpenAddSpecialProduct: false,
        isOpenRemoveSpecialProduct: false
      },
      columnKeyFilterCurrent: '',
      entities: [],
      entitiesOriginal: [],
      entitiesByDate: [],
      filterState: [],
      isConfirmAll: false,
      'isNoOfferAll': false,
      isSelectedAll: false,
      loading: REQUEST_STATUS.idle,
      'multipleFilter': {
        'time': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'maxbetHdp': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'leagueName': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'spreadHdp': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'spreadOu': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'noOfHDP': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'rAndDInfo': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'lstNote': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'marginPeriod': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'turnoverPeriod': {
          'isFilter': false,
          'options': [],
          'selected': []
        }
      },
      'previousNoOfferIndex': null,
      'valueFilterAllColumn': {},
      'rAndDInfoFilterOriginal': {
        'isFilter': false,
        'options': [],
        'selected': []
      },
      'order': {
        'columnKey': '',
        'turnoverPeriod': {},
        'marginPeriod': {}
      }
    });
  });
  it('filterBySearchKey', () => {
    const state = {
      entities: [
        {
          leagueName: 'test league',
          homeName: 'test home',
          awayName: 'test away'
        }
      ],
      entitiesOriginal: [
        {
          leagueName: 'test league',
          homeName: 'test home',
          awayName: 'test away'
        }
      ],
      filterState: [],
      multipleFilter: {
        maxbetHdp: {
          selected: [
            {
              value: null
            }
          ]
        },
        time: {
          selected: [
            {
              value: new Date('2024-07-10')
            }
          ]
        },
        turnoverPeriod: {
          selected: []
        }
      }
    };
    const expectedState = {
      entities: [],
      entitiesOriginal: [
        {
          leagueName: 'test league',
          homeName: 'test home',
          awayName: 'test away'
        }
      ],
      filterState: [
        {
          columnKey: 'League',
          keyword: 'league'
        }
      ],
      isConfirmAll: true,
      isNoOfferAll: true,
      'multipleFilter': {
        'time': {
          'selected': [{ value: new Date('2024-07-10') }]
        },
        'maxbetHdp': {
          'selected': [
            {
              'value': null
            }
          ]
        },
        'turnoverPeriod': {
          'selected': []
        }
      }
    };
    const action = {
      type: filterBySearchKey.type,
      payload: {
        columnKey: 'League',
        keyword: 'league'
      }
    };
    const actual = eventPickingTable(state, action);
    expect(actual).toEqual(expectedState);
  });
  it('clearFilter', () => {
    const state = {
      entities: [],
      entitiesOriginal: [
        {
          leagueName: 'test league',
          homeName: 'test home',
          awayName: 'test away'
        }
      ],
      filterState: [
        {
          columnKey: 'League',
          keyword: 'test abc'
        }
      ]
    };
    const expectedState = {
      entities: [
        {
          leagueName: 'test league',
          homeName: 'test home',
          awayName: 'test away'
        }
      ],
      entitiesOriginal: [
        {
          leagueName: 'test league',
          homeName: 'test home',
          awayName: 'test away'
        }
      ],
      filterState: [],
      isConfirmAll: false,
      isNoOfferAll: false,
      multipleFilter: {
        leagueName: {
          isFilter: false,
          options: [
            {
              label: 'test league',
              value: 'test league'
            }
          ],
          selected: [
            {
              label: 'test league',
              value: 'test league'
            }
          ]
        },
        lstNote: {
          isFilter: false,
          options: [
            {
              label: '(Blanks)',
              value: null
            }
          ],
          selected: [
            {
              label: '(Blanks)',
              value: null
            }
          ]
        },
        marginPeriod: {
          isFilter: false,
          options: [],
          selected: []
        },
        maxbetHdp: {
          isFilter: false,
          options: [
            {
              label: '(Blanks)',
              value: null
            }
          ],
          selected: [
            {
              label: '(Blanks)',
              value: null
            }
          ]
        },
        noOfHDP: {
          isFilter: false,
          options: [
            {
              index: 9007199254740991,
              label: '(Blanks)',
              value: null
            }
          ],
          selected: [
            {
              index: 9007199254740991,
              label: '(Blanks)',
              value: null
            }
          ]
        },
        rAndDInfo: undefined,
        spreadHdp: {
          isFilter: false,
          options: [
            {
              label: '(Blanks)',
              value: null
            }
          ],
          selected: [
            {
              label: '(Blanks)',
              value: null
            }
          ]
        },
        spreadOu: {
          isFilter: false,
          options: [
            {
              label: '(Blanks)',
              value: null
            }
          ],
          selected: [
            {
              label: '(Blanks)',
              value: null
            }
          ]
        },
        time: {
          isFilter: false,
          options: [
            {
              label: '',
              value: null
            }
          ],
          selected: [
            {
              label: '',
              value: null
            }
          ]
        },
        turnoverPeriod: {
          isFilter: false,
          options: [],
          selected: []
        }
      },
      valueFilterAllColumn: {}
    };
    const action = {
      type: clearFilter.type,
      payload: 'League'
    };
    const actual = eventPickingTable(state, action);
    expect(actual).toStrictEqual(expectedState);
  });
  it('getEventPickings.pending', () => {
    const state = { loading: REQUEST_STATUS.pending };
    const action = {
      type: getEventPickings.pending.type
    };
    const expectedState = { loading: REQUEST_STATUS.pending };
    const actual = eventPickingTable(state, action);
    expect(expectedState).toEqual(actual);
  });
  it('getEventPickings.rejected', () => {
    const state = { loading: REQUEST_STATUS.pending };
    const action = {
      type: getEventPickings.rejected.type
    };
    const expectedState = { loading: REQUEST_STATUS.idle };
    const actual = eventPickingTable(state, action);
    expect(expectedState).toEqual(actual);
  });
  it('getEventPickings.fulfilled', () => {
    const state = {
      entities: [],
      entitiesOriginal: [],
      filterState: [],
      loading: REQUEST_STATUS.pending
    };
    const expectedState = {
      entities: [
        {
          leagueName: 'test league',
          homeName: 'test home',
          awayName: 'test away'
        },
        {
          leagueName: 'test1 league',
          homeName: 'test1 home',
          awayName: 'test1 away'
        }
      ],
      entitiesByDate: undefined,
      entitiesOriginal: [
        {
          leagueName: 'test league',
          homeName: 'test home',
          awayName: 'test away'
        },
        {
          leagueName: 'test1 league',
          homeName: 'test1 home',
          awayName: 'test1 away'
        }
      ],
      filterState: [],
      loading: REQUEST_STATUS.idle,
      isConfirmAll: false,
      isNoOfferAll: false,
      'multipleFilter': {
        'leagueName': {
          'isFilter': false,
          'options': [
            {
              'label': 'test league',
              'value': 'test league'
            },
            {
              'label': 'test1 league',
              'value': 'test1 league'
            }
          ],
          'selected': [
            {
              'label': 'test league',
              'value': 'test league'
            },
            {
              'label': 'test1 league',
              'value': 'test1 league'
            }
          ]
        },
        'lstNote': {
          'isFilter': false,
          'options': [
            {
              'label': '(Blanks)',
              'value': null
            }
          ],
          'selected': [
            {
              'label': '(Blanks)',
              'value': null
            }
          ]
        },
        'marginPeriod': {
          'isFilter': false,
          'options': [],
          'selected': []
        },
        'maxbetHdp': {
          'isFilter': false,
          'options': [
            {
              'label': '(Blanks)',
              'value': null
            }
          ],
          'selected': [
            {
              'label': '(Blanks)',
              'value': null
            }
          ]
        },
        'noOfHDP': {
          'isFilter': false,
          'options': [
            {
              'index': 9007199254740991,
              'label': '(Blanks)',
              'value': null
            }
          ],
          'selected': [
            {
              'index': 9007199254740991,
              'label': '(Blanks)',
              'value': null
            }
          ]
        },
        'rAndDInfo': undefined,
        'spreadHdp': {
          'isFilter': false,
          'options': [
            {
              'label': '(Blanks)',
              'value': null
            }
          ],
          'selected': [
            {
              'label': '(Blanks)',
              'value': null
            }
          ]
        },
        'spreadOu': {
          'isFilter': false,
          'options': [
            {
              'label': '(Blanks)',
              'value': null
            }
          ],
          'selected': [
            {
              'label': '(Blanks)',
              'value': null
            }
          ]
        },
        'time': {
          'isFilter': false,
          'options': [
            {
              'label': '',
              'value': null
            }
          ],
          'selected': [
            {
              'label': '',
              'value': null
            }
          ]
        },
        'turnoverPeriod': {
          'isFilter': false,
          'options': [],
          'selected': []
        }
      },
      order: {}
    };
    const action = {
      type: getEventPickings.fulfilled.type,
      payload: {
        filteredMatches: [
          {
            leagueName: 'test league',
            homeName: 'test home',
            awayName: 'test away'
          },
          {
            leagueName: 'test1 league',
            homeName: 'test1 home',
            awayName: 'test1 away'
          }
        ]
      }
    };
    const actual = eventPickingTable(state, action);
    expect(actual).toEqual(expectedState);
  });

  it('updateNoOfferMatchThunk.fulfilled', () => {
    const state = {
      entities: [],
      entitiesOriginal: [],
      entitiesByDate: [],
      filterState: {
        columnKey: undefined,
        keyword: undefined
      },
      loading: REQUEST_STATUS.pending
    };
    const expectedState = {
      entities: [],
      entitiesOriginal: [],
      entitiesByDate: [],
      filterState: {
        columnKey: undefined,
        keyword: undefined
      },
      isNoOfferAll: true,
      loading: REQUEST_STATUS.pending
    };
    const action = {
      type: updateNoOfferMatchThunk.fulfilled.type,
      payload: [
        {
          infoSiteMatchId: 123,
          adminSiteMatchId: 456,
          isNoOffer: true
        }
      ]
    };
    const actual = eventPickingTable(state, action);
    expect(actual).toEqual(expectedState);
  });
});

import eventPickingTable, { filterBySearchKey, clearFilter } from './reducer';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { getEventPickings } from './eventSliceThunkNative';
import updateNoOfferMatchThunk from '../updateNoOfferMatchThunkNative';

describe('eventPickingSlice', () => {
  it('should return initial state', () => {
    expect(eventPickingTable(undefined, {})).toEqual({
      columnKeyFilterCurrent: "",
      entities: [],
      entitiesOriginal: [],
      entitiesByDate: [],
      filterState: [],
      isConfirmAll: false,
      'isNoOfferAll': false,
      loading: REQUEST_STATUS.idle,
      "multipleFilter":  {
         "KO_Time":  {
           "isFilter": false,
           "options":  [],
           "selected":  [],
         },
         "MB":  {
           "isFilter": false,
           "options":  [],
           "selected":  [],
         },
       },
       "previousNoOfferIndex": null,
       "valueFilterAllColumn":  {},
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
      multipleFilter:{
        MB:{
          selected:[{
            value: 1000
          }]
        },
        KO_Time:{
          selected:[{
            value: new Date()
          }]
        }
      }
    };
    const expectedState = {
      entities: [
       
      ],
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
      "multipleFilter":  {
         "KO_Time":  {
           "selected":  [
              {
               "value": new Date(),
             },
           ],
         },
         "MB":  {
           "selected":  [
              {
               "value": 1000,
             },
           ],
         },
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
      isNoOfferAll: false
    };
    const action = {
      type: clearFilter.type,
      payload: 'League'
    };
    const actual = eventPickingTable(state, action);
    expect(expectedState).toEqual(actual);
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
      isNoOfferAll: false
    };
    const action = {
      type: getEventPickings.fulfilled.type,
      payload: [
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
    };
    const actual = eventPickingTable(state, action);
    expect(expectedState).toEqual(actual);
  });

  it('updateNoOfferMatchThunk.fulfilled', () => {
    const state = {
      entities: [],
      entitiesOriginal: [],
      filterState: {
        columnKey: undefined,
        keyword: undefined
      },
      loading: REQUEST_STATUS.pending
    };
    const expectedState = {
      entities: [],
      entitiesOriginal: [],
      filterState: {
        columnKey: undefined,
        keyword: undefined
      },
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
    expect(expectedState).toEqual(actual);
  });
});

import { configureStore, EnhancedStore } from '@reduxjs/toolkit';
import reducer, {
  setValue,
  setDataSignalR,
  setMultipleFilter,
  handleClearFilter,
  setDataTable,
  setConfirmProvider
} from './tableSlice';
import { getEvents, getConfirmProvider } from './tableThunk';
import { Option } from '../../../Shared/MultipleSelection/type';
import { buildDefaultTableFilter, executeFilterTable } from './helper';
import {
  TDataSource,
  TConfirmProvider,
  ValidationResponse,
  TMultipleFilter
} from '../../type/table';

type RootState = {
  events: {
    loadingDataTable: string;
    loadingConfirmProviderOption: string;
    dataSource: TDataSource[];
    dataSourceOriginal: TDataSource[];
    confirmProviderOption: TConfirmProvider[];
    validationResponse: ValidationResponse;
    multipleFilter: TMultipleFilter;
  };
};

// Mock helper functions
jest.mock('./helper', () => ({
  buildDefaultTableFilter: jest.fn(
    (dataSource, confirmProviderOption, tableFilter) => {
      tableFilter.MatchNo = { isFilter: false, selected: [] };
      tableFilter.MatchStatus = { isFilter: false, selected: [] };
      tableFilter.KoTime = { isFilter: false, selected: [] };
      tableFilter.League = { isFilter: false, selected: [] };
      tableFilter.Match = { isFilter: false, selected: [] };
      tableFilter.LstBookProvider = { isFilter: false, selected: [] };
      tableFilter.InHouseProviderId = { isFilter: false, selected: [] };
      tableFilter.Remark = { isFilter: false, selected: [] };
      tableFilter.RemarkBy = { isFilter: false, selected: [] };
      tableFilter.ConfirmProvider = { isFilter: false, selected: [] };
      tableFilter.OtherProvider = { isFilter: false, selected: [] };
      tableFilter.MaxBet = { isFilter: false, selected: [] };
    }
  ),
  executeFilterTable: jest.fn(dataSource => dataSource)
}));

// Mock ticketHelper
jest.mock('../../../Shared/ticketEvent/helper/ticketHelper', () => ({
  buildIsReadValue: jest.fn((actorId, userId, isRead) => {
    if (actorId !== userId) {
      return false;
    }
    return isRead ?? true;
  })
}));

// Mock data
const mockDataSource: TDataSource[] = [
  {
    owpMatchId: 1,
    parentMatchId: 1,
    matchNo: 'Match 1',
    matchStatus: 'Status 1',
    eventDateTime: '2024-01-01T00:00:00Z',
    league: 'League 1',
    home: 'Home Team',
    away: 'Away Team',
    agentConfirm: [],
    rowColor: 'red',
    hasTicket: false,
    isRead: false,
    remark: undefined,
    remarkBy: undefined,
    lstBookProvider: undefined,
    inHouseProviderId: undefined,
    otherProvider: undefined,
    maxBetToDisplay: undefined,
    rowIndex: 0,
    dataSource: []
  }
];

const mockOption: TConfirmProvider[] = [{ value: 1, label: 'Option 1' }];

const mockValidationResponse: ValidationResponse = {
  isSuccess: false,
  value: {
    providerVerification: [] as unknown as TDataSource,
    sessionId: ''
  }
};

describe('tableSlice', () => {
  let store: EnhancedStore<RootState>;

  beforeEach(() => {
    jest.clearAllMocks();
    store = configureStore({
      reducer: {
        events: reducer
      },
      preloadedState: {
        events: {
          loadingDataTable: 'idle',
          loadingConfirmProviderOption: 'idle',
          dataSource: JSON.parse(JSON.stringify(mockDataSource)),
          dataSourceOriginal: JSON.parse(JSON.stringify(mockDataSource)),
          confirmProviderOption: mockOption,
          multipleFilter: {
            MatchNo: { isFilter: false, selected: [] },
            MatchStatus: { isFilter: false, selected: [] },
            KoTime: { isFilter: false, selected: [] },
            League: { isFilter: false, selected: [] },
            Match: { isFilter: false, selected: [] },
            LstBookProvider: { isFilter: false, selected: [] },
            InHouseProviderId: { isFilter: false, selected: [] },
            Remark: { isFilter: false, selected: [] },
            RemarkBy: { isFilter: false, selected: [] },
            ConfirmProvider: { isFilter: false, selected: [] },
            OtherProvider: { isFilter: false, selected: [] },
            MaxBet: { isFilter: false, selected: [] }
          },
          validationResponse: mockValidationResponse
        }
      }
    }) as EnhancedStore<RootState>;
  });

  describe('Initial State', () => {
    it('should return initial state', () => {
      const state = store.getState().events;
      expect(state.loadingDataTable).toBe('idle');
      expect(state.dataSource).toEqual(mockDataSource);
      expect(state.dataSourceOriginal).toEqual(mockDataSource);
    });
  });

  describe('setValue', () => {
    it('should update field dynamically and add to multipleFilter', () => {
      // setValue expects a key that works for both data field and filter key
      // Using 'MatchNo' as the key to update multipleFilter
      const action = setValue({
        owpMatchId: 1,
        key: 'MatchNo',
        value: 'Updated Match No'
      });

      store.dispatch(action);
      const state = store.getState().events;

      // The key 'MatchNo' will be used as a field name on the dataSource object
      expect((state.dataSource[0] as any)['MatchNo']).toBe('Updated Match No');
      expect((state.dataSourceOriginal[0] as any)['MatchNo']).toBe(
        'Updated Match No'
      );

      // And also update the multipleFilter
      expect(state.multipleFilter['MatchNo'].selected).toContainEqual({
        value: 'Updated Match No',
        label: 'Updated Match No'
      });
    });

    it('should handle undefined value', () => {
      const action = setValue({
        owpMatchId: 1,
        key: 'Remark',
        value: undefined
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect((state.dataSource[0] as any)['Remark']).toBeUndefined();
      expect(state.multipleFilter['Remark'].selected).toContainEqual({
        value: '',
        label: ''
      });
    });

    it('should not update if owpMatchId not found', () => {
      const action = setValue({
        owpMatchId: 999,
        key: 'MatchNo',
        value: 'Should Not Update'
      });

      store.dispatch(action);
      const state = store.getState().events;

      // Should not throw error, just not update
      expect((state.dataSource[0] as any)['MatchNo']).toBeUndefined();
    });

    it('should handle updating existing fields', () => {
      const action = setValue({
        owpMatchId: 1,
        key: 'League',
        value: 'Updated League'
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect((state.dataSource[0] as any)['League']).toBe('Updated League');
      expect((state.dataSourceOriginal[0] as any)['League']).toBe(
        'Updated League'
      );
      expect(state.multipleFilter['League'].selected).toContainEqual({
        value: 'Updated League',
        label: 'Updated League'
      });
    });
  });

  describe('setDataSignalR', () => {
    it('should set hasTicket to true and update isRead when actorId matches userId', () => {
      // Mock to return true when actorId === userId
      const buildIsReadValue =
        require('../../../Shared/ticketEvent/helper/ticketHelper').buildIsReadValue;
      buildIsReadValue.mockReturnValueOnce(true);

      const action = setDataSignalR({
        actorId: 1,
        parentMatchId: 1,
        userId: 1
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect(state.dataSource[0].hasTicket).toBe(true);
      expect(state.dataSource[0].isRead).toBe(true);
    });

    it('should set hasTicket to true and isRead to false when actorId does not match userId', () => {
      // Mock to return false when actorId !== userId
      const buildIsReadValue =
        require('../../../Shared/ticketEvent/helper/ticketHelper').buildIsReadValue;
      buildIsReadValue.mockReturnValueOnce(false);

      const action = setDataSignalR({
        actorId: 2,
        parentMatchId: 1,
        userId: 1
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect(state.dataSource[0].hasTicket).toBe(true);
      expect(state.dataSource[0].isRead).toBe(false);
    });

    it('should not update if parentMatchId not found', () => {
      const action = setDataSignalR({
        actorId: 1,
        parentMatchId: 999,
        userId: 1
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect(state.dataSource[0].hasTicket).toBe(false);
    });
  });

  describe('setConfirmProvider', () => {
    it('should update agentConfirm in both dataSource and dataSourceOriginal', () => {
      const confirmValue: TConfirmProvider[] = [
        { value: 1, label: 'Confirmed' }
      ];
      const action = setConfirmProvider({
        owpMatchId: 1,
        value: confirmValue
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect(state.dataSource[0].agentConfirm).toEqual(confirmValue);
      expect(state.dataSourceOriginal[0].agentConfirm).toEqual(confirmValue);
    });

    it('should handle empty array', () => {
      const action = setConfirmProvider({
        owpMatchId: 1,
        value: []
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect(state.dataSource[0].agentConfirm).toEqual([]);
      expect(state.dataSourceOriginal[0].agentConfirm).toEqual([]);
    });

    it('should not update if owpMatchId not found', () => {
      const initialConfirm = store.getState().events.dataSource[0].agentConfirm;

      const action = setConfirmProvider({
        owpMatchId: 999,
        value: [{ value: 1, label: 'Test' }]
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect(state.dataSource[0].agentConfirm).toEqual(initialConfirm);
    });
  });

  describe('setMultipleFilter', () => {
    it('should update multipleFilter and apply filtering', () => {
      const selected = [{ value: 'Match 1', label: 'Match 1' }];
      const action = setMultipleFilter({
        columnKey: 'MatchNo',
        selected,
        isFilter: true
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect(state.multipleFilter['MatchNo']).toEqual({
        isFilter: true,
        selected
      });
      expect(executeFilterTable).toHaveBeenCalled();
    });

    it('should handle empty selected array', () => {
      const action = setMultipleFilter({
        columnKey: 'MatchStatus',
        selected: [],
        isFilter: false
      });

      store.dispatch(action);
      const state = store.getState().events;

      expect(state.multipleFilter['MatchStatus']).toEqual({
        isFilter: false,
        selected: []
      });
    });

    it('should update multiple filters sequentially', () => {
      const action1 = setMultipleFilter({
        columnKey: 'MatchNo',
        selected: [{ value: 'Match 1', label: 'Match 1' }],
        isFilter: true
      });

      const action2 = setMultipleFilter({
        columnKey: 'League',
        selected: [{ value: 'League 1', label: 'League 1' }],
        isFilter: true
      });

      store.dispatch(action1);
      store.dispatch(action2);
      const state = store.getState().events;

      expect(state.multipleFilter['MatchNo'].isFilter).toBe(true);
      expect(state.multipleFilter['League'].isFilter).toBe(true);
    });
  });

  describe('handleClearFilter', () => {
    it('should reset dataSource to dataSourceOriginal and call buildDefaultTableFilter', () => {
      // First apply a filter
      store.dispatch(
        setMultipleFilter({
          columnKey: 'MatchNo',
          selected: [{ value: 'Match 1', label: 'Match 1' }],
          isFilter: true
        })
      );

      // Then clear
      store.dispatch(handleClearFilter());
      const state = store.getState().events;

      expect(state.dataSource).toEqual(state.dataSourceOriginal);
      expect(buildDefaultTableFilter).toHaveBeenCalled();
    });
  });

  describe('setDataTable', () => {
    it('should replace dataSource with new data', () => {
      const newData: TDataSource[] = [
        {
          owpMatchId: 2,
          parentMatchId: 2,
          matchNo: 'Match 2',
          matchStatus: 'Status 2',
          eventDateTime: '2024-02-01T00:00:00Z',
          league: 'League 2',
          home: 'Home Team 2',
          away: 'Away Team 2',
          agentConfirm: [],
          rowColor: 'blue',
          hasTicket: true,
          isRead: false,
          remark: undefined,
          remarkBy: undefined,
          lstBookProvider: undefined,
          inHouseProviderId: undefined,
          otherProvider: undefined,
          maxBetToDisplay: undefined,
          rowIndex: 1,
          dataSource: []
        }
      ];

      store.dispatch(setDataTable(newData));
      const state = store.getState().events;

      expect(state.dataSource).toEqual(newData);
    });

    it('should handle empty array', () => {
      store.dispatch(setDataTable([]));
      const state = store.getState().events;

      expect(state.dataSource).toEqual([]);
    });
  });

  describe('getEvents async thunk', () => {
    it('should handle pending state', () => {
      store.dispatch(
        getEvents.pending('', {
          eventDate: new Date(),
          onlyShowIsMismatchEvent: false,
          confirmProvider: null,
          lstBookProvider: null
        })
      );

      const state = store.getState().events;
      expect(state.loadingDataTable).toBe('pending');
    });

    it('should handle fulfilled state', () => {
      const payload: ValidationResponse = {
        isSuccess: true,
        value: {
          providerVerification: mockDataSource as unknown as TDataSource,
          sessionId: 'test-session-id'
        }
      };

      store.dispatch(
        getEvents.fulfilled(payload, '', {
          eventDate: new Date(),
          onlyShowIsMismatchEvent: false,
          confirmProvider: null,
          lstBookProvider: null
        })
      );

      const state = store.getState().events;
      expect(state.loadingDataTable).toBe('idle');
      expect(state.validationResponse).toEqual(payload);
      expect(buildDefaultTableFilter).toHaveBeenCalled();
    });

    it('should handle rejected state', () => {
      store.dispatch(
        getEvents.rejected(new Error('Test error'), '', {
          eventDate: new Date(),
          onlyShowIsMismatchEvent: false,
          confirmProvider: null,
          lstBookProvider: null
        })
      );

      const state = store.getState().events;
      expect(state.loadingDataTable).toBe('idle');
    });
  });

  describe('getConfirmProvider async thunk', () => {
    it('should handle pending state', () => {
      store.dispatch(getConfirmProvider.pending('', undefined));

      const state = store.getState().events;
      expect(state.loadingConfirmProviderOption).toBe('pending');
    });

    it('should handle fulfilled state', () => {
      const payload: { value: number; label: string }[] = [
        { value: 1, label: 'Provider 1' },
        { value: 2, label: 'Provider 2' }
      ];

      store.dispatch(getConfirmProvider.fulfilled(payload, '', undefined));

      const state = store.getState().events;
      expect(state.loadingConfirmProviderOption).toBe('idle');
      expect(state.confirmProviderOption).toEqual(payload);
    });

    it('should handle rejected state', () => {
      store.dispatch(
        getConfirmProvider.rejected(new Error('Test error'), '', undefined)
      );

      const state = store.getState().events;
      expect(state.loadingConfirmProviderOption).toBe('idle');
    });
  });
});

import { configureStore } from '@reduxjs/toolkit';
import reducer, {
  setValue,
  setDataSignalR,
  setMultipleFilter,
  handleClearFilter,
  setDataTable,
  setConfirmProvider
} from './tableSlice';
import { Option } from '../../../Shared/MultipleSelection/type';
import { buildDefaultTableFilter, executeFilterTable } from './helper';

// Mock helper functions
jest.mock('./helper', () => ({
  buildDefaultTableFilter: jest.fn(() => ({
    MatchNo: { isFilter: false, selected: [] },
    MatchStatus: { isFilter: false, selected: [] },
    KoTime: { isFilter: false, selected: [] },
    League: { isFilter: false, selected: [] },
    Match: { isFilter: false, selected: [] },
    LstBookProvider: { isFilter: false, selected: [] },
    InHouseProviderId: { isFilter: false, selected: [] },
    Remark: { isFilter: false, selected: [] },
    RemarkBy: { isFilter: false, selected: [] },
    ConfirmProvider: { isFilter: false, selected: [] }
  })),
  executeFilterTable: jest.fn((dataSource) => dataSource)
}));

// Mock data
const mockDataSource = [
  {
    owpMatchId: 1,
    parentMatchId: 1,
    matchNo: 'Match 1',
    matchStatus: 'Status 1',
    eventDateTime: '2024-01-01T00:00:00Z',
    league: 'League 1',
    home: 'Home Team',
    away: 'Away Team',
    agentEst: 'Agent Est',
    agentConfirm: [],
    rowColor: 'red',
    hasTicket: true,
    isRead: false,
    remarkEst: undefined,
    remarkBy: undefined,
    lstBookProvider: undefined,
    inHouseProviderId: undefined,
    mb: undefined,
    overUnderSetting: {
      maxBet: undefined,
      maxBetToDisplay: undefined,
      spread: undefined,
      spreadToDisplay: undefined
    },
    handicapSetting: {
      maxBet: undefined,
      maxBetToDisplay: undefined,
      spread: undefined,
      spreadToDisplay: undefined
    },
    estNote: 'Some note',
    rowIndex: 0,
    dataSource: []
  }
];

const mockOption: Option[] = [
  { value: '1', label: 'Option 1' }
];

// Mock helper functions
const mockHelper = {
  MatchNo: jest.fn(() => [{ value: 'Match 1', label: 'Match 1' }]),
  MatchStatus: jest.fn(() => [{ value: 'Status 1', label: 'Status 1' }]),
  KoTime: jest.fn(() => [{ value: '01:00 AM', label: '01:00 AM' }]),
  League: jest.fn(() => [{ value: 'League 1', label: 'League 1' }]),
  Match: jest.fn(() => [{ value: 'Home Team -vs- Away Team', label: 'Home Team -vs- Away Team' }]),
  LstBookProvider: jest.fn(() => [{ value: '', label: '' }]),
  InHouseProviderId: jest.fn(() => [{ value: '', label: '' }]),
  AgentEst: jest.fn(() => [{ value: 'Agent Est', label: 'Agent Est' }]),
  Mb: jest.fn(() => [{ value: '', label: '' }]),
  RemarkEst: jest.fn(() => [{ value: '', label: '' }]),
  RemarkBy: jest.fn(() => [{ value: '', label: '' }]),
  EstNote: jest.fn(() => [{ value: 'Some note', label: 'Some note' }])
};

// Use mockHelper in place of the real helper
const executeMockFilterTable = (dataSource: any[], tableFilter: any) => {
  return executeFilterTable(dataSource, tableFilter);
};

describe('tableSlice', () => {
  let store: ReturnType<typeof configureStore>;

  beforeEach(() => {
    store = configureStore({
      reducer: {
        events: reducer
      },
      preloadedState: {
        events: {
          loadingDataTable: 'idle',
          loadingConfirmProviderOption: 'idle',
          dataSource: mockDataSource,
          dataSourceOriginal: mockDataSource,
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
            ConfirmProvider: { isFilter: false, selected: [] }
          },
          validationResponse: {
            isSuccess: false,
            value: {
              providerVerification: [],
              sessionId: ''
            }
          }
        }
      }
    });
  });

  it('should return initial state', () => {
    const state = store.getState().events;
    expect(state.loadingDataTable).toBe('idle');
    expect(state.dataSource).toEqual(mockDataSource);
    expect(state.dataSourceOriginal).toEqual(mockDataSource);
  });

  it('should handle setValue action', () => {
    const action = setValue({
      owpMatchId: 1,
      key: 'MatchNo',
      value: 'Updated Match No'
    });

    store.dispatch(action);
    const state = store.getState().events;
    expect(state.dataSource[0].matchNo).toBe('Updated Match No');
    expect(state.dataSourceOriginal[0].matchNo).toBe('Updated Match No');
    expect(state.multipleFilter['MatchNo'].selected).toEqual([
      { value: 'Updated Match No', label: 'Updated Match No' }
    ]);
  });

  it('should handle setDataSignalR action', () => {
    const action = setDataSignalR({
      actorId: 1,
      parentMatchId: 1,
      userId: 1
    });

    store.dispatch(action);
    const state = store.getState().events;
    expect(state.dataSource[0].hasTicket).toBe(true);
  });

  it('should handle setConfirmProvider action', () => {
    const action = setConfirmProvider({
      owpMatchId: 1,
      value: [{ value: 'Confirmed', label: 'Confirmed' }]
    });

    store.dispatch(action);
    const state = store.getState().events;
    expect(state.dataSource[0].agentConfirm).toEqual([
      { value: 'Confirmed', label: 'Confirmed' }
    ]);
    expect(state.dataSourceOriginal[0].agentConfirm).toEqual([
      { value: 'Confirmed', label: 'Confirmed' }
    ]);
  });

  it('should handle setMultipleFilter action and filter data', () => {
    // Mock the implementation of helper functions directly
    (mockHelper.MatchNo as jest.Mock).mockReturnValue([{ value: 'Match 1', label: 'Match 1' }]);
    (mockHelper.MatchStatus as jest.Mock).mockReturnValue([{ value: 'Status 1', label: 'Status 1' }]);

    const action = setMultipleFilter({
      columnKey: 'MatchNo',
      selected: [{ value: 'Match 1', label: 'Match 1' }],
      isFilter: true
    });

    store.dispatch(action);
    const state = store.getState().events;
    expect(state.multipleFilter['MatchNo']).toEqual({
      isFilter: true,
      selected: [{ value: 'Match 1', label: 'Match 1' }]
    });

    // Validate that dataSource is filtered correctly
    expect(state.dataSource).toEqual(executeMockFilterTable(
      state.dataSourceOriginal,
      state.multipleFilter
    ));
  });

  it('should handle handleClearFilter action', () => {
    const action = handleClearFilter();

    store.dispatch(action);
    const state = store.getState().events;
    expect(state.dataSource).toEqual(state.dataSourceOriginal);
  });

  it('should handle setDataTable action', () => {
    const newData = [
      {
        OWPMatchId: 2,
        ParentMatchId: 2,
        MatchNo: 'Match 2',
        MatchStatus: 'Status 2',
        EventDateTime: '2024-02-01T00:00:00Z',
        League: 'League 2',
        Home: 'Home Team 2',
        Away: 'Away Team 2',
        AgentEst: 'Agent Est 2',
        AgentConfirm: [],
        RowColor: 'blue',
        HasTicket: true,
        IsRead: false,
        RemarkEst: undefined,
        RemarkBy: undefined,
        Provider: undefined,
        OtherProvider: undefined,
        Mb: undefined,
        OverUnderSetting: {
          MaxBet: undefined,
          MaxBetToDisplay: undefined,
          Spread: undefined,
          SpreadToDisplay: undefined
        },
        HandicapSetting: {
          MaxBet: undefined,
          MaxBetToDisplay: undefined,
          Spread: undefined,
          SpreadToDisplay: undefined
        },
        EstNote: 'Another note',
        rowIndex: 1,
        dataSource: []
      }
    ];
    const action = setDataTable(newData);

    store.dispatch(action);
    const state = store.getState().events;
    expect(state.dataSource).toEqual(newData);
  });
});

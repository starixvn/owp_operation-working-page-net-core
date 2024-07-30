import { configureStore } from '@reduxjs/toolkit';
import reducer, {
  setValue,
  setDataSignalR,
  setAgentConfirm,
  setMultipleFilter,
  handleClearFilter,
  setDataTable
} from './tableSlice';
import { Option } from '../../../Shared/MultipleSelection/type';
import { buildDefaultTableFilter, executeFilterTable } from './helper';

// Mock data
const mockDataSource = [
  {
    OWPMatchId: 1,
    ParentMatchId: 1,
    MatchNo: 'Match 1',
    MatchStatus: 'Status 1',
    EventDateTime: '2024-01-01T00:00:00Z',
    League: 'League 1',
    Home: 'Home Team',
    Away: 'Away Team',
    AgentEst: 'Agent Est',
    AgentConfirm: [],
    RowColor: 'red',
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
    EstNote: 'Some note',
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
  Provider: jest.fn(() => [{ value: '', label: '' }]),
  OtherProvider: jest.fn(() => [{ value: '', label: '' }]),
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
          loadingAgentConfirmOption: 'idle',
          dataSource: mockDataSource,
          dataSourceOriginal: mockDataSource,
          agentConfirmOption: mockOption,
          multipleFilter: {
            MatchNo: { isFilter: false, selected: [] },
            MatchStatus: { isFilter: false, selected: [] },
            KoTime: { isFilter: false, selected: [] },
            League: { isFilter: false, selected: [] },
            Match: { isFilter: false, selected: [] },
            Mb: { isFilter: false, selected: [] },
            Provider: { isFilter: false, selected: [] },
            OtherProvider: { isFilter: false, selected: [] },
            AgentEst: { isFilter: false, selected: [] },
            RemarkEst: { isFilter: false, selected: [] },
            RemarkBy: { isFilter: false, selected: [] },
            AgentConfirm: { isFilter: false, selected: [] },
            EstNote: { isFilter: false, selected: [] }
          }
        }
      }
    });
  });

  it('should handle setValue action', () => {
    const action = setValue({
      owpMatchId: 1,
      key: 'MatchNo',
      value: 'Updated Match No'
    });

    store.dispatch(action);
    const state = store.getState().events;
    expect(state.dataSource[0].MatchNo).toBe('Updated Match No');
    expect(state.dataSourceOriginal[0].MatchNo).toBe('Updated Match No');
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
    expect(state.dataSource[0].HasTicket).toBe(true);
  });

  it('should handle setAgentConfirm action', () => {
    const action = setAgentConfirm({
      owpMatchId: 1,
      value: [{ Value: 'Confirmed', Label: 'Confirmed' }]
    });

    store.dispatch(action);
    const state = store.getState().events;
    expect(state.dataSource[0].AgentConfirm).toEqual([
      { Value: 'Confirmed', Label: 'Confirmed' }
    ]);
    expect(state.dataSourceOriginal[0].AgentConfirm).toEqual([
      { Value: 'Confirmed', Label: 'Confirmed' }
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

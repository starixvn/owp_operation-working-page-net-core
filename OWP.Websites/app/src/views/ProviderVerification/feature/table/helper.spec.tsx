import {
  buildDefaultTableFilter,
  executeFilterTable,
  buildLabelItemOption
} from './helper';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import { TDataSource, TMultipleFilter } from '../../type/table';
import helper from './calculateFilterOption';

jest.mock('../../../Shared/DateTime/dateTimeHelper', () => ({
  formatTime: jest.fn()
}));

jest.mock('./calculateFilterOption', () => ({
  MatchNo: jest.fn(),
  MatchStatus: jest.fn(),
  KoTime: jest.fn(),
  League: jest.fn(),
  Match: jest.fn(),
  Provider: jest.fn(),
  OtherProvider: jest.fn(),
  AgentEst: jest.fn(),
  Mb: jest.fn(),
  MaxBet: jest.fn(),
  RemarkEst: jest.fn(),
  RemarkBy: jest.fn(),
  EstNote: jest.fn(),
  LstBookProvider: jest.fn()
}));

describe('buildDefaultTableFilter', () => {
  const dataSource = [];
  const confirmProviderOption = [{ value: 1, label: 'Agent 1' }];
  let tableFilter: TMultipleFilter;

  beforeEach(() => {
    tableFilter = {};
    // Mock the helper functions to return empty arrays
    helper.MatchNo = jest.fn().mockReturnValue([]);
    helper.MatchStatus = jest.fn().mockReturnValue([]);
    helper.KoTime = jest.fn().mockReturnValue([]);
    helper.League = jest.fn().mockReturnValue([]);
    helper.Match = jest.fn().mockReturnValue([]);
    helper.LstBookProvider = jest.fn().mockReturnValue([]);
    helper.InHouseProviderId = jest.fn().mockReturnValue([]);
    helper.Remark = jest.fn().mockReturnValue({ selected: [] });
    helper.RemarkBy = jest.fn().mockReturnValue({ selected: [] });
    helper.MaxBet = jest.fn().mockReturnValue({ selected: [] });
    helper.OtherProvider = jest.fn().mockReturnValue({ selected: [] });
    helper.AgentEst = jest.fn().mockReturnValue({ selected: [] });
    helper.Mb = jest.fn().mockReturnValue({ selected: [] });
    helper.RemarkEst = jest.fn().mockReturnValue({ selected: [] });
    helper.EstNote = jest.fn().mockReturnValue({ selected: [] });
    helper.LstBookProvider = jest.fn().mockReturnValue({ selected: [] });
  });

  it('should populate tableFilter with default values', () => {
    buildDefaultTableFilter(dataSource, confirmProviderOption, tableFilter);

    expect(tableFilter.MatchNo).toEqual({
      selected: [],
      isFilter: false
    });
    expect(tableFilter.ConfirmProvider).toEqual({
      selected: [...confirmProviderOption, { value: -1, label: '(Blanks)' }],
      isFilter: false
    });
  });
});

describe('executeFilterTable', () => {
  const dataSource: TDataSource[] = [
    {
      owpMatchId: 1,
      parentMatchId: 1,
      matchNo: '1',
      matchStatus: 'Scheduled',
      eventDateTime: '2024-07-11 01:00:00',
      home: 'Team A',
      away: 'Team B',
      league: 'League 1',
      lstBookProvider: 'Provider A',
      inHouseProviderId: 'Provider B',
      remark: 'Remark 1',
      remarkBy: 'User A',
      maxBetToDisplay: '100',
      otherProvider: 'Other Provider',
      agentConfirm: [{ label: 'Agent 1' }],
      rowColor: 'red',
      isRead: false,
      hasTicket: false,
      rowIndex: 0,
      dataSource: []
    }
  ];

  const tableFilter: TMultipleFilter = {
    MatchNo: {
      selected: [{ value: '1', label: '1' }],
      isFilter: false
    },
    MatchStatus: {
      selected: [{ value: 'Scheduled', label: 'Scheduled' }],
      isFilter: false
    },
    KoTime: {
      selected: [{ value: '01:00 AM', label: '01:00 AM' }],
      isFilter: false
    },
    League: {
      selected: [{ value: 'League 1', label: 'League 1' }],
      isFilter: false
    },
    Match: {
      selected: [{ value: 'Team A -vs- Team B', label: 'Team A -vs- Team B' }],
      isFilter: false
    },
    LstBookProvider: {
      selected: [{ value: 'Provider A', label: 'Provider A' }],
      isFilter: false
    },
    InHouseProviderId: {
      selected: [{ value: 'Provider B', label: 'Provider B' }],
      isFilter: false
    },
    Remark: {
      selected: [{ value: 'Remark 1', label: 'Remark 1' }],
      isFilter: false
    },
    RemarkBy: {
      selected: [{ value: 'User A', label: 'User A' }],
      isFilter: false
    },
    OtherProvider: {
      selected: [{ value: 'Other Provider', label: 'Other Provider' }],
      isFilter: false
    },
    MaxBet: {
      selected: [{ value: '100', label: '100' }],
      isFilter: false
    },
    ConfirmProvider: {
      selected: [{ value: 1, label: 'Agent 1' }],
      isFilter: false
    }
  };

  beforeEach(() => {
    formatTime.mockReturnValue('01:00 AM');
    // Mock all helper functions
    helper.MatchNo = jest.fn().mockReturnValue([]);
    helper.MatchStatus = jest.fn().mockReturnValue([]);
    helper.KoTime = jest.fn().mockReturnValue([]);
    helper.League = jest.fn().mockReturnValue([]);
    helper.Match = jest.fn().mockReturnValue([]);
    helper.LstBookProvider = jest.fn().mockReturnValue([]);
    helper.InHouseProviderId = jest.fn().mockReturnValue([]);
    helper.Remark = jest.fn().mockReturnValue({ selected: [] });
    helper.RemarkBy = jest.fn().mockReturnValue({ selected: [] });
    helper.MaxBet = jest.fn().mockReturnValue({ selected: [] });
    helper.OtherProvider = jest.fn().mockReturnValue({ selected: [] });
    helper.AgentEst = jest.fn().mockReturnValue({ selected: [] });
    helper.Mb = jest.fn().mockReturnValue({ selected: [] });
    helper.RemarkEst = jest.fn().mockReturnValue({ selected: [] });
    helper.EstNote = jest.fn().mockReturnValue({ selected: [] });
  });

  it('should return filtered data when all filters match', () => {
    const result = executeFilterTable(dataSource, tableFilter);
    expect(result).toHaveLength(1);
  });

  it('should return empty array if no matches found', () => {
    const noMatchFilter = {
      ...tableFilter,
      MatchNo: {
        selected: [{ value: '2', label: '2' }],
        isFilter: false
      }
    };
    const result = executeFilterTable(dataSource, noMatchFilter);
    expect(result).toHaveLength(0);
  });
});

describe('buildLabelItemOption', () => {
  it('should return value if not empty', () => {
    expect(buildLabelItemOption('test')).toBe('test');
  });

  it('should return (Blanks) if value is empty', () => {
    expect(buildLabelItemOption('')).toBe('(Blanks)');
    expect(buildLabelItemOption('   ')).toBe('(Blanks)');
    expect(buildLabelItemOption(null)).toBe('(Blanks)');
    expect(buildLabelItemOption(undefined)).toBe('(Blanks)');
  });
});

import {
  buildMaxbetItemInOption,
  buildDefaultTableFilter,
  executeFilterTable,
  buildLabelItemOption
} from './helper';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import { TDataSource, TMaxbetSetting, TMultipleFilter } from '../../type/table';
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
  RemarkEst: jest.fn(),
  RemarkBy: jest.fn(),
  EstNote: jest.fn()
}));

describe('buildMaxbetItemInOption', () => {
  it('should return (Blanks) if no HandicapSetting and OverUnderSetting', () => {
    const result = buildMaxbetItemInOption({});
    expect(result).toBe('(Blanks)');
  });

  it('should build string with HandicapSetting', () => {
    const maxbetSetting = {
      HandicapSetting: { MaxBetToDisplay: '100', SpreadToDisplay: '2.5' }
    };
    const result = buildMaxbetItemInOption(maxbetSetting);
    expect(result).toBe('100(2.5\n\n  )');
  });

  it('should build string with OverUnderSetting', () => {
    const maxbetSetting = {
      HandicapSetting: { MaxBetToDisplay: '100', SpreadToDisplay: '2.5' },
      OverUnderSetting: { SpreadToDisplay: '3.0' }
    };
    const result = buildMaxbetItemInOption(maxbetSetting);
    expect(result).toBe('100(2.5\n-3.0\n  )');
  });
});

describe('buildDefaultTableFilter', () => {
  const dataSource = [];
  const agentConfirmOption = [{ value: 1, label: 'Agent 1' }];
  let tableFilter: TMultipleFilter;

  beforeEach(() => {
    tableFilter = {};
  });

  it('should populate tableFilter with default values', () => {
    buildDefaultTableFilter(dataSource, agentConfirmOption, tableFilter);

    expect(tableFilter.MatchNo).toEqual({
      selected: helper.MatchNo(dataSource),
      isFilter: false
    });
    expect(tableFilter.AgentConfirm).toEqual({
      selected: [...agentConfirmOption, { value: -1, label: '(Blanks)' }],
      isFilter: false
    });
  });
});

describe('executeFilterTable', () => {
  const dataSource: TDataSource[] = [
    {
      MatchNo: '1',
      MatchStatus: 'Scheduled',
      EventDateTime: '2024-07-11 01:00:00',
      Home: 'Team A',
      Away: 'Team B',
      League: 'League 1',
      Provider: 'Provider A',
      OtherProvider: 'Provider B',
      AgentEst: 'Agent X',
      HandicapSetting: { MaxBetToDisplay: '100', SpreadToDisplay: '2.5' },
      OverUnderSetting: { SpreadToDisplay: '3.0' },
      RemarkEst: 'Remark 1',
      RemarkBy: 'User A',
      EstNote: 'Note 1',
      AgentConfirm: [{ Label: 'Agent 1' }],
      RowColor: 'red',
      IsRead: false,
      HasTicket: false,
      OWPMatchId: 1
    },
    {
      MatchNo: '2',
      MatchStatus: 'Completed',
      EventDateTime: '2024-07-11 01:00:00',
      Home: 'Team C',
      Away: 'Team D',
      League: 'League 2',
      Provider: 'Provider C',
      OtherProvider: 'Provider D',
      AgentEst: 'Agent Y',
      HandicapSetting: { MaxBetToDisplay: '200', SpreadToDisplay: '1.5' },
      OverUnderSetting: { SpreadToDisplay: '2.0' },
      RemarkEst: 'Remark 2',
      RemarkBy: 'User B',
      EstNote: 'Note 2',
      AgentConfirm: [{ Label: 'Agent 2' }],
      RowColor: 'blue',
      IsRead: false,
      HasTicket: false,
      OWPMatchId: 2
    }
  ];

  const tableFilter: TMultipleFilter = {
    MatchNo: { selected: [{ label: '1' }], isFilter: true },
    MatchStatus: { selected: [{ label: 'Scheduled' }], isFilter: true },
    KoTime: { selected: [{ label: 'formatted time' }], isFilter: true },
    League: { selected: [{ label: 'League 1' }], isFilter: true },
    Match: { selected: [{ label: 'Team A -vs- Team B' }], isFilter: true },
    Provider: { selected: [{ label: 'Provider A' }], isFilter: true },
    OtherProvider: { selected: [{ label: 'Provider B' }], isFilter: true },
    AgentEst: { selected: [{ label: 'Agent X' }], isFilter: true },
    Mb: { selected: [{ label: '100(2.5\n-3.0\n  )' }], isFilter: true },
    RemarkEst: { selected: [{ label: 'Remark 1' }], isFilter: true },
    RemarkBy: { selected: [{ label: 'User A' }], isFilter: true },
    EstNote: { selected: [{ label: 'Note 1' }], isFilter: true },
    AgentConfirm: { selected: [{ label: 'Agent 1' }], isFilter: true }
  };

  beforeEach(() => {
    (formatTime as jest.Mock).mockReturnValue('formatted time');
  });

  it('should return empty array if no matches found', () => {
    const result = executeFilterTable(dataSource, tableFilter);
    expect(result).toHaveLength(1);
  });

  it('should return empty array if no matches found', () => {
    tableFilter.MatchNo.selected = [{ label: '3' }];
    const result = executeFilterTable(dataSource, tableFilter);
    expect(result).toHaveLength(0);
  });
});

describe('buildLabelItemOption', () => {
  it('should return (Blanks) for null or undefined', () => {
    expect(buildLabelItemOption(null)).toBe('(Blanks)');
    expect(buildLabelItemOption(undefined)).toBe('(Blanks)');
  });

  it('should return (Blanks) for empty string', () => {
    expect(buildLabelItemOption('')).toBe('(Blanks)');
  });

  it('should return trimmed value', () => {
    expect(buildLabelItemOption('  value  ')).toBe('  value  ');
  });
});

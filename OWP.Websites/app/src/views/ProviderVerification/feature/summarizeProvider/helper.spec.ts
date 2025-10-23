import { summarize, compareDifferent } from './helper';
import { Option } from '../../../Shared/MultipleSelection/type';
import { TDataSource } from '../../type/table';
import { TCompareCountResult } from '../../type/provider';

describe('summarize', () => {
  test('correctly summarizes agent counts', () => {
    const data: TDataSource[] = [
      {
        agentConfirm: [
          { value: 1, label: 'Agent 1' },
          { value: 2, label: 'Agent 2' }
        ],
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: '1',
        matchStatus: 'active',
        eventDateTime: '2023-01-01',
        league: 'League 1',
        home: 'Home',
        away: 'Away',
        rowColor: undefined,
        hasTicket: false,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      },
      {
        agentConfirm: [{ value: 1, label: 'Agent 1' }],
        owpMatchId: 2,
        parentMatchId: 2,
        matchNo: '2',
        matchStatus: 'active',
        eventDateTime: '2023-01-01',
        league: 'League 2',
        home: 'Home',
        away: 'Away',
        rowColor: undefined,
        hasTicket: false,
        isRead: false,
        rowIndex: 1,
        dataSource: []
      }
    ];

    const agentOption: Option[] = [
      { value: '1', label: 'Agent 1' },
      { value: '2', label: 'Agent 2' }
    ];

    const result = summarize(data, agentOption);

    expect(result).toEqual([
      { agentName: 'Agent 1', count: 2 },
      { agentName: 'Agent 2', count: 1 }
    ]);
  });

  test('handles empty data gracefully', () => {
    const data: TDataSource[] = [];
    const agentOption: Option[] = [
      { value: '1', label: 'Agent 1' },
      { value: '2', label: 'Agent 2' }
    ];

    const result = summarize(data, agentOption);

    expect(result).toEqual([
      { agentName: 'Agent 1', count: 0 },
      { agentName: 'Agent 2', count: 0 }
    ]);
  });

  test('handles empty agent options', () => {
    const data: TDataSource[] = [
      {
        agentConfirm: [{ value: 1, label: 'Agent 1' }],
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: '1',
        matchStatus: 'active',
        eventDateTime: '2023-01-01',
        league: 'League 1',
        home: 'Home',
        away: 'Away',
        rowColor: undefined,
        hasTicket: false,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      }
    ];

    const agentOption: Option[] = [];

    const result = summarize(data, agentOption);

    expect(result).toEqual([]);
  });

  test('correctly counts agents appearing multiple times', () => {
    const data: TDataSource[] = [
      {
        agentConfirm: [
          { value: 1, label: 'Agent 1' },
          { value: 1, label: 'Agent 1' }
        ],
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: '1',
        matchStatus: 'active',
        eventDateTime: '2023-01-01',
        league: 'League 1',
        home: 'Home',
        away: 'Away',
        rowColor: undefined,
        hasTicket: false,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      }
    ];

    const agentOption: Option[] = [{ value: '1', label: 'Agent 1' }];

    const result = summarize(data, agentOption);

    expect(result).toEqual([{ agentName: 'Agent 1', count: 2 }]);
  });

  test('ignores agents not in agent options', () => {
    const data: TDataSource[] = [
      {
        agentConfirm: [
          { value: 1, label: 'Agent 1' },
          { value: 3, label: 'Agent 3' }
        ],
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: '1',
        matchStatus: 'active',
        eventDateTime: '2023-01-01',
        league: 'League 1',
        home: 'Home',
        away: 'Away',
        rowColor: undefined,
        hasTicket: false,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      }
    ];

    const agentOption: Option[] = [
      { value: '1', label: 'Agent 1' },
      { value: '2', label: 'Agent 2' }
    ];

    const result = summarize(data, agentOption);

    expect(result).toEqual([
      { agentName: 'Agent 1', count: 1 },
      { agentName: 'Agent 2', count: 0 }
    ]);
  });
});

describe('compareDifferent', () => {
  test('correctly identifies differences when agent times do not match data source', () => {
    const agentRawData = '2024-07-08 01:00 PM\n2024-07-08 03:00 PM\n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08T13:00:00Z', // formats to 01:00 PM
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      },
      {
        lstBookProvider: 'Provider2',
        inHouseProviderId: 'OtherProvider2',
        remark: 'Remark2',
        remarkBy: 'RemarkBy2',
        owpMatchId: 2,
        parentMatchId: 2,
        matchNo: 'Match2',
        matchStatus: 'Status2',
        eventDateTime: '2024-07-08T14:00:00Z', // formats to 02:00 PM
        league: 'League2',
        home: 'Home2',
        away: 'Away2',
        agentConfirm: [{ value: 2, label: 'Agent2' }],
        rowColor: 'Blue',
        hasTicket: false,
        isRead: true,
        rowIndex: 1,
        dataSource: []
      }
    ];

    // Agent data has full date-time strings, but dataSource formats to time only
    // So they don't match and all agent times are reported as differences
    const result: TCompareCountResult[] = [
      { koTime: '2024-07-08 01:00 PM', dataCount: 0, agentFileCount: 1 },
      { koTime: '2024-07-08 03:00 PM', dataCount: 0, agentFileCount: 1 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });

  test('handles empty agent raw data gracefully', () => {
    const agentRawData = '';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08T13:00:00Z',
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual([]);
  });

  test('handles empty data source gracefully', () => {
    const agentRawData = '01:00 PM\n02:00 PM\n';
    const dataSource: TDataSource[] = [];

    const result: TCompareCountResult[] = [
      { koTime: '01:00 PM', dataCount: 0, agentFileCount: 1 },
      { koTime: '02:00 PM', dataCount: 0, agentFileCount: 1 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });

  test('returns empty result if all counts match', () => {
    const agentRawData = '01:00 PM\n02:00 PM\n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08 13:00:00', // formats to 01:00 PM
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      },
      {
        lstBookProvider: 'Provider2',
        inHouseProviderId: 'OtherProvider2',
        remark: 'Remark2',
        remarkBy: 'RemarkBy2',
        owpMatchId: 2,
        parentMatchId: 2,
        matchNo: 'Match2',
        matchStatus: 'Status2',
        eventDateTime: '2024-07-08 14:00:00', // formats to 02:00 PM
        league: 'League2',
        home: 'Home2',
        away: 'Away2',
        agentConfirm: [{ value: 2, label: 'Agent2' }],
        rowColor: 'Blue',
        hasTicket: false,
        isRead: true,
        rowIndex: 1,
        dataSource: []
      }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual([]);
  });

  test('returns discrepancy when agent has time not in data source', () => {
    const agentRawData = '01:00 PM\n02:00 PM\n03:00 PM\n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08 13:00:00', // formats to 01:00 PM
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      },
      {
        lstBookProvider: 'Provider2',
        inHouseProviderId: 'OtherProvider2',
        remark: 'Remark2',
        remarkBy: 'RemarkBy2',
        owpMatchId: 2,
        parentMatchId: 2,
        matchNo: 'Match2',
        matchStatus: 'Status2',
        eventDateTime: '2024-07-08 14:00:00', // formats to 02:00 PM
        league: 'League2',
        home: 'Home2',
        away: 'Away2',
        agentConfirm: [{ value: 2, label: 'Agent2' }],
        rowColor: 'Blue',
        hasTicket: false,
        isRead: true,
        rowIndex: 1,
        dataSource: []
      }
    ];

    // 03:00 PM is in agent data but not in data source
    const result: TCompareCountResult[] = [
      { koTime: '03:00 PM', dataCount: 0, agentFileCount: 1 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });

  test('handles multiple entries at the same time', () => {
    const agentRawData = '01:00 PM\n01:00 PM\n02:00 PM\n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08 13:00:00', // formats to 01:00 PM
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      },
      {
        lstBookProvider: 'Provider2',
        inHouseProviderId: 'OtherProvider2',
        remark: 'Remark2',
        remarkBy: 'RemarkBy2',
        owpMatchId: 2,
        parentMatchId: 2,
        matchNo: 'Match2',
        matchStatus: 'Status2',
        eventDateTime: '2024-07-08 14:00:00', // formats to 02:00 PM
        league: 'League2',
        home: 'Home2',
        away: 'Away2',
        agentConfirm: [{ value: 2, label: 'Agent2' }],
        rowColor: 'Blue',
        hasTicket: false,
        isRead: true,
        rowIndex: 1,
        dataSource: []
      }
    ];

    // Agent has 2 entries at 01:00 PM, data source has only 1
    const result: TCompareCountResult[] = [
      { koTime: '01:00 PM', dataCount: 1, agentFileCount: 2 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });

  test('filters out "Time" header row from agent data', () => {
    const agentRawData = 'Time\n01:00 PM\n02:00 PM\n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08 13:00:00', // formats to 01:00 PM
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      }
    ];

    // "Time" header should be filtered out, only check 01:00 PM and 02:00 PM
    const result: TCompareCountResult[] = [
      { koTime: '02:00 PM', dataCount: 0, agentFileCount: 1 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });

  test('handles tab-separated values in agent data', () => {
    const agentRawData = '01:00 PM\textra\tdata\n02:00 PM\tmore\tinfo\n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08 13:00:00', // formats to 01:00 PM
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      }
    ];

    // Only first column (before tab) should be used for koTime
    const result: TCompareCountResult[] = [
      { koTime: '02:00 PM', dataCount: 0, agentFileCount: 1 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });

  test('handles whitespace in agent data', () => {
    const agentRawData = '  01:00 PM  \n\n  02:00 PM  \n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08 13:00:00', // formats to 01:00 PM (without leading/trailing spaces)
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      }
    ];

    // Empty lines are filtered, but whitespace is preserved in koTime (not trimmed after split)
    const result: TCompareCountResult[] = [
      { koTime: '  01:00 PM  ', dataCount: 0, agentFileCount: 1 },
      { koTime: '  02:00 PM  ', dataCount: 0, agentFileCount: 1 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });

  test('does not report data source times not in agent data', () => {
    const agentRawData = '01:00 PM\n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: '2024-07-08 13:00:00', // formats to 01:00 PM
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      },
      {
        lstBookProvider: 'Provider2',
        inHouseProviderId: 'OtherProvider2',
        remark: 'Remark2',
        remarkBy: 'RemarkBy2',
        owpMatchId: 2,
        parentMatchId: 2,
        matchNo: 'Match2',
        matchStatus: 'Status2',
        eventDateTime: '2024-07-08 14:00:00', // formats to 02:00 PM (not in agent data)
        league: 'League2',
        home: 'Home2',
        away: 'Away2',
        agentConfirm: [{ value: 2, label: 'Agent2' }],
        rowColor: 'Blue',
        hasTicket: false,
        isRead: true,
        rowIndex: 1,
        dataSource: []
      }
    ];

    // Function only checks agent keys, so 02:00 PM in data source is not reported
    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual([]);
  });

  test('handles null or invalid eventDateTime in data source', () => {
    const agentRawData = '01:00 PM\n';
    const dataSource: TDataSource[] = [
      {
        lstBookProvider: 'Provider1',
        inHouseProviderId: null,
        remark: 'Remark1',
        remarkBy: 'RemarkBy1',
        owpMatchId: 1,
        parentMatchId: 1,
        matchNo: 'Match1',
        matchStatus: 'Status1',
        eventDateTime: null as any, // null eventDateTime
        league: 'League1',
        home: 'Home1',
        away: 'Away1',
        agentConfirm: [{ value: 1, label: 'Agent1' }],
        rowColor: 'Red',
        hasTicket: true,
        isRead: false,
        rowIndex: 0,
        dataSource: []
      },
      {
        lstBookProvider: 'Provider2',
        inHouseProviderId: 'OtherProvider2',
        remark: 'Remark2',
        remarkBy: 'RemarkBy2',
        owpMatchId: 2,
        parentMatchId: 2,
        matchNo: 'Match2',
        matchStatus: 'Status2',
        eventDateTime: '', // empty eventDateTime
        league: 'League2',
        home: 'Home2',
        away: 'Away2',
        agentConfirm: [{ value: 2, label: 'Agent2' }],
        rowColor: 'Blue',
        hasTicket: false,
        isRead: true,
        rowIndex: 1,
        dataSource: []
      }
    ];

    // null/empty eventDateTime should be filtered out by formatTime returning null
    const result: TCompareCountResult[] = [
      { koTime: '01:00 PM', dataCount: 0, agentFileCount: 1 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });
});

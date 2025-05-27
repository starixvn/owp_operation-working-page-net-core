import { summarize, compareDifferent } from './helper'; // Replace with the correct path
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import { Option } from '../../../Shared/MultipleSelection/type';
import { TDataSource } from '../../type/table';
import { TAgentCompare, TCompareCountResult } from '../../type/provider';

describe('summarize', () => {
  test('correctly summarizes agent counts', () => {
    const data: TDataSource[] = [
      {
        agentConfirm: [{ value: 1, label: 'Agent 1' }, { value: 2, label: 'Agent 2' }],
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
});

describe('compareDifferent', () => {
  test('correctly identifies differences between agent data and data source', () => {
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
        eventDateTime: '2024-07-08T13:00:00Z', // 01:00 PM
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
        eventDateTime: '2024-07-08T14:00:00Z', // 02:00 PM
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
        eventDateTime: '2024-07-08T13:00:00Z', // 01:00 PM
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

    const result: TCompareCountResult[] = [];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
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

  test('returns empty result if no discrepancies found', () => {
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
        eventDateTime: '2024-07-08T13:00:00Z', // 01:00 PM
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
        eventDateTime: '2024-07-08T14:00:00Z', // 02:00 PM
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

    const result: TCompareCountResult[] = [
      { koTime: '01:00 PM', dataCount: 0, agentFileCount: 1 },
      { koTime: '02:00 PM', dataCount: 0, agentFileCount: 1 },
      { koTime: '03:00 PM', dataCount: 0, agentFileCount: 1 }
    ];

    const differences = compareDifferent(agentRawData, dataSource);

    expect(differences).toEqual(result);
  });
});

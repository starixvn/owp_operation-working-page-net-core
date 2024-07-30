import { summarize, compareDifferent } from './helper'; // Replace with the correct path
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import { Option } from '../../../Shared/MultipleSelection/type';
import { TDataSource } from '../../type/table';
import { TAgentCompare, TCompareCountResult } from '../../type/provider';

describe('summarize', () => {
  test('correctly summarizes agent counts', () => {
    const data: TDataSource[] = [
      { AgentConfirm: [{ Label: 'Agent 1' }, { Label: 'Agent 2' }] },
      { AgentConfirm: [{ Label: 'Agent 1' }] }
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
        Provider: 'Provider1',
        OtherProvider: null,
        RemarkEst: 'Remark1',
        RemarkBy: 'RemarkBy1',
        OWPMatchId: 1,
        ParentMatchId: 1,
        MatchNo: 'Match1',
        MatchStatus: 'Status1',
        EventDateTime: '2024-07-08T13:00:00Z', // 01:00 PM
        League: 'League1',
        Home: 'Home1',
        Away: 'Away1',
        AgentEst: 'AgentEst1',
        AgentConfirm: [{ Value: 1, Label: 'Agent1' }],
        RowColor: 'Red',
        HandicapSetting: {
          MaxBet: 10,
          MaxBetToDisplay: '10',
          Spread: 5,
          SpreadToDisplay: '5'
        },
        OverUnderSetting: {
          MaxBet: 20,
          MaxBetToDisplay: '20',
          Spread: 10,
          SpreadToDisplay: '10'
        },
        HasTicket: true,
        IsRead: false,
        rowIndex: 0,
        dataSource: [],
        EstNote: 'Note 1'
      },
      {
        Provider: 'Provider2',
        OtherProvider: 'OtherProvider2',
        RemarkEst: 'Remark2',
        RemarkBy: 'RemarkBy2',
        OWPMatchId: 2,
        ParentMatchId: 2,
        MatchNo: 'Match2',
        MatchStatus: 'Status2',
        EventDateTime: '2024-07-08T14:00:00Z', // 02:00 PM
        League: 'League2',
        Home: 'Home2',
        Away: 'Away2',
        AgentEst: 'AgentEst2',
        AgentConfirm: [{ Value: 2, Label: 'Agent2' }],
        RowColor: 'Blue',
        HandicapSetting: {
          MaxBet: 15,
          MaxBetToDisplay: '15',
          Spread: 7,
          SpreadToDisplay: '7'
        },
        OverUnderSetting: {
          MaxBet: 25,
          MaxBetToDisplay: '25',
          Spread: 12,
          SpreadToDisplay: '12'
        },
        HasTicket: false,
        IsRead: true,
        rowIndex: 1,
        dataSource: [],
        EstNote: 'Note 1'
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
        Provider: 'Provider1',
        OtherProvider: null,
        RemarkEst: 'Remark1',
        RemarkBy: 'RemarkBy1',
        OWPMatchId: 1,
        ParentMatchId: 1,
        MatchNo: 'Match1',
        MatchStatus: 'Status1',
        EventDateTime: '2024-07-08T13:00:00Z', // 01:00 PM
        League: 'League1',
        Home: 'Home1',
        Away: 'Away1',
        AgentEst: 'AgentEst1',
        AgentConfirm: [{ Value: 1, Label: 'Agent1' }],
        RowColor: 'Red',
        HandicapSetting: {
          MaxBet: 10,
          MaxBetToDisplay: '10',
          Spread: 5,
          SpreadToDisplay: '5'
        },
        OverUnderSetting: {
          MaxBet: 20,
          MaxBetToDisplay: '20',
          Spread: 10,
          SpreadToDisplay: '10'
        },
        HasTicket: true,
        IsRead: false,
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
        Provider: 'Provider1',
        OtherProvider: null,
        RemarkEst: 'Remark1',
        RemarkBy: 'RemarkBy1',
        OWPMatchId: 1,
        ParentMatchId: 1,
        MatchNo: 'Match1',
        MatchStatus: 'Status1',
        EventDateTime: '2024-07-08T13:00:00Z', // 01:00 PM
        League: 'League1',
        Home: 'Home1',
        Away: 'Away1',
        AgentEst: 'AgentEst1',
        AgentConfirm: [{ Value: 1, Label: 'Agent1' }],
        RowColor: 'Red',
        HandicapSetting: {
          MaxBet: 10,
          MaxBetToDisplay: '10',
          Spread: 5,
          SpreadToDisplay: '5'
        },
        OverUnderSetting: {
          MaxBet: 20,
          MaxBetToDisplay: '20',
          Spread: 10,
          SpreadToDisplay: '10'
        },
        HasTicket: true,
        IsRead: false,
        rowIndex: 0,
        dataSource: []
      },
      {
        Provider: 'Provider2',
        OtherProvider: 'OtherProvider2',
        RemarkEst: 'Remark2',
        RemarkBy: 'RemarkBy2',
        OWPMatchId: 2,
        ParentMatchId: 2,
        MatchNo: 'Match2',
        MatchStatus: 'Status2',
        EventDateTime: '2024-07-08T14:00:00Z', // 02:00 PM
        League: 'League2',
        Home: 'Home2',
        Away: 'Away2',
        AgentEst: 'AgentEst2',
        AgentConfirm: [{ Value: 2, Label: 'Agent2' }],
        RowColor: 'Blue',
        HandicapSetting: {
          MaxBet: 15,
          MaxBetToDisplay: '15',
          Spread: 7,
          SpreadToDisplay: '7'
        },
        OverUnderSetting: {
          MaxBet: 25,
          MaxBetToDisplay: '25',
          Spread: 12,
          SpreadToDisplay: '12'
        },
        HasTicket: false,
        IsRead: true,
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

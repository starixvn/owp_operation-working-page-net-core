import reducer, { sumProvider } from './summarizeProviderSlice'; // Adjust the import based on your file structure
import { summarize } from './helper';
import { TProviderCount } from '../../type/provider';
import { TDataSource } from '../../type/table';
import { Option } from '../../../Shared/MultipleSelection/type';

jest.mock('./helper', () => ({
  summarize: jest.fn()
}));

describe('sumProviderSlice', () => {
  it('should handle sumProvider', () => {
    const mockData: TDataSource[] = [
      {
        Provider: 'Provider1',
        OtherProvider: null,
        RemarkEst: 'Remark1',
        RemarkBy: 'RemarkBy1',
        OWPMatchId: 1,
        ParentMatchId: 1,
        MatchNo: 'Match1',
        MatchStatus: 'Status1',
        EventDateTime: '2024-07-08T13:00:00Z',
        League: 'League1',
        Home: 'Home1',
        Away: 'Away1',
        AgentEst: 'AgentEst1',
        AgentConfirm: [{ Value: 1, Label: 'Agent1' }],
        RowColor: 'Red',
        HandicapSetting: { MaxBet: 10, MaxBetToDisplay: '10', Spread: 5, SpreadToDisplay: '5' },
        OverUnderSetting: { MaxBet: 20, MaxBetToDisplay: '20', Spread: 10, SpreadToDisplay: '10' },
        HasTicket: true,
        IsRead: false,
        rowIndex: 0,
        dataSource: []
      }
    ];
    const agentOptions: Option[] = [
      { value: '1', label: 'Agent1' }
    ];

    (summarize as jest.Mock).mockReturnValue([{ agentName: 'Agent1', count: 1 }]);

    const action = {
      type: sumProvider.type,
      payload: { mockData, agentOptions }
    };

    const actual = reducer(undefined, action);

    const expected: { sumTable: Array<{ agentName: string; count: number }> } = {
      sumTable: [{ agentName: 'Agent1', count: 1 }]
    };

    expect(actual.sumTable).toEqual(expected.sumTable);
  });
});

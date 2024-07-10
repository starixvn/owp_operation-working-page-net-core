import helper from './calculateFilterOption';
import { Option } from "../../../Shared/MultipleSelection/type";
import { TDataSource } from '../../type/table';

const mockDataSource: TDataSource[] = [
  {
    OWPMatchId: 1,
    ParentMatchId: 1,
    MatchNo: 'Match 1',
    MatchStatus: 'Status 1',
    EventDateTime: '2024-07-10T15:00:00Z',
    League: 'League 1',
    Home: 'Team A',
    Away: 'Team B',
    AgentEst: 'Agent 1',
    AgentConfirm: [],
    RowColor: 'red',
    EstNote: 'Note 1',
    RemarkEst: 'Remark 1',
    RemarkBy: 'RemarkBy 1',
    HandicapSetting: { MaxBet: 100, MaxBetToDisplay: '100', Spread: 5, SpreadToDisplay: '5' },
    OverUnderSetting: { MaxBet: 200, MaxBetToDisplay: '200', Spread: 10, SpreadToDisplay: '10' },
    HasTicket: true,
    IsRead: false,
    rowIndex: 0,
    dataSource: []
  },
  {
    OWPMatchId: 2,
    ParentMatchId: 2,
    MatchNo: 'Match 2',
    MatchStatus: 'Status 2',
    EventDateTime: '2024-07-10T16:00:00Z',
    League: 'League 2',
    Home: 'Team C',
    Away: 'Team D',
    AgentEst: 'Agent 2',
    AgentConfirm: [],
    RowColor: 'blue',
    EstNote: 'Note 2',
    RemarkEst: 'Remark 2',
    RemarkBy: 'RemarkBy 2',
    HandicapSetting: { MaxBet: 300, MaxBetToDisplay: '300', Spread: 15, SpreadToDisplay: '15' },
    OverUnderSetting: { MaxBet: 400, MaxBetToDisplay: '400', Spread: 20, SpreadToDisplay: '20' },
    HasTicket: false,
    IsRead: true,
    rowIndex: 1,
    dataSource: []
  }
];

describe('helper', () => {
  describe('calculateMatchNoOption', () => {
    it('should return options with unique labels for match numbers', () => {
      const result = helper.MatchNo(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: 'Match 1' },
        { value: 2, label: 'Match 2' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateMatchStatusOption', () => {
    it('should return options with unique labels for match statuses', () => {
      const result = helper.MatchStatus(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: 'Status 1' },
        { value: 2, label: 'Status 2' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateKoOption', () => {
    it('should return options sorted by event date time', () => {
      const result = helper.KoTime(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: '10:00 PM' },
        { value: 2, label: '11:00 PM' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateLeagueOption', () => {
    it('should return options with unique labels for leagues', () => {
      const result = helper.League(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: 'League 1' },
        { value: 2, label: 'League 2' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateMatchOption', () => {
    it('should return options with unique labels for match descriptions', () => {
      const result = helper.Match(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: 'Team A -vs- Team B' },
        { value: 2, label: 'Team C -vs- Team D' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateProviderOption', () => {
    it('should return options with unique labels for providers', () => {
      const result = helper.Provider(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: '(Blanks)' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateOtherProviderOption', () => {
    it('should return options with unique labels for other providers', () => {
      const result = helper.OtherProvider(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: '(Blanks)' },
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateAgentOption', () => {
    it('should return options with unique values for agents', () => {
      const result = helper.AgentEst(mockDataSource);
      const expected: Option[] = [
        { value: 'Agent 1', label: 'Agent 1' },
        { value: 'Agent 2', label: 'Agent 2' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateMbOption', () => {
    it('should return options sorted by max bet value', () => {
      const result = helper.Mb(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: '100(5\n-10\n  )' },
        { value: 2, label: '300(15\n-20\n  )' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateRemarkOption', () => {
    it('should return options with unique labels for remarks', () => {
      const result = helper.RemarkEst(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: 'Remark 1' },
        { value: 2, label: 'Remark 2' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateRemarkByOption', () => {
    it('should return options with unique labels for remarks by', () => {
      const result = helper.RemarkBy(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: 'RemarkBy 1' },
        { value: 2, label: 'RemarkBy 2' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateAgentConfirmOption', () => {
    it('should return options with an additional blank option', () => {
      const result = helper.AgentConfirm(mockDataSource);
      const expected: Option[] = [
        { value: -1, label: '(Blanks)' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateEstNoteOption', () => {
    it('should return options with unique labels for estimate notes', () => {
      const result = helper.EstNote(mockDataSource);
      const expected: Option[] = [
        { value: 'Note 1', label: 'Note 1' },
        { value: 'Note 2', label: 'Note 2' }
      ];
      expect(result).toEqual(expected);
    });
  });
});

import helper from './calculateFilterOption';
import { Option } from "../../../Shared/MultipleSelection/type";
import { TDataSource } from '../../type/table';

const mockDataSource: TDataSource[] = [
  {
    owpMatchId: 1,
    parentMatchId: 1,
    matchNo: 'Match 1',
    matchStatus: 'Status 1',
    eventDateTime: '2024-07-10T15:00:00Z',
    league: 'League 1',
    home: 'Team A',
    away: 'Team B',
    lstBookProvider: 'Provider 1',
    inHouseProviderId: 'InHouse 1',
    remark: 'Remark 1',
    remarkBy: 'RemarkBy 1',
    agentConfirm: [{ value: 'Agent 1', label: 'Agent 1' }],
    rowColor: 'red',
    hasTicket: true,
    isRead: false,
    rowIndex: 0,
    dataSource: []
  },
  {
    owpMatchId: 2,
    parentMatchId: 2,
    matchNo: 'Match 2',
    matchStatus: 'Status 2',
    eventDateTime: '2024-07-10T16:00:00Z',
    league: 'League 2',
    home: 'Team C',
    away: 'Team D',
    lstBookProvider: 'Provider 2',
    inHouseProviderId: 'InHouse 2',
    remark: 'Remark 2',
    remarkBy: 'RemarkBy 2',
    agentConfirm: [{ value: 'Agent 2', label: 'Agent 2' }],
    rowColor: 'blue',
    hasTicket: false,
    isRead: true,
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

  describe('calculateLSTBookProviderOption', () => {
    it('should return options with unique labels for LST book providers', () => {
      const result = helper.LstBookProvider(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: 'Provider 1' },
        { value: 2, label: 'Provider 2' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateInhouseProviderIdOption', () => {
    it('should return options with unique labels for in-house provider IDs', () => {
      const result = helper.InHouseProviderId(mockDataSource);
      const expected: Option[] = [
        { value: 1, label: 'InHouse 1' },
        { value: 2, label: 'InHouse 2' }
      ];
      expect(result).toEqual(expected);
    });
  });

  describe('calculateRemarkOption', () => {
    it('should return options with unique labels for remarks', () => {
      const result = helper.Remark(mockDataSource);
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

  describe('calculateConfirmProviderOption', () => {
    it('should return options with an additional blank option', () => {
      const result = helper.ConfirmProvider();
      const expected: Option[] = [
        { value: -1, label: '(Blanks)' }
      ];
      expect(result).toEqual(expected);
    });
  });
});

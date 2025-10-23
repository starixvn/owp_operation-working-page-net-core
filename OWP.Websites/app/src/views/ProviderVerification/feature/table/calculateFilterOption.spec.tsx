import helper from './calculateFilterOption';
import { Option } from '../../../Shared/MultipleSelection/type';
import { TDataSource } from '../../type/table';

// Mock dependencies
jest.mock('../../../Shared/DateTime/dateTimeHelper', () => ({
  formatTime: jest.fn(
    (value: string, startOf: string, formatOutput: string) => {
      if (!value) return null;
      // Simple mock implementation for testing
      const date = new Date(value);
      const hours = date.getUTCHours();
      const minutes = date.getUTCMinutes();
      const period = hours >= 12 ? 'PM' : 'AM';
      const hour12 = hours % 12 || 12;
      const paddedHour = hour12.toString().padStart(2, '0');
      const paddedMinute = minutes.toString().padStart(2, '0');
      return `${paddedHour}:${paddedMinute} ${period}`;
    }
  )
}));

jest.mock('./helper', () => ({
  buildLabelItemOption: jest.fn((value: string | undefined | null) =>
    value?.trim().length ? value : '(Blanks)'
  )
}));

const mockConfirmProviderOption = [
  { value: 1, label: 'Provider A' },
  { value: 2, label: 'Provider B' }
];

jest.mock('../../app/storeNative', () => ({
  __esModule: true,
  default: {
    getState: () => ({
      events: {
        confirmProviderOption: mockConfirmProviderOption
      }
    })
  }
}));

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
    otherProvider: 'Other 1',
    maxBetToDisplay: 'MaxBet 1',
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
    otherProvider: 'Other 2',
    maxBetToDisplay: 'MaxBet 2',
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

describe('calculateFilterOption helper', () => {
  describe('MatchNo', () => {
    it('should return unique options for match numbers', () => {
      const result = helper.MatchNo(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'Match 1' },
        { value: 2, label: 'Match 2' }
      ]);
    });

    it('should handle null/undefined/empty match numbers with (Blanks)', () => {
      const dataWithBlanks: TDataSource[] = [
        { ...mockDataSource[0], matchNo: '' },
        { ...mockDataSource[1], owpMatchId: 3, matchNo: 'Match 3' }
      ];

      const result = helper.MatchNo(dataWithBlanks);

      expect(result).toEqual([
        { value: 3, label: 'Match 3' },
        { value: 1, label: '(Blanks)' }
      ]);
    });

    it('should remove duplicate labels', () => {
      const dataWithDuplicates: TDataSource[] = [
        mockDataSource[0],
        { ...mockDataSource[0], owpMatchId: 3 }
      ];

      const result = helper.MatchNo(dataWithDuplicates);

      expect(result).toHaveLength(1);
      expect(result[0].label).toBe('Match 1');
    });
  });

  describe('MatchStatus', () => {
    it('should return unique options for match statuses', () => {
      const result = helper.MatchStatus(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'Status 1' },
        { value: 2, label: 'Status 2' }
      ]);
    });

    it('should handle blank match statuses', () => {
      const dataWithBlanks: TDataSource[] = [
        { ...mockDataSource[0], matchStatus: '' },
        mockDataSource[1]
      ];

      const result = helper.MatchStatus(dataWithBlanks);

      expect(result).toEqual([
        { value: 2, label: 'Status 2' },
        { value: 1, label: '(Blanks)' }
      ]);
    });
  });

  describe('KoTime', () => {
    it('should return options sorted by event date time', () => {
      const result = helper.KoTime(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result[0].value).toBe(1); // Earlier time
      expect(result[1].value).toBe(2); // Later time
      expect(result).toEqual([
        { value: 1, label: '03:00 PM' },
        { value: 2, label: '04:00 PM' }
      ]);
    });

    it('should sort by date correctly', () => {
      const unsortedData: TDataSource[] = [
        {
          ...mockDataSource[0],
          owpMatchId: 1,
          eventDateTime: '2024-07-10T18:00:00Z'
        },
        {
          ...mockDataSource[1],
          owpMatchId: 2,
          eventDateTime: '2024-07-10T12:00:00Z'
        }
      ];

      const result = helper.KoTime(unsortedData);

      expect(result[0].value).toBe(2); // 12:00 should come first
      expect(result[1].value).toBe(1); // 18:00 should come second
    });
  });

  describe('League', () => {
    it('should return unique options for leagues', () => {
      const result = helper.League(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'League 1' },
        { value: 2, label: 'League 2' }
      ]);
    });

    it('should handle null/undefined league with (Blanks)', () => {
      const dataWithNull: TDataSource[] = [
        // @ts-ignore - Testing null value
        { ...mockDataSource[0], league: null },
        mockDataSource[1]
      ];

      const result = helper.League(dataWithNull);

      expect(result).toEqual([
        { value: 2, label: 'League 2' },
        { value: 1, label: '(Blanks)' }
      ]);
    });

    it('should keep empty string as-is for league (not converted to Blanks)', () => {
      const dataWithEmptyString: TDataSource[] = [
        { ...mockDataSource[0], league: '' },
        mockDataSource[1]
      ];

      const result = helper.League(dataWithEmptyString);

      expect(result).toEqual([
        { value: 1, label: '' },
        { value: 2, label: 'League 2' }
      ]);
    });
  });

  describe('Match', () => {
    it('should return unique options for match descriptions', () => {
      const result = helper.Match(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'Team A -vs- Team B' },
        { value: 2, label: 'Team C -vs- Team D' }
      ]);
    });

    it('should format home vs away correctly', () => {
      const customData: TDataSource[] = [
        { ...mockDataSource[0], home: 'Home Team', away: 'Away Team' }
      ];

      const result = helper.Match(customData);

      expect(result[0].label).toBe('Home Team -vs- Away Team');
    });
  });

  describe('LstBookProvider', () => {
    it('should return unique options for LST book providers', () => {
      const result = helper.LstBookProvider(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'Provider 1' },
        { value: 2, label: 'Provider 2' }
      ]);
    });

    it('should handle null/undefined providers', () => {
      const dataWithNull: TDataSource[] = [
        { ...mockDataSource[0], lstBookProvider: null },
        mockDataSource[1]
      ];

      const result = helper.LstBookProvider(dataWithNull);

      expect(result).toEqual([
        { value: 2, label: 'Provider 2' },
        { value: 1, label: '(Blanks)' }
      ]);
    });
  });

  describe('InHouseProviderId', () => {
    it('should return unique options for in-house provider IDs', () => {
      const result = helper.InHouseProviderId(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'InHouse 1' },
        { value: 2, label: 'InHouse 2' }
      ]);
    });

    it('should handle blank in-house provider IDs', () => {
      const dataWithBlanks: TDataSource[] = [
        { ...mockDataSource[0], inHouseProviderId: '' },
        mockDataSource[1]
      ];

      const result = helper.InHouseProviderId(dataWithBlanks);

      expect(result).toEqual([
        { value: 2, label: 'InHouse 2' },
        { value: 1, label: '(Blanks)' }
      ]);
    });
  });

  describe('OtherProvider', () => {
    it('should return unique options for other providers', () => {
      const result = helper.OtherProvider(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'Other 1' },
        { value: 2, label: 'Other 2' }
      ]);
    });

    it('should handle null other providers', () => {
      const dataWithNull: TDataSource[] = [
        { ...mockDataSource[0], otherProvider: null },
        mockDataSource[1]
      ];

      const result = helper.OtherProvider(dataWithNull);

      expect(result).toEqual([
        { value: 2, label: 'Other 2' },
        { value: 1, label: '(Blanks)' }
      ]);
    });
  });

  describe('MaxBet', () => {
    it('should return unique options for max bet to display', () => {
      const result = helper.MaxBet(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'MaxBet 1' },
        { value: 2, label: 'MaxBet 2' }
      ]);
    });

    it('should handle null max bet values', () => {
      const dataWithNull: TDataSource[] = [
        { ...mockDataSource[0], maxBetToDisplay: undefined },
        mockDataSource[1]
      ];

      const result = helper.MaxBet(dataWithNull);

      expect(result).toEqual([
        { value: 2, label: 'MaxBet 2' },
        { value: 1, label: '(Blanks)' }
      ]);
    });
  });

  describe('Remark', () => {
    it('should return unique options for remarks', () => {
      const result = helper.Remark(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'Remark 1' },
        { value: 2, label: 'Remark 2' }
      ]);
    });

    it('should handle blank remarks', () => {
      const dataWithBlanks: TDataSource[] = [
        { ...mockDataSource[0], remark: null },
        mockDataSource[1]
      ];

      const result = helper.Remark(dataWithBlanks);

      expect(result).toEqual([
        { value: 2, label: 'Remark 2' },
        { value: 1, label: '(Blanks)' }
      ]);
    });
  });

  describe('RemarkBy', () => {
    it('should return unique options for remark by', () => {
      const result = helper.RemarkBy(mockDataSource);

      expect(result).toHaveLength(2);
      expect(result).toEqual([
        { value: 1, label: 'RemarkBy 1' },
        { value: 2, label: 'RemarkBy 2' }
      ]);
    });

    it('should handle blank remark by values', () => {
      const dataWithBlanks: TDataSource[] = [
        { ...mockDataSource[0], remarkBy: '' },
        mockDataSource[1]
      ];

      const result = helper.RemarkBy(dataWithBlanks);

      expect(result).toEqual([
        { value: 2, label: 'RemarkBy 2' },
        { value: 1, label: '(Blanks)' }
      ]);
    });
  });

  describe('ConfirmProvider', () => {
    it('should return options from store with additional blank option', () => {
      const result = helper.ConfirmProvider();

      expect(result).toHaveLength(3);
      expect(result).toEqual([
        { value: 1, label: 'Provider A' },
        { value: 2, label: 'Provider B' },
        { value: -1, label: '(Blanks)' }
      ]);
    });
  });

  describe('swapBlanks behavior', () => {
    it('should move (Blanks) option to the end when present', () => {
      const dataWithBlanks: TDataSource[] = [
        { ...mockDataSource[0], matchNo: '' },
        { ...mockDataSource[1], owpMatchId: 3, matchNo: 'Match 3' }
      ];

      const result = helper.MatchNo(dataWithBlanks);
      const lastItem = result[result.length - 1];

      expect(lastItem.label).toBe('(Blanks)');
    });

    it('should not affect order when no blanks present', () => {
      const result = helper.MatchNo(mockDataSource);

      expect(result).toEqual([
        { value: 1, label: 'Match 1' },
        { value: 2, label: 'Match 2' }
      ]);
    });
  });

  describe('Edge cases', () => {
    it('should handle empty data source', () => {
      const result = helper.MatchNo([]);

      expect(result).toEqual([]);
    });

    it('should handle single item data source', () => {
      const result = helper.MatchNo([mockDataSource[0]]);

      expect(result).toHaveLength(1);
      expect(result[0].label).toBe('Match 1');
    });

    it('should handle all blank values', () => {
      const allBlanksData: TDataSource[] = [
        { ...mockDataSource[0], matchNo: '' },
        { ...mockDataSource[1], matchNo: '' }
      ];

      const result = helper.MatchNo(allBlanksData);

      expect(result).toHaveLength(1);
      expect(result[0].label).toBe('(Blanks)');
    });
  });
});

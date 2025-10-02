import {
  convertResourceToOptions,
  convertResponseToCondition,
  mapEditSettingFromDataSource,
  convertDivisionResponseToCondition,
  convertConditionResponseToDisplayValue,
  hasInvalidDivision
} from './helper';
import { TCondition, TDataSource } from '../table/TableType';
import { TLeagueCategory } from '../component/Type';
import MaxbetConst from '../../../constants/MaxbetConst';

// Mock uuid
jest.mock('uuid', () => ({
  v4: jest.fn(() => 'mock-uuid-123')
}));

// Mock MaxbetConst
jest.mock('../../../constants/MaxbetConst', () => ({
  expressionOption: [
    { value: 1, label: 'Contains' },
    { value: 2, label: 'Equals' }
  ],
  divisionExpressionOption: [
    { value: 1, label: 'Contains' },
    { value: 2, label: 'Pairwise Contains' }
  ]
}));

describe('helper.ts', () => {
  describe('convertConditionResponseToDisplayValue', () => {
    it('should return empty string for empty or null response', () => {
      expect(convertConditionResponseToDisplayValue([])).toBe('');
      expect(convertConditionResponseToDisplayValue(null as any)).toBe('');
      expect(convertConditionResponseToDisplayValue(undefined as any)).toBe('');
    });

    it('should handle Contains conditions', () => {
      const response: TCondition[] = [
        { type: 'Contains', value: 'D1' },
        { type: 'Contains', value: 'D2' }
      ];
      expect(convertConditionResponseToDisplayValue(response)).toBe('*D1*;*D2*');
    });

    it('should handle Equals conditions', () => {
      const response: TCondition[] = [
        { type: 'Equals', value: 'D1,D2' },
        { type: 'Equals', value: 'D3' }
      ];
      expect(convertConditionResponseToDisplayValue(response)).toBe('D1,D2;D3');
    });

    it('should handle Pairwise Contains conditions', () => {
      const response: TCondition[] = [
        { type: 'Pairwise Contains', value: 'D1,D2' }
      ];
      expect(convertConditionResponseToDisplayValue(response)).toBe('*(D1)*-vs-*(D2)*');
    });

    it('should handle mixed conditions in order', () => {
      const response: TCondition[] = [
        { type: 'Contains', value: 'D2' },
        { type: 'Equals', value: 'D1,D2' },
        { type: 'Pairwise Contains', value: 'D1,D2' }
      ];
      expect(convertConditionResponseToDisplayValue(response)).toBe('*D2*;D1,D2;*(D1)*-vs-*(D2)*');
    });

    it('should handle multiple Pairwise Contains conditions', () => {
      const response: TCondition[] = [
        { type: 'Pairwise Contains', value: 'D2,D3' },
        { type: 'Pairwise Contains', value: 'D1,D2' }
      ];
      expect(convertConditionResponseToDisplayValue(response)).toBe('*(D2)*-vs-*(D3)*;*(D1)*-vs-*(D2)*');
    });

    it('should handle Pairwise Contains with same second value', () => {
      const response: TCondition[] = [
        { type: 'Pairwise Contains', value: 'D1,D3' },
        { type: 'Pairwise Contains', value: 'D2,D3' }
      ];
      expect(convertConditionResponseToDisplayValue(response)).toBe('*(D1)*-vs-*(D3)*;*(D2)*-vs-*(D3)*');
    });

    it('should handle complex mixed conditions', () => {
      const response: TCondition[] = [
        { type: 'Pairwise Contains', value: 'D1,D3' },
        { type: 'Pairwise Contains', value: 'D1,D4' },
        { type: 'Pairwise Contains', value: 'D2,D3' },
        { type: 'Pairwise Contains', value: 'D2,D4' }
      ];
      expect(convertConditionResponseToDisplayValue(response)).toBe('*(D1)*-vs-*(D3)*;*(D1)*-vs-*(D4)*;*(D2)*-vs-*(D3)*;*(D2)*-vs-*(D4)*');
    });

    it('should handle Pairwise Contains with invalid format', () => {
      const response: TCondition[] = [
        { type: 'Pairwise Contains', value: 'D1' }, // missing comma
        { type: 'Pairwise Contains', value: 'D1,D2' } // valid
      ];
      expect(convertConditionResponseToDisplayValue(response)).toBe('*(D1)*-vs-*(D2)*');
    });
  });

  describe('convertResourceToOptions', () => {
    it('should convert TLeagueCategory array to options format', () => {
      const resource: TLeagueCategory[] = [
        { RefName: 'ref1', Id: 1, Value: 'League 1' },
        { RefName: 'ref2', Id: 2, Value: 'League 2' }
      ];
      const result = convertResourceToOptions(resource);
      expect(result).toEqual([
        { value: 1, label: 'League 1' },
        { value: 2, label: 'League 2' }
      ]);
    });

    it('should handle empty array', () => {
      expect(convertResourceToOptions([])).toEqual([]);
    });
  });

  describe('convertResponseToCondition', () => {
    it('should convert TCondition array to TExpressionRow array', () => {
      const response: TCondition[] = [
        { type: 'Contains', value: 'test1' },
        { type: 'Equals', value: 'test2' }
      ];
      const result = convertResponseToCondition(response);
      
      expect(result).toHaveLength(2);
      expect(result[0]).toEqual({
        key: 'mock-uuid-123',
        condition: { value: 1, label: 'Contains' },
        value: 'test1'
      });
      expect(result[1]).toEqual({
        key: 'mock-uuid-123',
        condition: { value: 2, label: 'Equals' },
        value: 'test2'
      });
    });

    it('should handle empty array', () => {
      expect(convertResponseToCondition([])).toEqual([]);
    });
  });

  describe('convertDivisionResponseToCondition', () => {
    it('should convert TCondition array to TExpressionRow array using division options', () => {
      const response: TCondition[] = [
        { type: 'Contains', value: 'test1' },
        { type: 'Pairwise Contains', value: 'test2,test3' }
      ];
      const result = convertDivisionResponseToCondition(response);
      
      expect(result).toHaveLength(2);
      expect(result[0]).toEqual({
        key: 'mock-uuid-123',
        condition: { value: 1, label: 'Contains' },
        value: 'test1'
      });
      expect(result[1]).toEqual({
        key: 'mock-uuid-123',
        condition: { value: 2, label: 'Pairwise Contains' },
        value: 'test2,test3'
      });
    });
  });

  describe('mapEditSettingFromDataSource', () => {
    const mockDataSource: TDataSource = {
      leagueSettingId: 1,
      owpLeagueId: 2,
      leagueGroupId: 3,
      leagueGroup: 'Test Group',
      league: 'Test League',
      spread_HDP_Value: 1.5,
      spread_OU_Value: 2.5,
      spread_Note: 'test note',
      spreadShow: '1.5',
      maxbetWD_HDP_Show: '100',
      wD_NoOfHDP_Show: '5',
      maxbetWK_HDP_Show: '200',
      wK_NoOfHDP_Show: '10',
      lstNote: 'lst note',
      remark2: 'remark2',
      comment: 'comment',
      condition: {
        hasTv: true,
        hasIpTv: false,
        round: [{ type: 'Contains', value: 'test' }],
        division: [{ type: 'Equals', value: 'test2' }],
        remark: 'condition remark'
      },
      shortLeague: 'TL',
      isBlackList: false,
      noOfferLive: true
    };

    it('should map dataSource to edit setting format', () => {
      const result = mapEditSettingFromDataSource(mockDataSource);
      
      expect(result).toEqual({
        leagueSettingId: 1,
        owpLeagueId: 2,
        league: 'Test League',
        leagueGroupId: 3,
        leagueGroup: 'Test Group',
        spread_HDP: '1.5',
        spread_OU: '2.5',
        spread: '1.5',
        maxbetWD_HDP: '100',
        wD_NoOfHDP: '5',
        maxbetWK_HDP: '200',
        wK_NoOfHDP: '10',
        lstNote: 'lst note',
        remark2: 'remark2',
        comment: 'comment',
        hasTv: true,
        hasIpTv: false,
        round: [{ type: 'Contains', value: 'test' }],
        division: [{ type: 'Equals', value: 'test2' }],
        remark: 'condition remark',
        shortLeague: 'TL',
        isBlackList: false,
        noOfferLive: true
      });
    });

    it('should handle null/undefined values', () => {
      const dataSourceWithNulls: TDataSource = {
        ...mockDataSource,
        spread_HDP_Value: null as any,
        spread_OU_Value: undefined as any
      };
      
      const result = mapEditSettingFromDataSource(dataSourceWithNulls);
      expect(result.spread_HDP).toBe(undefined);
      expect(result.spread_OU).toBe(undefined);
    });
  });

  describe('hasInvalidDivision', () => {
    it('should return true for invalid Pairwise Contains format', () => {
      const division: TCondition[] = [
        { type: 'Pairwise Contains', value: ',D2' }, // starts with comma
        { type: 'Pairwise Contains', value: 'D1,' }, // ends with comma
        { type: 'Pairwise Contains', value: 'D1,,D2' }, // multiple commas
        { type: 'Pairwise Contains', value: 'D1 ,D2' }, // space before comma
        { type: 'Pairwise Contains', value: 'D1, D2' } // space after comma
      ];
      
      expect(hasInvalidDivision(division)).toBe(true);
    });

    it('should return false for valid Pairwise Contains format', () => {
      const division: TCondition[] = [
        { type: 'Pairwise Contains', value: 'D1,D2' },
        { type: 'Contains', value: 'D1' }
      ];
      
      expect(hasInvalidDivision(division)).toBe(false);
    });

    it('should return false for non-Pairwise Contains conditions', () => {
      const division: TCondition[] = [
        { type: 'Contains', value: 'D1' },
        { type: 'Equals', value: 'D2' }
      ];
      
      expect(hasInvalidDivision(division)).toBe(false);
    });

    it('should handle empty array', () => {
      expect(hasInvalidDivision([])).toBe(false);
    });
  });
});

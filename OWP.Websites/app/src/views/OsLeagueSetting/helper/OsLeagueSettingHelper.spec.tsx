import { ISettingDetail } from '../typing/settingDetailType';
import { Option } from '../typing/type';
import {
  formatLeagueDetailResponse,
  buildDefaultSelected,
  buildPostModelAddOrSaveSetting,
  setDuplicateField,
  validateRequiredField
} from './OsLeagueSettingHelper';

describe('OsLeagueSettingHelper', () => {
  it('formatLeagueDetailResponse when source is null', () => {
    const source = [];

    const result = formatLeagueDetailResponse(source);

    expect(result).toEqual(null);
  });

  it('formatLeagueDetailResponse should return value', () => {
    const source = [
      {
        betDelay: 'Bet 1,Bet 2',
        betTypes: 'Type 1,Type 2',
        rounds: 'Round 1,Round 2',
        offerLines: 'Line 1,Line 2',
        providers: 'WBs,St'
      }
    ];

    const expected: ISettingDetail = {
      betDelay: ['Bet 1', 'Bet 2'],
      betTypes: ['Type 1', 'Type 2'],
      rounds: ['Round 1', 'Round 2'],
      offerLines: ['Line 1', 'Line 2'],
      providers: ['WBs', 'St']
    };

    const result = formatLeagueDetailResponse(source);

    expect(result).toEqual(expected);
  });

  it('buildDefaultSelected', () => {
    const options: Option[] = [
      {
        value: 1,
        label: 'abc',
        key: 'k1',
        disabled: true
      }
    ];

    const expected: Option = {
      value: 1,
      label: '',
      key: 'k1',
      disabled: true
    };

    const result = buildDefaultSelected(options);

    expect(result).toEqual(expected);
  });

  it('buildPostModelAddOrSaveSetting', () => {
    const source = {
      betDelay: ['Bet 1', 'Bet 2'],
      betTypes: ['Type 1', 'Type 2'],
      rounds: ['Round 1', 'Round 2'],
      offerLines: ['Line 1', 'Line 2'],
      providers: ['WBs', 'St']
    };

    const expected = {
      betDelay: 'Bet 1,Bet 2',
      betTypes: 'Type 1,Type 2',
      rounds: 'Round 1,Round 2',
      offerLines: 'Line 1,Line 2',
      providers: 'WBs,St'
    };

    const result = buildPostModelAddOrSaveSetting(source);

    expect(result).toEqual(expected);
  });

  it('validateRequiredField true', () => {
    const source = {
      leagueName: 'League A',
      maxbetHDP: 123,
      maxbetOU: 111,
      betDelay: ['Bet 1', 'Bet 2'],
      betTypes: ['Type 1', 'Type 2'],
      rounds: ['Round 1', 'Round 2'],
      offerLines: ['Line 1', 'Line 2'],
      providers: ['WBs', 'St']
    };

    const result = validateRequiredField(source);

    expect(result.valid).toEqual(true);
  });
  it('validateRequiredField false', () => {
    const source = {
      leagueName: 'League A',
      maxbetHDP: null,
      maxbetOU: null,
      betDelay: ['Bet 1', 'Bet 2'],
      betTypes: ['Type 1', 'Type 2'],
      rounds: ['Round 1', 'Round 2'],
      offerLines: ['Line 1', 'Line 2'],
      providers: ['WBs', 'St']
    };

    const result = validateRequiredField(source);

    expect(result.valid).toEqual(false);
  });
});

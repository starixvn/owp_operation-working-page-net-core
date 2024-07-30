import { SubTypeOption } from './Type';
import { getSubjectBySubtype, isMatchCheckType } from './LogWorkHelper';

describe('getSubjectBySubtype', () => {
  test('returns an empty string when subtype label includes "Match Check"', () => {
    const subtype: SubTypeOption = {
      value: 21,
      label: 'Some Match Check Subtype'
    };
    const subject = getSubjectBySubtype(subtype);
    expect(subject).toBe('');
  });

  test('returns the subtype label when it does not include "Match Check"', () => {
    const subtype: SubTypeOption = { value: 1, label: 'Some Other Subtype' };
    const subject = getSubjectBySubtype(subtype);
    expect(subject).toBe('Some Other Subtype');
  });
});

describe('isMatchCheckType', () => {
  test('returns true when subtype label includes "Match Check"', () => {
    const subtype: SubTypeOption = {
      value: 21,
      label: 'Some Match Check Subtype'
    };
    const isMatchCheck = isMatchCheckType(subtype);
    expect(isMatchCheck).toBe(true);
  });

  test('returns false when subtype label does not include "Match Check"', () => {
    const subtype: SubTypeOption = { value: 1, label: 'Some Other Subtype' };
    const isMatchCheck = isMatchCheckType(subtype);
    expect(isMatchCheck).toBe(false);
  });
});

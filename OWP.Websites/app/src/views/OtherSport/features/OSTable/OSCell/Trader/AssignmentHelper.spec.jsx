import {
  getTraderNameFromInput,
  getAssignmentNoteToShow,
  formatOtherAssignment
} from './AssignmentHelper';

describe('AssignmentHelper under test', () => {
  it('getTraderNameFromInput under test, when have valueInput', () => {
    const valueInput = '#esther test note';
    const expected = {
      userName: '#esther',
      noteFirst: '',
      noteSecond: ' test note'
    };
    const actual = getTraderNameFromInput(valueInput);
    expect(actual).toEqual(expected);
  });

  it('getTraderNameFromInput under test, when not have valueInput', () => {
    const valueInput = '';
    const expected = {
      userName: '',
      noteFirst: '',
      noteSecond: ''
    };
    const actual = getTraderNameFromInput(valueInput);
    expect(actual).toEqual(expected);
  });

  it('getTraderNameFromInput under test, when not have assignment in valueInput', () => {
    const valueInput = 'esther test';
    const expected = {
      userName: ' ',
      noteFirst: 'esther test ',
      noteSecond: ' '
    };
    const actual = getTraderNameFromInput(valueInput);
    expect(actual).toEqual(expected);
  });

  it('getTraderNameFromInput under test, when have assignmentNote', () => {
    const valueInput = 'abc #esther test';
    const expected = {
      userName: '#esther',
      noteFirst: 'abc ',
      noteSecond: ' test'
    };
    const actual = getTraderNameFromInput(valueInput);
    expect(actual).toEqual(expected);
  });

  it('getAssignmentNoteToShow under test, when have valueInput', () => {
    const valueInput = '#esther test';
    const expected = {
      userName: 'esther',
      noteFirst: '',
      noteSecond: ' test'
    };
    const actual = getAssignmentNoteToShow(valueInput);
    expect(actual).toEqual(expected);
  });

  it('formatOtherAssignment under test, when have valueInput', () => {
    const valueInput = '#esther test';
    const expected = {
      userName: 'esther',
      noteFirst: '',
      noteSecond: ' test'
    };
    const actual = formatOtherAssignment(valueInput);
    expect(actual).toEqual(expected);
  });
});

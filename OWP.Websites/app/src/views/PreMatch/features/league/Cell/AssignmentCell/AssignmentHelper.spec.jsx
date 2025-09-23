import { validateAssignment } from './AssignmentHelper';
import axios from 'axios';

jest.mock('axios');

describe('AssignmentHelper under test', () => {
  it('validateAssignment  under test, when have value', async () => {
    const responseData = {
      data: [{ UserId: 123, Username: 'esther' }]
    };
    axios.post.mockResolvedValue(responseData);
    const value = 'esther';
    const oldValue = '#esther';
    const expected = {
      validateResult: true,
      valueAfterValidate: 'esther',
      valueBeforeValidate: '',
      userId: 123
    };
    const actual = await validateAssignment(value, oldValue);
    expect(actual).toEqual(expected);
  });

  it('validateAssignment  under test, when note have value', async () => {
    const responseData = {
      data: []
    };
    axios.post.mockResolvedValue(responseData);
    const value = '';
    const oldValue = 'esther';
    const expected = {
      validateResult: false,
      valueAfterValidate: '',
      valueBeforeValidate: 'esther',
      userId: 0
    };
    const actual = await validateAssignment(value, oldValue);
    expect(actual).toEqual(expected);
  });

  it('validateAssignment  under test, when old note not have value', async () => {
    const responseData = {
      data: []
    };
    axios.post.mockResolvedValue(responseData);
    const value = 'esther';
    const expected = {
      validateResult: false,
      valueAfterValidate: '',
      valueBeforeValidate: '',
      userId: 0
    };
    const actual = await validateAssignment(value);
    expect(actual).toEqual(expected);
  });
});

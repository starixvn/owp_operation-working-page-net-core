import { render, screen } from '@testing-library/react';
import Assignment from './Assignment';
import { TAssignment } from '../../type/TTaskListTable';

const testAssignments: TAssignment[] = [
  {
    UserId: '1',
    Username: 'John',
    Nickname: 'JD',
    IsCurrentUser: true,
    IsHasContactInfo: false,
    Phone: null,
    Skype: 'john123',
    Extension: '123',
    AssignmentNote: 'Some note'
  },
  {
    UserId: '2',
    Username: 'Jane',
    Nickname: 'JDoe',
    IsCurrentUser: false,
    IsHasContactInfo: true,
    Phone: '123-456-7890',
    Skype: 'jane456',
    Extension: '456',
    AssignmentNote: 'Some other note'
  },
  {
    UserId: '3',
    Username: 'Jac',
    Nickname: 'JDoe',
    IsCurrentUser: false,
    IsHasContactInfo: false,
    Phone: '123-456-7890',
    Skype: 'jac456',
    Extension: '456',
    AssignmentNote: 'Some other note'
  }
];

describe('Assignment component', () => {
  it('renders all assignments', () => {
    render(<Assignment assignment={testAssignments} />);
    const john = screen.queryByText(/John/i);
    const jane = screen.queryByText(/Jane/i);
    const jac = screen.queryByText(/Jac/i);
    expect(john).toBeInTheDocument();
    expect(jane).toBeInTheDocument();
    expect(jac).toBeInTheDocument();
  });
});

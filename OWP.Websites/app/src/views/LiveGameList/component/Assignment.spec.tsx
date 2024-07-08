import Assignment from './Assignment';
import { render } from '@testing-library/react';

describe('Assignment', () => {
  it('should render assignee', () => {
    const props = {
      assignment: [
        {
          IsCurrentUser: false,
          IsHasContactInfo: true,
          AssignmentNote: null,
          AssignmentType: 1,
          BettypeId: 1,
          Extension: null,
          IsCancelled: false,
          Nickname: 'sa',
          Phone: null,
          Skype: '123na',
          UserId: 1183,
          Username: 'sa'
        }
      ]
    };

    const { container } = render(<Assignment {...props} />);
    const dropdown = container.querySelector('.dropdown-button');
    expect(dropdown).toBeInTheDocument();
  });

  it('should render other assignee', () => {
    const props = {
      assignment: [
        {
          IsCurrentUser: true,
          IsHasContactInfo: false,
          AssignmentNote: null,
          AssignmentType: 1,
          BettypeId: 1,
          Extension: null,
          IsCancelled: false,
          Nickname: 'sa',
          Phone: null,
          Skype: '123na',
          UserId: 1183,
          Username: 'sa'
        }
      ]
    };
    const { container } = render(<Assignment {...props} />);
    const myAssignment = container.querySelector('.my-assign');
    expect(myAssignment).toBeInTheDocument();
  });
});

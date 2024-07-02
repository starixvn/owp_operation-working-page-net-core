import { render, screen } from '@testing-library/react';
import OtherIssueCell from './OtherIssueCell';

const props = {
  homeName: 'Arsenal',
  awayName: 'Liverpool',
  leagueName: 'test',
  matchName: {},
  eventDateTime: '2020-09-17 01:14:51.750',
  subject: 'New Match A',
  isRead: true
};

describe('OtherIssueCell under test', () => {
  it('should render match-cell', () => {
    render(<OtherIssueCell {...props} />);
    expect(screen.getByText(props.subject)).toBeInTheDocument();
  });
});

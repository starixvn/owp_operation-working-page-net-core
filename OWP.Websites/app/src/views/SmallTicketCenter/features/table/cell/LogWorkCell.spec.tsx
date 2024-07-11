import React from 'react';
import { render } from '@testing-library/react';
import LogWorkCell from './LogWorkCell';

jest.mock(
  './MatchCell',
  () =>
    ({ homeName, owpLeagueName, awayName, koTime, isRead, isChild }: any) =>
      (
        <div data-testid='match-cell'>
          <span>Home: {homeName}</span>
          <span>League: {owpLeagueName}</span>
          <span>Away: {awayName}</span>
          <span>Time: {koTime}</span>
          <span>Read: {isRead ? 'true' : 'false'}</span>
          <span>Child: {isChild ? 'true' : 'false'}</span>
        </div>
      )
);

jest.mock('./OtherIssueCell', () => ({ subject, isRead }: any) => (
  <div data-testid='other-issue-cell'>
    <span>Subject: {subject}</span>
    <span>Read: {isRead ? 'true' : 'false'}</span>
  </div>
));

describe('LogWorkCell component', () => {
  it('renders MatchCell when leagueName is provided', () => {
    const props = {
      isChild: false,
      homeName: 'Home Team',
      leagueName: 'Premier League',
      awayName: 'Away Team',
      koTime: '12:00 PM',
      subject: '',
      isRead: true
    };
    const { getByTestId } = render(<LogWorkCell {...props} />);
    const matchCell = getByTestId('match-cell');

    expect(matchCell).toBeInTheDocument();
    expect(matchCell).toHaveTextContent('Home: Home Team');
    expect(matchCell).toHaveTextContent('League: Premier League');
    expect(matchCell).toHaveTextContent('Away: Away Team');
    expect(matchCell).toHaveTextContent('Time: 12:00 PM');
    expect(matchCell).toHaveTextContent('Read: true');
    expect(matchCell).toHaveTextContent('Child: false');
  });

  it('renders OtherIssueCell when leagueName is not provided', () => {
    const props = {
      isChild: true,
      homeName: '',
      leagueName: '',
      awayName: '',
      koTime: '',
      subject: 'Other Issue',
      isRead: false
    };
    const { getByTestId } = render(<LogWorkCell {...props} />);
    const otherIssueCell = getByTestId('other-issue-cell');

    expect(otherIssueCell).toBeInTheDocument();
    expect(otherIssueCell).toHaveTextContent('Subject: Other Issue');
    expect(otherIssueCell).toHaveTextContent('Read: false');
  });
});

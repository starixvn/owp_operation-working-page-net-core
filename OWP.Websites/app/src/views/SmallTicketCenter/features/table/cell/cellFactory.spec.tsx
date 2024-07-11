import React from 'react';
import { render } from '@testing-library/react';
import cellFactory from './cellFactory';

jest.mock('./LeagueCell', () => ({ owpLeagueName, isRead }) => (
  <div data-testid='league-cell'>
    <span>{owpLeagueName}</span>
    <span>{isRead ? 'Read' : 'Unread'}</span>
  </div>
));

jest.mock(
  './MatchCell',
  () =>
    ({ homeName, owpLeagueName, awayName, koTime, isRead, isChild }) =>
      (
        <div data-testid='match-cell'>
          <span>{homeName}</span>
          <span>{owpLeagueName}</span>
          <span>{awayName}</span>
          <span>{koTime}</span>
          <span>{isRead ? 'Read' : 'Unread'}</span>
          <span>{isChild ? 'Child' : 'Not child'}</span>
        </div>
      )
);

jest.mock('./LogWorkCell', () => ({ subject, isRead }) => (
  <div data-testid='logwork-cell'>
    <span>{subject}</span>
    <span>{isRead ? 'Read' : 'Unread'}</span>
  </div>
));

jest.mock('./OtherIssueCell', () => ({ subject, isRead }) => (
  <div data-testid='other-issue-cell'>
    <span>{subject}</span>
    <span>{isRead ? 'Read' : 'Unread'}</span>
  </div>
));

describe('cellFactory function', () => {
  it('renders LeagueCell correctly', () => {
    const props = {
      eventType: 1,
      owpLeagueName: 'Premier League',
      isRead: true
    };
    const { getByTestId } = render(cellFactory(props));
    expect(getByTestId('league-cell')).toBeInTheDocument();
    expect(getByTestId('league-cell')).toHaveTextContent('Premier League');
    expect(getByTestId('league-cell')).toHaveTextContent('Read');
  });

  it('renders MatchCell correctly for eventType 2', () => {
    const props = {
      eventType: 2,
      homeName: 'Home Team',
      owpLeagueName: 'Premier League',
      awayName: 'Away Team',
      koTime: '12:00 PM',
      isRead: false
    };
    const { getByTestId } = render(cellFactory(props));
    expect(getByTestId('match-cell')).toBeInTheDocument();
    expect(getByTestId('match-cell')).toHaveTextContent('Home Team');
    expect(getByTestId('match-cell')).toHaveTextContent('Premier League');
    expect(getByTestId('match-cell')).toHaveTextContent('Away Team');
    expect(getByTestId('match-cell')).toHaveTextContent('12:00 PM');
    expect(getByTestId('match-cell')).toHaveTextContent('Unread');
    expect(getByTestId('match-cell')).toHaveTextContent('Not child');
  });

  it('renders MatchCell correctly for eventType 5', () => {
    const props = {
      eventType: 5,
      homeName: 'Home Team',
      owpLeagueName: 'Champions League',
      awayName: 'Away Team',
      koTime: '3:00 PM',
      isRead: true
    };
    const { getByTestId } = render(cellFactory(props));
    expect(getByTestId('match-cell')).toBeInTheDocument();
    expect(getByTestId('match-cell')).toHaveTextContent('Home Team');
    expect(getByTestId('match-cell')).toHaveTextContent('Champions League');
    expect(getByTestId('match-cell')).toHaveTextContent('Away Team');
    expect(getByTestId('match-cell')).toHaveTextContent('3:00 PM');
    expect(getByTestId('match-cell')).toHaveTextContent('Read');
    expect(getByTestId('match-cell')).toHaveTextContent('Not child');
  });

  it('renders LogWorkCell correctly', () => {
    const props = {
      eventType: 6,
      subject: 'Log Work Item',
      isRead: false
    };
    const { getByTestId } = render(cellFactory(props));
    expect(getByTestId('logwork-cell')).toBeInTheDocument();
    expect(getByTestId('logwork-cell')).toHaveTextContent('Log Work Item');
    expect(getByTestId('logwork-cell')).toHaveTextContent('Unread');
  });

  it('renders OtherIssueCell correctly for unknown eventType', () => {
    const props = {
      eventType: 10,
      subject: 'Other Issue',
      isRead: true
    };
    const { getByTestId } = render(cellFactory(props));
    expect(getByTestId('other-issue-cell')).toBeInTheDocument();
    expect(getByTestId('other-issue-cell')).toHaveTextContent('Other Issue');
    expect(getByTestId('other-issue-cell')).toHaveTextContent('Read');
  });
});

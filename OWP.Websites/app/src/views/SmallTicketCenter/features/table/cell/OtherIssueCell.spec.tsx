import React from 'react';
import { render } from '@testing-library/react';
import MatchCell from './MatchCell';

jest.mock(
  '../../../../Shared/common/CustomTable/commonCell/commonContentNew/MatchContent',
  () =>
    ({ evenDate, koTime, matchName, leagueName }: any) =>
      (
        <div data-testid='match-content'>
          <span>Even Date: {evenDate}</span>
          <span>Kickoff Time: {koTime}</span>
          <span>
            Match Name: {matchName.homeName} vs {matchName.awayName}
          </span>
          <span>League Name: {leagueName}</span>
        </div>
      )
);

describe('MatchCell component', () => {
  it('renders MatchCell with provided props', () => {
    const props = {
      homeName: 'Home Team',
      awayName: 'Away Team',
      koTime: '2024-07-16T12:00:00Z',
      owpLeagueName: 'Premier League',
      isChild: true,
      isRead: false
    };
    const { getByTestId } = render(<MatchCell {...props} />);
    const matchContent = getByTestId('match-content');

    expect(matchContent).toBeInTheDocument();
  });
});

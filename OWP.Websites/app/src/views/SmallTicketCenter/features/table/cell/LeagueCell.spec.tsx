import React from 'react';
import { render } from '@testing-library/react';
import LeagueCell from './LeagueCell';

jest.mock(
  '../../../../Shared/common/CustomTable/commonCell/commonContentNew/LeagueContent',
  () =>
    ({ leagueName }: { leagueName: string }) =>
      (
        <div data-testid='league-content'>
          <span>{leagueName}</span>
        </div>
      )
);

describe('LeagueCell component', () => {
  it('renders LeagueCell with owpLeagueName and isRead', () => {
    const props = {
      owpLeagueName: 'Premier League',
      isRead: true
    };
    const { getByTestId } = render(<LeagueCell {...props} />);

    const leagueCell = getByTestId('league-content');
    expect(leagueCell).toBeInTheDocument();
  });
});

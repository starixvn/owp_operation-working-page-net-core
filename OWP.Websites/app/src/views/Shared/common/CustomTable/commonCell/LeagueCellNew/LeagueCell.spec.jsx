import { render, screen } from '@testing-library/react';
import LeagueCell from './LeagueCell';

const props = {
  leagueName: 'esther',
  isRead: true
};

describe('LeagueCell under test', () => {
  it('should render LeagueContent', () => {
    render(<LeagueCell {...props} />);
    expect(screen.getByText('esther')).toBeInTheDocument();
  });
});

import { render, screen } from '@testing-library/react';
import LeagueContent from './LeagueContent';

const props = {
  leagueName: 'league name test'
};

describe('LeagueContent under test', () => {
  it('should render leagueName', () => {
    const { container } = render(<LeagueContent {...props} />);

    expect(container.querySelector('.league-Name')).toBeInTheDocument();
    expect(screen.getByText(props.leagueName)).toBeInTheDocument();
  });
});

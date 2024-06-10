import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import { LeagueCell } from './LeagueCell';

describe('LeagueCell under test', () => {
  const setProps = data => ({
    dataSource: [
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        matchOffer: '15s Accept',
        autoLink: 'YES',

        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        maxBets: [],
        ...data
      }
    ],
    rowIndex: 0
  });

  it('should render correct', () => {
    const { container } = render(<LeagueCell {...setProps({})} />);
    expect(container.querySelectorAll('.league-name-cell')).toHaveLength(1);
  });

  it('should render category label', () => {
    const { container } = render(
      <LeagueCell {...setProps({ category: 'Asian 4' })} />
    );

    expect(container.querySelector('.category').textContent).toEqual('Asian 4');
  });

  it('should render noHDP label', () => {
    const { container } = render(
      <LeagueCell {...setProps({ noHDP: '3HDP', category: 'Asian 4' })} />
    );

    expect(container.querySelector('.no-hdp').textContent).toEqual('3HDP');
  });
});

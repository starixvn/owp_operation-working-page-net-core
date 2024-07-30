import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MatchCell } from './MatchCell';

describe('match cell under test', () => {
  const setProps = data => ({
    dataSource: [
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        homeName: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
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
    const { container } = render(<MatchCell {...setProps({})} />);
    expect(container.querySelectorAll('.match-cell')).toHaveLength(1);
  });

  it('should render match-offer label', () => {
    const { container } = render(
      <MatchCell {...setProps({ matchOffer: '15s Accept' })} />
    );

    expect(container.querySelector('.match-offer').textContent).toEqual(
      '15s Accept'
    );
  });

  it('should render away label', () => {
    const { container } = render(<MatchCell {...setProps()} />);
    expect(container.querySelector('.away').textContent).toEqual(
      'Guangzhou R&F'
    );
    expect(container.querySelector('.home').textContent).toEqual(
      'Dalian Pro FC (N)'
    );
  });

  it('should render autolink label', () => {
    const { container } = render(
      <MatchCell {...setProps({ autoLink: 'AutoLink' })} />
    );

    const autoLinkElement = container.querySelector('span.auto-link');
    expect(autoLinkElement.textContent).toEqual('AutoLink');
  });
});

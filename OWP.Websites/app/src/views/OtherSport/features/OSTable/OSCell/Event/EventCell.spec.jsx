import { fireEvent, render } from '@testing-library/react';
import '@testing-library/jest-dom';
import EventCell from './EventCell';

const setUp = (autoLink, category, noHDP, matchOffer) => {
  return {
    dataSource: [
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: category,
        noHDP: noHDP,
        matchOffer: matchOffer,
        autoLink: autoLink,
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        assignments: [
          {
            osMatchId: 19,
            betTypeName: '1H (HDP)',
            assignmentType: 1,
            userId: 360418,
            username: 'starix1',
            assignmentNote: null
          }
        ],
        maxBets: []
      }
    ],
    rowIndex: 0
  };
};
describe('EventCell under test', () => {
  it('autoLink content is YES, should show text `Auto Link`', () => {
    const { container } = render(
      <EventCell {...setUp('YES', 'Asian 4', '3HDP', '15s Accept')} />
    );
    expect(container.querySelector('.category').textContent).toEqual('Asian 4');
    expect(container.querySelector('.no-hdp').textContent).toEqual('3HDP');
    expect(container.querySelector('.league-name').textContent).toEqual(
      'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)'
    );
    expect(container.querySelector('.match-offer').textContent).toEqual(
      '15s Accept'
    );
    expect(container.querySelector('.auto-link').textContent).toEqual(
      'Auto Link'
    );
  });

  it('autoLink content is YES, should show content of autoLink', () => {
    const { container } = render(<EventCell {...setUp('no please add')} />);
    const autoLink = container.querySelector('.auto-link');
    expect(autoLink).toBeInTheDocument();
    expect(autoLink.textContent).toEqual('no please add');
  });

  it('autoLink content is null, should not show content of autoLink', () => {
    const { container } = render(<EventCell {...setUp()} />);
    const autoLink = container.querySelector('.auto-link');
    expect(autoLink).toBeNull();
  });
});

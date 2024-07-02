import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import ProviderCell from './ProviderCell';

const setUp = (provider, otherProvider, reporter) => {
  return {
    dataSource: [
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        dateTime: '2021-01-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: provider,
        otherProvider: otherProvider,
        service: 'InPlay',
        remark: '',
        reporter: reporter,
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
describe('ProviderCell under test', () => {
  it('should render 1 provider-cell', () => {
    const { container } = render(
      <ProviderCell {...setUp('TV/BGTV/Glive53', 'BGtv/RB2tv/BETtv/')} />
    );

    expect(container.querySelectorAll('.provider-cell').length).toEqual(1);
    expect(container.querySelector('.provider').textContent).toEqual(
      'TV/BGTV/Glive53'
    );
  });

  it('should not render full items in provider-cell, when do not have provider, otherProvider', () => {
    const { container } = render(<ProviderCell {...setUp()} />);
    expect(container.querySelectorAll('.provider').length).toEqual(0);
    expect(container.querySelectorAll('.other-provider').length).toEqual(0);
  });

  it('should not render full items in provider-cell, when report', () => {
    const { container } = render(
      <ProviderCell {...setUp('TV/BGTV/Glive53', 'BGtv/RB2tv/BETtv/', 'abc')} />
    );

    expect(container.querySelectorAll('.button-provider').length).toEqual(1);
  });
});

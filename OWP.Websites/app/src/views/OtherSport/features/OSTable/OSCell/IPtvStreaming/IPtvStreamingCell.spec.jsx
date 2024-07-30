import { fireEvent, render } from '@testing-library/react';
import '@testing-library/jest-dom';
import IPtvStreamingCell from './IPtvStreamingCell';

const setUp = (iptv, wb) => {
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
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        iptv: iptv,
        wb: wb,
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
describe('IPtvStreamingCell under test', () => {
  it('should render 2 TooltipButton', () => {
    const { container } = render(
      <IPtvStreamingCell
        {...setUp('Euro40', 'IMG-B/Watch & Bet-160939/Glive-65')}
      />
    );
    expect(container.querySelectorAll('button').length).toEqual(2);
  });

  it('should not render 2 TooltipButton, when do not have iptv, wb', () => {
    const { container } = render(<IPtvStreamingCell {...setUp()} />);
    expect(container.querySelectorAll('button').length).toEqual(0);
  });

  it('should one TooltipButton, when have iptv', () => {
    const { container } = render(<IPtvStreamingCell {...setUp('Euro40')} />);
    expect(container.querySelectorAll('button').length).toEqual(1);
  });

  it('should one TooltipButton, when have wb', () => {
    const { container } = render(<IPtvStreamingCell {...setUp(null, 'abc')} />);
    expect(container.querySelectorAll('button').length).toEqual(1);
  });
});

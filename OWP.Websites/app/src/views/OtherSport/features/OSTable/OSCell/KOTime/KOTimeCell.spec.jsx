import { fireEvent, render } from '@testing-library/react';
import KOTimeCell from './KOTimeCell';

const setUp = dateTime => {
  return {
    dataSource: [
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        dateTime: dateTime,
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
describe('KOTimeCell under test', () => {
  it('should render 2 TooltipButton', () => {
    const { container } = render(
      <KOTimeCell {...setUp('2021-01-01T01:00:00')} />
    );
    expect(container.querySelectorAll('.ko-time-cell').length).toEqual(1);
  });

  it('should not render, when do not have dateTime', () => {
    const { container } = render(<KOTimeCell {...setUp()} />);
    expect(container.querySelectorAll('.ko-time-cell').length).toEqual(0);
  });

  it('should  render match status', () => {
    const { container } = render(
      <KOTimeCell {...setUp('2021-01-01T01:00:00')} />
    );
    const matchStatus = container.querySelectorAll('.match-status');
    expect(matchStatus.length).toEqual(1);
  });
});

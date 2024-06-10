import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import RemarkCell from './RemarkCell';

const setUp = otherProvider => {
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
        otherProvider: otherProvider,
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
describe('RemarkCell under test', () => {
  it('should render RemarkCell', () => {
    const { container } = render(<RemarkCell {...setUp()} />);
    expect(container.querySelectorAll('.remark-cell').length).toEqual(1);
  });
});

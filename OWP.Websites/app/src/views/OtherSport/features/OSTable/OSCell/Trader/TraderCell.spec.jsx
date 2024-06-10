import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import TraderCell from './TraderCell';

const setUp = traderMaxbetModels => {
  return {
    dataSource: [
      {
        osMatchId: 19,
        matchStatus: 'LIVe',
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
        service: 'InPlay',
        remark: '',
        traderMaxbetModels: traderMaxbetModels
      }
    ],
    rowIndex: 0
  };
};

describe('TraderCell under test', () => {
  it('should render TraderCell', () => {
    const traderMaxbetModels = [
      {
        betTypeName: '1H (HDP)',
        maxbet: {
          osMatchId: 19,
          betTypeName: '1H (HDP)',
          maxBet: 1001
        },
        traderAssignment: {
          osMatchId: 19,
          betTypeName: '1H (HDP)',
          mainTrader: 'mainTrader',
          assignmentNote: 'assignmentNote'
        }
      }
    ];
    const { container } = render(<TraderCell {...setUp(traderMaxbetModels)} />);
    expect(container.querySelectorAll('.list-trader-os').length).toEqual(1);
    expect(container.querySelectorAll('.bettype-cell').length).toEqual(1);
    expect(container.querySelectorAll('.assignment-cell').length).toEqual(1);
  });
});

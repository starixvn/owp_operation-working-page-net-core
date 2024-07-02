import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import SportCell from './SportCell';
import { TraderMaxbetModel } from '../Share/Type';

const setUp = (traderMaxbetModels: Array<TraderMaxbetModel>, sport: string) => {
  return {
    dataSource: [
      {
        osMatchId: 19,
        matchStatus: 'LIVe',
        sport: sport,
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
        iptv: '',
        matchNo: 1,
        otherProvider: '',
        reporter: '',
        traderMaxbetModels: traderMaxbetModels
      }
    ],
    rowIndex: 0
  };
};

describe('SpreadCell under test', () => {
  it('should render SpreadCell with displayMaxbetNormal', () => {
    const traderMaxbetModels = [
      {
        hiden: false,
        betTypeName: '1H (HDP)',
        matchSettings: [
          {
            betTypeName: '',
            maxBet: 2000,
            spread: '20%',
            order: 1
          }
        ],
        traderAssignment: {
          mainTrader: 'mainTrader',
          assignmentNote: 'assignmentNote'
        }
      },
      {
        hiden: false,
        betTypeName: '1H (OU)',
        matchSettings: [
          {
            betTypeName: '1H (OU)',
            maxBet: 2000,
            spread: '30%',
            order: 1
          }
        ],
        traderAssignment: {
          mainTrader: 'mainTrader',
          assignmentNote: 'assignmentNote'
        }
      }
    ];
    const { container } = render(
      <SportCell {...setUp(traderMaxbetModels, 'Basketball')} />
    );
    expect(container.querySelectorAll('.sport-cell').length).toEqual(1);
  });
});

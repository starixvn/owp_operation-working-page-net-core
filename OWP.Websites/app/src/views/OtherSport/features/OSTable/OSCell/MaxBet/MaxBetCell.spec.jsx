import MaxBetCell from './MaxBetCell';
import { UncontrolledTooltip } from 'reactstrap';

import { render } from '@testing-library/react';
import '@testing-library/jest-dom';

const setUp = (traderMaxbetModels, sport) => {
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
        traderMaxbetModels: traderMaxbetModels
      }
    ],
    rowIndex: 0
  };
};

describe('MaxBetCell under test', () => {
  it('should render MaxBetCell with displayMaxbetNormal', () => {
    const traderMaxbetModels = [
      {
        betTypeName: '1H (HDP)',
        matchSettings: [
          {
            betTypeName: '',
            maxBet: 1001
          }
        ],
        traderAssignment: {
          mainTrader: 'mainTrader',
          assignmentNote: 'assignmentNote'
        }
      },
      {
        betTypeName: '1H (OU)',
        matchSettings: [
          {
            betTypeName: '1H (OU)',
            maxBet: null
          }
        ],
        traderAssignment: {
          mainTrader: 'mainTrader',
          assignmentNote: 'assignmentNote'
        }
      }
    ];
    const { container } = render(
      <MaxBetCell {...setUp(traderMaxbetModels, 'Basketball')} />
    );
    expect(container.querySelectorAll('.maxbet').length).toEqual(2);
  });

  it('should render MaxBetCell with displayMaxbetSpecific', () => {
    const traderMaxbetModels = [
      {
        betTypeName: '1H (HDP)',
        matchSettings: [
          {
            betTypeName: '',
            maxBet: 1
          }
        ],
        traderAssignment: {
          mainTrader: 'mainTrader',
          assignmentNote: 'assignmentNote'
        }
      }
    ];
    const { container } = render(
      <MaxBetCell {...setUp(traderMaxbetModels, 'Cricket')} />
    );
    expect(container.querySelector('.maxbet-specific').textContent).toEqual(
      'Admin Setting'
    );
  });

  it('should render MaxBetCell default height', () => {
    const { container } = render(<MaxBetCell {...setUp([], 'Cricket')} />);
    expect(container.querySelectorAll('.list-maxbets-os').length).toEqual(1);
  });
});

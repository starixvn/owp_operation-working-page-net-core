import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import TableBody from './TableBody';
import { TDataSource } from '../../type/table';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';

// Mock components used within TableBody
jest.mock('./cell/ReadonlyCommonCell', () => ({ className, value, index, isCanToolTip }) => (
  <td className={className} data-testid={className}>{value}</td>
));
jest.mock('./cell/TicketCell', () => ({ HasTicket, IsRead }) => (
  <td data-testid="ticket-cell">{`${HasTicket}-${IsRead}`}</td>
));
jest.mock('./cell/AgentConfirmCell', () => ({ agentConfirm }) => (
  <td data-testid="agent-confirm-cell">{agentConfirm.map(a => a.label).join(', ')}</td>
));
jest.mock('./cell/MBCell', () => ({ HandicapSetting, OverUnderSetting }) => (
  <td data-testid="mb-cell">{`Handicap: ${HandicapSetting.MaxBet}, OverUnder: ${OverUnderSetting.MaxBet}`}</td>
));
jest.mock('./cell/EstNoteCell', () => ({ value }) => (
  <td data-testid="est-note-cell">{value}</td>
));
jest.mock('./cell/MatchCell', () => ({ home, away }) => (
  <td data-testid="match-cell">{`${home} vs ${away}`}</td>
));

// Mock data
const mockDataSource: TDataSource[] = [
  {
    OWPMatchId: 1,
    ParentMatchId: 1,
    MatchNo: 'Match 1',
    MatchStatus: 'Status 1',
    EventDateTime: '2024-01-01T00:00:00Z',
    League: 'League 1',
    Home: 'Home Team',
    Away: 'Away Team',
    AgentEst: 'Agent Est',
    AgentConfirm: [{ Value: 1, Label: 'Confirmed' }],
    RowColor: 'red',
    HasTicket: true,
    IsRead: false,
    RemarkEst: 'Remark 1',
    RemarkBy: 'Remark By 1',
    Provider: 'Provider 1',
    OtherProvider: 'Other Provider 1',
    OverUnderSetting: {
      MaxBet: 100,
      MaxBetToDisplay: '100',
      Spread: 1,
      SpreadToDisplay: '1'
    },
    HandicapSetting: {
      MaxBet: 200,
      MaxBetToDisplay: '200',
      Spread: 2,
      SpreadToDisplay: '2'
    },
    EstNote: 'Note 1',
    rowIndex: 0,
    dataSource: []
  }
];

describe('TableBody', () => {
  it('renders correctly with given data', () => {
    render(<table><TableBody dataSource={mockDataSource} /></table>);

    // Check if elements are rendered correctly
    expect(screen.getByTestId('match_no')).toHaveTextContent('Match 1');
    expect(screen.getByTestId('match_status')).toHaveTextContent('Status 1');
    expect(screen.getByTestId('league')).toHaveTextContent('League 1');
    expect(screen.getByTestId('match-cell')).toHaveTextContent('Home Team vs Away Team');
    expect(screen.getByTestId('ticket-cell')).toHaveTextContent('true-false');
    expect(screen.getByTestId('provider')).toHaveTextContent('Provider 1');
    expect(screen.getByTestId('other_provider')).toHaveTextContent('Other Provider 1');
    expect(screen.getByTestId('agent')).toHaveTextContent('Agent Est');
    expect(screen.getByTestId('mb-cell')).toHaveTextContent('Handicap: 200, OverUnder: 100');
    expect(screen.getByTestId('remark')).toHaveTextContent('Remark 1');
    expect(screen.getByTestId('remark__by')).toHaveTextContent('Remark By 1');
    expect(screen.getByTestId('agent-confirm-cell')).toHaveTextContent('Confirmed');
    expect(screen.getByTestId('est-note-cell')).toHaveTextContent('Note 1');
  });
});

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
jest.mock('./cell/TicketCell', () => ({ dataSource, rowIndex }) => (
  <td data-testid="ticket-cell">Ticket</td>
));
jest.mock('./cell/ConfirmProviderCell', () => ({ confirmProvider }) => (
  <td data-testid="agent-confirm-cell">{confirmProvider.map(a => a.label).join(', ')}</td>
));
jest.mock('./cell/NoteCell', () => ({ value }) => (
  <td data-testid="est-note-cell">{value}</td>
));
jest.mock('./cell/MatchCell', () => ({ home, away }) => (
  <td data-testid="match-cell">{`${home} vs ${away}`}</td>
));

// Mock the formatTime function
jest.mock('../../../Shared/DateTime/dateTimeHelper', () => ({
  formatTime: jest.fn().mockReturnValue('01:00 AM')
}));

// Mock the useAppSelector hook
jest.mock('../../app/hookNative', () => ({
  useAppSelector: jest.fn()
}));

// Mock data
const mockDataSource: TDataSource[] = [
  {
    owpMatchId: 1,
    parentMatchId: 1,
    matchNo: 'Match 1',
    matchStatus: 'Status 1',
    eventDateTime: '2024-01-01T00:00:00Z',
    league: 'League 1',
    home: 'Home Team',
    away: 'Away Team',
    lstBookProvider: 'Provider 1',
    inHouseProviderId: 'Other Provider 1',
    remark: 'Remark 1',
    remarkBy: 'Remark By 1',
    agentConfirm: [{ value: 1, label: 'Confirmed' }],
    rowColor: 'red',
    hasTicket: true,
    isRead: false,
    rowIndex: 0,
    dataSource: []
  }
];

// Mock virtualizer
const mockVirtualizer = {
  getTotalSize: () => 100,
  getVirtualItems: () => [{ index: 0, start: 0, size: 50 }],
  unsubs: [],
  options: {},
  scrollElement: null,
  targetWindow: null,
  measureElement: jest.fn(),
  scrollToIndex: jest.fn(),
  scrollToOffset: jest.fn(),
  scrollTo: jest.fn(),
  setScrollElement: jest.fn(),
  setTargetWindow: jest.fn(),
  setOptions: jest.fn(),
  setCount: jest.fn(),
  setOffset: jest.fn(),
  setIndex: jest.fn(),
  setScrollOffset: jest.fn(),
  setScrollElement: jest.fn(),
  setTargetWindow: jest.fn(),
  setOptions: jest.fn(),
  setCount: jest.fn(),
  setOffset: jest.fn(),
  setIndex: jest.fn(),
  setScrollOffset: jest.fn(),
  setScrollElement: jest.fn(),
  setTargetWindow: jest.fn(),
  setOptions: jest.fn(),
  setCount: jest.fn(),
  setOffset: jest.fn(),
  setIndex: jest.fn(),
  setScrollOffset: jest.fn(),
  setScrollElement: jest.fn(),
  setTargetWindow: jest.fn(),
  setOptions: jest.fn(),
  setCount: jest.fn(),
  setOffset: jest.fn(),
  setIndex: jest.fn(),
  setScrollOffset: jest.fn(),
  setScrollElement: jest.fn(),
  setTargetWindow: jest.fn(),
  setOptions: jest.fn(),
  setCount: jest.fn(),
  setOffset: jest.fn(),
  setIndex: jest.fn(),
  setScrollOffset: jest.fn()
} as any;

describe('TableBody', () => {
  beforeEach(() => {
    const { useAppSelector } = require('../../app/hookNative');
    useAppSelector.mockImplementation(callback =>
      callback({
        filterState: {
          filter: {
            eventDate: new Date()
          }
        }
      })
    );
  });

  it('renders correctly with given data', () => {
    render(<table><TableBody dataSource={mockDataSource} virtualizer={mockVirtualizer} /></table>);

    // Check if elements are rendered correctly
    expect(screen.getByTestId('match_no')).toHaveTextContent('Match 1');
    expect(screen.getByTestId('match_status')).toHaveTextContent('Status 1');
    expect(screen.getByTestId('ko_time')).toHaveTextContent('01:00 AM');
    expect(screen.getByTestId('league')).toHaveTextContent('League 1');
    expect(screen.getByTestId('match-cell')).toHaveTextContent('Home Team vs Away Team');
    expect(screen.getByTestId('ticket-cell')).toHaveTextContent('Ticket');
    expect(screen.getByTestId('lst-book-provider')).toHaveTextContent('Provider 1');
    expect(screen.getByTestId('inhouse-providerId')).toHaveTextContent('Other Provider 1');
    expect(screen.getByTestId('agent-confirm-cell')).toHaveTextContent('Confirmed');

    // Check both est-note-cell elements
    const estNoteCells = screen.getAllByTestId('est-note-cell');
    expect(estNoteCells).toHaveLength(2);
    expect(estNoteCells[0]).toHaveTextContent('Remark 1');
    expect(estNoteCells[1]).toHaveTextContent('Remark By 1');
  });
});

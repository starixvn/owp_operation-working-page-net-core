import React from 'react';
import { render } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import TicketCell from './TicketCell';
import * as hooks from '../../../app/hookNative';
import { setDataTable } from '../tableSlice';
import TicketCellCommon from '../../../../Shared/ticketEvent/component/TicketCellCommon';
import { TTicketCell } from '../../../type/table';

jest.mock('../../../app/hookNative');
jest.mock('../../../../Shared/ticketEvent/component/TicketCellCommon', () => jest.fn(() => <div>TicketCellCommon</div>));
jest.mock('../tableSlice', () => ({
  setDataTable: jest.fn()
}));

describe('TicketCell', () => {
  const mockDispatch = jest.fn();
  const defaultProps = {
    rowIndex: 1,
    dataSource: [{
      owpMatchId: 123,
      parentMatchId: 1,
      matchNo: 'Test Ticket',
      matchStatus: 'Status',
      eventDateTime: '2024-01-01T00:00:00Z',
      league: 'Test League',
      home: 'Home Team',
      away: 'Away Team',
      agentConfirm: [],
      rowColor: 'red',
      hasTicket: false,
      isRead: false,
      rowIndex: 0,
      dataSource: []
    }]
  };

  beforeEach(() => {
    jest.clearAllMocks();
    (hooks.useAppDispatch as jest.Mock).mockReturnValue(mockDispatch);
  });

  it('should render TicketCellCommon with correct props', () => {
    const { getByText } = render(<TicketCell {...defaultProps} />);

    expect(getByText('TicketCellCommon')).toBeInTheDocument();
    expect(TicketCellCommon).toHaveBeenCalledWith(
      expect.objectContaining({
        dataSource: expect.arrayContaining([
          expect.objectContaining({
            HasTicket: false,
            IsRead: false,
            Home: 'Home Team',
            Away: 'Away Team',
            EventDateTime: '2024-01-01T00:00:00Z',
            OWPMatchId: 123,
            League: 'Test League'
          })
        ]),
        rowIndex: defaultProps.rowIndex,
        updateDataSources: expect.any(Function)
      }),
      {}
    );
  });

  it('should dispatch setDataTable action when updateDataSources is called', () => {
    render(<TicketCell {...defaultProps} />);

    // Retrieve the actual function passed as updateDataSources prop
    const updateDataSources = (TicketCellCommon as unknown as jest.Mock).mock.calls[0][0].updateDataSources;
    const newData = [{ id: 2, name: 'Updated Ticket' }];

    // Call the updateDataSources function with new data
    updateDataSources(newData);

    expect(mockDispatch).toHaveBeenCalledWith(setDataTable(newData));
  });
});

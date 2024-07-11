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
  const defaultProps: TTicketCell = {
    rowIndex: 1,
    dataSource: [{ id: 1, name: 'Test Ticket', HasTicket: false }]
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
        dataSource: defaultProps.dataSource,
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

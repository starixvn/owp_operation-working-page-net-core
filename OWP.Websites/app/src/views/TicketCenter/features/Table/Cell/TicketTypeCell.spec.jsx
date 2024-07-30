import { render, screen } from '@testing-library/react';
import TicketTypeCell from './TicketTypeCell';

jest.mock('../../../app/storeNative.tsx');
const props = {
  rowIndex: 2,
  dataSource: [
    null,
    null,
    {
      typeName: 'Event Checking',
      ticketSubTypeName: 'check type',
      isRead: true
    }
  ]
};

describe('TicketTypeCell under test', () => {
  it('should render TicketTypeCell', () => {
    render(<TicketTypeCell {...props} />);

    expect(screen.getByText('Event Checking'));
    expect(screen.getByText('check type'));
  });
});

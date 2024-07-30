import { render, screen } from '@testing-library/react';
import TicketIdCell from './TicketIdCell';

jest.mock('../../../app/storeNative.tsx');
const props = {
  rowIndex: 0,
  dataSource: [{ TicketId: 1 }]
};

describe('TicketIdCell under test', () => {
  it('should render TicketIdCell', () => {
    const { container } = render(<TicketIdCell {...props} />);

    expect(container.querySelector(`[id="TicketId0"]`)).toBeInTheDocument();
  });
});

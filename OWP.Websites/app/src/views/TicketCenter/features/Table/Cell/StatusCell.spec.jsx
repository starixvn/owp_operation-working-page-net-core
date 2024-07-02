import { render, screen } from '@testing-library/react';
import StatusCell from './StatusCell';

describe('StatusCell under test', () => {
  it('should render StatusCell correct', () => {
    const props = {
      dataSource: [
        {
          ticketStatus: 'Cancel',
          isRead: true
        }
      ],
      rowIndex: 0
    };
    render(<StatusCell {...props} />);

    expect(screen.getByText(/Cancel/i)).toBeInTheDocument();
  });

  it('should render has className:has-unread', () => {
    const props = {
      dataSource: [
        {
          ticketStatus: 'Cancel',
          isRead: false
        }
      ],
      rowIndex: 0
    };
    const { container } = render(<StatusCell {...props} />);

    const element = container.querySelector(
      `[class="fixedDataTableCellLayout_wrap1 public_fixedDataTableCell_wrap1 has-unread status-cell"]`
    );
    expect(element).toBeInTheDocument();
  });
});

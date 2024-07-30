import TicketDetailHeader from './TicketDetailHeader';
import { render } from '@testing-library/react';

describe('test ticket detail header', () => {
  it('should render correct with subject', () => {
    const props = {
      ticketId: 12,
      subject: 'Other subject',
      homeName: '',
      awayName: '',
      eventName: 'League',
      koTime: new Date()
    };
    const { getByText } = render(<TicketDetailHeader {...props} />);

    const element = getByText(/#12 - Other subject/);
    expect(element).toBeInTheDocument();
  });

  it('should render correct with language', () => {
    const props = {
      ticketId: 12,
      subject: '',
      homeName: 'Team A',
      awayName: 'Team B',
      eventName: 'League',
      koTime: new Date()
    };
    const { getByText } = render(<TicketDetailHeader {...props} />);

    const element = getByText(/#12 - League/);
    expect(element).toBeInTheDocument();
  });

  it('should render correct with match', () => {
    const props = {
      ticketId: 12,
      subject: '',
      homeName: 'Team A',
      awayName: 'Team B',
      eventName: 'League',
      koTime: new Date()
    };
    const { getByText } = render(<TicketDetailHeader {...props} />);

    const element = getByText(/Team A -vs- Team B/);
    expect(element).toBeInTheDocument();
  });
});

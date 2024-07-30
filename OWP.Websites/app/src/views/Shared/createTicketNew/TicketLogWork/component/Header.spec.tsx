import { render, fireEvent } from '@testing-library/react';
import Header from './Header';

describe('Header', () => {
  it('renders the Header component correctly with the ticket header', () => {
    const ticketHeader = 'Some Ticket Header';
    const handleClosePopup = jest.fn();

    const { getByText } = render(
      <Header ticketHeader={ticketHeader} handleClosePopup={handleClosePopup} />
    );

    const headerElement = getByText(`Type: ${ticketHeader}`);
    expect(headerElement).toBeInTheDocument();
  });

  it('calls handleClosePopup when the close button is clicked', () => {
    const ticketHeader = 'Some Ticket Header';
    const handleClosePopup = jest.fn();

    const { container } = render(
      <Header ticketHeader={ticketHeader} handleClosePopup={handleClosePopup} />
    );

    const closeButton = container.querySelector('.close-chat-board-popup');
    closeButton && fireEvent.click(closeButton);

    expect(handleClosePopup).toHaveBeenCalledTimes(1);
  });
});

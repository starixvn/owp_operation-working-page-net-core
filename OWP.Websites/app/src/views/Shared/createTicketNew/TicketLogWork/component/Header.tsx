type Props = {
  ticketHeader: string;
  handleClosePopup: () => void;
};

const Header = ({ ticketHeader, handleClosePopup }: Props) => {
  return (
    <>
      <span className='close-chat-board-popup' onClick={handleClosePopup} />
      <div className='header-popup'>
        <h1> Type: {ticketHeader}</h1>
      </div>
    </>
  );
};

export default Header;

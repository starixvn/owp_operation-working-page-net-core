import { useEffect, useMemo, useState } from 'react';
import PropTypes from 'prop-types';
import { MessageTextOutlineIcon } from 'mdi-react';
import TicketListPopup from './TicketListPopup';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  handleShowTicket: PropTypes.func.isRequired,
  dataSource: PropTypes.array.isRequired
};

const IconOpenMessage = props => {
  const { rowIndex, handleShowTicket, dataSource } = props;
  const { IsRead, TicketList } = dataSource[rowIndex];

  const [isOpenTicketList, setIsOpenTicketList] = useState(false);
  const color = useMemo(
    () => (IsRead ? 'icon-message' : 'icon-unread-message'),
    [IsRead]
  );

  useEffect(() => {
    handleShowTicket();
  }, [handleShowTicket, isOpenTicketList]);

  return (
    <>
      <MessageTextOutlineIcon
        className={color}
        onClick={() => setIsOpenTicketList(true)}
        color={color}
      />
      {isOpenTicketList && (
        <TicketListPopup
          rowIndex={rowIndex}
          dataSource={dataSource}
          ticketList={TicketList}
          onCloseTicketList={() => setIsOpenTicketList(false)}
        />
      )}
    </>
  );
};

IconOpenMessage.propTypes = propTypes;
export default IconOpenMessage;

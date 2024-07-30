import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { MessageTextOutlineIcon } from 'mdi-react';
import TicketList from '../component/TicketList';

const color = {
  'true': 'icon-message',
  'false': 'icon-unread-message'
};
const propTypes = {
  dataSources: PropTypes.array,
  index: PropTypes.number,
  onShowTicketList: PropTypes.func,
  updateDataSources: PropTypes.func,
  handleOpenCreateTicket: PropTypes.func
};

const OpenTicketListHandler = props => {
  const {
    dataSources,
    index,
    onShowTicketList,
    updateDataSources,
    handleOpenCreateTicket
  } = props;
  const { IsRead } = dataSources[index];

  const [isOpenTicketList, setIsOpenTicketList] = useState(false);
  const iconClass = color[`${IsRead}`];

  useEffect(() => {
    isOpenTicketList && onShowTicketList && onShowTicketList(isOpenTicketList);
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isOpenTicketList]);

  return (
    <>
      <MessageTextOutlineIcon
        className={iconClass}
        onClick={() => setIsOpenTicketList(true)}
        color={iconClass}
      />
      {isOpenTicketList && (
        <TicketList
          dataSources={dataSources}
          index={index}
          updateDataSources={updateDataSources}
          onCloseTicketList={() => setIsOpenTicketList(false)}
          handleOpenCreateTicket={handleOpenCreateTicket}
        />
      )}
    </>
  );
};

OpenTicketListHandler.propTypes = propTypes;
export default OpenTicketListHandler;

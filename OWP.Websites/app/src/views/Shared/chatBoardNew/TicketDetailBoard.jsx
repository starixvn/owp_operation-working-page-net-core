import { useSelector } from 'react-redux';

import TicketDetail from './TicketDetail';
import PropTypes from 'prop-types';

const TicketDetailBoard = ({ isSmallBoard = false }) => {
  const { isOpenPopup } = useSelector(state => state.chatBoard);
  return (
    <>
      {isOpenPopup && (
        <TicketDetail isSmallBoard={isSmallBoard} policy='pagePolicy' />
      )}
    </>
  );
};

TicketDetailBoard.propTypes = {
  isSmallBoard: PropTypes.bool
};
export default TicketDetailBoard;

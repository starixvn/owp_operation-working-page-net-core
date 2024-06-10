import LogWorkPopup from './LogWorkPopup';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';

const propTypes = {
  creatorInfo: PropTypes.object.isRequired
};

const TicketLogWorkCreator = props => {
  const { isOpenLogWork } = useSelector(state => state.createTicketBoard);

  return isOpenLogWork && <LogWorkPopup creatorInfo={props.creatorInfo} />;
};

TicketLogWorkCreator.propTypes = propTypes;

export default TicketLogWorkCreator;

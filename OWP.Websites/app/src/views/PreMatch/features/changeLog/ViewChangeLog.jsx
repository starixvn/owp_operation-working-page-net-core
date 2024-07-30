import ChangeLog from './ChangeLog';
import ModalCommon from '../share/ModalCommon';
import PropTypes from 'prop-types';

const propTypes = {
  isDay: PropTypes.bool.isRequired
};

const ViewChangeLog = ({ isDay }) => {
  return (
    <ModalCommon className='change-log' title=' View Change Log' size='xl'>
      <h2>Change Log</h2>
      <ChangeLog isDay={isDay} />
    </ModalCommon>
  );
};
ViewChangeLog.propTypes = propTypes;
export default ViewChangeLog;

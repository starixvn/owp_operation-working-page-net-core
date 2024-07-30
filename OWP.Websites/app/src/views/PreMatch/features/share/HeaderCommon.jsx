import PropTypes from 'prop-types';
import GoToMyTicket from '../../../Shared/TicketCenterPrematch/GoToMyTicket';

const propType = {
  title: PropTypes.string.isRequired,
  lastUpdate: PropTypes.string,
  children: PropTypes.node
};

const HeaderCommon = props => {
  const { title, lastUpdate } = props;

  return (
    <div className='header pre-match'>
      <div className='top-header'>
        <h2>
          <span className='title'>{title}</span>
          <span className='lastUpdate'>{lastUpdate}</span>
        </h2>
        <GoToMyTicket />
        {props.children}
      </div>
    </div>
  );
};

HeaderCommon.propTypes = propType;
export default HeaderCommon;

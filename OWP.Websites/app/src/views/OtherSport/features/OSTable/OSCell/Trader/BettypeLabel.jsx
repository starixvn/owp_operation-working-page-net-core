import PropTypes from 'prop-types';

const BettypeLabel = ({ betTypeName }) => {
  return (
    <span className='betTypeName'>
      {betTypeName}
    </span>
  );
};

BettypeLabel.propTypes = {
  betTypeName: PropTypes.string.isRequired
};

export default BettypeLabel;

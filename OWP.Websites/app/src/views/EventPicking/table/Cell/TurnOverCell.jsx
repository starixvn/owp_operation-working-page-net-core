import PropTypes from 'prop-types';
import TGRUnavailableInfoIcon from '../../../../assets/svg-element/TGRUnavailableInfoIcon';

const propTypes = {
  value: PropTypes.string,
  color: PropTypes.string,
  name: PropTypes.string,
  isTGRUnAvailable: PropTypes.bool
};

const TurnOverCell = props => {
  const { value, color, name, isTGRUnAvailable } = props;

  return (
    <>
      <td
        style={{
          width: '30px',
          height: '30px',
          backgroundColor: '#44546b',
          color: '#fff',
          fontWeight: 700,
          paddingLeft: '3px'
        }}>
        {name}
      </td>
      <td
        style={{
          width: '100px',
          height: '30px',
          color,
          textAlign: 'right',
          paddingRight: '3px'
        }}>
        {!isTGRUnAvailable ? <TGRUnavailableInfoIcon /> : value}
      </td>
    </>
  );
};

TurnOverCell.propTypes = propTypes;
export default TurnOverCell;

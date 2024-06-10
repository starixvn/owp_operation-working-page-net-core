import PropTypes from 'prop-types';

const propTypes = {
  value: PropTypes.string,
  color: PropTypes.string,
  name: PropTypes.string
};

const TurnOverCell = props => {
  const { value, color, name } = props;

  return (
    <>
      <td
        style={{
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
          height: '30px',
          color: color,
          textAlign: 'right',
          paddingRight: '3px'
        }}>
        {value}
      </td>
    </>
  );
};

TurnOverCell.propTypes = propTypes;
export default TurnOverCell;

import PropTypes from 'prop-types';

const propTypes = {
  value: PropTypes.string,
  color: PropTypes.string
};

const MarginCell = props => {
  const { value, color } = props;
  return (
    <td
      className='margin-cell'
      style={{
        color: color,
        textAlign: 'right',
        padding: '3px'
      }}>
      {value}
    </td>
  );
};

MarginCell.propTypes = propTypes;
export default MarginCell;

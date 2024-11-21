import PropTypes from 'prop-types';
import TGRUnavailableInfoIcon from '../../../../assets/svg-element/TGRUnavailableInfoIcon';

const propTypes = {
    value: PropTypes.string,
    color: PropTypes.string,
    isTGRUnAvailable: PropTypes.bool
};

const MarginCell = props => {
    const { value, color, isTGRUnAvailable } = props;
    return (
        <td
            className='margin-cell'
            style={{
                color,
                textAlign: 'right',
                padding: '3px'
            }}>
            {isTGRUnAvailable ? <TGRUnavailableInfoIcon /> : value}
        </td>
    );
};

MarginCell.propTypes = propTypes;
export default MarginCell;

import PropTypes from 'prop-types';
import IconOpenSmallTicket from '../../../../IconCustom/IconOpenSmallTicket';
import { isEqual } from 'lodash';

const propTypes = {
  headerName: PropTypes.string.isRequired,
  onOrder: PropTypes.func.isRequired,
  orderType: PropTypes.string,
  color: PropTypes.string,
  isOpenTab: PropTypes.bool
};

const OrderableCell = props => {
  const { headerName, color, orderType, onOrder, isOpenTab = false } = props;

  const handleOnClickOrder = () => {
    const orderSelection = isEqual(orderType, 'ORDER_ASC')
      ? 'ORDER_DESC'
      : 'ORDER_ASC';
    onOrder(orderSelection);
  };

  return (
    <div className={color} onClick={handleOnClickOrder}>
      <span>{headerName}</span>{' '}
      {isOpenTab && <IconOpenSmallTicket></IconOpenSmallTicket>}
      <span className={`sort-icon ${orderType}`} />
    </div>
  );
};

OrderableCell.propTypes = propTypes;
export default OrderableCell;

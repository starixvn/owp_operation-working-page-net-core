import PropTypes from 'prop-types';
import { FilterConst } from '../../constants/reportConst';
import { isEqual } from 'lodash';

const propTypes = {
  columnName: PropTypes.string.isRequired,
  onOrder: PropTypes.func.isRequired,
  orderType: PropTypes.string,
  columnTotal: PropTypes.string
};

const OrderableCell = props => {
  const { columnName, columnTotal, orderType, onOrder } = props;

  const handleOnClickOrder = () => {
    const orderSelection = isEqual(orderType, FilterConst.ORDER_ASC)
      ? FilterConst.ORDER_DESC
      : FilterConst.ORDER_ASC;
    onOrder(orderSelection);
  };
  return (
    <div
      className={columnTotal}
      onClick={handleOnClickOrder}>
      <span>{columnName}</span>
      <span className={`sort-icon ${orderType}`} />
    </div>
  );
};

OrderableCell.propTypes = propTypes;
export default OrderableCell;

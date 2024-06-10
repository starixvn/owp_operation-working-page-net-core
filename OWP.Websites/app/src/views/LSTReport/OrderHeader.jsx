import PropTypes from 'prop-types';
import ReportConst from './constant/ReportConst';

const propTypes = {
  color: PropTypes.string,
  name: PropTypes.string.isRequired,
  onOrder: PropTypes.func.isRequired,
  subColId: PropTypes.string,
  orderName: PropTypes.string
};

const handleOrder = (e, subColId, onOrder, orderName) => {
  e.preventDefault();
  const order =
    orderName === ReportConst.ORDER_DESC
      ? ReportConst.ORDER_ASC
      : ReportConst.ORDER_DESC;
  onOrder(subColId, order);
};

const OrderHeader = ({ name, subColId, color, orderName, onOrder }) => {
  return (
    <div className={color}>
      <span>{name}</span>
      <span
        className={`sort-icon ${orderName}`}
        onClick={e => handleOrder(e, subColId, onOrder, orderName)}
      />
    </div>
  );
};

OrderHeader.propTypes = propTypes;
export default OrderHeader;

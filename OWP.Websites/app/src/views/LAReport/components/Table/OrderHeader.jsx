import { useCallback } from 'react';
import PropTypes from 'prop-types';
import OrderableCell from '../common/OrderableCell';
import { useDispatch } from 'react-redux';
import { order } from '../../actions/action';
import { isEqual } from 'lodash';
import { COLUMN_ID_HIGHLIGHT } from '../../constants/reportConst';

const propTypes = {
  columnId: PropTypes.string.isRequired,
  columnName: PropTypes.string.isRequired,
  rest: PropTypes.object
};

const OrderHeader = headerModel => {
  const { columnId, columnName, rest } = headerModel;
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  const handleOrder = orderType => {
    const postModel = {
      columnId,
      orderType
    };
    stableDispatch(order(postModel));
  };
  const columnTotal = COLUMN_ID_HIGHLIGHT.some(i => isEqual(columnId, i))
    ? 'col-total'
    : '';
  const props = {
    columnTotal,
    columnName,
    orderType: isEqual(columnId, rest.columnId) ? rest.orderType : '',
    onOrder: handleOrder
  };
  return <OrderableCell {...props} />;
};

OrderHeader.propTypes = propTypes;
export default OrderHeader;

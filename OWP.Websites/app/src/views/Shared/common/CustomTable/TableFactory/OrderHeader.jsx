import { useCallback } from 'react';
import PropTypes from 'prop-types';
import OrderableCell from '../commonCell/OrderCell/OrderableCell';
import { isEqual } from 'lodash';
import { useDispatch } from 'react-redux';

const propTypes = {
  columnKey: PropTypes.string.isRequired,
  headerName: PropTypes.string.isRequired
};

const OrderHeader = headerModel => {
  const { columnKey, headerName, sortState, sortFunc, isOpenTab } = headerModel;
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  const handleOrder = orderType => {
    const postModel = {
      columnKey,
      orderType
    };
    stableDispatch(sortFunc(postModel));
  };

  const props = {
    headerName,
    isOpenTab,
    orderType: isEqual(columnKey, sortState.columnKey)
      ? sortState.orderType
      : '',
    onOrder: handleOrder
  };

  return <OrderableCell {...props} />;
};

OrderHeader.propTypes = propTypes;
export default OrderHeader;

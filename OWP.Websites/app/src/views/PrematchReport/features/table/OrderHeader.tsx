import { isEqual } from 'lodash';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import { ColumnOrder } from '../../typeNative';
import { sort } from './reportSlice';

type Props = {
  columnId: string;
  subColName: string;
  subColId: string;
};

const OrderHeader = (props: Props) => {
  const { columnId, subColName, subColId } = props;
  const dispatch = useDispatch();
  const { columnOrder } = useSelector((state: RootState) => state.report);
  const getOrderType = () =>
    isEqual(subColId, columnOrder.columnId) ||
    (isEqual(subColId, columnOrder.subColumnId) &&
      isEqual(columnOrder.columnId, columnId))
      ? columnOrder.orderType
      : '';
  const orderType = getOrderType();

  const handleClickSort = () => {
    const order = orderType === 'desc' ? 'asc' : 'desc';
    const columnOther: ColumnOrder = {
      columnId,
      subColumnId: subColId,
      orderType: order
    };
    dispatch(sort(columnOther));
  };

  return (
    <div onClick={() => handleClickSort()}>
      <span>{subColName}</span>
      <span className={`sort-icon ${orderType}`} />
    </div>
  );
};

export default OrderHeader;

import { ISortState } from '../../type/TableType';
import { RootState } from '../../app/storeNative';
import { useSelector } from 'react-redux';
import { ORDER_ASC, ORDER_DESC } from '../TableSchema.json';
import { isEqual } from 'lodash';
import { useAppDispatch } from '../../app/hookNative';
import { sortFunc } from '../tableSlice/tableSlice';
type SortHeaderProps = {
  columnKey: string;
};
const SortHeader = ({ columnKey }: SortHeaderProps) => {
  const dispatch = useAppDispatch();
  const model = useSelector((state: RootState) => state.events);
  const { sortState } = model;
  const { orderType } = sortState;
  const handleOnClickOrder = () => {
    const sortStateNew: ISortState = {
      columnKey,
      orderType: isEqual(orderType, ORDER_ASC) ? ORDER_DESC : ORDER_ASC
    };
    dispatch(sortFunc(sortStateNew));
  };

  return (
    <span onClick={handleOnClickOrder} className={`sort-icon ${orderType}`} />
  );
};

export default SortHeader;

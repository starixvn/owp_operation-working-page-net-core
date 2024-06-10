import { isEqual } from 'lodash';
import useStore from '../../app/store';
import { SortHeaderProps, SortState } from '../type/TableType.native';
import { buildOrderSelection } from '../../helper/LaKpiHelper';

const SortHeader = (props: SortHeaderProps) => {
  const { columnKey, headerName, property } = props;
  const sortDataSources = useStore(state => state.sortDataSources);
  const sortState = useStore(state => state.sortState);
  const { columnKeySelected, orderType, propertySelected } = sortState;
  const handleOnClickOrder = () => {
    const orderSelection = buildOrderSelection(sortState, props);
    const sortStateNew: SortState = {
      columnKeySelected: columnKey,
      orderType: orderSelection,
      propertySelected: property
    };
    sortDataSources(sortStateNew);
  };

  return (
    <div className='sort-header'>
      <span className='sort-header-name'>{headerName}</span>
      <span
        onClick={handleOnClickOrder}
        className={`sort-icon ${
          isEqual(propertySelected, property) &&
          isEqual(columnKeySelected, columnKey)
            ? orderType
            : ''
        }`}
      />
    </div>
  );
};
export default SortHeader;
